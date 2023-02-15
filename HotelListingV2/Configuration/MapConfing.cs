using AutoMapper;
using HotelListingV2.Data;
using HotelListingV2.Models.Country;
using HotelListingV2.Models.Hotels;
using HotelListingV2.Models.Models;

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
            CreateMap<ApiUser,ApiUserDto>().ReverseMap();
        }
    }
}
