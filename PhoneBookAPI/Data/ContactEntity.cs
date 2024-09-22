using System.ComponentModel.DataAnnotations;

namespace PhoneBookAPI.Data
{
    public class ContactEntity
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

        [MaxLength(100)] 
        public string? Address { get; set; }
    }
}
