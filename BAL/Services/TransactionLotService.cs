using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.Helper;
using CTS_BE.Model;
using CTS_BE.Model.e_Kuber;
using Type = CTS_BE.Model.e_Kuber.Type;
namespace CTS_BE.BAL
{
    public class TransactionLotService : ITransactionLotService
    {
        private readonly ITransactionLotRepository _TransactionLotRepository;
        private readonly IMapper _mapper;
        public TransactionLotService(ITransactionLotRepository TransactionLotRepository, IMapper mapper)
        {
            _TransactionLotRepository = TransactionLotRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateLot(long userId)
        {
            return await _TransactionLotRepository.NewLot(userId);
        }
        public async Task<List<TransactionLotModel>> pendingLots()
        {
            List<TransactionLot> transactionLots = (List<TransactionLot>) await _TransactionLotRepository.GetAllByConditionAsync(e => e.Status == 1);
            return _mapper.Map<List<TransactionLotModel>>(transactionLots);
        }
        public async Task<EKuber> GetXMLData(long lotId)
        {
            string UDCH = "0116";
            string DbtrAcctNo = "01516701174";
            string DebtorIFSC = "RBIS0GOWBEP";
            string FileSequenceNumber = "0055";
            string DateofAuthorization = "20240415";
            string LotNumber = "CKP000210";
            string LotType = "01";
            string PaymentAdviceDate = "20240710";
            string PaymentAdviceYYMM = "202407";
            string ReservedField = DbtrAcctNo.PadLeft(12, '0');
            string FileName = "EPV8" + UDCH + ReservedField + DateofAuthorization + FileSequenceNumber;
            EKuber eKuberData = await _TransactionLotRepository.GetSingleSelectedColumnByConditionAsync(e => e.Id == lotId, e => new EKuber
            {
                requestPayload = new RequestPayload
                {
                    AppHdr = new AppHdr
                    {
                        Fr = new Fr { OrgId = new OrgId { Id = new Id { OrgId = new OrgId { Othr = new Othr { Id = UDCH } } } } },
                        To = new To { FIId = new FIId { FinInstnId = new FinInstnId { ClrSysMmbId = new ClrSysMmbId { MmbId = "RBI" } } } },
                        BizMsgIdr = FileName,
                        MsgDefIdr = "pain.001.001.08",
                        BizSvc = "CustomerCreditTransferInitiationV08",
                        CreDt = DateTime.Now
                    },
                    Document = new Document
                    {
                        CstmrCdtTrfInitn = new CstmrCdtTrfInitn
                        {
                            GrpHdr = new GrpHdr
                            {
                                MsgId = FileName,
                                CreDtTm = DateTime.Now,
                                Authstn = new Authstn
                                {
                                    Prtry = "ALL",
                                },
                                NbOfTxs = (int)e.NumberOfBeneficiary,
                                CtrlSum = (decimal)e.TotalAmount,
                                InitgPty = new InitgPty
                                {
                                    Nm = "WEST BENGAL",
                                    Id = new Id
                                    {
                                        OrgId = new OrgId
                                        {
                                            Othr = new Othr
                                            {
                                                Id = UDCH
                                            }
                                        }
                                    },
                                    CtctDtls = new ContactDetails
                                    {
                                        EmailAdr = "gZoKZ@example.com",
                                    }
                                }
                            },
                            PmtInf = new PmtInf
                            {
                                //TODO:: What will be the "PaymentAdviceDate" and "PaymentAdviceYYMM"
                                PmtInfId = UDCH+e.LotNo+LotType+PaymentAdviceDate+PaymentAdviceYYMM,
                                PmtMtd = "TRF",
                                BtchBookg = "true",
                                NbOfTxs = (int)e.NumberOfBeneficiary,
                                CtrlSum = (decimal)e.TotalAmount,
                                PmtTpInf = new PaymentTypeInformation
                                {
                                    InstrPrty ="HIGH",
                                    SvcLvl = new ServiceLevel
                                    {
                                        Prtry = "NEFT"
                                    }
                                },
                                ReqdExctnDt = DateTime.Now.ToString("yyyy-MM-dd"),
                                Dbtr = new Debtor
                                {
                                    Nm = "FINANCE DEPT",
                                    PstlAdr = new PostalAddress
                                    {
                                        Dept = "FD",
                                        SubDept = ""
                                    },
                                    Id = new Id
                                    {
                                        OrgId = new OrgId
                                        {
                                            Othr = new Othr
                                            {
                                                Id = UDCH
                                            }
                                        }
                                    }
                                },
                                DbtrAcct = new DebtorAccount
                                {
                                    Id = new Id
                                    {
                                        Othr = new Othr
                                        {
                                            Id = DbtrAcctNo
                                        }
                                    }
                                },
                                DbtrAgt = new DebtorAgent
                                {
                                    FinInstnId = new FinInstnId
                                    {
                                        ClrSysMmbId = new ClrSysMmbId
                                        {
                                            MmbId = DebtorIFSC
                                        }
                                    }
                                },
                                CdtTrfTxInf = e.TransactionLotHasBeneficiaries.Select(lotBeneficiarie=> new CreditTransferTransactionInformation{
                                    PmtId = new PaymentId
                                    {
                                        InstrId = "DRNPAYEE001",//TODO:: What will be the "InstrId"
                                        EndToEndId = "EP"+UDCH+ReservedField+DateTimeHelper.GetJulianDate(DateTime.Now)+"000001"
                                    },
                                    Amt = new Amount
                                    {
                                        InstdAmt = new InstdAmt
                                        {
                                            Amt = (decimal)lotBeneficiarie.Amount,
                                            CcyOfTrf = "INR"
                                        }
                                    },
                                    CdtrAgt = new CreditorAgent
                                    {
                                        FinInstnId = new FinInstnId
                                        {
                                            ClrSysMmbId = new ClrSysMmbId
                                            {
                                                MmbId = lotBeneficiarie.IfscCode
                                            }
                                        },
                                        BrnchId = new BranchId
                                        {
                                            Id = lotBeneficiarie.IfscCode
                                        }
                                    },
                                    Cdtr = new Creditor
                                    {
                                        Nm = lotBeneficiarie.BeneficiaryName,
                                    },
                                    CdtrAcct = new CreditorAccount
                                    {
                                        Id = new Id
                                        {
                                            Othr = new Othr
                                            {
                                                Id = lotBeneficiarie.AccountNumber.ToString().Trim()
                                            }
                                        },
                                        Tp = new Type 
                                        {
                                            Cd = "10"
                                        }
                                    }
                                }).ToList()
                            }
                        }
                    }
                }
            });
            // // ===================================================================
            // EKuber data = new EKuber
            // {
            //     requestPayload = new RequestPayload
            //     {

            //         AppHdr = new AppHdr
            //         {
            //             Fr = new Fr { OrgId = new OrgId { Id = new Id { OrgId = new OrgId { Othr = new Othr { Id = UDCH } } } } },
            //             To = new To
            //             {
            //                 FIId = new FIId
            //                 {
            //                     FinInstnId = new FinInstnId
            //                     {
            //                         ClrSysMmbId = new ClrSysMmbId
            //                         {
            //                             MmbId = "RBI"
            //                         }
            //                     }
            //                 }
            //             },
            //             BizMsgIdr = FileName,
            //             MsgDefIdr = "pain.001.001.08",
            //             BizSvc = "CustomerCreditTransferInitiationV08",
            //             CreDt = DateTime.Now
            //         },
            //         Document = new Document
            //         {
            //             CstmrCdtTrfInitn = new CstmrCdtTrfInitn
            //             {
            //                 GrpHdr = new GrpHdr
            //                 {
            //                     MsgId = FileName,
            //                     CreDtTm = DateTime.Now,
            //                     Authstn = new Authstn
            //                     {
            //                         Prtry = "ALL",
            //                     },
            //                     NbOfTxs = 2,
            //                     CtrlSum = 41637.00m,
            //                     InitgPty = new InitgPty
            //                     {
            //                         Nm = "WEST BENGAL",
            //                         Id = new Id
            //                         {
            //                             OrgId = new OrgId
            //                             {
            //                                 Othr = new Othr
            //                                 {
            //                                     Id = UDCH
            //                                 }
            //                             }
            //                         },
            //                         CtctDtls = new ContactDetails
            //                         {
            //                             EmailAdr = "user@email.com"
            //                         }
            //                     }
            //                 },
            //                 PmtInf = new PmtInf
            //                 {
            //                     PmtInfId = UDCH + LotNumber + LotType + PaymentAdviceDate + PaymentAdviceYYMM,
            //                     PmtMtd = "TRF",
            //                     BtchBookg = "true",
            //                     NbOfTxs = 2,
            //                     CtrlSum = 41637.00m,
            //                     PmtTpInf = new PaymentTypeInformation
            //                     {
            //                         InstrPrty = "HIGH",
            //                         SvcLvl = new ServiceLevel
            //                         {
            //                             Prtry = "NEFT"
            //                         }
            //                     },
            //                     ReqdExctnDt = DateTime.Now.ToString("yyyy-MM-dd"),
            //                     Dbtr = new Debtor
            //                     {
            //                         Nm = "FINANCE DEPT",
            //                         PstlAdr = new PostalAddress
            //                         {
            //                             Dept = "FD",
            //                             SubDept = ""
            //                         },
            //                         Id = new Id
            //                         {
            //                             OrgId = new OrgId
            //                             {
            //                                 Othr = new Othr
            //                                 {
            //                                     Id = UDCH
            //                                 }
            //                             }
            //                         }
            //                     },
            //                     DbtrAcct = new DebtorAccount
            //                     {
            //                         Id = new Id
            //                         {
            //                             Othr = new Othr
            //                             {
            //                                 Id = DbtrAcctNo
            //                             }
            //                         }
            //                     },
            //                     DbtrAgt = new DebtorAgent
            //                     {
            //                         FinInstnId = new FinInstnId
            //                         {
            //                             ClrSysMmbId = new ClrSysMmbId
            //                             {
            //                                 MmbId = DebtorIFSC
            //                             }
            //                         }
            //                     },
            //                     CdtTrfTxInf = new List<CreditTransferTransactionInformation>
            //                     {
            //                         new CreditTransferTransactionInformation
            //                         {
            //                             PmtId = new PaymentId
            //                             {
            //                                 InstrId = "DRNPAYEE001",
            //                                 EndToEndId = "EP"+UDCH+ReservedField+DateTimeHelper.GetJulianDate(DateTime.Now)+"000001"
            //                             },
            //                             Amt = new Amount
            //                             {
            //                             InstdAmt = new InstdAmt{
            //                                     Amt = 40916.00m,
            //                                     CcyOfTrf = "INR"
            //                                 }

            //                             },
            //                             CdtrAgt = new CreditorAgent
            //                             {
            //                                 FinInstnId = new FinInstnId
            //                                 {
            //                                     ClrSysMmbId = new ClrSysMmbId
            //                                     {
            //                                         MmbId = "SBIN0014061"
            //                                     }
            //                                 },
            //                                 BrnchId = new BranchId
            //                                 {
            //                                     Id = "SBIN0014061"
            //                                 }
            //                             },
            //                             Cdtr = new Creditor
            //                             {
            //                                 Nm = "FARM FIELD PVT LTD"
            //                             },
            //                             CdtrAcct = new CreditorAccount
            //                             {
            //                                 Id = new Id
            //                                 {
            //                                     Othr = new Othr
            //                                     {
            //                                         Id = "69174199438"
            //                                     }
            //                                 },
            //                                 Tp = new Type
            //                                 {
            //                                     Cd = "10"
            //                                 }
            //                             }
            //                         },
            //                         new CreditTransferTransactionInformation
            //                         {
            //                             PmtId = new PaymentId
            //                             {
            //                                 InstrId = "DRNPAYEE002",
            //                                 EndToEndId = "EP"+UDCH+ReservedField+DateTimeHelper.GetJulianDate(DateTime.Now)+"0002"
            //                             },
            //                             Amt = new Amount
            //                             {
            //                                 InstdAmt = new InstdAmt{
            //                                     Amt = 721.00m,
            //                                     CcyOfTrf = "INR"
            //                                 }
            //                             },
            //                             CdtrAgt = new CreditorAgent
            //                             {
            //                                 FinInstnId = new FinInstnId
            //                                 {
            //                                     ClrSysMmbId = new ClrSysMmbId
            //                                     {
            //                                         MmbId = "SBIN0014062"
            //                                     }
            //                                 },
            //                                 BrnchId = new BranchId
            //                                 {
            //                                     Id = "SBIN0014062"
            //                                 }
            //                             },
            //                             Cdtr = new Creditor
            //                             {
            //                                 Nm = "Test Name"
            //                             },
            //                             CdtrAcct = new CreditorAccount
            //                             {
            //                                 Id = new Id
            //                                 {
            //                                     Othr = new Othr
            //                                     {
            //                                         Id = "24022000007067"
            //                                     }
            //                                 },
            //                                 Tp = new Type
            //                                 {
            //                                     Cd = "10"
            //                                 }
            //                             }
            //                         }
            //                     }
            //                 }
            //             }
            //         }
            //     }
            // };
            return eKuberData;
        }
    }
}