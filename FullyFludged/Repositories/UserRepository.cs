﻿using FullyFludged.Data;
using FullyFludged.DTOs;
using FullyFludged.Interfaces;
using FullyFludged.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace FullyFludged.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // To check whether the user exist or not using username
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        //Registering new user
        public async Task<User> RegisterAsync(RegisterRequestDto requestDto)
        {
            if (await UserExistsAsync(requestDto.Username))
                throw new Exception("Username already taken");

            using var hmac = new HMACSHA512();
            var passwordBytes = Encoding.UTF8.GetBytes(requestDto.Password);
            var user = new User
            {
                Username = requestDto.Username,
                PasswordHash = hmac.ComputeHash(passwordBytes),
                PasswordSalt = hmac.Key,
                Role = requestDto.Role
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        //Getting user details using username
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var userRaw = await _context.Users
            .Where(u => u.Username == username)
            .Select(u => new User
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role,
                PasswordHash = u.PasswordHash,
                PasswordSalt = u.PasswordSalt
            })
            .FirstOrDefaultAsync();

            if (userRaw == null)
                return null;

            return userRaw;
        }

        //refresh token

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                throw new Exception("User not found");

            existingUser.RefreshToken = user.RefreshToken;
            existingUser.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;


            // Mark only the required properties as modified
            _context.Entry(existingUser).Property(x => x.RefreshToken).IsModified = true;
            _context.Entry(existingUser).Property(x => x.RefreshTokenExpiryTime).IsModified = true;

            await _context.SaveChangesAsync();
        }

    }
}
