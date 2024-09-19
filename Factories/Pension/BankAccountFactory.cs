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
                .RuleFor(d => d.BranchCode, f=>f.PickRandom(531,746))
                .RuleFor(d => d.BankCode, f => f.PickRandom(2,3))
                .RuleFor(d => d.AccountHolderName, f => f.Person.FullName);
        }

    }
}