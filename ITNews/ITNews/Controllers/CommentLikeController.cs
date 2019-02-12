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
using ITNews.DTO.CommentLikeDto;
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

        public CommentLikeController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/CommentLike/ByComment/commentId
        [HttpGet("ByComment/{commentId}")]
        public async Task<IActionResult> GetCommentLike([FromRoute] int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentLikeDto = new CommentLikeDto
            {
                CommentId = commentId,
                CountLike = await context.CommentLikes
                    .Where(like => like.CommentId == commentId).CountAsync()
            };

            return Ok(commentLikeDto);
        }


        // POST: api/CommentLike
        [HttpPost]
        public async Task<IActionResult> PostCommentLike([FromBody] CreateCommentLikeDto createCommentLikeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCommentLike = mapper.Map<CreateCommentLikeDto, CommentLike>(createCommentLikeDto);
            context.CommentLikes.Add(newCommentLike);
            await context.SaveChangesAsync();

            var commentLikeDto = new CommentLikeDto
            {
                CommentId = newCommentLike.CommentId,
                CountLike = await context.CommentLikes
                    .Where(like => like.CommentId == newCommentLike.CommentId).CountAsync()
            };

            return CreatedAtRoute(new {id = newCommentLike.Id}, commentLikeDto);
        }

        // DELETE: api/CommentLike/
        [HttpDelete]
        public async Task<IActionResult> DeleteCommentLike([FromBody] CreateCommentLikeDto createCommentLikeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentLike = await context.CommentLikes.SingleOrDefaultAsync(like =>
                like.CommentId == createCommentLikeDto.CommentId && like.UserId == createCommentLikeDto.UserId);
            if (commentLike == null)
            {
                return NotFound();
            }

            context.CommentLikes.Remove(commentLike);
            await context.SaveChangesAsync();

            return Ok(commentLike);
            //}

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


            //// DELETE: api/CommentLike/5
            //[HttpDelete("{id}")]
            //public async Task<IActionResult> DeleteCommentLike([FromRoute] int id)
            //{
            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }

            //    var commentLike = await context.CommentLikes.FindAsync(id);
            //    if (commentLike == null)
            //    {
            //        return NotFound();
            //    }

            //    context.CommentLikes.Remove(commentLike);
            //    await context.SaveChangesAsync();

            //    return Ok(commentLike);
            //}
        }
    }

}