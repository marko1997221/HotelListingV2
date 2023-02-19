using HotelListingV2.Interfejsi;
using HotelListingV2.Models.Models;
using HotelListingV2.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthMenager authMenager;
        private readonly ILogger<AccountController> logger;

        public AccountController(IAuthMenager authMenager, ILogger<AccountController> logger)
        {
            this.authMenager = authMenager;
            this.logger = logger;
        }

        // api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody]ApiUserDto apiUserDto)
        {
            logger.LogInformation($"Registration attempt for{apiUserDto.Email}");
            var errors =  await authMenager.Register(apiUserDto);
            try
            {
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"$Something went wrong in the{nameof(Register)} - user registration attempt for {apiUserDto.Email}");
                return Problem($"Something went wrong in {nameof(Register)}. Please contact support", statusCode: 500);
            }
           
        }

        // api/Account/register
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LogInDto apiUserDto)
        {
            logger.LogInformation($"Attempting to login user {apiUserDto.Email}");
            try
            {
                var authResponce = await authMenager.LogIn(apiUserDto);
                if (authResponce == null)
                {
                    return Unauthorized();
                }
                return Ok(authResponce);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, $"$Something went wrong in the{nameof(Login)} - user registration attempt for {apiUserDto.Email}");
                return Problem($"Something went wrong in {nameof(Login)}. Please contact support", statusCode: 500);
            }
            
        }
        [HttpPost]
        [Route("Refresh")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Refresh([FromBody] AuthResponseDto request)
        {
            var authResponce = await authMenager.VerifyRefreshToken(request);
            if (authResponce == null)
            {
                return Unauthorized();
            }
            return Ok(authResponce);
        }
    }
}
