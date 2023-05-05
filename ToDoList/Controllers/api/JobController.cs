using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobController(AppDbContext context)
        {
            _context = context;
        }

        //Get the list of tasks api/Jobs/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> Get()
        {
            return await _context.Jobs.Include(x=>x.JobPriority).Include(x=>x.JobStatus).ToListAsync();
        }

        // GET the task by id api/Jobs/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> Get(int id)
        {
            Job job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id);
            if (job == null)
                return NotFound();

            return new ObjectResult(job);
        }

        // POST api/Jobs
        [HttpPost]
        public async Task<ActionResult<Job>> Post(Job job)
        {
            if (job == null)
            {
                return BadRequest();
            }
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }

        // PUT api/Jobs
        [HttpPut]
        public async Task<ActionResult<Job>> Put(Job job)
        {
            if (job == null)
            {
                return BadRequest();
            }
            if (!await _context.Jobs.AnyAsync(x => x.Id == job.Id))
            {
                return NotFound();
            }
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }

        //DELETE api/Jobs/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Job>> Delete(int id)
        {
            Job job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }
    }
}
