using Clicktobuyservice;
using Clicktobuyservice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Clicktobuyservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAddressController : ControllerBase
    {
        private readonly ClicktoBuyDbContext _context;

        public OrderAddressController(ClicktoBuyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderAddress>>> GetOrderAddresses()
        {
            return await _context.OrderAddress.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderAddress>> GetOrderAddress(long id)
        {
            var orderAddress = await _context.OrderAddress.FindAsync(id);

            if (orderAddress == null)
            {
                return NotFound();
            }

            return orderAddress;
        }

        [HttpPost]
        public async Task<ActionResult<OrderAddress>> PostOrderAddress(OrderAddress orderAddress)
        {
            _context.OrderAddress.Add(orderAddress);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderAddress), new { id = orderAddress.AddressId }, orderAddress);
        }
    }
}
