using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITNews.DTO;
using ITNews.Data;
using ITNews.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ITNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public TagController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Tag
        [HttpGet]
        public IEnumerable<TagDto> GetTagDto()
        {
            var contextTags = context.Tags;
            var listTagDto = mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(contextTags);
            return listTagDto;
        }

        // GET: api/Tag/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagDto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tag = await context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            var tagDto = mapper.Map<Tag,TagDto>(tag);
            return Ok(tagDto);
        }

        // PUT: api/Tag/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTagDto([FromRoute] int id, [FromBody] TagDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tagDto.Id)
            {
                return BadRequest();
            }

            var tag = mapper.Map<TagDto,Tag>(tagDto);
            tag.ModifiedAt = DateTime.Now;
            context.Entry(tag).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
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

        // POST: api/Tag
        [HttpPost]
        public async Task<IActionResult> PostTagDto([FromBody] TagDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTag = mapper.Map<TagDto,Tag>(tagDto);
            newTag.CreatedAt = DateTime.Now;
            newTag.ModifiedAt = newTag.CreatedAt;
            context.Tags.Add(newTag);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetTagDto", new { id = newTag.Id }, newTag);
        }

        // DELETE: api/Tag/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagDto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tag = await context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            context.Tags.Remove(tag);
            await context.SaveChangesAsync();
            var tagDto = mapper.Map<Tag,TagDto>(tag);
            return Ok(tagDto);
        }

        private bool TagExists(int id)
        {
            return context.Tags.Any(e => e.Id == id);
        }
    }
}