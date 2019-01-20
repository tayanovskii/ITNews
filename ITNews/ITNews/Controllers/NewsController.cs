using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITNews.Data;
using ITNews.Data.Entities;
using ITNews.DTO;

namespace ITNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public NewsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/News
        [HttpGet]
        public IEnumerable<NewsCardDto> GetCardNews()
        {
            var listNews = context.News;
            var listNewsCardDto = mapper.Map<IEnumerable<News>,IEnumerable<NewsCardDto>>(listNews);
            return listNewsCardDto;
        }

        //// GET: api/News/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetNews([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var news = await context.News.FindAsync(id);

        //    if (news == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(news);
        //}

        //// PUT: api/News/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutNews([FromRoute] int id, [FromBody] News news)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != news.Id)
        //    {
        //        return BadRequest();
        //    }

        //    context.Entry(news).State = EntityState.Modified;

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!NewsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/News
        [HttpPost]
        public async Task<IActionResult> PostNews([FromBody] CreateNewsDto createNewsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            context.News.Add(news);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news.Id }, news);
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var news = await context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            context.News.Remove(news);
            await context.SaveChangesAsync();

            return Ok(news);
        }

        private bool NewsExists(int id)
        {
            return context.News.Any(e => e.Id == id);
        }
    }
}