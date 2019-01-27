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
using Microsoft.AspNetCore.Authorization;

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
        [HttpGet("listCardNews")]
        //[Route("listCardNews")]
        [AllowAnonymous]
        public IEnumerable<NewsCardDto> GetCardNews()
        {
            var listNews = context.News.Include(news => news.User).Include(news => news.Ratings);
            var listNewsCardDto = mapper.Map<IEnumerable<News>, IEnumerable<NewsCardDto>>(listNews);
            return listNewsCardDto;
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFullNews([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findNews = await context.News
                .Include(news => news.User)
                .Include(news => news.Comments)
                .Include(news => news.Ratings).SingleOrDefaultAsync(news => news.Id == id);

            if (findNews == null)
            {
                return NotFound();
            }

            var findNewsDto = mapper.Map<News,FullNewsDto>(findNews);

            return Ok(findNewsDto);
        }

        // GET: api/News/GetNewsByUser
        [HttpGet("{NewsByUser/idUser}")]
        public async Task<IActionResult> GetNewsByUser([FromRoute] string userId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findNews = await context.News.Include(news => news.User).Include(news => news.Ratings).SingleOrDefaultAsync(news => news.UserId == userId);

            if (findNews == null)
            {
                return NotFound();
            }

            var findNewsCardDto = mapper.Map<News, NewsCardDto>(findNews);

            return Ok(findNewsCardDto);
        }

        //// PUT: api/News/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews([FromRoute] int id, [FromBody] EditNewsDto editNewsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != editNewsDto.Id)
            {
                return BadRequest();
            }


            //todo correct
            var editNews = mapper.Map<EditNewsDto,News>(editNewsDto);
            editNews.ModifiedAt = DateTime.Now;
            context.Entry(editNews).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
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

        // POST: api/News
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> PostNews([FromBody] CreateNewsDto createNewsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var news = mapper.Map<News>(createNewsDto);
            news.CreatedAt = DateTime.Now;
            news.ModifiedAt = news.CreatedAt;
            //todo correct
            context.News.Add(news);
            await context.SaveChangesAsync();

            //return CreatedAtAction("GetNews", new { id = news.Id }, news);
            return Ok();
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