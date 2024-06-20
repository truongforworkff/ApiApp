using System.ComponentModel.DataAnnotations;

namespace FptJobBack.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public JobPosting JobPosting { get; set; }
    }
}
