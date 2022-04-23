using HotelListing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//This Controller is made to demonstrate the versioning and doesn't have any importance
namespace HotelListing.Controllers
{
    //We changed the route to be the same as CountryController to see the effects
    //But we also added apiversion so we can request the version  in our requests using parameteres
    [ApiVersion("2.0", Deprecated = true)]
    //[Route("api/country")]
    [Route("api/[controller]")]
    //using this route we can ask for the version like this: api/2.0/country; this is like a new route.
   // [Route("api/{v:apiversion}/country")]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private DatabaseContext _db;

        public CountryV2Controller(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(_db.Countries);
        }
    }
}
