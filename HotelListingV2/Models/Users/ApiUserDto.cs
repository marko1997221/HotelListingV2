using HotelListingV2.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace HotelListingV2.Models.Models
{
    public class ApiUserDto:LogInDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
       
    }
}
