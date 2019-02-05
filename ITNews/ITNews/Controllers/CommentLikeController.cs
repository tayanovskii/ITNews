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
using ITNews.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ITNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentLikeController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private IHubContext<CommentLikeHub> hubContext;

        public CommentLikeController(ApplicationDbContext context, IMapper mapper, IHubContext<CommentLikeHub> hubContext)
        {
            this.context = context;
            this.mapper = mapper;
            this.hubContext = hubContext;
        }

        // GET: api/CommentLike/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentLike([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentLike = await context.CommentLikes.FindAsync(id);
            if (commentLike == null)
            {
                return NotFound();
            }
            var commentLikeDto = mapper.Map<CommentLike,CommentLikeDto>(commentLike);
            return Ok(commentLikeDto);
        }


        // POST: api/CommentLike
        [HttpPost]
        public async Task<IActionResult> PostCommentLike([FromBody] CommentLikeDto commentLikeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCommentLike = mapper.Map<CommentLikeDto,CommentLike>(commentLikeDto);
            context.CommentLikes.Add(newCommentLike);
            await context.SaveChangesAsync();
            await hubContext.Clients.Group(newCommentLike.Id.ToString()).SendAsync("AddCommentLike");
            return CreatedAtRoute( new { id = newCommentLike.Id }, commentLikeDto);
        }

        //// PUT: api/CommentLike/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCommentLike([FromRoute] int id, [FromBody] CommentLike commentLike)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != commentLike.Id)
        //    {
        //        return BadRequest();
        //    }

        //    context.Entry(commentLike).State = EntityState.Modified;

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CommentLikeExists(id))
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

        // DELETE: api/CommentLike/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentLike([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentLike = await context.CommentLikes.FindAsync(id);
            if (commentLike == null)
            {
                return NotFound();
            }

            context.CommentLikes.Remove(commentLike);
            await context.SaveChangesAsync();

            return Ok(commentLike);
        }

        private bool CommentLikeExists(int id)
        {
            return context.CommentLikes.Any(e => e.Id == id);
        }
    }
}