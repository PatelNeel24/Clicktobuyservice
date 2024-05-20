using Clicktobuyservice.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clicktobuyservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ClicktoBuyDbContext _context;

        private IConfiguration _configuration;
        public RegisterController(ClicktoBuyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserTbl model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           

            // Save image data to database
            var data = new RegisterUserTbl
            {
                FirstName = model.FirstName,
              LastName=model.LastName,
              EmailId = model.EmailId,
              CreatedDate = DateTime.Now,
              Password=model.Password
            };

            _context.RegisterUserTbl.Add(data);
            await _context.SaveChangesAsync();

            return Ok("User Added successfully");
        }




    }
}
