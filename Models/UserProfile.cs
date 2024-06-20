namespace FptJobBack.Models
{
    public class UserProfile
    {

        public int ProfileId { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
