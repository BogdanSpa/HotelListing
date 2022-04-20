using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTO
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = $"Password must have at least 8 characters", MinimumLength = 8)]
        public string Password { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
