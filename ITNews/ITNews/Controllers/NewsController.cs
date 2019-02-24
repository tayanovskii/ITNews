using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITNews.Data;
using ITNews.Data.Entities;
using ITNews.DTO;
using ITNews.DTO.NewsDto;
using ITNews.Services.News;
using ITNews.Services.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Nest;
using static System.String;

namespace ITNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ITagService tagService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INewsService newsService;
        private readonly IElasticClient elasticClient;

        public NewsController(ApplicationDbContext context, IMapper mapper, ITagService tagService,
            UserManager<ApplicationUser> userManager, INewsService newsService, IElasticClient elasticClient)
        {
            this.context = context;
            this.mapper = mapper;
            this.tagService = tagService;
            this.userManager = userManager;
            this.newsService = newsService;
            this.elasticClient = elasticClient;
        }

        // GET: api/News
        [HttpGet("listCardNews")]
        [AllowAnonymous]
        public IEnumerable<NewsCardDto> GetCardNews()
        {
            var listNews = context.News.Include(news => news.User)
                .ThenInclude(user => user.CommentLikes)
                .Include(news => news.NewsTags)
                .ThenInclude(tag => tag.Tag)
                .Include(news => news.NewsCategories)
                .ThenInclude(category => category.Category)
                .Include(news => news.Comments)
                .ThenInclude(comment => comment.Likes)
                .Include(news => news.Ratings);
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

            var findNews = await context.News.Include(news => news.User)
                .ThenInclude(user => user.CommentLikes)
                .Include(news => news.Comments)
                .ThenInclude(comment => comment.Likes)
                .Include(news => news.Ratings)
                .Include(news => news.NewsCategories)
                .ThenInclude(category => category.Category)
                .Include(news => news.NewsTags)
                .ThenInclude(tag => tag.Tag)
                .SingleOrDefaultAsync(news => news.Id == id);

            if (findNews == null)
            {
                return NotFound();
            }

            findNews.VisitorCount++;
            await context.SaveChangesAsync();

            var findNewsDto = mapper.Map<News, FullNewsDto>(findNews);
            findNewsDto.IsNewsRatedByUser = false;

            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var userRatingsByNews = findNews.Ratings.FirstOrDefault(rating => rating.UserId == currentUser.Id);
                if (userRatingsByNews != null) findNewsDto.IsNewsRatedByUser = true;
                var commentLikesByCurrentUser = findNews.Comments
                    .Where(comment => comment.Likes.Any(like => like.UserId == currentUser.Id))
                    .Select(comment => comment.Id);
                findNewsDto.CommentsLikedByUser = commentLikesByCurrentUser;
            }

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

            var findNews = await context.News.Include(news => news.NewsTags)
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

        // GET: api/News/ByUser
        [HttpGet("ByUser/{userId}")]
        public IActionResult GetNewsByUser([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listFindNews = context.News.Include(news => news.User)
                .ThenInclude(user => user.CommentLikes)
                .Include(news => news.Comments)
                .ThenInclude(comment => comment.Likes)
                .Include(news => news.Ratings)
                .Include(news => news.NewsCategories)
                .ThenInclude(category => category.Category)
                .Include(news => news.NewsTags)
                .ThenInclude(tag => tag.Tag)
                .Where(news => news.UserId == userId);

            var listFindNewsCardDto = mapper.Map<IEnumerable<News>, IEnumerable<NewsCardDto>>(listFindNews);

            return Ok(listFindNewsCardDto);
        }

        // GET: api/News/ByTag
        [HttpGet("ByTag/{tagId}")]
        public IActionResult GetNewsByTad([FromRoute] int tagId)
        {
            var listFindNews = context.News.Include(news => news.User)
                .ThenInclude(user => user.CommentLikes)
                .Include(news => news.Comments)
                .ThenInclude(comment => comment.Likes)
                .Include(news => news.Ratings)
                .Include(news => news.NewsCategories)
                .ThenInclude(category => category.Category)
                .Include(news => news.NewsTags)
                .ThenInclude(tag => tag.Tag)
                .Where(news => news.NewsTags.Any(tag => tag.TagId == tagId));

            var listFindNewsCardDto = mapper.Map<IEnumerable<News>, IEnumerable<NewsCardDto>>(listFindNews);

            return Ok(listFindNewsCardDto);
        }

        // GET: api/News/ByCategory/categoryId
        [HttpGet("ByCategory/{categoryId}")]
        public IActionResult GetNewsByCategory([FromRoute] int categoryId)
        {
            var listFindNews = context.News.Include(news => news.User)
                .ThenInclude(user => user.CommentLikes)
                .Include(news => news.Comments)
                .ThenInclude(comment => comment.Likes)
                .Include(news => news.Ratings)
                .Include(news => news.NewsCategories)
                .ThenInclude(category => category.Category)
                .Include(news => news.NewsTags)
                .ThenInclude(tag => tag.Tag)
                .Where(news => news.NewsCategories.Any(category => category.CategoryId == categoryId));

            var listFindNewsCardDto = mapper.Map<IEnumerable<News>, IEnumerable<NewsCardDto>>(listFindNews);

            return Ok(listFindNewsCardDto);
        }

        [HttpGet]
        public async Task<QueryResultDto<NewsCardDto>> GetNewsQuery([FromQuery] NewsQueryDto filterResource)
        {
            var filter = mapper.Map<NewsQueryDto, NewsQuery>(filterResource);
            var queryResult = await newsService.GetNewsAsync(filter);

            return mapper.Map<QueryResult<News>, QueryResultDto<NewsCardDto>>(queryResult);
        }

        //[Route("/News/search")]
        [HttpGet("search")]
        public async Task<IActionResult> Find([FromQuery] NewsSearchQueryDto newsQuery)
        {
            if (newsQuery.Page <= 0)
                newsQuery.Page = 1;

            if (newsQuery.PageSize <= 0)
                newsQuery.PageSize = 10;

            Expression<Func<SearchDescriptor<News>, ISearchRequest>> searchExpression = s =>
                 s.Query(q => q.QueryString(d => d.Query(newsQuery.Query)))
                     .From((newsQuery.Page - 1) * newsQuery.PageSize)
                     .Size(newsQuery.PageSize);

            Expression<Func<News, object>> sortField = news => news.CreatedAt;

            if (!IsNullOrEmpty(newsQuery.SortBy))
            {
                if (newsQuery.IsSortAscending)
                {

                    searchExpression = s =>
                        s.Query(q => q.QueryString(d => d.Query(newsQuery.Query)))
                            .From((newsQuery.Page - 1) * newsQuery.PageSize)
                            .Size(newsQuery.PageSize).Sort(sortDescriptor =>
                        sortDescriptor.Field(fieldDescriptor =>
                            fieldDescriptor.Field(sortField).Order(SortOrder.Ascending)));
                }
                else
                {
                    searchExpression = s =>
                        s.Query(q => q.QueryString(d => d.Query(newsQuery.Query)))
                            .From((newsQuery.Page - 1) * newsQuery.PageSize)
                            .Size(newsQuery.PageSize).Sort(sortDescriptor =>
                                sortDescriptor.Field(fieldDescriptor =>
                                    fieldDescriptor.Field(sortField).Order(SortOrder.Descending)));
                }
            }

            var response = await elasticClient.SearchAsync(
                searchExpression.Compile());

            return Ok(response.Documents);
        }

        // PUT: api/News/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews([FromRoute] int id, [FromBody] EditNewsDto editNewsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var editNews = await context.News.Include(news => news.NewsTags)
                .ThenInclude(tag => tag.Tag)
                .Include(news => news.NewsCategories)
                .ThenInclude(category => category.Category)
                .SingleOrDefaultAsync(news => news.Id == id);

            if (editNews == null)
            {
                return NotFound();
            }

            var addedTagsDto = tagService.AddTags(editNewsDto.Tags).Result;
            editNewsDto.Tags = addedTagsDto;

            mapper.Map(editNewsDto, editNews);
            var newsTags = editNews.NewsTags.ToList();
            foreach (var tag in addedTagsDto)
            {
                newsTags.Add(new NewsTag() {TagId = tag.Id});
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
                    return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
                }
            }

            await elasticClient.UpdateAsync<News>(editNews, u => u.Doc(editNews));

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

            var addedTagsDto = tagService.AddTags(createNewsDto.Tags).Result;
            createNewsDto.Tags = addedTagsDto;

            var news = mapper.Map<CreateNewsDto, News>(createNewsDto);
            var newsTags = new List<NewsTag>();
            foreach (var tagDto in addedTagsDto)
            {
                newsTags.Add(new NewsTag() {TagId = tagDto.Id});
            }

            news.NewsTags = newsTags;
            news.VisitorCount = 0;
            news.CreatedAt = DateTime.Now;
            news.ModifiedAt = news.CreatedAt;
            news.ModifiedBy = news.UserId;
            context.News.Add(news);
            await context.SaveChangesAsync();
            await elasticClient.IndexDocumentAsync(news);
            //return Ok();
            return CreatedAtAction("GetFullNews", new {id = news.Id});
        }

        [HttpGet("search/reindex")]
        public async Task<IActionResult> ReIndex()
        {
            await elasticClient.DeleteByQueryAsync<News>(q => q.MatchAll());

            var allNews = await context.News.ToListAsync();

            foreach (var news in allNews)
            {
                await elasticClient.IndexDocumentAsync(news);
            }

            return Ok($"{allNews.Count} news reindexed");
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deletedNews = await context.News.FindAsync(id);
            if (deletedNews == null)
            {
                return NotFound();
            }

            //context.News.Remove(news);
            //await context.SaveChangesAsync();
            deletedNews.SoftDeleted = true;
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
            await elasticClient.DeleteAsync<News>(deletedNews);
            return Ok(deletedNews.Id);
        }

        private bool NewsExists(int id)
        {
            return context.News.Any(e => e.Id == id);
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       