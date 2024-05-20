using Clicktobuyservice;
using Clicktobuyservice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Clicktobuyservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ClicktoBuyDbContext _context;

        private IConfiguration _configuration;

        public ShoppingCartController(ClicktoBuyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [Authorize]
        [HttpPost("AddToCart")]
        public async Task<ActionResult<string>> AddToCart(ShoppingCart model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if the same product for the same user is already in the cart
                var existingCartItem = await _context.ShoppingCart
                    .FirstOrDefaultAsync(x => x.UserID == model.UserID && x.ProductID == model.ProductID);

                if (existingCartItem != null)
                {
                    // If the same product exists in the cart, update the quantity
                    existingCartItem.Quantity += model.Quantity;
                    _context.Entry(existingCartItem).State = EntityState.Modified;
                }
                else
                {
                    // If the product is not in the cart, add it
                    var newCartItem = new ShoppingCart
                    {
                        ProductID = model.ProductID,
                        ProductName = model.ProductName,
                        Quantity = model.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        Price = model.Price,
                        UserID = model.UserID
                    };

                    _context.ShoppingCart.Add(newCartItem);
                }

                await _context.SaveChangesAsync();
                return Ok(new CommonResponse { IsSuccess = true, Message="Product added to cart successfully" });

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                return StatusCode(500, "An error occurred while adding the product to the cart");
            }
        }

        [HttpGet("GetUserCart")]
        public async Task<ActionResult<List<ShoppingCartResponse>>> GetUserCart(long userId)
        {
            var response = new List<ShoppingCartResponse>();

            if (_context.ShoppingCart != null && _context.ProductTbl != null)
            {
                var data = await _context.ShoppingCart
                                          .Where(x => x.UserID == userId)
                                          .ToListAsync();

                foreach (var item in data)
                {
                    var product = await _context.ProductTbl.FirstOrDefaultAsync(x => x.Id == item.ProductID);

                    if (product != null)
                    {
                        var obj = new ShoppingCartResponse
                        {
                            ShoppingCartId = item.ShoppingCartID,
                            ProductImage = product.ProdctImage,
                            Description = product.Description,
                            Price = product.Price,
                            ProductName = product.ProductName,
                            Quantity = item.Quantity
                        };
                        response.Add(obj);
                    }
                }
            }

            return Ok(response);
        }


        [Authorize]
        [HttpPost("RemoveFromCart")]
        public async Task<ActionResult<string>> RemoveFromCart([FromBody] List<long> shoppingCartIds)
        {
            if (shoppingCartIds == null || !shoppingCartIds.Any())
            {
                return BadRequest("Invalid shopping cart IDs");
            }

            try
            {
                var cartItems = await _context.ShoppingCart
                    .Where(x => shoppingCartIds.Contains(x.ShoppingCartID))
                    .ToListAsync();

                if (!cartItems.Any())
                {
                    return NotFound("No matching items found in the cart");
                }

                _context.ShoppingCart.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                return Ok(new CommonResponse { IsSuccess = true, Message = "Items removed from cart successfully" });

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                return StatusCode(500, "An error occurred while removing items from the cart");
            }
        }

    }
}
