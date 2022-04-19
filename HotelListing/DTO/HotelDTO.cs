using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTO
{
    public class HotelDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country name too long")]
        public string Address { get; set; }
        [Required]
        [Range(1,5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
        public CountryDTO Country { get; set; }
    }
}
