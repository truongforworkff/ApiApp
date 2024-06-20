using AutoMapper;
using FptJobBack.Data;
using FptJobBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FptJobBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostingsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobPostingsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobPostingDtoGet>>> GetAllJobPostings()
        {
            var jobPostings = await _context.JobPostings
                .Include(j => j.Category)
                .Include(j => j.User)
                .ToListAsync();

            var jobPostingsDto = jobPostings.Select(j => new JobPostingDtoGet
            {
                JobPostingId = j.JobPostingId,
                Title = j.Title,
                Description = j.Description,
                PostedDate = j.PostedDate,
                CategoryId = j.CategoryId,
                Category = j.Category != null ? new CategoryDto
                {
                    CategoryId = j.Category.CategoryId,
                    Name = j.Category.Name
                } : null,
                UserId = j.UserId,
                User = j.User != null ? new UserDto
                {
                    UserId = j.User.UserId,
                    Username = j.User.Username,
                    Email = j.User.Email,
                    DateCreated = j.User.DateCreated,
                    LastLogin = j.User.LastLogin,
                    Role = j.User.Role,

                } : null
            }).ToList();

            return jobPostingsDto;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JobPostingDtoGet>> GetJobPosting(int id)
        {
            var jobPosting = await _context.JobPostings
                .Include(j => j.Category)
                .Include(j => j.User)
                .FirstOrDefaultAsync(j => j.JobPostingId == id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            var jobPostingDto = new JobPostingDtoGet
            {
                JobPostingId = jobPosting.JobPostingId,
                Title = jobPosting.Title,
                Description = jobPosting.Description,
                PostedDate = jobPosting.PostedDate,
                CategoryId = jobPosting.CategoryId,
                Category = new CategoryDto
                {
                    CategoryId = jobPosting.Category.CategoryId,
                    Name = jobPosting.Category.Name
                    // Thêm các thuộc tính khác của CategoryDto nếu cần thiết
                },
                UserId = jobPosting.UserId,
                User = new UserDto
                {
                    UserId = jobPosting.User.UserId,
                    Username = jobPosting.User.Username,
                    Email = jobPosting.User.Email,
                    DateCreated = jobPosting.User.DateCreated,
                    LastLogin = jobPosting.User.LastLogin,
                    Role = jobPosting.User.Role,
                    // Thêm các thuộc tính khác của UserDto nếu cần thiết
                }
            };

            return jobPostingDto;
        }

        [HttpPost]
        public async Task<IActionResult> PostJobPosting(JobPostingDto jobPostingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new JobPosting object
            var jobPosting = new JobPosting
            {
                Title = jobPostingDto.Title,
                Description = jobPostingDto.Description,
                PostedDate = DateTime.UtcNow,
                CategoryId = jobPostingDto.CategoryId,
                UserId = jobPostingDto.UserId
            };

            _context.JobPostings.Add(jobPosting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobPosting", new { id = jobPosting.JobPostingId }, jobPosting);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobPosting(int id, JobPostingDto jobPostingDto)
        {
            if (id != jobPostingDto.JobPostingId)
            {
                return BadRequest("JobPostingId mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jobPosting = await _context.JobPostings.FindAsync(id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            // Update properties of the jobPosting entity
            jobPosting.Title = jobPostingDto.Title;
            jobPosting.Description = jobPostingDto.Description;
            jobPosting.CategoryId = jobPostingDto.CategoryId;

            // Check and update UserId if needed
            if (jobPosting.UserId != jobPostingDto.UserId)
            {
                jobPosting.UserId = jobPostingDto.UserId;
            }

            // Set the entity state to Modified to ensure it updates in the database
            _context.Entry(jobPosting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobPostingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // DELETE: api/JobPostings/{id}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobPosting(int id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }

            _context.JobPostings.Remove(jobPosting);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool JobPostingExists(int id)
        {
            return _context.JobPostings.Any(e => e.JobPostingId == id);
        }


    }






}
