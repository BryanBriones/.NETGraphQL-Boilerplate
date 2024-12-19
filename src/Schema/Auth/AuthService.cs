using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthCore.Helpers;
using AuthCore.Models;
using Microsoft.IdentityModel.Tokens;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace AuthCore.Services;

public class AuthService
{
    private readonly PharmaservAdminContext _context;
    public AuthService(IDbContextFactory<PharmaservAdminContext> contextFactory){
        _context = contextFactory.CreateDbContext();
    }

    public async Task<string> Authenticate(string email, string password)
    {
        //Replace this process from Laravel sign in result
        var userLogin = await _context.UserLogins.Where(login => login.Email == email).FirstOrDefaultAsync();

          
          var roles = new List<string>();

            if (userLogin?.UserRole == "Admin")
            {
                roles.Add("Admin");
            }


            User user = new()
                {
                   Id = Guid.NewGuid(),
                   Email = userLogin?.Email,
                   Roles = roles
                };
                
                return GenerateToken(user);
      

            throw new AuthenticationException();

    }

    public string GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "issuer",
            Audience = "audience",
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));

        foreach (var role in user.Roles)
            claims.AddClaim(new Claim(ClaimTypes.Role, role));

        return claims;
    }
}