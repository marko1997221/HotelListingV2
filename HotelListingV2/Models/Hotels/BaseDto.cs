using System.ComponentModel.DataAnnotations;

namespace HotelListingV2.Models.Hotels
{
    public abstract class BaseDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public double Rating { get; set; }
    }
}