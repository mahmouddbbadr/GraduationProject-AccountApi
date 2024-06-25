using GraduationProject.Dtos;
using GraduationProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GraduationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly IConfiguration config;
        public AccountController(UserManager<ApplicationUser> usermanager, IConfiguration config)
        {
            this.usermanager = usermanager;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegisterDto RegisterDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = RegisterDto.UserName;
                user.Email = RegisterDto.Email;
                var result = await usermanager.CreateAsync(user, RegisterDto.Password);
                if (result.Succeeded)
                {
                    return Ok("Account Added!");
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }
            return BadRequest(ModelState);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto LoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await usermanager.FindByNameAsync(LoginDto.UserName);
                if (user != null)
                {
                    var found = await usermanager.CheckPasswordAsync(user, LoginDto.Password);
                    if (found)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await usermanager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        SecurityKey securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
                        SigningCredentials signingcredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken MyToken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],
                            audience: config["JWT:ValidAudiance"],
                            claims: claims,
                            expires: DateTime.Now.AddDays(10),
                            signingCredentials: signingcredentials
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                            expiration = MyToken.ValidTo
                        });

                    }
                    return Unauthorized("username or password is not correct");

                }
                return Unauthorized("username or password is not correct");

            }
            return Unauthorized("username or password is not correct");
        }
    }
}
