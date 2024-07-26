using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PensionerBankAccountRepository :
        Repository<BankAccount, PensionDbContext>,
        IPensionerBankAccountRepository
    {
        public PensionerBankAccountRepository(PensionDbContext context) : base(context)
        {
        }
    }
}