﻿using FullyFludged.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FullyFludged.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Serilog.Context;

namespace FullyFludged.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdCRUDController : ControllerBase
    {
        private readonly ApplicationDbContext _context; 
        private readonly ILogger<ProdCRUDController> _logger;

        public ProdCRUDController(ILogger<ProdCRUDController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        //AddProduct
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            LogContext.PushProperty("Method", "FullyFludged.CreateProduct");
            _context.Products.Add(product);
            _logger.LogInformation($"An error occurred in the GetPersonAgreementAccountInquiry method of the analyzer manage");
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        //GetAllProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        //GetProduct By ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            if (prod == null)
            {
                return NotFound();
            }
            return prod;
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            if (prod == null)
            {
                return NotFound();
            }
            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
            return Ok(prod);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product prod)
        {
            if (id != prod.Id)
            {
                return BadRequest();
            }

            _context.Entry(prod).State=EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id==id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();

        }
    }
}
