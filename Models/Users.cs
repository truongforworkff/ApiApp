using System.ComponentModel.DataAnnotations;

namespace FptJobBack.Models
{
    public class Users 
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Lưu ý: Không nên lưu mật khẩu ở dạng plain text, hãy hash nó
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Role { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public ICollection<JobPosting> JobPostings { get; set; }
    }
}
