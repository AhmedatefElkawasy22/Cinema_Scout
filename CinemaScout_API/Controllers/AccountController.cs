using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaScout_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtOptions _jwtOptions;
        public UserManager<IdentityUser> userManager { get; }
        public IConfiguration configuration { get; }

        public AccountController(UserManager<IdentityUser> _userManager, IConfiguration _configuration, JwtOptions jwtOptions)
        {
            userManager = _userManager;
            configuration = _configuration;
            _jwtOptions = jwtOptions;
        }


        [HttpPost("/Registration")]
        public async Task<IActionResult> Registration(RegisterUserDTO user)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = user.Username,
                    Email = user.Email
                };

                IdentityResult result = await userManager.CreateAsync(identityUser, user.Password);

                if (result.Succeeded)
                    return Ok("User created successfully");
                return BadRequest(result.Errors);

            }
            return BadRequest(ModelState);
        }
        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginUserDTo user)
        {
            if (ModelState.IsValid)
            {
                IdentityUser FindUser = await userManager.FindByNameAsync(user.UserName);

                if (FindUser != null && await userManager.CheckPasswordAsync(FindUser, user.Password))
                {
                    // Add claims
                    List<Claim> allclaims = new List<Claim>
                         {
                              new Claim(ClaimTypes.Name, FindUser.UserName),
                              new Claim(ClaimTypes.NameIdentifier, FindUser.Id),
                              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                         };

                    var Roles = await userManager.GetRolesAsync(FindUser);
                    foreach (var role in Roles)
                    {
                        allclaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        audience: _jwtOptions.Audience,
                        claims: allclaims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: credentials
                    );

                    return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
                }
                return BadRequest(FindUser != null ? "Incorrect username or password" : "User not found");
            }
            return BadRequest(ModelState);
        }

    }
}
