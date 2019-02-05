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
using ITNews.DTO.Rating;
using ITNews.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ITNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private IHubContext<RatingHub> hubContext;
        private readonly IMapper mapper;
        public RatingController(ApplicationDbContext context, IMapper mapper, IHubContext<RatingHub> hubContext)
        {
            this.context = context;
            this.mapper = mapper;
            this.hubContext = hubContext;
        }

        // GET: api/Rating/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRating([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rating = await context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            var ratingDto = mapper.Map<Rating,RatingDto>(rating);
            return Ok(ratingDto);
        }

        //// PUT: api/Rating/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRating([FromRoute] int id, [FromBody] Rating rating)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != rating.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(rating).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RatingExists(id))
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

        // POST: api/Rating
        [HttpPost]
        public async Task<IActionResult> PostRating([FromBody] CreateRatingDto createRatingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newRating = mapper.Map<CreateRatingDto, Rating>(createRatingDto);   
            context.Ratings.Add(newRating);
            await context.SaveChangesAsync();

            var ratingDto = new RatingDto {NewsId = newRating.NewsId};
            var listRatingsByNews =context.Ratings.Where(rating => rating.NewsId == newRating.NewsId);
            ratingDto.Rating = await listRatingsByNews.AverageAsync(rating => rating.Value);
            ratingDto.RatingCount = await listRatingsByNews.CountAsync();

            await hubContext.Clients.All.SendAsync("AddRating", ratingDto);
            return CreatedAtRoute(new { id = newRating.Id }, ratingDto);
        }

        // DELETE: api/Rating/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rating = await context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            context.Ratings.Remove(rating);
            await context.SaveChangesAsync();

            return Ok(rating);
        }

        private bool RatingExists(int id)
        {
            return context.Ratings.Any(e => e.Id == id);
        }
    }
}