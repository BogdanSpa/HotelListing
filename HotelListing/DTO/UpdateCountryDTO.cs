namespace HotelListing.DTO
{
    public class UpdateCountryDTO: CreateCountryDTO
    {
        public IList<CreateHotelDTO> Hotels { get; set; }
    }
}
