using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.Helper
{
    public partial class PensionMapperClass : Profile
    {
        public PensionMapperClass()
        {
            // Format mappings like this => CreateMap<Entity, DTO>().ReverseMap();
            // Keep the order of mappings consistent and avoid mapping DTOs to DTOs

            CreateMap<PpoReceipt, ManualPpoReceiptEntryDTO>()
                .ReverseMap();
            CreateMap<PpoReceipt, ManualPpoReceiptResponseDTO>().ReverseMap();
            CreateMap<PpoReceipt, ListAllPpoReceiptsResponseDTO>().ReverseMap();

            CreateMap<Pensioner, PensionerEntryDTO>().ReverseMap();
            CreateMap<Pensioner, PensionerResponseDTO>().ReverseMap();
            CreateMap<Pensioner, PensionerListItemDTO>().ReverseMap();
            CreateMap<Pensioner, PpoComponentRevisionPpoListItemDTO>().ReverseMap();

            CreateMap<PpoStatusFlag, PensionStatusEntryDTO>().ReverseMap();
            CreateMap<PpoStatusFlag, PensionStatusDTO>().ReverseMap();

            CreateMap<PrimaryCategory, PensionPrimaryCategoryResponseDTO>().ReverseMap();
            CreateMap<PrimaryCategory, PensionPrimaryCategoryEntryDTO>().ReverseMap();

            CreateMap<SubCategory, PensionSubCategoryResponseDTO>().ReverseMap();
            CreateMap<SubCategory, PensionSubCategoryEntryDTO>().ReverseMap();

            CreateMap<Category, PensionCategoryResponseDTO>().ReverseMap();
            CreateMap<Category, PensionCategoryListDTO>().ReverseMap();

            CreateMap<Breakup, PensionBreakupResponseDTO>().ReverseMap();
            CreateMap<Breakup, PensionBreakupEntryDTO>().ReverseMap();

            CreateMap<ComponentRate, ComponentRateResponseDTO>().ReverseMap();
            CreateMap<ComponentRate, ComponentRateEntryDTO>().ReverseMap();

            CreateMap<PpoComponentRevision, PpoComponentRevisionResponseDTO>().ReverseMap();
            CreateMap<PpoComponentRevision, PpoComponentRevisionEntryDTO>().ReverseMap();
            CreateMap<PpoComponentRevision, PpoPaymentListItemDTO>().ReverseMap();

            CreateMap<PpoBill, PpoBillEntryDTO>().ReverseMap();
            CreateMap<PpoBill, PpoBillResponseDTO>().ReverseMap();
            CreateMap<PpoBill, InitiateFirstPensionBillResponseDTO>().ReverseMap();
            CreateMap<PpoBill, PpoBillResponseDTO>().ReverseMap();
            CreateMap<PpoBill, PpoBillSaveResponseDTO>().ReverseMap();
            CreateMap<PpoBill, PpoRegularBillDetailsDTO>().ReverseMap();

            CreateMap<PpoBillBreakup, PpoBillBreakupResponseDTO>().ReverseMap();
            CreateMap<PpoBillBreakup, PpoBillBreakupEntryDTO>().ReverseMap();
            CreateMap<PpoBillBreakup, PpoPaymentListItemDTO>().ReverseMap();
            CreateMap<PpoBillBreakup, PpoPaymentHistoryResponseDTO>().ReverseMap();

            CreateMap<Bill, RegularBillResponseDTO>().ReverseMap();
            CreateMap<Bill, BillResponseDTO>().ReverseMap();

            CreateMap<Bank, BankResponseDTO>().ReverseMap();

            CreateMap<Branch, BranchResponseDTO>().ReverseMap();
            CreateMap<Branch, BranchListItemResponseDTO>().ReverseMap();

            CreateMap<PpoSanctionDetail, PpoSanctionDetailsResponseDTO>().ReverseMap();
            CreateMap<PpoSanctionDetail, PpoSanctionDetailsEntryDTO>().ReverseMap();

            CreateMap<Nominee, NomineeResponseDTO>().ReverseMap();
            CreateMap<Nominee, NomineeEntryDTO>().ReverseMap();

            CreateMap<LifeCertificate, LifeCertificateResponseDTO>().ReverseMap();
            CreateMap<LifeCertificate, LifeCertificateEntryDTO>().ReverseMap();

            CreateMap<UploadedFile, FileEntryDTO>().ReverseMap();
            CreateMap<UploadedFile, FileResponseDTO>().ReverseMap();

            CreateMap<EppoReceipt, EPpoReceiptEntryDTO>().ReverseMap();
            CreateMap<EppoReceipt, EPpoReceiptResponseDTO>().ReverseMap();
            CreateMap<EppoReceipt, EPpoReceiptWithdrawlEntryDTO>().ReverseMap();
            CreateMap<EppoReceipt, EPpoReceiptWithdrawlResponseDTO>().ReverseMap();
            CreateMap<EppoReceipt, EPpoReceiptPpoIdResponseDTO>().ReverseMap();
            CreateMap<EppoReceipt, EPpoReceiptDetailDTO>().ReverseMap();
            CreateMap<EppoReceipt, EPpoReceiptListDTO>().ReverseMap();

            CreateMap<EppoRevision, EPpoReceiptRevisionEntryDTO>().ReverseMap();
            CreateMap<EppoRevision, EPpoReceiptRevisionResponseDTO>().ReverseMap();

            CreateMap<EppoAmount, EPpoAmountEntryDTO>().ReverseMap();

            CreateMap<EppoNominee, EPpoNomineeEntryDTO>().ReverseMap();

            CreateMap<AccountHead, AccountHeadResponseDTO>().ReverseMap();

            CreateMap<BytransferHead, ByTransferHeadEntryDTO>().ReverseMap();
            CreateMap<BytransferHead, ByTransferHeadUpdateDTO>().ReverseMap();
            CreateMap<BytransferHead, ByTransferHeadResponseDTO>().ReverseMap();
            CreateMap<PpoBytransfer, PpoByTransferAmountEntryDTO>().ReverseMap();
            CreateMap<PpoBytransfer, PpoByTransferAmountUpdateDTO>().ReverseMap();
            CreateMap<PpoBytransfer, PpoByTransferAmountResponseListDTO>().ReverseMap();
            CreateMap<PpoBytransfer, PpoByTransferAmountResponseDTO>().ReverseMap();
        }
    }
}
