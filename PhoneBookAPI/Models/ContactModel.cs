using System.ComponentModel.DataAnnotations;

namespace PhoneBookAPI.Models
{
    public class ContactModel
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        [MaxLength(50)] 
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [MaxLength(100)] // Limits the Address length to 100 characters
        public string Address { get; set; }
    }
}
