using CTS_BE.DAL.Entities;
namespace CTS_BE.DAL.Interfaces
{
    public interface IChequeIndentRepository: IRepository<ChequeIndent>
    {
        public  Task<bool> InsertIndent(string indentData);
    }
}