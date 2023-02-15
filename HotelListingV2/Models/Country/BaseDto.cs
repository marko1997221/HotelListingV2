using System.ComponentModel.DataAnnotations;

namespace HotelListingV2.Models.Country
{
    public abstract class BaseDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}