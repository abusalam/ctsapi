using CTS_BE.DAL;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class BaseRepository<T, Tcontext>: IBaseRepository<T> where T : class where Tcontext : DbContext
    {
        public Tcontext DbContext { get; set; }
        public BaseRepository(Tcontext context)
        {
            this.DbContext = context;
        }

        IBaseRepository<T> IBaseRepository<T>.WithUserScope()
        {
            throw new NotImplementedException();
        }
    }
}