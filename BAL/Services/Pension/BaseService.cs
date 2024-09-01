using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.BAL.Services.Pension
{
    public abstract class BaseService : IBaseService
    {
        private readonly IClaimService _claimService;
        protected int _userId;
        protected int _dataCount;
        public BaseService(
                IClaimService claimService
            )
        {
            _claimService = claimService;
            _userId = _claimService.GetUserId();
            _dataCount = 0;
        }
        public int DataCount()
        {
            return _dataCount;
        }
        protected void SetCreatedBy<T>(T entity) where T : class 
        {
            entity?.GetType()?.GetProperty("CreatedBy")?.SetValue(entity, _userId);
            entity?.GetType()?.GetProperty("CreatedAt")?.SetValue(entity, DateTime.Now);
            entity?.GetType()?.GetProperty("ActiveFlag")?.SetValue(entity, true);
        }
        protected void SetUpdatedBy<T>(T entity) where T : class
        {
            entity?.GetType()?.GetProperty("UpdatedBy")?.SetValue(entity, _userId);
            entity?.GetType()?.GetProperty("UpdatedAt")?.SetValue(entity, DateTime.Now);
        }
        protected string GetUserName()
        {
            return _claimService.GetUserName();
        }
    }
}