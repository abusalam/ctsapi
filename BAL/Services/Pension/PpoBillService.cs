using CTS_BE.DAL.Entities.Pension;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoBillService(IClaimService claimService) : BaseService(claimService)
    {
        protected static PpoComponentRevision PreparePpoComponentRevision(
            PpoComponentRevision revision,
            List<PpoComponentRevision> ppoComponentRevisions,
            long pensionerId,
            int ppoId,
            int createdBy
        )
        {
            PpoComponentRevision? ppoComponentRevisionFound = ppoComponentRevisions.FirstOrDefault(
                entity => entity.RateId == revision.RateId
            );
            if (ppoComponentRevisionFound != null)
            {
                revision = ppoComponentRevisionFound;
                return revision;
            }
            revision.ActiveFlag = true;
            revision.PensionerId = pensionerId;
            revision.PpoId = ppoId;
            revision.CreatedBy = createdBy;
            revision.CreatedAt = DateTime.Now;
            return revision;
        }
    }
}
