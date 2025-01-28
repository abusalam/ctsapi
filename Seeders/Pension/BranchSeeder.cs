using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class BranchSeeder(PensionDbContext context) : ISeeder
    {
        public void Seed(int count = 0)
        {
            if (context.Branches.Any())
            {
                return;
            }

            new BankSeeder(context).Seed(count);

            var branches = new[]
            {
                new Branch
                {
                    Id = 1,
                    BankId = 1,
                    IfscCode = "ABHY0065001",
                    BranchName = "RTGS-HO",
                    BranchAddress =
                        "ABHYUDAYA BANK BLDG., B.NO.71, NEHRU NAGAR, KURLA (E), MUMBAI-400024",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "25260173",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 2,
                    BankId = 1,
                    IfscCode = "ABHY0065002",
                    BranchName = "ABHYUDAYA NAGAR",
                    BranchAddress =
                        "ABHYUDAYA EDUCATION SOCIETY, OPP. BLDG. NO. 18, ABHYUDAYA NAGAR, KALACHOWKY, MUMBAI - 400033",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "24702643",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 3,
                    BankId = 1,
                    IfscCode = "ABHY0065003",
                    BranchName = "BAIL BAZAR",
                    BranchAddress =
                        "KMSPM''S SCHOOL, WADIA ESTATE, BAIL BAZAR-KURLA(W), MUMBAI-400070",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "25032202",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 4,
                    BankId = 1,
                    IfscCode = "ABHY0065004",
                    BranchName = "BHANDUP",
                    BranchAddress = "CHETNA APARTMENTS, J.M.ROAD, BHANDUP, MUMBAI-400078",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "25963157",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 5,
                    BankId = 1,
                    IfscCode = "ABHY0065005",
                    BranchName = "DARUKHANA",
                    BranchAddress = "POTIA IND.ESTATE, REAY ROAD (E), DARUKHANA, MUMBAI-400010",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "23778164",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 6,
                    BankId = 1,
                    IfscCode = "ABHY0065006",
                    BranchName = "FORT",
                    BranchAddress =
                        "ABHYUDAYA BANK BLDG., 251, PERIN NARIMAN STREET, FORT, MUMBAI-400001",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "22614468",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 7,
                    BankId = 1,
                    IfscCode = "ABHY0065007",
                    BranchName = "GHATKOPAR",
                    BranchAddress =
                        "UNIT NO 2 & 3, SILVER HARMONY BLDG,NEW MANIKLAL ESTATE, GHATKOPAR (WEST), MUMBAI-400086",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "25116673",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 8,
                    BankId = 1,
                    IfscCode = "ABHY0065008",
                    BranchName = "KANJUR",
                    BranchAddress =
                        "BHANDARI CO-OP. HSG. SOCIETY, KANJUR VILLAGE, KANJUR (EAST), MUMBAI-400078",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "25783519",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 9,
                    BankId = 1,
                    IfscCode = "ABHY0065009",
                    BranchName = "NEHRU NAGAR",
                    BranchAddress =
                        "ABHYUDAYA BANK BLDG., B.NO.71, NEHRU NAGAR, KURLA (E), MUMBAI-400024",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "25222386",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 10,
                    BankId = 1,
                    IfscCode = "ABHY0065010",
                    BranchName = "PAREL",
                    BranchAddress =
                        "SHRAMA SAFALYA, 63 G.D.AMBEKAR MARG, PAREL VILLAGE, MUMBAI-400012",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "24137707",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 11,
                    BankId = 1,
                    IfscCode = "ABHY0065011",
                    BranchName = "SEWRI",
                    BranchAddress =
                        "NAVNIDHI INDUSTRIAL ESTATE, ACHARYA DONDHE MARG, SEWRI, MUMBAI-400015",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "24136008",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 12,
                    BankId = 1,
                    IfscCode = "ABHY0065012",
                    BranchName = "WADALA",
                    BranchAddress =
                        "B.P.T.MARKET BLDG., NADKARNI PARK, WADALA (EAST), MUMBAI-400037",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "24184512",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 13,
                    BankId = 1,
                    IfscCode = "ABHY0065013",
                    BranchName = "WORLI",
                    BranchAddress =
                        "LANDMARK,NEXT TO MAHINDRA TOWERS, PLOT NO.1, J M BHOSLE MARG, WORLI, MUMBAI-400018",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "24921104",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 14,
                    BankId = 1,
                    IfscCode = "ABHY0065014",
                    BranchName = "MUMBRA",
                    BranchAddress = "RIZVI APARTMENTS, OPP. RAILWAY STATION, MUMBRA-400612",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "25462172",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 15,
                    BankId = 1,
                    IfscCode = "ABHY0065015",
                    BranchName = "TURBHE",
                    BranchAddress =
                        "A.P.M.C.MARKET, ADMINISTRATIVE BLDG, TURBHE, NAVI MUMBAI-40070",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "27888044",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 16,
                    BankId = 1,
                    IfscCode = "ABHY0065016",
                    BranchName = "VASHI",
                    BranchAddress = "ABHYUDAYA BANK BLDG., SECTOR 17, VASHI, NAVI MUMBAI-400705",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "27892458",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 17,
                    BankId = 1,
                    IfscCode = "ABHY0065017",
                    BranchName = "MOBILE BANK",
                    BranchAddress = "ABHYUDAYA BANK BLDG., SECTOR 17, VASHI, NAVI MUMBAI-400703",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "27892444",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 18,
                    BankId = 1,
                    IfscCode = "ABHY0065018",
                    BranchName = "NEW PANVEL",
                    BranchAddress = "ABHYUDAYA BANK BLDG., SECTOR 17, NEW PANVEL-410206",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "27453585",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 19,
                    BankId = 1,
                    IfscCode = "ABHY0065019",
                    BranchName = "KALAMBOLI",
                    BranchAddress =
                        "BLDG F-4, SHOP NO 17-20, SECTOR 3 EB, KALAMBOLI, NAVI MUMBAI-410218",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "27420148",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 20,
                    BankId = 1,
                    IfscCode = "ABHY0065020",
                    BranchName = "DHARAVI",
                    BranchAddress = "WESTERN INDIA TANNERIES, SION DHARAVI ROAD, MUMBAI-400017",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "24077126",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 113,
                    BankId = 2,
                    IfscCode = "ADCB0000001",
                    BranchName = "RTGS-HO",
                    BranchAddress = "75, REHMAT MANZIL, V. N. ROAD, CURCHGATE, MUMBAI - 400020",
                    DistrictName = "GREATER MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "39534100",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 114,
                    BankId = 2,
                    IfscCode = "ADCB0000002",
                    BranchName = "BANGALORE",
                    BranchAddress =
                        "7CITI CENTRE, 28, CHURCH STREET, OFF M. G. ROAD BANGALORE 560001",
                    DistrictName = "BANGALORE URBAN",
                    CityName = "BANGALORE",
                    StateName = "KARNATAKA",
                    PhoneNo = "25582000",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 115,
                    BankId = 3,
                    IfscCode = "ABPB0000001",
                    BranchName = "RTGS-HO",
                    BranchAddress =
                        "LEVEL 17 AND 18,BIRLA AURORA TOWERS,DR.ANNIE BESANT ROAD,WORLI,MUMBAI 400030,INDIA",
                    DistrictName = "MUMBAI",
                    CityName = "MUMBAI",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "62307000",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 116,
                    BankId = 3,
                    IfscCode = "ABPB0000002",
                    BranchName = "CENTRAL PROCESSING CENTRE",
                    BranchAddress =
                        "LEVEL 5,TOWER NO 2,TVH BELICIAA TOWERS,MRC NAGAR,CHENNAI 600028,INDIA",
                    DistrictName = "CHENNAI",
                    CityName = "CHENNAI",
                    StateName = "TAMIL NADU",
                    PhoneNo = "66420200",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 117,
                    BankId = 4,
                    IfscCode = "AMCB0660002",
                    BranchName = "MUMTAJ MAHAL, ZAKARIYA MASZID, RELIEF ROAD, AHMEDABAD-380 001",
                    BranchAddress =
                        "LEVEL 5,TOWER NO 2,TVH BELICIAA TOWERS,MRC NAGAR,CHENNAI 600028,INDIA",
                    DistrictName = "AHMEDABAD",
                    CityName = "AHMEDABAD",
                    StateName = "GUJARAT",
                    PhoneNo = "7922139024",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 118,
                    BankId = 4,
                    IfscCode = "AMCB0660003",
                    BranchName = "MANINAGAR",
                    BranchAddress = "JAWAHAR CHOWK CHAR RASTA,MANINAGAR, AHMEDABAD-380 008.",
                    DistrictName = "AHMEDABAD",
                    CityName = "AHMEDABAD",
                    StateName = "GUJARAT",
                    PhoneNo = "7925450515",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 152,
                    BankId = 5,
                    IfscCode = "AMDN0000001",
                    BranchName = "RTGS-HO",
                    BranchAddress = "PLOT NO 33,MARKET YARD,STATION ROAD,AHMEDNAGAR -414001",
                    DistrictName = "AHMEDNAGAR",
                    CityName = "AHMEDNAGAR",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "2414405",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Branch
                {
                    Id = 153,
                    BankId = 5,
                    IfscCode = "AMDN0000101",
                    BranchName = "DALMANDAI",
                    BranchAddress = "ANAND COMPLEX,DALMANDAI BRANCH,AHMEDNAGAR-414001",
                    DistrictName = "AHMEDNAGAR",
                    CityName = "AHMEDNAGAR",
                    StateName = "MAHARASHTRA",
                    PhoneNo = "2414415",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
            };

            if (count <= 28)
            {
                context.Branches.AddRange(branches);
            }
            else
            {
                context.Branches.AddRange(branches);
                var additionalBranches = new List<Branch>();
                for (int i = 0; i < count - 28; i++)
                {
                    additionalBranches.Add(
                        new Branch
                        {
                            Id = 29 + i,
                            BankId = 1,
                            IfscCode = $"ABHY00{29 + i}",
                            BranchName = $"Branch {29 + i}",
                            BranchAddress = $"Address {29 + i}",
                            DistrictName = $"District {29 + i}",
                            CityName = $"City {29 + i}",
                            StateName = $"State {29 + i}",
                            PhoneNo = $"9{i:00000000}",
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                    );
                }
                context.Branches.AddRange(additionalBranches);
            }
            context.SaveChanges();
        }
    }
}
