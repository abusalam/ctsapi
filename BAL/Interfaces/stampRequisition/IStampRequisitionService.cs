using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.stampRequisition
{
    public interface IStampRequisitionService
    {
        Task<bool> CreateStampRequisition(StampRequisitionInsertDTO stampRequisition);
        Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitions(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<bool> RequisitionApprovedByStampClerk(StampRequisitionApprovedByClerkDTO stampRequisition);
        Task<bool> RequisitionRejectedByStampClerk(long stampRequisitionId);
        Task<bool> RequisitionApprovedByTO(StampRequisitionApprovedByTODTO stampRequisition);
        Task<bool> RequisitionRejectedByTO(long stampRequisitionStagingId);
        Task<bool> DeliveredByDEO(long stampRequisitionId);
        Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsWaitingForApprovalByTO(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsWaitingForPayment(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsForClerk(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<bool> PaymentRegisterByDEO(StampRequisitionPaymentDTO stampRequisition);
        Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsWaitingForPaymentVerification(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsWaitingForDelivery(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<TRFormDataDTO> TrFromGenerationData(long stampRequisitionId);
        Task<CalculationDTO> CalculateAllDetails(DataForCalculationDTO srd);
    }
}