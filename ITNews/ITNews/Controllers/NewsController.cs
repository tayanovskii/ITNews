using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITNews.Data;
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
        //[HttpGet]
        //public IEnumerable<CreateNewsDto> GetAllNews()
        //{
        //    return
        //    //return context.NewsViewModel;
        //}

        //// GET: api/News/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetNewsViewModel([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var newsViewModel = await context.NewsViewModel.FindAsync(id);

        //    if (newsViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(newsViewModel);
        //}

        //[Authorize]
        //// PUT: api/News/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutNewsViewModel([FromRoute] int id, [FromBody] CreateNewsDto newsViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != newsViewModel.Id)
        //    {
        //        return BadRequest();
        //    }

        //    context.Entry(newsViewModel).State = EntityState.Modified;

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!NewsViewModelExists(id))
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

        //[Authorize]
        //// POST: api/News
        //[HttpPost]
        //public async Task<IActionResult> PostNewsViewModel([FromBody] CreateNewsDto newsViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    context.NewsViewModel.Add(newsViewModel);
        //    await context.SaveChangesAsync();

        //    return CreatedAtAction("GetNewsViewModel", new { id = newsViewModel.Id }, newsViewModel);
        //}

        //[Authorize]
        //// DELETE: api/News/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteNewsViewModel([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var newsViewModel = await context.NewsViewModel.FindAsync(id);
        //    if (newsViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    context.NewsViewModel.Remove(newsViewModel);
        //    await context.SaveChangesAsync();

        //    return Ok(newsViewModel);
        //}

        //private bool NewsViewModelExists(int id)
        //{
        //    return context.NewsViewModel.Any(e => e.Id == id);
        //}
    }
}