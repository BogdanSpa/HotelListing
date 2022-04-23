using AutoMapper;
using HotelListing.Data;
using HotelListing.DTO;
using HotelListing.IRepository;
using HotelListing.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        //here we override the global caching for example purpose only
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = true)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {

            var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
            var result = _mapper.Map<IList<CountryDTO>>(countries);
            return Ok(result);

        }

        [ResponseCache(Duration = 60)]
        [HttpGet]
        [Route("Countries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCountries()
        {

            var countries = await _unitOfWork.Countries.GetAll();
            var result = _mapper.Map<IList<CountryDTO>>(countries);
            return Ok(result);

        }


        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {

            var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
            var result = _mapper.Map<CountryDTO>(country);
            return Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Invalid POST attempt in {nameof(CreateCountry)}");
                return StatusCode(500, "Internal Server Error! Please try again later!");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            //We have error handling globally so we dont need try catch, the app automatically use try and when
            //an exception occurs it will throw automatically the exception.
            //We did that as a service extension in ServiceExtenstion with ConfigureExceptionHandler();
            //bellow is an example how the code would look without it(commented).
            // try
            // {

            var country = await _unitOfWork.Countries.Get(q => q.Id == id);

            if (country == null)
            {
                _logger.LogError($"Invalid PUT attempt in {nameof(UpdateCountry)}");
                return BadRequest("Submited data is invalid");
            }

            //var updateHotel = _mapper.Map<Hotel>(hotelDTO);
            //updateHotel.Id = id;

            _mapper.Map(countryDTO, country);
            _unitOfWork.Countries.Update(country);
            _unitOfWork.Save();

            return NoContent();
            //}
            //catch (Exception ex)
            //{

            //    _logger.LogError(ex, $"Invalid PUT attempt in {nameof(UpdateCountry)}");
            //    return StatusCode(500, "Internal Server Error! Please try again later!");
            //}

        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest("Invalid country id");
            }


            var country = await _unitOfWork.Hotels.Get(q => q.Id == id);

            if (country == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest("Invalid hotel id");
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();

            return NoContent();

        }
    }
}
