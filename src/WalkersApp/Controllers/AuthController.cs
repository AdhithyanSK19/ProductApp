using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WalkersApp.Models.DTOs;
using WalkersApp.Repository;

namespace WalkersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> _userManager, ITokenRepository _tokenRepository)
        {
            userManager = _userManager;
            tokenRepository = _tokenRepository; 
        }

        //api/Auth/registerUser

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Username
            };
            var result = await userManager.CreateAsync(identityUser, registerRequest.Password);

            if (result.Succeeded)
            {
                if (registerRequest.Roles != null && registerRequest.Roles.Any())
                {
                    result = await userManager.AddToRolesAsync(identityUser, registerRequest.Roles);
                    if (result.Succeeded)
                    {
                        return Ok("User added successfully!!!");
                    }
                }
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (isPasswordValid)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null && roles.Any())
                    {
                        var token = tokenRepository.CreateJWTToken(user, [..roles]);
                        //generate token
                        var response = new LoginResponseDto()
                        {
                            JwtToken = token,
                        };
                        return Ok(response);
                    }

                }

            }

            return BadRequest("Username or password is incorrect");
        }
    }
}
