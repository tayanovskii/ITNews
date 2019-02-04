using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITNews.Data;
using ITNews.Data.Entities;
using ITNews.DTO;
using ITNews.DTO.UserDto;

namespace ITNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserProfileController(ApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper; 
            this.context = context;
        }

        // GET: api/UserProfile
        [HttpGet]
        public IEnumerable<UserProfileDto> GetUserProfile()
        {
            var listUserProfiles = context.UserProfile;
            var listUserProfileDto = mapper.Map<IEnumerable<UserProfile>,IEnumerable<UserProfileDto>>(listUserProfiles);
            return listUserProfileDto;
        }

        // GET: api/UserProfile/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userProfile = await context.UserProfile.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            var userProfileDto = mapper.Map<UserProfile,UserProfileDto>(userProfile);   
            return Ok(userProfileDto);
        }

        // GET: api/UserProfile/ByUser
        [HttpGet("ByUser/{userId}")]
        public async Task<IActionResult> GetProfileByUser([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userProfile = await context.UserProfile.SingleOrDefaultAsync(profile => profile.UserId == userId);

            if (userProfile == null)
            {
                return NotFound();
            }

            var userProfileDto = mapper.Map<UserProfile, UserProfileDto>(userProfile);
            return Ok(userProfileDto);
        }

        // PUT: api/UserProfile/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile([FromRoute] int id, [FromBody] UserProfileDto userProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userProfileDto.Id)
            {
                return BadRequest();
            }

            var editUserProfile = mapper.Map<UserProfileDto,UserProfile>(userProfileDto);

            context.Entry(editUserProfile).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                }
            }

            return NoContent();
        }

        // POST: api/UserProfile
        [HttpPost]
        public async Task<IActionResult> PostUserProfile([FromBody] UserProfileDto userProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUserProfile = mapper.Map<UserProfileDto, UserProfile>(userProfileDto);

            context.UserProfile.Add(newUserProfile);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetUserProfile", new { id = newUserProfile.Id }, userProfileDto);
        }

        // DELETE: api/UserProfile/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userProfile = await context.UserProfile.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            context.UserProfile.Remove(userProfile);
            await context.SaveChangesAsync();

            return Ok(userProfile);
        }

        private bool UserProfileExists(int id)
        {
            return context.UserProfile.Any(e => e.Id == id);
        }
    }
}