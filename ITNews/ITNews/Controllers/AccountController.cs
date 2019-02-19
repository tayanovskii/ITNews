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
using AutoMapper;
using ITNews.Configurations;
using ITNews.Data;
using ITNews.Data.Entities;
using ITNews.DTO;
using ITNews.DTO.AccountDto;
using ITNews.DTO.UserDto;
using ITNews.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly PhotoSettings photoSettings;
        private readonly TokenSettings tokenSettings;
        private readonly IMapper mapper;
        private readonly IUserService userService;


        public AccountController(
            ApplicationDbContext context,
            IHostingEnvironment host,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IOptionsSnapshot<PhotoSettings> photoSettings,
            IOptionsSnapshot<TokenSettings> tokenSettings,
            IEmailSender emailSender,
            IMapper mapper,
            IUserService userService
                )
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.host = host;
            this.photoSettings = photoSettings.Value;
            this.tokenSettings = tokenSettings.Value;
            this.mapper = mapper;
            this.userService = userService;
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
                    return Ok(await GetToken(user));
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
                var user = mapper.Map<RegistrationDto,ApplicationUser>(registrationModel);

                var identityResult = await userManager.CreateAsync(user, registrationModel.Password);
                ;
                
                if (identityResult.Succeeded)
                {
                    user.CreatedBy = user.Id;
                    await userManager.UpdateAsync(user);
                    await userManager.AddToRoleAsync(user, "reader");  //todo constant class helper
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

        [HttpPost("blockUser/{userId}")]
        public async Task<IActionResult> BlockUser([FromRoute] string userId)
        {
            var blockedUser = await userManager.FindByIdAsync(userId);
            if (blockedUser == null) return NotFound();
            blockedUser.UserBlocked = true;
            await userManager.UpdateAsync(blockedUser);
            return Ok(userId);
        }

        [HttpPost("unBlockUser/{userId}")]
        public async Task<IActionResult> UnBlockUser([FromRoute] string userId)
        {
            var unBlockedUser = await userManager.FindByIdAsync(userId);
            if (unBlockedUser == null) return NotFound();
            unBlockedUser.UserBlocked = false;
            await userManager.UpdateAsync(unBlockedUser);
            return Ok(userId);
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
                var defaultAvatar = photoSettings.DefaultAvatar;
                var userProfile = new UserProfile()
                {
                    UserId = user.Id,
                    Avatar = defaultAvatar
                };
                await context.UserProfile.AddAsync(userProfile);
                await context.SaveChangesAsync();
                user.ModifiedAt = DateTime.Now;
                user.ModifiedBy = user.Id;
                user.UserProfileId = userProfile.Id;
                await userManager.UpdateAsync(user);

                return Ok("Email сonfirmed. Please go to the website and login");

            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }


        private async Task<string> GetToken(ApplicationUser user)
        {
            var utcNow = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
            };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim("role", role));
            }
            claims.Add(new Claim("userBlocked", user.UserBlocked.ToString()));
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Key));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddDays(tokenSettings.Lifetime),
                audience: tokenSettings.Audience,
                issuer: tokenSettings.Issuer
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

        // GET: api/Account/listUsers
        [HttpGet("listUsers")]
        public IEnumerable<UserMiniCardDto> GetUsers()
        {
            var users = context.Users
                .Include(user => user.UserProfile)
                .Include(user => user.Comments).ThenInclude(comment => comment.Likes);

            var listUserMiniCardDto = mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserMiniCardDto>>(users);
            return listUserMiniCardDto;
        }

        // GET: api/Account/availableRoles
        [HttpGet("availableRoles")]
        public List<IdentityRole> GetAvailableRoles()
        {
            var identityRoles = roleManager.Roles.ToList();
            return identityRoles;
        }

        // GET: api/Account/ManageUsers
        [HttpGet("manageUsers")]
        public IEnumerable<ManageUserDto> GetManageUsers()
        {
            var users = context.Users
                .Include(user => user.Comments)
                .ThenInclude(comment => comment.Likes);


            var listManageUserDto = new List<ManageUserDto>();
            foreach (var user in users)
            {
                var manageUserDto = mapper.Map<ApplicationUser, ManageUserDto>(user);
                manageUserDto.Roles = userManager.GetRolesAsync(user).Result;
                listManageUserDto.Add(manageUserDto);
            }
            return listManageUserDto;
        }

        // GET: api/Account/QueryManageUsers
        [HttpGet("queryManageUsers")]
        public async Task<QueryResultDto<ManageUserDto>> GetQueryManageUsers([FromQuery] UserQueryDto filterResource)
        {
            var filter = mapper.Map<UserQueryDto, UserQuery>(filterResource);
            var queryResult = await userService.GetUsersAsync(filter);


            var resultDto = mapper.Map<QueryResult<ApplicationUser>, QueryResultDto<ManageUserDto>>(queryResult);
            foreach (var userDto in resultDto.Items)
            {

                userDto.Roles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(userDto.UserId));
            }

            return resultDto;
        }

        // PUT: api/Account/userId
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutManageUsers([FromRoute] string userId, [FromBody] ManageUserDto manageUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            var changedUser = await userManager.FindByIdAsync(userId);
            if (changedUser == null) return NotFound();

            changedUser.ModifiedAt = DateTime.Now;
            if (ClaimsPrincipal.Current != null)
            {
                changedUser.ModifiedBy = userManager.GetUserAsync(ClaimsPrincipal.Current).Result.Id;
            }
            changedUser.UserBlocked = manageUserDto.UserBlocked;

            var userRoles = await userManager.GetRolesAsync(changedUser);
            var editedRoles = manageUserDto.Roles;
            var removedRoles = userRoles.Except(editedRoles).ToList();
            if (removedRoles.Any())
            {
                await userManager.RemoveFromRolesAsync(changedUser, removedRoles);
            }

            var addedRoles = manageUserDto.Roles.Except(userRoles).ToList();
            if (addedRoles.Any())
            {
                await userManager.AddToRolesAsync(changedUser, addedRoles);
            }

            return NoContent();
        }

        // DELETE: api/Account/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] string userId)
        {
            var deletedUser = await userManager.FindByIdAsync(userId);
            if (deletedUser == null) return NotFound();

            var identityResult = await userManager.DeleteAsync(deletedUser);
            
            if (!identityResult.Succeeded) return BadRequest(ModelState);

            return Ok(deletedUser.Id);

        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            if (signInManager.SignOutAsync().IsCompleted) return Ok();
            return BadRequest(ModelState);
        }

        //[HttpPost("lockUser/{userId},{forDays}")]
        //public async Task<IActionResult> LockUserAccount([FromRoute] string userId, int? forDays)
        //{
        //    var lockedUser = await userManager.FindByIdAsync(userId);
        //    if (lockedUser == null) return NotFound();
        //    var result = await userManager.SetLockoutEnabledAsync(lockedUser, true);
        //    if (result.Succeeded)
        //    {
        //        if (forDays.HasValue)
        //        {
        //            await userManager.SetLockoutEndDateAsync(lockedUser, DateTimeOffset.UtcNow.AddDays(forDays.Value));
        //        }
        //        else
        //        {
        //            await userManager.SetLockoutEndDateAsync(lockedUser, DateTimeOffset.MaxValue);
        //        }
        //    }
        //    return Ok(userId);
        //}

        //[HttpPost("unLockUser/{userId}")]
        //public async Task<IActionResult> UnLockUserAccount([FromRoute] string userId)
        //{
        //    var unLockedUser = await userManager.FindByIdAsync(userId);
        //    if (unLockedUser == null) return NotFound();
        //    var result = await userManager.SetLockoutEnabledAsync(unLockedUser, false);
        //    if (result.Succeeded)
        //    {
        //        await userManager.ResetAccessFailedCountAsync(unLockedUser);
        //    }
        //    return Ok(userId);
        //}

    }
}