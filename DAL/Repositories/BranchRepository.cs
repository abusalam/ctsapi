using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore;
namespace CTS_BE.DAL
{
   public class BranchRepository : Repository<Branch, CTSDBContext>, IBranchRepository
   {
        private readonly CTSDBContext _context;
        public BranchRepository(
            CTSDBContext context
        ) : base(context)
        {
            _context = context;
        }

        public async Task<BranchDeatilsDTO?> GetBranchByCode(
            short branchCode
        )
        {
            var branchData = await _context.Branchs
                .Where(
                    x => x.IsActive
                    && x.BranchCode == branchCode
                )
                .Join(
                    _context.Banks,
                    x => x.BankCode,
                    y => y.BankCode,
                    (x, y) =>  new BranchDeatilsDTO() {
                        BankName = y.BankName,
                        BranchName = x.BranchName,
                        MircCode = x.MicrCode ?? "",
                        BranchAddress = x.Address ?? ""
                    }
                )
                .FirstOrDefaultAsync();
            return branchData;
        }
   }
}