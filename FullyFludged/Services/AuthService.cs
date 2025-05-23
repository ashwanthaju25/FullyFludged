﻿using FullyFludged.DTOs;
using FullyFludged.Interfaces;
using FullyFludged.Models;
using System.Security.Cryptography;
using System.Text;

namespace FullyFludged.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /*public async Task<LoginResponseDto?> Authenticate(LoginRequestDto loginRequest)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);// Getting user details from db

            if (user == null)
                return null;//username incorrect

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

            // Compare byte by byte 
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return null;//invalid password
            }


            
            string token = _jwtTokenGenerator.GenerateToken(user.Username, user.Id, user.Role);
            return new LoginResponseDto { Token = token };
        }*/
        public async Task<LoginResponseDto?> Authenticate(LoginRequestDto loginRequest)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);
            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

            for (int i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != user.PasswordHash[i])
                    return null;

            string jwt = _jwtTokenGenerator.GenerateToken(user.Username, user.Id, user.Role);

            string refreshToken;
            if (string.IsNullOrEmpty(user.RefreshToken) || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                // Only generate if it's missing or expired
                refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userRepository.UpdateUserAsync(user);
            }
            else
            {
                refreshToken = user.RefreshToken; // Reuse the existing one
            }

            return new LoginResponseDto { Token = jwt, RefreshToken = refreshToken };
        }

    }
}



/* // Convert salt back to byte[] from Base64 string
 var saltBytes = Convert.FromBase64String(user.PasswordSalt);

 using var hmac = new HMACSHA512(saltBytes);
 var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

 // Convert stored hash from Base64 string to byte[] to compare
 var storedHashBytes = Convert.FromBase64String(user.PasswordHash);

 // Compare hash byte by byte
 if (!computedHash.SequenceEqual(storedHashBytes))
     return null; // Invalid password
*/