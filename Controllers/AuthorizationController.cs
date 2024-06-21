using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CBEsApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CBEsApi.Models;

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


    public struct RegisterCreate
    {
        [Required(ErrorMessage = "User Name is required")]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? Role { get; set; }
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
    public string GenerateToken([FromBody] string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);

        var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, username),
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

    [HttpPost("Login", Name = "Login")]
    public IActionResult Login([FromBody] CbesUser request)
    {
        CbesUser? user = _db.CbesUsers.FirstOrDefault(doc => doc.Username == request.Username && doc.Password == request.Password && doc.IsDeleted == false);

        if (user == null)
        {
            return NotFound();
        }

        string bearerToken;

        try
        {
            bearerToken = GenerateToken(user.Username);
        }
        catch
        {
            return BadRequest(new Response
            {
                Status = 400,
                Message = "Bad Request to Username & Password",
                Data = null
            }
            );
        }
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

    [HttpPost("Register", Name = "Register")]
    public ActionResult<Response> Register(RegisterCreate registerCreate)
    {
        if (registerCreate.Role.IsNullOrEmpty())
        {
            registerCreate.Role = "user";
        }

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

        return user.Username == null || user.Password == null
            ? BadRequest(new Response
            {
                Status = 400,
                Message = "Bad Request",
                Data = null
            })
            : Ok(new Response
            {
                Status = 201,
                Message = "Created Successfully",
                Data = user
            });
    }


}
