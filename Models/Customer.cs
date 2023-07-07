using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Customer
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Range(18, int.MaxValue, ErrorMessage = "Age is not valid.")]
        public int Age { get; set; }
        [Required]
        public int Id { get; set; } 
    }
}
