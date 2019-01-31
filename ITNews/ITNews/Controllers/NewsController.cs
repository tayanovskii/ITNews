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
        [AllowAnonymous]
        public IEnumerable<NewsCardDto> GetCardNews()
        {
            var listNews = context.News.Include(news => news.User)
                .Include(news => news.NewsTags).ThenInclude(tag => tag.Tag)
                .Include(news => news.NewsCategories).ThenInclude(category => category.Category)
                .Include(news => news.Comments)
                .Include(news => news.Ratings);
            var listNewsCardDto = mapper.Map<IEnumerable<News>, IEnumerable<NewsCardDto>>(listNews);
            return listNewsCardDto;
        }

        //Todo search news by content!!!

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
                .Include(news => news.Ratings)
                .Include(news => news.NewsCategories).ThenInclude(category => category.Category)
                .Include(news => news.NewsTags).ThenInclude(tag => tag.Tag)
                .SingleOrDefaultAsync(news => news.Id == id);

            if (findNews == null)
            {
                return NotFound();
            }

            findNews.VisitorCount++;
            await context.SaveChangesAsync();
            var findNewsDto = mapper.Map<News,FullNewsDto>(findNews);

            return Ok(findNewsDto);
        }
      
        // GET: api/News/ForEdit
        [HttpGet("ForEdit/{newsId}")]
        public async Task<IActionResult> GetNewsForEdit([FromRoute] int newsId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findNews = await context.News
                .Include(news => news.NewsTags)
                    .ThenInclude(tags => tags.Tag)
                .Include(news => news.NewsCategories)
                    .ThenInclude(categories => categories.Category)
                .SingleOrDefaultAsync(news => news.Id == newsId);

            if (findNews == null)
            {
                return NotFound();
            }

            var findNewsEditDto = mapper.Map<News, EditNewsDto>(findNews);

            return Ok(findNewsEditDto);
        }

        // GET: api/News/NewsByUser
        [HttpGet("NewsByUser/{userId}")]
        public IActionResult GetNewsByUser([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listFindNews = context.News
                .Include(news => news.User)
                .Include(news => news.Ratings)
                .Where(news => news.UserId == userId).ToList();

            if (!listFindNews.Any())
            {
                return NotFound();
            }

            var listFindNewsCardDto = mapper.Map<IEnumerable<News>, IEnumerable<NewsCardDto>>(listFindNews);

            return Ok(listFindNewsCardDto);
        }

        //// PUT: api/News/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews([FromRoute] int id, [FromBody] EditNewsDto editNewsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var editNews = await context.News
                .Include(news => news.NewsTags).ThenInclude(tag => tag.Tag)
                .Include(news => news.NewsCategories).ThenInclude(category => category.Category)
                .SingleOrDefaultAsync(news => news.Id == id);


            if (editNews == null)
            {
                return NotFound();
            }

            var listTag = new List<TagDto>();

            //todo check that newTag does`nt exist in database

            //input TagID = 0 denote that it`s a new tag and we need to add it to database
            var listNewTagDto = editNewsDto.Tags.Where(tagDto => tagDto.Id == 0).ToList();
            if (listNewTagDto.Any())
            {
                var listNewTags = mapper.Map<IEnumerable<TagDto>, IEnumerable<Tag>>(listNewTagDto);
                foreach (var newTag in listNewTags)
                {
                    newTag.CreatedAt = DateTime.Now;
                    newTag.ModifiedAt = newTag.CreatedAt;
                }
                await context.Tags.AddRangeAsync(listNewTags);
                await context.SaveChangesAsync();
                var listNewTag = mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(listNewTags);
                listTag.AddRange(listNewTag);
            }
            var listExitingTagDto = editNewsDto.Tags.Except(listNewTagDto);
            listTag.AddRange(listExitingTagDto);

            mapper.Map(editNewsDto, editNews);
            var newsTags = editNews.NewsTags.ToList();
            foreach (var tag in listTag)
            {
                newsTags.Add(new NewsTag()
                {
                    TagId = tag.Id

                });
            }

            editNews.NewsTags = newsTags;
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
          
            var listTag = new List<TagDto>();

            //input TagID = 0 denote that it`s a new tag and we need to add it to database
            var listNewTagDto = createNewsDto.Tags.Where(tagDto => tagDto.Id == 0).ToList();  
            if (listNewTagDto.Any())
            {
                var listNewTags = mapper.Map<IEnumerable<TagDto>,IEnumerable<Tag>>(listNewTagDto);
                foreach (var newTag in listNewTags)
                {
                    newTag.CreatedAt = DateTime.Now;
                    newTag.ModifiedAt = newTag.CreatedAt;
                }
                await context.Tags.AddRangeAsync(listNewTags);
                await context.SaveChangesAsync();
                var listNewTag= mapper.Map<IEnumerable<Tag>,IEnumerable<TagDto>>(listNewTags);
                listTag.AddRange(listNewTag);
            }
            var listExitingTagDto = createNewsDto.Tags.Except(listNewTagDto);
            listTag.AddRange(listExitingTagDto);

            var news = mapper.Map<CreateNewsDto, News>(createNewsDto);
            var newsTags = new List<NewsTag>();
            foreach (var tagDto in listTag)
            {
               newsTags.Add(new NewsTag()
               {
                   TagId = tagDto.Id
                   
               });  
            }

            news.NewsTags = newsTags;
           
            news.VisitorCount = 0;
            news.CreatedAt = DateTime.Now;
            news.ModifiedAt = news.CreatedAt;
            news.ModifiedBy = news.UserId;
            context.News.Add(news);
            await context.SaveChangesAsync();
            //return Ok();
            return CreatedAtAction("GetFullNews", new { id = news.Id });
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