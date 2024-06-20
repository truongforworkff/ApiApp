using FptJobBack.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FptJobBack.Models;


namespace FptJobBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
        {
            // Tìm kiếm user trong database
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }


            // Cập nhật thời gian đăng nhập lần cuối
            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            // Trả về thông tin phân biệt loại user
            return Ok(new { Role = user.Role, UserId = user.UserId });
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Kiểm tra xem người dùng đã tồn tại hay chưa
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                if (existingUser != null)
                {
                    return BadRequest("Username is already taken.");
                }

                // Tạo một đối tượng User mới
                var newUser = new Users
                {
                    Username = request.Username,
                    Password = request.Password,
                    Email = request.Email,
                    Role = "User", // Gán vai trò mặc định cho người dùng mới
                    DateCreated = DateTime.Now,
                    LastLogin = null // Không có lần đăng nhập trước đó
                                     // Các thuộc tính khác có thể cần thiết cho quá trình đăng ký
                };

                // Thêm người dùng mới vào cơ sở dữ liệu
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Trả về thông tin về người dùng đã đăng ký thành công
                return Ok(new { Role = newUser.Role, UserId = newUser.UserId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error registering user: {ex.Message}");
            }
        }

    }
}
