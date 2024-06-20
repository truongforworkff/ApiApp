namespace FptJobBack.Models
{
    public class JobPosting
    {

        public int JobPostingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PostedDate { get; set; }
        public int CategoryId { get; set; } // Mã ngành nghề
        public Category Category { get; set; } // Tham chiếu đến ngành nghề
        public int UserId { get; set; } // Mã người đăng
        public Users User { get; set; }
    }
}
