using Bogus;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class BankAccountFactory : BaseFactory<PensionerBankAcEntryDTO>
    {
        public BankAccountFactory()
        {
            _faker = new Faker<PensionerBankAcEntryDTO>()
                // .StrictMode(true)
                .RuleFor(d => d.PayMode, f => f.PickRandom('Q','B'))
                .RuleFor(d => d.BankAcNo, f => f.Random.Replace("################"))
                .RuleFor(d => d.IfscCode, f => f.Random.Replace("????#######"))
                .RuleFor(d => d.BranchCode, 531)
                .RuleFor(d => d.BankCode, 2)
                .RuleFor(d => d.AccountHolderName, f => f.Person.FullName);
        }

    }
}