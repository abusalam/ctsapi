using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.Helper
{
    public partial class PensionMapperClass : Profile
    {
        public PensionMapperClass()
        {
            CreateMap<ManualPpoReceiptEntryDTO, PpoReceipt>().ReverseMap();
            CreateMap<ManualPpoReceiptResponseDTO, PpoReceipt>().ReverseMap();
            CreateMap<ListAllPpoReceiptsResponseDTO, PpoReceipt>().ReverseMap();
            CreateMap<PensionStatusEntryDTO, PpoStatusFlag>().ReverseMap();
            CreateMap<PensionStatusDTO, PpoStatusFlag>().ReverseMap();
            CreateMap<PensionerEntryDTO, Pensioner>();
            CreateMap<PensionerResponseDTO, Pensioner>().ReverseMap();
            CreateMap<PensionerListItemDTO, Pensioner>().ReverseMap();
            CreateMap<PensionerResponseDTO, PensionerEntryDTO>().ReverseMap();
            CreateMap<PensionerBankAcDTO, BankAccount>().ReverseMap();
            CreateMap<PensionPrimaryCategoryResponseDTO, PrimaryCategory>().ReverseMap();
            CreateMap<PensionPrimaryCategoryResponseDTO, PensionPrimaryCategoryEntryDTO>().ReverseMap();
            CreateMap<PensionSubCategoryResponseDTO, SubCategory>().ReverseMap();
            CreateMap<PensionSubCategoryResponseDTO, PensionSubCategoryEntryDTO>().ReverseMap();
            CreateMap<PensionCategoryResponseDTO, Category>().ReverseMap();
            CreateMap<PensionCategoryListDTO, Category>().ReverseMap();
            CreateMap<PensionCategoryResponseDTO, PensionCategoryEntryDTO>().ReverseMap();
            CreateMap<PensionBreakupResponseDTO, Breakup>().ReverseMap();
            CreateMap<PensionBreakupResponseDTO, PensionBreakupEntryDTO>().ReverseMap();
            CreateMap<ComponentRateResponseDTO, ComponentRate>().ReverseMap();
            CreateMap<ComponentRateResponseDTO, ComponentRateEntryDTO>().ReverseMap();
            CreateMap<PpoComponentRevisionResponseDTO, PpoComponentRevision>().ReverseMap();
        }
    }
}