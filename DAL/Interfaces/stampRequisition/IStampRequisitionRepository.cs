using CTS_BE.DAL.Entities;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.stampRequisition
{
    public interface IStampRequisitionRepository : IRepository<VendorStampRequisition>
    {
        Task<bool> ApproveByStampClerk(long vendorStampRequisitionId, short sheet, short label);
        Task<bool> ApproveByTO(StampRequisitionApprovedByTODTO stampRequisition);
    }
}
