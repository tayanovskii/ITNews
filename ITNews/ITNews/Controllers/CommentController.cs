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
using ITNews.DTO.CommentDto;
using ITNews.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ITNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private IHubContext<CommentHub> hubContext;
        private readonly IMapper mapper;

        public CommentController(ApplicationDbContext context, IHubContext<CommentHub> hubContext, IMapper mapper)
        {
            this.hubContext = hubContext;
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await context.Comments
                .Include(c => c.User)
                .SingleOrDefaultAsync(c => c.Id == id);
           
            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = mapper.Map<Comment,CommentDto>(comment);   
            return Ok(commentDto);
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment([FromRoute] int id, [FromBody] EditCommentDto editCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var editComment = await context.Comments.SingleOrDefaultAsync(comment => comment.Id == id);

            if (id != editComment.Id)
            {
                return BadRequest();
            }

            mapper.Map(editCommentDto, editComment);
            editComment.ModifiedAt = DateTime.Now;
            context.Entry(editComment).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
                }
            }
            context.Entry(editComment).Reference(c => c.User).Load();
            var commentDtoFromDataBase = mapper.Map<Comment, CommentDto>(editComment);
            await hubContext.Clients.Group(commentDtoFromDataBase.NewsId.ToString()).SendAsync("EditComment", commentDtoFromDataBase);
            return NoContent();
        }

        // POST: api/Comment
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = mapper.Map<CreateCommentDto, Comment>(createCommentDto);
            comment.CreatedAt = DateTime.Now;
            comment.ModifiedAt = comment.CreatedAt;
            comment.ModifiedBy = comment.UserId;
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            
            context.Entry(comment).Reference(c => c.User).Load();
                   
            var commentDtoFromDataBase = mapper.Map<Comment,CommentDto>(comment);

            await hubContext.Clients.Group(commentDtoFromDataBase.NewsId.ToString()).SendAsync("AddComment", commentDtoFromDataBase);

            return CreatedAtRoute(new { id = comment.Id }, commentDtoFromDataBase);
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            return Ok(comment);
        }

        private bool CommentExists(int id)
        {
            return context.Comments.Any(e => e.Id == id);
        }
    }
}