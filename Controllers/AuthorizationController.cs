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
    public IActionResult Login(string username, string password)
    {
        CbesUser? user = _db.CbesUsers.FirstOrDefault(doc => doc.Username == username && doc.Password == password && doc.IsDeleted == false);

        if (user == null)
        {
            return BadRequest(new Response
            {
                Status = 400,
                Message = "Bad Request to Username & Password",
                Data = null
            }
            );
        }

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
