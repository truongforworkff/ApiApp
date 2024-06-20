namespace FptJobBack.Models
{
    public class JobPostingDto
    {
        public int JobPostingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }


    }
}
