using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CBEsApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CBEsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CBEsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly ILogger<AuthorizationController> _logger;

    public AuthorizationController(ILogger<AuthorizationController> logger)
    {
        _logger = logger;
    }

    private const string TokenSecret = "YourSecretKeyForAuthenticationOfApplicationDeveloper";

    private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(10);

    private CbesManagementContext _db = new CbesManagementContext();

    public struct RequestCreate
    {
        /// <summary>
        /// Username of the User
        /// </summary>
        /// <example>admin</example>
        /// <required>true</required>
        public string? Username { get; set; }

        /// <summary>
        /// Password of the User
        /// </summary>
        /// <example>1234</example>
        /// <required>true</required>
        public string? Password { get; set; }
    }

    public struct RegisterCreate
    {
        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }

    /// <summary>
    /// Generate Token
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /GenerateToken
    ///     
    ///     {
    ///         "username": "string"
    ///     }
    ///     
    /// </remarks>
    [HttpPost("/GenerateToken", Name = "GenerateToken")]
    public string GenerateToken([FromBody] CbesUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);

        var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("ID", user.Id.ToString()),

            // Add more claims as needed
            };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifetime),
            Issuer = "http://localhost:8000",
            Audience = "http://localhost:8000",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        if (token != null)
        {
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        else
        {
            return "Failed to write token.";
        }
    }

    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/Authorization/login
    ///     
    ///     {
    ///         "username": "admin",
    ///         "password": "1234"
    ///     }
    ///     
    /// </remarks>
    [HttpPost("Login", Name = "Login")]
    public ActionResult Login([FromBody] RequestCreate request)
    {
        CbesUser user = _db.CbesUsers.FirstOrDefault(doc => doc.Username == request.Username && doc.IsDeleted == false);

        // Check if user is locked out due to consecutive failed login attempts
        if (user.IsLog == false && user.LoginFailedCount >= 5)
        {
            // If the user is still within the lockout period
            if (user.LoginDate != null && user.LoginDate.Value.AddMinutes(5) > DateTime.UtcNow)
            {
                TimeSpan timeToWait = user.LoginDate.Value.AddMinutes(5) - DateTime.UtcNow;
                int secondsToWait = (int)Math.Ceiling(timeToWait.TotalSeconds);

                return StatusCode(429, new Response
                {
                    Status = 429,
                    Message = $"ผู้ใช้ถูก Block เนื่องจากกรอกข้อมูลผิดต่อเนื่อง {user.LoginFailedCount}/5 ครั้ง โปรดลองอีกครั้งในภายหลัง (รออีก {secondsToWait} วินาที)",
                    Data = null
                });
            }

            // If the lockout period has passed, reset failed attempts and unlock the account
            if (user.LoginDate.Value.AddMinutes(5) <= DateTime.UtcNow)
            {
                user.IsLog = true;
                user.LoginFailedCount = 0;

                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }


            // Successful login
            string bearerToken = GenerateToken(user);

            return Ok(new Response
            {
                Status = 200,
                Message = "Login Success",
                Data = new
                {
                    token = bearerToken,
                }
            });
        }


        // If password is incorrect, increment login failed count and save changes
        if (user.Password != request.Password)
        {
            user.LoginFailedCount = user.LoginFailedCount + 1;
            user.LoginDate = DateTime.UtcNow;

            // Check if login failed count exceeds limit
            if (user.LoginFailedCount >= 5)
            {
                user.IsLog = false;
            }

            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
        }

        return BadRequest(new Response
        {
            Status = 401,
            Message = "Unauthorized to Password",
            Data = null
        });
    }


    [HttpPost("Register", Name = "Register")]
    public ActionResult<Response> Register(RegisterCreate registerCreate)
    {
        CbesUser user = new CbesUser
        {
            Username = registerCreate.Username,
            Password = registerCreate.Password,
        };


        try
        {
            user = CbesUser.Create(_db, user);
        }
        catch
        {
            // Handle unique key constraint violation (duplicate username)
            return StatusCode(409, new Response
            {
                Status = 409,
                Message = "Username already exists",
                Data = null
            });
        }

        return Ok(new Response
        {
            Status = 201,
            Message = "Created Successfully",
            Data = user
        });
    }


}
