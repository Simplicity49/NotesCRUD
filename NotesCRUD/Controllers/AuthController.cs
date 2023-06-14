using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NotesCRUD.Data.DbContexts;
using NotesCRUD.Data.Models;
using NotesCRUD.Data.Repository.Interface;
using NotesCRUD.Data.RequestModel;

namespace NotesCRUD.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly NotesCRUDDbContext _context;
    private readonly IAuthRepo _authRepo;

    public AuthController(IConfiguration configuration, NotesCRUDDbContext context, IAuthRepo authRepo)
    {
        _configuration = configuration;
        _context = context;
        _authRepo = authRepo;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] Register request)
    {
        var user = _authRepo.Create(request);
        return Ok("user created successfully");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Login request)
    {
        var user = _authRepo.Authenticate(request.username, request.password);
        if (user != "invalid user")
        {
            GenerateJwtToken(user);
            return Ok("login successful");
        }
        return Ok(user);
    }


    private string GenerateJwtToken(string userId)
    {

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes (_configuration["Jwt:SecretKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                //new Claim("Id", Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                //new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                //new Claim(JwtRegisteredClaimNames.Jti,
                //Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return jwtToken;
        //return tokenHandler.WriteToken(token);
    }
}

