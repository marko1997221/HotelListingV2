using HotelListingV2.Models.Models;
using HotelListingV2.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListingV2.Interfejsi
{
    public interface IAuthMenager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto);
        Task<bool> LogIn(LogInDto logInDto);
    }
}
