using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class TreasurySeeder(PensionDbContext context) : ISeeder
    {
        public void Seed(int count = 0)
        {
            if (context.Treasuries.Any())
            {
                return;
            }

            var treasuries = new[]
            {
                new Treasury
                {
                    TreasuryCode = "JAC",
                    TreasuryName = "Alipurduar",
                    DistrictCode = "AP",
                    TreasuryAddress = "P.O.ALIPURDUARCOURT,DIST-ALIPURDUAR",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "03564255166",
                    PhoneNo2 = null,
                    Fax = "1011555",
                    EMail = "apd.try@gmail.com",
                    Pincode = "736122",
                    IntTreasuryCode = "20003",
                    PensionFlag = 'Y',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "HGC",
                    TreasuryName = "Arambagh",
                    DistrictCode = "HG",
                    TreasuryAddress = "XXX",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "7319542077",
                    PhoneNo2 = null,
                    Fax = "1011404",
                    EMail = "to.hgc-wb.nic.in",
                    Pincode = "712601",
                    IntTreasuryCode = "12003",
                    PensionFlag = 'Y',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "BUD",
                    TreasuryName = "Asansol-II",
                    DistrictCode = "BP",
                    TreasuryAddress = "SDO OFFICE CAMPUS, TREASURY BUILDINGS, ASANSOL",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "7595068258",
                    PhoneNo2 = "03412258036",
                    Fax = "1011264",
                    EMail = "to.bud-wb@gov.com",
                    Pincode = "713304",
                    IntTreasuryCode = "04004",
                    PensionFlag = 'N',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "DDA",
                    TreasuryName = "Balurghat-I",
                    DistrictCode = "DD",
                    TreasuryAddress = "BALURGHAT,THANAMORE",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "8373069018",
                    PhoneNo2 = "03522255617",
                    Fax = "1011651",
                    EMail = "treasury1balurghat@gmail.com",
                    Pincode = "731101",
                    IntTreasuryCode = "06001",
                    PensionFlag = 'Y',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "NPA",
                    TreasuryName = "Barasat-I",
                    DistrictCode = "NP",
                    TreasuryAddress = "OFFICE OF THE DISTRICT MAGISTRATE,NORTH 24 PARGANAS",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "03325846270",
                    PhoneNo2 = "7595068155",
                    Fax = "1011054",
                    EMail = "to.npa-wb@gov.in",
                    Pincode = "700124",
                    IntTreasuryCode = "05001",
                    PensionFlag = 'N',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "SPD",
                    TreasuryName = "Baruipur",
                    DistrictCode = "SP",
                    TreasuryAddress = "ZILLA PARISHAD BHAVAN BARUIPUR, KOLKATA-700144",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "03324339002",
                    PhoneNo2 = "7595068152",
                    Fax = "1011021",
                    EMail = "to.spd-wb@gov.in",
                    Pincode = "700144",
                    IntTreasuryCode = "03004",
                    PensionFlag = 'Y',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "MUA",
                    TreasuryName = "Berhampore-I",
                    DistrictCode = "MU",
                    TreasuryAddress = "BERHAMPORE",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "7595068363",
                    PhoneNo2 = "03482258195",
                    Fax = "1011172",
                    EMail = "bertreasury1@gmail.com",
                    Pincode = "742101",
                    IntTreasuryCode = "19001",
                    PensionFlag = 'Y',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "MUB",
                    TreasuryName = "Berhampore-II",
                    DistrictCode = "MU",
                    TreasuryAddress =
                        "O/O THE D.M AND COLLECTOR MURSHIDABAD..NEW ADMINSTRATIVE BUILDINGS. BERHAMPORE. MURSHIDABAD",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "03482255706",
                    PhoneNo2 = "7595068366",
                    Fax = "1011183",
                    EMail = "to.mub-wb@gov.in",
                    Pincode = "742101",
                    IntTreasuryCode = "19002",
                    PensionFlag = 'N',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "BRD",
                    TreasuryName = "Birbhum-II",
                    DistrictCode = "BR",
                    TreasuryAddress = "BIRBHUMTREASURY-II,P.O.-SURIDIST.-BIRBHUM",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "03462258303",
                    PhoneNo2 = "7595068279",
                    Fax = "1011426",
                    EMail = "to.brd-wb@gov.in",
                    Pincode = "731101",
                    IntTreasuryCode = "18004",
                    PensionFlag = 'Y',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "BAB",
                    TreasuryName = "Bishnupur",
                    DistrictCode = "BA",
                    TreasuryAddress = "BISHNUPUR,DIST.-BANKURA.",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "03244252192",
                    PhoneNo2 = "7595068237",
                    Fax = "1011426",
                    EMail = "to.bab-wb@nic.in",
                    Pincode = "722122",
                    IntTreasuryCode = "14002",
                    PensionFlag = 'Y',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Treasury
                {
                    TreasuryCode = "DAA",
                    TreasuryName = "Darjeeling",
                    DistrictCode = "DA",
                    TreasuryAddress = "DARJEELING",
                    Address1 = null,
                    Address2 = null,
                    PhoneNo1 = "03244252192",
                    PhoneNo2 = "7595068237",
                    Fax = "1011426",
                    EMail = "to.bab-wb@nic.in",
                    Pincode = "722122",
                    IntTreasuryCode = "14002",
                    PensionFlag = 'Y',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
            };

            var newTreasuries = new List<Treasury>();
            if (count <= treasuries.Length)
            {
                newTreasuries.AddRange(treasuries);
            }
            else
            {
                newTreasuries.AddRange(treasuries);
                var additionalTreasuries = new List<Treasury>();
                Random random = new Random();
                var treasuryCodeSet = new HashSet<string>(treasuries.Select(t => t.TreasuryCode));

                for (int i = 0; i < count - treasuries.Length; i++)
                {
                    string treasuryCode;
                    while (true)
                    {
                        treasuryCode =
                            $"{(char)random.Next(65, 91)}{(char)random.Next(65, 91)}{(char)random.Next(65, 91)}";
                        if (!treasuryCodeSet.Contains(treasuryCode))
                        {
                            treasuryCodeSet.Add(treasuryCode);
                            break;
                        }
                    }

                    additionalTreasuries.Add(
                        new Treasury
                        {
                            TreasuryCode = treasuryCode,
                            TreasuryName = $"Treasury {i + 1}",
                            DistrictCode =
                                $"{(char)random.Next(65, 91)}{(char)random.Next(65, 91)}",
                            TreasuryAddress = $"Address {random.Next(1, 100)}",
                            Address1 = $"Address 1 {random.Next(1, 100)}",
                            Address2 = $"Address 2 {random.Next(1, 100)}",
                            PhoneNo1 = $"0{random.Next(100000000, 999999999)}",
                            PhoneNo2 = $"0{random.Next(100000000, 999999999)}",
                            Fax = $"0{random.Next(1000000, 9999999)}",
                            EMail = $"email{random.Next(1, 100)}@example.com",
                            Pincode = $"{random.Next(100000, 999999)}",
                            IntTreasuryCode = $"14{random.Next(100, 999)}",
                            PensionFlag = random.NextDouble() < 0.5 ? 'Y' : 'N',
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                    );
                }

                newTreasuries.AddRange(additionalTreasuries);
            }

            context.Treasuries.AddRange(newTreasuries);
            context.SaveChanges();
        }
    }
}
