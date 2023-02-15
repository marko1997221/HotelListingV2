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

        public AccountController(IAuthMenager authMenager)
        {
            this.authMenager = authMenager;
        }

        // api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody]ApiUserDto apiUserDto)
        {
            var errors =  await authMenager.Register(apiUserDto);
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

        // api/Account/register
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LogInDto apiUserDto)
        {
            var isValid = await authMenager.LogIn(apiUserDto);
            if (!isValid)
            {
                return Unauthorized();
            }
            return Ok();
        }
    }
}
