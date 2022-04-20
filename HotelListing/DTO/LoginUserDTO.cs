using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTO
{
    public class LoginUserDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = $"Password must have at least 8 characters", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
