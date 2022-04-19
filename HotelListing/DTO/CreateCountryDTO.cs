using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTO
{
    //Country doesn't need Id when added to db
    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country name too long")]
        public string Name { get; set; }
    }
}
