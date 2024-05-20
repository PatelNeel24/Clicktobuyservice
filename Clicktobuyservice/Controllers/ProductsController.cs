using Clicktobuyservice;
using Clicktobuyservice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Clicktobuyservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ClicktoBuyDbContext _context;

        private IConfiguration _configuration;

        public ProductsController(ClicktoBuyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<ActionResult<Login>> Login(Login model)
        {
            var user = await _context.RegisterUserTbl
                .FirstOrDefaultAsync(x => x.EmailId == model.Username && x.Password == model.Password);

            if (user != null)
            {
                var token = GenerateJwtToken(); // Assuming this method generates a JWT token
                return Ok(new LoginResponse { IsSuccess = true, Token = token ,UserId= user.Id});
            }

            return Unauthorized(new LoginResponse
            {
                IsSuccess = false,
                ErrorCode = 401, // Unauthorized error code
                ErrorMessage = "Invalid username or password."
            });
        }

        private string GenerateJwtToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], null, expires: DateTime.Now.AddHours(1), signingCredentials: credential
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<ProductTbl>>> GetProducts()
        {
            var products= new List<ProductTbl>();
            if (_context.ProductTbl != null)
            {
                var data = _context.ProductTbl;
            products = await _context.ProductTbl.ToListAsync();
            }
            return Ok(products);
        }

        [HttpGet("GetProductById")]
        public async Task<ActionResult<ProductTbl>> GetProductById(long id)
        {
            if (_context == null || _context.ProductTbl == null)
            {
                return NotFound();
            }

            var product = await _context.ProductTbl.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(ProductTbl ProductModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            byte[] imageData= { };
            if (ProductModel.ProdctImage != null) {
              imageData = Convert.FromBase64String(ProductModel.ProdctImage);

            }

            // Save image data to database
            var data = new ProductTbl
            {
                ProductName = ProductModel.ProductName,
                Description = ProductModel.Description,
                Price=ProductModel.Price,
                CreateDate= DateTime.Now,
                ContentType=ProductModel.ContentType,
                ProdctImage= ProductModel.ProdctImage
            };

            _context.ProductTbl.Add(data);
            await _context.SaveChangesAsync();

            return Ok("Product uploaded successfully");
        }

    }
}
