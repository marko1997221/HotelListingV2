using HotelListingV2.Data;
using HotelListingV2.Interfejsi;

namespace HotelListingV2.Implementacija
{
    public class HotelImplementacija : GenericImplementation<Hotel> , IHotelInterface
    {
        public HotelImplementacija(HotelListingDbContext context) : base(context)
        {
        }
    }
}
