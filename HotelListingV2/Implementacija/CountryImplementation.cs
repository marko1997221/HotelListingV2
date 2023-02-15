using HotelListingV2.Data;
using HotelListingV2.Interfejsi;

namespace HotelListingV2.Implementacija
{
    public class CountryImplementation : GenericImplementation<Country> , ICountyInterface
    {
        public CountryImplementation(HotelListingDbContext context) : base(context)
        {
        }
    }
}
