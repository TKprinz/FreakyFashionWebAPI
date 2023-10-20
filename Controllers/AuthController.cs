using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using FreakyFashionWebAPI.Data;


namespace FreakyFashionWebAPI.Controllers;

// /auth -> AuthController
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IConfiguration config;

    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        this.context = context;
        this.config = config;
    }

    [HttpPost]
    public ActionResult<TokenDto> Authenticate(AuthenticateRequest authenticateRequest)
    {
        // 1 - Kontrollera om detta är en behörig användare
        var user = context.Users.FirstOrDefault(x =>
               x.UserName == authenticateRequest.UserName
            && x.Password == authenticateRequest.Password);

        // 1:1 - Om användaren inte finns, returnera 401 Unauthorized
        if (user is null)
        {
            return Unauthorized(); // 401 Unauthorized
        }

        // 1:2 - Om användaren finns, generera JTW och returnera till klienten
        var tokenDto = new TokenDto
        {
            Token = GenenerateToken()
        };

        return tokenDto; // 200 OK
    }

    private string GenenerateToken()
    {
        var signingKey = Convert.FromBase64String(config["Jwt:SigningSecret"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(signingKey),
            SecurityAlgorithms.HmacSha256Signature)
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = jwtTokenHandler
          .CreateJwtSecurityToken(tokenDescriptor);

        // Generera  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
        return jwtTokenHandler.WriteToken(jwtSecurityToken);
    }
}

public class TokenDto
{
    public string Token { get; set; }
}

public class AuthenticateRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
};