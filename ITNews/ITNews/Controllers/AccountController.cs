using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ITNews.Configurations;
using ITNews.Data;
using ITNews.Data.Entities;
using ITNews.DTO;
using ITNews.DTO.AccountDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ITNews.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
 public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IHostingEnvironment host;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly PhotoSettings photoSettings;


        public AccountController(
            ApplicationDbContext context,
            IHostingEnvironment host,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IOptionsSnapshot<PhotoSettings> options,
            IEmailSender emailSender)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.host = host;
            photoSettings = options.Value;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginModel.UserName);
                if (user != null)
                {
                    // check confirm email
                    if (!await userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "You have not confirmed your email");
                        return BadRequest(ModelState);
                    }
                }

                var result = await signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password,
                    isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok(GetToken(user));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Wrong username or login");
                }
            }

            return BadRequest(ModelState);
        }
 

        [Authorize]
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()
                );
            return Ok(GetToken(user));
        }


        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    //TODO: Use Automapper instaed of manual binding  
                
                    UserName = registrationModel.UserName,
                    Email = registrationModel.Email
                };
                
                var identityResult = await userManager.CreateAsync(user, registrationModel.Password);
                
                if (identityResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");  //todo constant class helper
                    var confirmationToken = await userManager.
                        GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail",
                        "Account", new
                        {
                            userId = user.Id,
                            token = confirmationToken
                        },
                        protocol: HttpContext.Request.Scheme);
                    try
                    {
                        await emailSender.SendEmailAsync(user.Email, "Confirm email for ITNews site",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.");
                    }
                    catch (Exception e)
                    {
                        return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                    }
                    return Ok("To complete the registration, check the email and follow the link in the email!");
                }
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }
            return BadRequest(ModelState);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest("Empty data in confirmation link");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var identityResult = await userManager.ConfirmEmailAsync(user,token);

            if (identityResult.Succeeded)
            {
                //return Ok(GetToken(user));
                //return CreatedAtAction("Login", user);
                var defaultAvatar = Path.Combine(host.WebRootPath, photoSettings.DefaultAvatar);
                await context.UserProfile.AddAsync(new UserProfile()
                {
                    UserId = user.Id,
                    Avatar = defaultAvatar
                });

                await context.SaveChangesAsync();
                return Ok("Email сonfirmed. Please go to the website and login");

            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }


        private string GetToken(ApplicationUser user)
        {
            var utcNow = DateTime.UtcNow;
            //??? or use var identity = new ClaimsIdentity(new[]{new Claim..})
            var claims = new List<Claim>
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
            };
            var userRoles = userManager.GetRolesAsync(user).Result;
            foreach (var role in userRoles)
            {
                claims.Add(new Claim("role", role));
            }
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddDays(configuration.GetValue<int>("Tokens:Lifetime")),
                audience: configuration.GetValue<string>("Tokens:Audience"),
                issuer: configuration.GetValue<string>("Tokens:Issuer")
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

    }
}