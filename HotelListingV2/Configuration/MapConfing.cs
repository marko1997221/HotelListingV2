using AutoMapper;
using HotelListingV2.Data;
using HotelListingV2.Models.Country;
using HotelListingV2.Models.Hotels;

namespace HotelListingV2.Configuration

{
    public class MapConfing: Profile
    {
        public MapConfing()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, GetHotelsDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelsDto>().ReverseMap();
        }
    }
}
