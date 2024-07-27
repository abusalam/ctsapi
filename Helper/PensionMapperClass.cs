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
            CreateMap<PensionerEntryDTO, Pensioner>()
                .ForAllMembers(
                        options => options.Condition(
                            (src, dest, srcMember) => srcMember != null
                        )
                    );
            CreateMap<PensionerResponseDTO, Pensioner>().ReverseMap();
            CreateMap<PensionerListDTO, Pensioner>().ReverseMap();
            CreateMap<PensionerResponseDTO, PensionerEntryDTO>().ReverseMap();
            CreateMap<PensionerBankAcDTO, BankAccount>().ReverseMap();
        }
    }
}