using AutoMapper;
using CTS_BE.DTOs;
using System.Data.Common;

namespace CTS_BE.Helper
{
    public class MapperClass : Profile
    {
        public MapperClass()
        {
            CreateMap<IDictionary<string, object>, BillsListDTO>()
            .ForMember(dest => dest.BillId, opt => opt.MapFrom(src => src["bill_id"]))
            .ForMember(dest => dest.DdoCode, opt => opt.MapFrom(src => src["ddo_code"]))
            .ForMember(dest => dest.DdoDesignation, opt => opt.MapFrom(src => src["designation"]))
            .ForMember(dest => dest.BillNo, opt => opt.MapFrom(src => src["bill_no"]))
            .ForMember(dest => dest.BillDate, opt => opt.MapFrom(src => src["bill_date"]));
            
        }
    }
}
