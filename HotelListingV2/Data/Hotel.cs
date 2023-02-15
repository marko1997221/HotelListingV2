using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingV2.Data
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }
    }
}
