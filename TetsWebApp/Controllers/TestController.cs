using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TetsWebApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TetsWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public static int GetNewId() => id++;
        private static int id = 1;

        public static List<TestEntity> List = new List<TestEntity>()
        {
            new TestEntity
            {
                Id = GetNewId(),
                Name = "New Entity",
                Created = DateTime.Now
            }
        };


        // GET: api/<TestController>
        [HttpGet]
        public IEnumerable<TestEntity> Get()
        {
            return List;
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestEntity>> Get(int id)
        {
            var entity = List.FirstOrDefault(x => x.Id == id);
            if (entity is null)
                return NotFound();

            return entity;
        }

        // POST api/<TestController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] TestEntity value)
        {
            value.Id = GetNewId();
            value.Created = DateTime.Now;
            List.Add(value);
            return value.Id;
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TestEntity value)
        {
            if(id != value.Id)
                return BadRequest();

            if (!List.Any(x => x.Id == id))
                return NotFound();

            List.RemoveAll(x => x.Id == id);
            List.Add(value);

            return NoContent();
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!List.Any(x => x.Id == id))
                return NotFound();

            List.RemoveAll(x => x.Id == id);

            return NoContent();
        }
    }
}
