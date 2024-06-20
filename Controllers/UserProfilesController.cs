using AutoMapper;
using FptJobBack.Data;
using FptJobBack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FptJobBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public UserProfilesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/UserProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileCreateDto>>> GetUserProfiles()
        {
            var userProfiles = await _context.UserProfiles
                                             .Include(up => up.User)
                                             .ToListAsync();

            var userProfileDtos = _mapper.Map<List<UserProfileCreateDto>>(userProfiles);

            return Ok(userProfileDtos);
        }

        // GET: api/UserProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileCreateDto>> GetUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles
                                            .Include(up => up.User)
                                            .FirstOrDefaultAsync(up => up.ProfileId == id);

            if (userProfile == null)
            {
                return NotFound();
            }

            var userProfileDto = _mapper.Map<UserProfileCreateDto>(userProfile);
            return userProfileDto;
        }


        // GET: api/UserProfiles/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<UserProfileCreateDto>>> GetUserProfilesByUserId(int userId)
        {
            var userProfiles = await _context.UserProfiles
                                             .Include(up => up.User) // Include User navigation property
                                             .Where(up => up.UserId == userId)
                                             .ToListAsync();

            if (!userProfiles.Any())
            {
                return NotFound();
            }

            var userProfileDtos = _mapper.Map<IEnumerable<UserProfileCreateDto>>(userProfiles);
            return Ok(userProfileDtos);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, UserProfileUpdateDto userProfileDto)
        {
            if (id != userProfileDto.ProfileId)
            {
                return BadRequest("Profile ID mismatch");
            }

            var userProfile = await _context.UserProfiles
                                            .FirstOrDefaultAsync(up => up.ProfileId == id);

            if (userProfile == null)
            {
                return NotFound();
            }

            // Update properties of userProfile with values from userProfileDto
            userProfile.FullName = userProfileDto.FullName;
            userProfile.ContactNumber = userProfileDto.ContactNumber;
            userProfile.Address = userProfileDto.Address;
            userProfile.Education = userProfileDto.Education;
            userProfile.Experience = userProfileDto.Experience;
            userProfile.Skills = userProfileDto.Skills;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // HTTP 204 No Content
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception if needed
                return StatusCode(500, "Failed to update the profile. Please try again later.");
            }
        }




        // POST: api/UserProfiles

        [HttpPost]
        public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfileCreateDto userProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userProfile = new UserProfile
            {
                FullName = userProfileDto.FullName,
                ContactNumber = userProfileDto.ContactNumber,
                Address = userProfileDto.Address,
                Education = userProfileDto.Education,
                Experience = userProfileDto.Experience,
                Skills = userProfileDto.Skills,
                UserId = userProfileDto.UserId
            };

//            {
//                "fullName": "John Doe",
//    "contactNumber": "123-456-7890",
//    "address": "123 Main St",
//    "education": "B.Sc. in Computer Science",
//    "experience": "5 years",
//    "skills": "C#, ASP.NET Core",
//    "userId": 1
//}




            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserProfile", new { id = userProfile.ProfileId }, userProfile);
        }

        // DELETE: api/UserProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            try
            {
                _context.UserProfiles.Remove(userProfile);
                await _context.SaveChangesAsync();
                return NoContent(); // HTTP 204 No Content
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to delete the profile. Please try again later.");
            }
        }


        
    }
}
