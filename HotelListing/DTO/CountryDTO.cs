using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTO
{
    public class CountryDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country name too long")]
        public string Name { get; set; }
        public IList<HotelDTO> Hotels { get; set; }
    }

    
}
