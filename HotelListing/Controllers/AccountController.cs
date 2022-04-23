using AutoMapper;
using HotelListing.Data;
using HotelListing.DTO;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        public AccountController(UserManager<ApiUser> userManager,
            ILogger<AccountController> logger, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration attempt for user {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<ApiUser>(userDTO);
            user.UserName = userDTO.Email;
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
            {
                //could be sensitive data
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState); //It might show sensitive data, did it for debugging purpose
            }

            await _userManager.AddToRolesAsync(user, userDTO.Roles);
            return Ok();

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"Login attempt for user {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (!await _authManager.ValidateUser(userDTO))
            {
                return Unauthorized(userDTO);
            }

            return Accepted(new { Token = await _authManager.CreateToken() });

        }
    }
}
