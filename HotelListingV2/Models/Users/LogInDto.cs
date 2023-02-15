using System.ComponentModel.DataAnnotations;

namespace HotelListingV2.Models.Users
{
    public class LogInDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
