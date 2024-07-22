namespace CTS_BE.DAL.Interfaces.Pension
{
  public interface IBaseRepository<T>
  {
    protected IBaseRepository<T> WithUserScope();

  }
}