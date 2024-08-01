using CTS_BE.BAL.Interfaces.Pension;

namespace CTS_BE.BAL.Services.Pension
{
    public abstract class BaseService : IBaseService
    {
        protected int _userId;
        protected int _dataCount;
        public BaseService()
        {
            _dataCount = 0;
            _userId = 0;
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
    }
}