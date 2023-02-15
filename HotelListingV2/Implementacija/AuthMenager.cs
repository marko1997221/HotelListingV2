using AutoMapper;
using HotelListingV2.Data;
using HotelListingV2.Interfejsi;
using HotelListingV2.Models.Models;
using HotelListingV2.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListingV2.Implementacija
{
    public class AuthMenager : IAuthMenager
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;

        public AuthMenager(IMapper mapper, UserManager<ApiUser> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<bool> LogIn(LogInDto logInDto)
        {
            bool isValid = false;
            var user = await userManager.FindByNameAsync(logInDto.Email);
            try
            {
                var validPassword = await userManager.CheckPasswordAsync(user, logInDto.Password);
                isValid= validPassword;
            }
            catch (Exception)
            {

            }
            
            return isValid;
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto)
        {
            var user= mapper.Map<ApiUser>(apiUserDto);
            user.UserName = apiUserDto.Email;
            
            var message=await userManager.CreateAsync(user,apiUserDto.Password);
            if (message.Succeeded)
    {
                await userManager.AddToRoleAsync(user,"User");
            }
            return message.Errors;
        }
    }
}
