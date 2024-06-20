namespace FptJobBack.Models
{
    public class UserDto
    {

        public int UserId { get; set; }
        public string Username { get; set; }
       
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Role { get; set; }

        public UserProfileCreateDto UserProfile { get; set; }
        public List<string> JobPostings { get; set; }
    }
}
