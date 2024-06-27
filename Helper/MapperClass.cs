﻿using AutoMapper;
using CTS_BE.DAL.Entities;
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

            CreateMap<StampLabelMaster, StampLabelMasterDTO>();
            CreateMap<StampLabelMasterInsertDTO, StampLabelMaster>();
            CreateMap<StampCategory, StampCategoryDTO>();
            CreateMap<StampCategoryInsertDTO, StampCategory>();
            CreateMap<StampVendor, StampVendorDTO>()
                .ForMember(d => d.EffectiveFrom, opt => opt.MapFrom(src => src.EffectiveFrom.ToString()))
                .ForMember(d => d.ValidUpto, opt => opt.MapFrom(src => src.ValidUpto.ToString()));
            CreateMap<StampVendorInsertDTO, StampVendor>();
                //.ForMember(d => d.EffectiveFrom, opt => opt.MapFrom(src => src.EffectiveFrom.HasValue ? src.EffectiveFrom.Value : default))
                //.ForMember(d => d.ValidUpto, opt => opt.MapFrom(src => src.ValidUpto.HasValue ? src.ValidUpto.Value: default));
            CreateMap<StampTypeInsertDTO, StampType>();
            CreateMap<StampType, StampTypeDTO>();
            CreateMap<DiscountDetailsInsertDTO, DiscountDetail>();
            CreateMap<StampIndentInsertDTO, StampIndent>();
            CreateMap<StampIndent, StampIndentDTO>();
            CreateMap<StampInvoiceInsertDTO, StampInvoice>();
            CreateMap<StampWalletInsertDTO, StampWallet>();

        }
    }
}
