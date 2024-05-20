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
    public class UserAddressController : ControllerBase
    {
        private readonly ClicktoBuyDbContext _context;

        public UserAddressController(ClicktoBuyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAddress>>> GetUserAddresses()
        {
            return await _context.UserAddress.ToListAsync();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserAddress>>> GetUserAddressesByUserId(long userId)
        {
            var userAddresses = await _context.UserAddress
                                        .Where(address => address.UserID == userId)
                                        .ToListAsync();

            if (userAddresses == null || userAddresses.Count == 0)
            {
                return NotFound();
            }

            return userAddresses;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserAddress>> GetUserAddress(int id)
        {
            var userAddress = await _context.UserAddress.FindAsync(id);

            if (userAddress == null)
            {
                return NotFound();
            }

            return userAddress;
        }

        [HttpPost]
        public async Task<ActionResult<UserAddress>> PostUserAddress(UserAddress userAddress)
        {
            try
            {
                _context.UserAddress.Add(userAddress);
                await _context.SaveChangesAsync();
                return Ok(new CommonResponse { IsSuccess = true ,Message="Address Save Successfully"});

            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, $"An error occurred while saving the user address: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<UserAddress>> PutUserAddress(UserAddress userAddress)
        {
            try
            {
                if (userAddress.IsDefaultAddress == false)
                {

                    var defaultAddresses = await _context.UserAddress
                                      .Where(address => address.UserID == userAddress.UserID && address.IsDefaultAddress == true && address.IsActive == true)
                                      .ToListAsync();
                    if (defaultAddresses.Count == 1)
                    {
                        return Ok(new CommonResponse { IsSuccess = false, Message = "Please set alteast one default address" });
                    }
                }
            
                    var existingAddresses = await _context.UserAddress
                                            .Where(address => address.UserID == userAddress.UserID)
                                            .ToListAsync();
                    existingAddresses.ForEach(x => x.IsDefaultAddress = false);
                    _context.UserAddress.UpdateRange(existingAddresses);
                    var existingAddress = await _context.UserAddress.FindAsync(userAddress.AddressID);
                    if (existingAddress == null)
                    {
                        return NotFound();
                    }

                    // Update the existing address properties
                    existingAddress.FullName = userAddress.FullName;
                    existingAddress.Email = userAddress.Email;
                    existingAddress.Phone = userAddress.Phone;
                    existingAddress.AddressLine1 = userAddress.AddressLine1;
                    existingAddress.AddressLine2 = userAddress.AddressLine2;
                    existingAddress.City = userAddress.City;
                    existingAddress.StateProvince = userAddress.StateProvince;
                    existingAddress.Country = userAddress.Country;
                    existingAddress.PostalCode = userAddress.PostalCode;
                    existingAddress.IsDefaultAddress = userAddress.IsDefaultAddress;
                    existingAddress.IsActive = userAddress.IsActive;
                    existingAddress.Title = userAddress.Title;
                    // Add more properties as needed

                    _context.Entry(existingAddress).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return Ok(new CommonResponse { IsSuccess = true, Message = "Address updated successfully" });
                
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, $"An error occurred while updating the user address: {ex.Message}");
            }
        }

    }
}