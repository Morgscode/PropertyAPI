using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MySqlConnector;
using PropertyApi.Models;

namespace PropertyApi.Controllers
{
	[ApiController]
	[Route("account")]
	public class AccountController : ControllerBase
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

        [HttpPost("register")]
		public async Task<ActionResult> Register(ApplicationRegistration model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

			await _signInManager.SignInAsync(user, isPersistent: false);

			return Ok(user);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(ApplicationLogin model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null)
			{
				return Unauthorized();
			}

			var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes("Y7Em7t$R0zDS06u@y!!6kOB7zVaU@hj2");
			var tokenDescriptior = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id),
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
            var token = tokenHandler.CreateToken(tokenDescriptior);
            var jwt = tokenHandler.WriteToken(token);

			var response = new
			{
				User = user,
				Token = jwt
			};

            return Ok(response);
        }
	}
}
