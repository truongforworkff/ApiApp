namespace FptJobBack.Models
{
    public class JobPostingDtoGet
    {
        public int JobPostingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PostedDate { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
    }
}
