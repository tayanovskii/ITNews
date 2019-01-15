using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.Data;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ITNews.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        public ApplicationDbContext _context { get; }

        public NewsController(ApplicationDbContext context) {
            this._context = context;
        }

        // GET api/news
        [HttpGet("")]
        public ActionResult<IEnumerable<string>> Gets()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/news/5
        [HttpGet("{id}")]
        public ActionResult<string> GetById(int id)
        {
            return "value" + id;
        }

        // POST api/news
        [HttpPost("")]
        public void Post([FromBody] string value) { }

        // PUT api/news/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE api/news/5
        [HttpDelete("{id}")]
        public void DeleteById(int id) { }
    }
}