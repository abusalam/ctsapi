using System.Xml;
using System.Xml.Serialization;
using CTS_BE.BAL.Interfaces.paymandate;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Model.e_Kuber;
using Type = CTS_BE.Model.e_Kuber.Type;

namespace CTS_BE.BAL.Services.paymandate
{
    public class PaymandateService : IPaymandateService
    {
        private readonly ITokenRepository _tokenRepository;

        public PaymandateService(ITokenRepository TokenRepository)
        {
            _tokenRepository = TokenRepository;
        }
        public async Task<IEnumerable<PayMandateShortListDTO>> TokenForShortList()
        {
            IEnumerable<PayMandateShortListDTO> payMandateShortListDTO = (IEnumerable<PayMandateShortListDTO>)await _tokenRepository.GetSelectedColumnByConditionAsync(entity => entity.TokenFlow.StatusId == (int)Enum.TokenStatus.FrowardbyTreasuryOfficer,

                entity => new PayMandateShortListDTO
                {
                    TokenId = entity.Id,
                    TokenDate = entity.TokenDate,
                    BillNo = entity.Bill.BillNo,
                    BillDate = entity.Bill.BillDate,
                    TRFormats = entity.Bill.TrMaster.WbFormCode,
                    BillTypes = "",
                    BillModule = "",
                    BillPeriod = "",
                    //NoOfBeneficiarie = 0,
                    NeAmount = entity.Bill.NetAmount,
                    //ECSAmount = 0,
                    //ChequeAmount = 0,
                    DetailHead = entity.Bill.DetailHead.ToString(),
                    HeadOfAccounts = new HOAChain
                    {
                        Demand = entity.Bill.Demand,
                        DetailHead = entity.Bill.DetailHead,
                        MajorHead = entity.Bill.MajorHead,
                        MinorHead = entity.Bill.MinorHead,
                        SchemeHead = entity.Bill.SchemeHead,
                        SubMajorHead = entity.Bill.SubMajorHead,
                        VotedCharged = entity.Bill.VotedCharged,
                    },
                });
            return payMandateShortListDTO;
        }
        public async Task<bool> NewShortList(long loggendInUserId, List<NewShortlistDTO> newShortlistDTO)
        {
            return await _tokenRepository.PaymandateShortList(loggendInUserId, Helper.JSONHelper.ObjectToJson(newShortlistDTO));
        }
        //public async Task<EKuber> GetXMLData()
        //{
        //    EKuber data = new EKuber
        //    {
        //        requestPayload = new RequestPayload
        //        {
        //            AppHdr = new AppHdr
        //            {
        //                Fr = new Fr(new OrgId(new Id(new OrgId(new Othr("123456"))))),
        //                To = new To(new FIId(new FinInstnId(new ClrSysMmbId("XYZ")))),
        //                BizMsgIdr = "CORE2102024123456",
        //                MsgDefIdr = "pain.masla",
        //                BizSvc = "JANATA",
        //                CreDt = DateTime.Now
        //            },
        //            Document = new Document
        //            {
        //                CstmrCdtTrfInitn = new CstmrCdtTrfInitn
        //                {
        //                    GrpHdr = new GrpHdr
        //                    {
        //                        MsgId = "CORE2102024123456",
        //                        CreDtTm = DateTime.Now,
        //                        Authstn = "ALL",
        //                        NbOfTxs = 2000000,
        //                        CtrlSum = 17000000001.00m,
        //                        InitgPty = new InitgPty
        //                        {
        //                            Nm = "NOWARE",
        //                            Id = new Id(new OrgId(new Othr("123456"))),
        //                            CtctDtls = new ContactDetails { EmailAdr = "test@email.com" }
        //                        }
        //                    },
        //                    PmtInf = new PmtInf
        //                    {
        //                        PmtInfId = "1234CODE1234566",
        //                        PmtMtd = "MRF",
        //                        BtchBookg = true,
        //                        NbOfTxs = 2,
        //                        CtrlSum = 17000000001.00m,
        //                        PmtTpInf = new PaymentTypeInformation
        //                        {
        //                            InstrPrty = "HIGHLIGHT",
        //                            SvcLvl = new ServiceLevel { Prtry = "PYP" }
        //                        },
        //                        ReqdExctnDt = new DateTime(2025, 10, 10),
        //                        Dbtr = new Debtor
        //                        {
        //                            Nm = "FIN",
        //                            PstlAdr = new PostalAddress { Dept = "KFC", SubDept = "KFC123455" },
        //                            Id = new Id(new OrgId(new Othr("123456")))
        //                        },
        //                        DbtrAcct = new DebtorAccount { Id = new Id(new Othr("123456")) },
        //                        DbtrAgt = new DebtorAgent { FinInstnId = new FinInstnId(new ClrSysMmbId("MAC147852")) },
        //                        CdtTrfTxInf = new List<CreditTransferTransactionInformation>
        //                {
        //                    new CreditTransferTransactionInformation
        //                    {
        //                        PmtId = new PaymentId { InstrId = "17000000055", EndToEndId = "CORE2102024123456" },
        //                        Amt = new Amount { Amt = 12515.00m, CcyOfTrf = "USD" },
        //                        CdtrAgt = new CreditorAgent
        //                        {
        //                            FinInstnId = new FinInstnId(new ClrSysMmbId("ECO123456")),
        //                            BrnchId = new BranchId("ECO123456")
        //                        },
        //                        Cdtr = new Creditor { Nm = "AMAZON" },
        //                        CdtrAcct = new CreditorAccount { Id = new Id(new Othr("147852369")), Tp = new Type { Cd = "1000" } }
        //                    },
        //                    new CreditTransferTransactionInformation
        //                    {
        //                        PmtId = new PaymentId { InstrId = "1700000099", EndToEndId = "CORE2102024123457" },
        //                        Amt = new Amount { Amt = 72200.00m, CcyOfTrf = "USD" },
        //                        CdtrAgt = new CreditorAgent
        //                        {
        //                            FinInstnId = new FinInstnId(new ClrSysMmbId("ECO147852369")),
        //                            BrnchId = new BranchId("ECO147852369")
        //                        },
        //                        Cdtr = new Creditor { Nm = "VAT" },
        //                        CdtrAcct = new CreditorAccount { Id = new Id(new Othr("147852369")), Tp = new Type { Cd = "1000" } }
        //                    }
        //                }
        //                    }
        //                }
        //            }
        //        }
        //};
        //    return data;
        //}
        public EKuber GetXMLData()
        {
            string UDCH = "0116";
            string DbtrAcctNo = "01516701174";
            string DebtorIFSC = "RBIS0GOWBEP";
            string FileSequenceNumber = "0001";
            string DateofAuthorization = "20240415";
            string LotNumber = "CKP000210";
            string LotType = "01";
            string PaymentAdviceDate = "20240415";
            string PaymentAdviceYYMM = "202402";
            string ReservedField = DbtrAcctNo.PadLeft(12, '0');
            string FileName = "EPV8" + UDCH + ReservedField + DateofAuthorization + FileSequenceNumber;
            EKuber data = new EKuber
            {
                requestPayload = new RequestPayload
                {

                    AppHdr = new AppHdr
                    {
                        Fr = new Fr { OrgId = new OrgId { Id = new Id { OrgId = new OrgId { Othr = new Othr { Id = UDCH } } } } },
                        To = new To
                        {
                            FIId = new FIId
                            {
                                FinInstnId = new FinInstnId
                                {
                                    ClrSysMmbId = new ClrSysMmbId
                                    {
                                        MmbId = "RBI"
                                    }
                                }
                            }
                        },
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
                                NbOfTxs = 2,
                                CtrlSum = 41637.00m,
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
                                        EmailAdr = "user@email.com"
                                    }
                                }
                            },
                            PmtInf = new PmtInf
                            {
                                PmtInfId = UDCH + LotNumber + LotType + PaymentAdviceDate + PaymentAdviceYYMM,
                                PmtMtd = "TRF",
                                BtchBookg = "true",
                                NbOfTxs = 2,
                                CtrlSum = 41637.00m,
                                PmtTpInf = new PaymentTypeInformation
                                {
                                    InstrPrty = "HIGH",
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
                                CdtTrfTxInf = new List<CreditTransferTransactionInformation>
                                {
                                    new CreditTransferTransactionInformation
                                    {
                                        PmtId = new PaymentId
                                        {
                                            InstrId = "DRNPAYEE001",
                                            EndToEndId = "EP"+UDCH+ReservedField+DateTimeHelper.GetJulianDate(DateTime.Now)+"000001"
                                        },
                                        Amt = new Amount
                                        {
                                        InstdAmt = new InstdAmt{
                                                Amt = 40916.00m,
                                                CcyOfTrf = "INR"
                                            }

                                        },
                                        CdtrAgt = new CreditorAgent
                                        {
                                            FinInstnId = new FinInstnId
                                            {
                                                ClrSysMmbId = new ClrSysMmbId
                                                {
                                                    MmbId = "SBIN0014061"
                                                }
                                            },
                                            BrnchId = new BranchId
                                            {
                                                Id = "SBIN0014061"
                                            }
                                        },
                                        Cdtr = new Creditor
                                        {
                                            Nm = "FARM FIELD PVT LTD"
                                        },
                                        CdtrAcct = new CreditorAccount
                                        {
                                            Id = new Id
                                            {
                                                Othr = new Othr
                                                {
                                                    Id = "69174199438"
                                                }
                                            },
                                            Tp = new Type
                                            {
                                                Cd = "10"
                                            }
                                        }
                                    },
                                    new CreditTransferTransactionInformation
                                    {
                                        PmtId = new PaymentId
                                        {
                                            InstrId = "DRNPAYEE002",
                                            EndToEndId = "EP"+UDCH+ReservedField+DateTimeHelper.GetJulianDate(DateTime.Now)+"0002"
                                        },
                                        Amt = new Amount
                                        {
                                            InstdAmt = new InstdAmt{
                                                Amt = 721.00m,
                                                CcyOfTrf = "INR"
                                            }
                                        },
                                        CdtrAgt = new CreditorAgent
                                        {
                                            FinInstnId = new FinInstnId
                                            {
                                                ClrSysMmbId = new ClrSysMmbId
                                                {
                                                    MmbId = "SBIN0014062"
                                                }
                                            },
                                            BrnchId = new BranchId
                                            {
                                                Id = "SBIN0014062"
                                            }
                                        },
                                        Cdtr = new Creditor
                                        {
                                            Nm = "Test Name"
                                        },
                                        CdtrAcct = new CreditorAccount
                                        {
                                            Id = new Id
                                            {
                                                Othr = new Othr
                                                {
                                                    Id = "24022000007067"
                                                }
                                            },
                                            Tp = new Type
                                            {
                                                Cd = "10"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }



            };
            return data;
        }
        public EKuber GetRecallXMLData()
        {
            string UDCH = "0116";
            string DbtrAcctNo = "01516701174";
            string DebtorIFSC = "RBIS0GOWBEP";
            string FileSequenceNumber = "0001";
            string DateofAuthorization = "20240415";
            string LotNumber = "CKP000210";
            string LotType = "01";
            string PaymentAdviceDate = "20240415";
            string PaymentAdviceYYMM = "202402";
            string ReservedField = DbtrAcctNo.PadLeft(12, '0');
            string FileName = "EPV8" + UDCH + ReservedField + DateofAuthorization + FileSequenceNumber;
            EKuber data = new EKuber
            {
                requestPayload = new RequestPayload
                {
                    AppHdr = new AppHdr
                    {
                        Fr = new Fr
                        {
                            OrgId = new OrgId
                            {
                                Id = new Id
                                {
                                    OrgId = new OrgId
                                    {
                                        Othr = new Othr
                                        {
                                            Id = "116"
                                        }
                                    }
                                }
                            }
                        },
                        To = new To
                        {
                            FIId = new FIId
                            {
                                FinInstnId = new FinInstnId
                                {
                                    ClrSysMmbId = new ClrSysMmbId
                                    {
                                        MmbId = "RBI"
                                    }
                                }
                            }
                        },
                        BizMsgIdr = "EPV80116001516701101201909170175",
                        MsgDefIdr = "pain.007.001.07",
                        BizSvc = "CustomerPaymentReversalV07",
                        CreDt = DateTime.Now
                    },
                    Document = new Document
                    {
                        CstmrPmtRvsl = new CstmrPmtRvsl
                        {
                            GrpHdr = new GrpHdr
                            {
                                MsgId = "EPV80116001516701101201909170175",
                                CreDtTm = DateTime.Now,
                                NbOfTxs = 1,
                                CtrlSum = 2000,
                                GrpRvsl = true,
                                InitgPty = new InitgPty
                                {
                                    Nm = "GOVERNMENT OF WEST BENGAL",
                                    Id = new Id
                                    {
                                        OrgId = new OrgId
                                        {
                                            Othr = new Othr
                                            {
                                                Id = "116"
                                            }
                                        }
                                    }
                                }
                            },
                            OrgnlGrpInf = new OrgnlGrpInf
                            {
                                OrgnlMsgId = "EPV80116001516701101201909170175",
                                OrgnlMsgNmId = "pain.001.001.08",
                                OrgnlCreDtTm = DateTime.Now
                            },
                            OrgnlPmtInfAndRvsl = new OrgnlPmtInfAndRvsl
                            {
                                TxInf = new TxInf
                                {
                                    RvslId = "EPV80116001516701101201909170175",
                                    OrgnlInstrId = 7405003,
                                    OrgnlEndToEndId = "EP011600500300007419260000001",
                                    OrgnlInstdAmt = 2000,
                                    RvslRsnInf = new RvslRsnInf
                                    {
                                        Rsn = new Rsn
                                        {
                                            Cd = "RC0005"
                                        },
                                        AddtlInf = "Wrong Payment Date"

                                    }
                                }
                            }

                        }
                    }
                }
            };
            return data;
        }

        public void GenerateXML(EKuber kuber, string fileName, string filePath)
        {
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = new System.Text.UTF8Encoding(false) // set false to not include BOM
            };

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                // Start RequestPayload element
                writer.WriteStartElement("RequestPayload");
                // Write AppHdr element
                writer.WriteStartElement("AppHdr");
                // Write Fr element
                writer.WriteStartElement("Fr");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Id");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");
                //*UDCH code of Govt - Message Sender Identification (or) System code e.g. PFMS
                writer.WriteElementString("Id", kuber.requestPayload.AppHdr.Fr.OrgId.Id.OrgId.Othr.Id);//???
                writer.WriteEndElement(); // Othr
                writer.WriteEndElement(); // OrgId
                writer.WriteEndElement(); // Id
                writer.WriteEndElement(); // OrgId
                writer.WriteEndElement(); // Fr

                // Write To element
                writer.WriteStartElement("To");
                writer.WriteStartElement("FIId");
                writer.WriteStartElement("FinInstnId");
                writer.WriteStartElement("ClrSysMmbId");
                // *Message Receiver identification – Default “RBI”
                writer.WriteElementString("MmbId", "RBI");//DEFAULT "RBI"
                writer.WriteEndElement(); // ClrSysMmbId
                writer.WriteEndElement(); // FinInstnId
                writer.WriteEndElement(); // FIId
                writer.WriteEndElement(); // To
                // *Identification of the Business Message Same as file name
                writer.WriteElementString("BizMsgIdr", fileName);//??? WHAT WIIL BE THE FILE NAME FORMAT ???
                // *Identification of the Message Definition Default pain.001.001.08
                writer.WriteElementString("MsgDefIdr", "pain.001.001.08");//DEFAULT
                // *Business Service
                writer.WriteElementString("BizSvc", "CustomerCreditTransferInitiationV08");//DEFAULT
                // *Creation Date
                writer.WriteElementString("CreDt", currentDateTime);

                writer.WriteEndElement(); // AppHdr
                // Write Document element
                writer.WriteStartElement("Document");
                // Write CstmrCdtTrfInitn element
                writer.WriteStartElement("CstmrCdtTrfInitn");
                // Write GrpHdr element
                writer.WriteStartElement("GrpHdr");
                /*
                *  Point to point reference, as assigned by the chain to unambiguously identify the message. 
                *  instructing party, and sent to the next party in the chain.Same as file name
                */
                writer.WriteElementString("MsgId", fileName);//?? IS IT ALLWAS THE FILE NAME ???
                // *Date and time at which the message was created.
                writer.WriteElementString("CreDtTm", currentDateTime);
                writer.WriteStartElement("Authstn");
                /*
                * Proprietary code can be 
                * -"PART" - Originating system confirms that, the file can be authorized partially in case of exceptions
                * -"ALL" - Originating system confirms that, the file should be authorized only if all the payments are accepted 
                */
                writer.WriteElementString("Prtry", kuber.requestPayload.Document.CstmrCdtTrfInitn.GrpHdr.Authstn.Prtry);
                writer.WriteEndElement(); // Authstn
                // *Total number of transactions in the entire message.
                writer.WriteElementString("NbOfTxs", kuber.requestPayload.Document.CstmrCdtTrfInitn.GrpHdr.NbOfTxs.ToString());//TODO::Change it to the length of transactions (Number Of Transactions)
                /*
                * It is a originator's option to include. If included, value will be checked. The sum is the hash total of values in Instructed Amount. (Total Amount)
                */
                writer.WriteElementString("CtrlSum", kuber.requestPayload.Document.CstmrCdtTrfInitn.GrpHdr.CtrlSum.ToString());//TODO:: Change it to Total Amount

                // Write InitgPty element
                writer.WriteStartElement("InitgPty");
                // *Name of the party initiates the payment
                writer.WriteElementString("Nm", kuber.requestPayload.Document.CstmrCdtTrfInitn.GrpHdr.InitgPty.Nm);//TODO:: Change it to Name of the party initiates
                writer.WriteStartElement("Id");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");
                // *UDCH Code of Party that initiates the payment
                writer.WriteElementString("Id", kuber.requestPayload.Document.CstmrCdtTrfInitn.GrpHdr.InitgPty.Id.OrgId.Othr.Id);//???
                writer.WriteEndElement(); // Othr
                writer.WriteEndElement(); // OrgId
                writer.WriteEndElement(); // Id
                writer.WriteStartElement("CtctDtls");
                writer.WriteElementString("EmailAdr", kuber.requestPayload.Document.CstmrCdtTrfInitn.GrpHdr.InitgPty.CtctDtls.EmailAdr);//TODO:: Change it to email
                writer.WriteEndElement(); // CtctDtls
                writer.WriteEndElement(); // InitgPty

                writer.WriteEndElement(); // GrpHdr

                // Write PmtInf element
                writer.WriteStartElement("PmtInf");
                /*
                  Unique identification, as assigned by a sending party, to unambiguously identify the payment information group within the message.Field Format suggested by RBI.
                 -Advice Sender UDCH Code(4)+AdviceNumber(9)+Sub Advice Number(2) + Advice Date(8 - YYYYMMDD) + Month and Year(6) YYYYMM - -LPAD with zeros for the individual fields for actual size                                                
                */
                writer.WriteElementString("PmtInfId", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.PmtInfId);//TODO:: Change it to Payment Information Identification
                /*
                 Specifies the means of payment that will be used to move the amount of money. 
                 -TRF= CreditTransfer: Transfer of an amount of money in the books of the account servicer., 
                 -TRA=TransferAdvice Transfer of an amount of money in the books of the account servicer. An advice should be sent back to the account owner. Default "TRF"
                */
                writer.WriteElementString("PmtMtd", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.PmtMtd);//?DEFAULT?
                /*
                    |true Or false| Default "true"
                    -True will indicate single debit seprately for all credit transactions
                    -False will require individual debit for each credit 
                */
                writer.WriteElementString("BtchBookg", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.BtchBookg.ToString());//DEFAULT
                /*
                    Total number of transactions within a Payment Information batch.
                */
                writer.WriteElementString("NbOfTxs", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.NbOfTxs.ToString());//TODO:: Change it to Number Of Transactions
                /*
                    Total Amount within a Payment Information batch
                */
                writer.WriteElementString("CtrlSum", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.CtrlSum.ToString());//TODO:: Change it to Total Amount
                writer.WriteStartElement("PmtTpInf");
                /*
                    Indicator of the urgency or order of importance that the instructing party would like the instructed party to apply to the processing of the instruction. 
                    HIGH - Priority level is high, 
                    NORM - Priority level is normal
                */
                writer.WriteElementString("InstrPrty", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.PmtTpInf.InstrPrty);//TODO:: Change it to Instruction Priority
                writer.WriteStartElement("SvcLvl");
                /*
                * e-Payment Modes – ITRF / NEFT / RTGS / NACH / NECS / CPYM (cheque payment) / APBS - Aadhaar Based Payments/ IGAA - Inter Government Adjustment Advice
                * Mode "ITRF" indicates that, transfer within the accounts maintained at RBI
                */
                writer.WriteElementString("Prtry", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.PmtTpInf.SvcLvl.Prtry);//DEFAULT
                writer.WriteEndElement(); // SvcLvl
                writer.WriteEndElement(); // PmtTpInf
                //*This will be creation date or future date within financial Year (Value Date)
                writer.WriteElementString("ReqdExctnDt", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.ReqdExctnDt.ToString());//? WITCH DATE?

                // Write Dbtr element
                writer.WriteStartElement("Dbtr");
                //*Name by which a party is known and which is usually used to identify that party
                writer.WriteElementString("Nm", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.Dbtr.Nm);//TODO:: Change it to Name
                writer.WriteStartElement("PstlAdr");
                //*Identification of a division
                writer.WriteElementString("Dept", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.Dbtr.PstlAdr.Dept);//TODO:: Change it to Department
                //*Identification of a sub-division
                if (kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.Dbtr.PstlAdr.SubDept != "")
                {
                    writer.WriteElementString("SubDept", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.Dbtr.PstlAdr.SubDept);//TODO:: Change it to Sub Department
                }
                writer.WriteEndElement(); // PstlAdr
                writer.WriteStartElement("Id");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");
                //*UDCH Code of Debtor as assigned by RBI (or) Advice Sender UDCH Code in case of IGAA
                writer.WriteElementString("Id", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.Dbtr.Id.OrgId.Othr.Id);//??
                writer.WriteEndElement(); // Othr
                writer.WriteEndElement(); // OrgId
                writer.WriteEndElement(); // Id
                writer.WriteEndElement(); // Dbtr

                // Write DbtrAcct element
                writer.WriteStartElement("DbtrAcct");
                writer.WriteStartElement("Id");
                writer.WriteStartElement("Othr");
                //*Account Number of Debtor as assigned by RBI (or) Debtor UDCH Code in case of IGAA
                writer.WriteElementString("Id", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.DbtrAcct.Id.Othr.Id);//??
                writer.WriteEndElement(); // Othr
                writer.WriteEndElement(); // Id
                writer.WriteEndElement(); // DbtrAcct

                // Write DbtrAgt element
                writer.WriteStartElement("DbtrAgt");
                writer.WriteStartElement("FinInstnId");
                writer.WriteStartElement("ClrSysMmbId");
                //*IFSC Code of RBI PAD (or) Optional in case of IGAA
                writer.WriteElementString("MmbId", kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.DbtrAgt.FinInstnId.ClrSysMmbId.MmbId);//TODO:: Change it to Member Identification
                writer.WriteEndElement(); // ClrSysMmbId
                writer.WriteEndElement(); // FinInstnId
                writer.WriteEndElement(); // DbtrAgt

                // Write CdtTrfTxInf elements
                foreach (CreditTransferTransactionInformation creditTransferTransactionInformation in kuber.requestPayload.Document.CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf)
                {
                    writer.WriteStartElement("CdtTrfTxInf");
                    writer.WriteStartElement("PmtId");
                    /*
                        Id to be returned only to ordering party in account statement reporting. 
                        Unique identification as assigned by an instructing party for an instructed party to unambiguously identify the instruction. 
                        Sanction No. and Date (or) GOI Scheme Code in case of IGAA (or) Case Id in case of MoE ECR 
                    */
                    writer.WriteElementString("InstrId", creditTransferTransactionInformation.PmtId.InstrId);//TODO:: Change it to Instruction Identification
                    /**
                        Unique identification assigned by the initiating party to unambiguously identify the transaction. 
                        This identification is passed on, unchanged, throughout the entire end-to-end chain.
                        Transaction Id format : 
                        Type(2)+UDCH Code(4)+Reserved Field(12)+Date of Authorization(2) YY+Julian Date (3)+Sequence Number(6)
                    */
                    writer.WriteElementString("EndToEndId", creditTransferTransactionInformation.PmtId.EndToEndId);//TODO:: Change it to End To End Identification
                    writer.WriteEndElement(); // PmtId
                    writer.WriteStartElement("Amt");
                    writer.WriteStartElement("InstdAmt");
                    /**
                        Amount of money to be moved between the debtor and creditor, 
                        before deduction of charges,as ordered by the initiating party.
                    */
                    writer.WriteElementString("Amt", creditTransferTransactionInformation.Amt.InstdAmt.Amt.ToString());//TODO:: Change it to Amount
                    //*Specifies the currency of the, to be transferred amount
                    writer.WriteElementString("CcyOfTrf", creditTransferTransactionInformation.Amt.InstdAmt.CcyOfTrf.ToString());//DEFAULT
                    writer.WriteEndElement(); // InstdAmt
                    writer.WriteEndElement(); // Amt
                    writer.WriteStartElement("CdtrAgt");
                    writer.WriteStartElement("FinInstnId");
                    writer.WriteStartElement("ClrSysMmbId");
                    /*
                        Beneficiary IFSC (or)
                        N/A in case of APBS (or)
                        Optional in case of IGAA
                    */
                    writer.WriteElementString("MmbId", creditTransferTransactionInformation.CdtrAgt.FinInstnId.ClrSysMmbId.MmbId);//TODO:: Change it to Beneficiary IFSC
                    writer.WriteEndElement(); // ClrSysMmbId
                    writer.WriteEndElement(); // FinInstnId
                    writer.WriteStartElement("BrnchId");
                    /*
                        Unique and unambiguous identification of a branch of a financial institution.
                    */
                    writer.WriteElementString("Id", creditTransferTransactionInformation.CdtrAgt.BrnchId.Id);//TODO:: Change it to branch of a financial institution
                    writer.WriteEndElement(); // BrnchId
                    writer.WriteEndElement(); // CdtrAgt
                    writer.WriteStartElement("Cdtr");
                    /*
                        Creditor Name
                    */
                    writer.WriteElementString("Nm", creditTransferTransactionInformation.Cdtr.Nm);//TODO:: Change it to Creditor Name
                    writer.WriteEndElement(); // Cdtr
                    writer.WriteStartElement("CdtrAcct");
                    writer.WriteStartElement("Id");
                    writer.WriteStartElement("Othr");
                    // Account Number of the Beneficiary (or) Beneficiary Aadhaar Number in case of APBS (or) Contra Party Code / Creditor UDCH code in case of IGAA
                    writer.WriteElementString("Id", creditTransferTransactionInformation.CdtrAcct.Id.Othr.Id);//TODO:: Change it to Account Number of the Beneficiary
                    writer.WriteEndElement(); // Othr
                    writer.WriteEndElement(); // Id
                    writer.WriteStartElement("Tp");
                    // Specifies the nature, or use of the account
                    writer.WriteElementString("Cd", creditTransferTransactionInformation.CdtrAcct.Tp.Cd);//TODO:: Change it to ???
                    writer.WriteEndElement(); // Tp
                    writer.WriteEndElement(); // CdtrAcct
                    writer.WriteEndElement(); // CdtTrfTxInf
                }

                writer.WriteEndElement(); // PmtInf

                writer.WriteEndElement(); // CstmrCdtTrfInitn

                writer.WriteEndElement(); // Document

                writer.WriteEndElement(); // RequestPayload

                writer.WriteEndDocument();
            }
        }
        public EKuber GetDNAcknowledgementXMLData(string fileName, string acknowledgementType, string eventCode)
        {
            string FullFileName = acknowledgementType + fileName;
            EKuber data = new EKuber
            {
                requestPayload = new RequestPayload
                {
                    AppHdr = new AppHdr
                    {
                        Fr = new Fr
                        {
                            OrgId = new OrgId

                            {
                                Id = new Id
                                {
                                    OrgId = new OrgId
                                    {
                                        Othr = new Othr
                                        {
                                            Id = "116"
                                        }
                                    }

                                }
                            }
                        },
                        To = new To
                        {
                            FIId = new FIId
                            {
                                FinInstnId = new FinInstnId
                                {
                                    ClrSysMmbId = new ClrSysMmbId
                                    {
                                        MmbId = "RBI"
                                    }
                                }
                            }
                        },
                        BizMsgIdr = FullFileName,
                        MsgDefIdr = "admi.004.001.02",
                        BizSvc = "SystemEventNotificationV02",
                        CreDt = DateTime.Now,
                    },
                    Document = new Document
                    {
                        SysEvtNtfctn = new SysEvtNtfctn
                        {
                            EvtInf = new EvtInf
                            {
                                EvtCd = eventCode,
                                EvtTm = ""
                            }
                        }
                    }
                }
            };
            return data;
        }
        public void GenerateDNACKXML(EKuber kuber, string fileName, string filePath)
        {
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = new System.Text.UTF8Encoding(false) // set false to not include BOM
            };

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                // Start RequestPayload element
                writer.WriteStartElement("RequestPayload");
                // Write AppHdr element
                writer.WriteStartElement("AppHdr");
                // Write Fr element
                writer.WriteStartElement("Fr");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Id");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");
                //*UDCH code of Govt - Message Sender Identification (or) System code e.g. PFMS
                writer.WriteElementString("Id", kuber.requestPayload.AppHdr.Fr.OrgId.Id.OrgId.Othr.Id);//???
                writer.WriteEndElement(); // Othr
                writer.WriteEndElement(); // OrgId
                writer.WriteEndElement(); // Id
                writer.WriteEndElement(); // OrgId
                writer.WriteEndElement(); // Fr

                // Write To element
                writer.WriteStartElement("To");
                writer.WriteStartElement("FIId");
                writer.WriteStartElement("FinInstnId");
                writer.WriteStartElement("ClrSysMmbId");
                // *Message Receiver identification – Default “RBI”
                writer.WriteElementString("MmbId", kuber.requestPayload.AppHdr.To.FIId.FinInstnId.ClrSysMmbId.MmbId);//DEFAULT "RBI"
                writer.WriteEndElement(); // ClrSysMmbId
                writer.WriteEndElement(); // FinInstnId
                writer.WriteEndElement(); // FIId
                writer.WriteEndElement(); // To
                // *Identification of the Business Message Same as file name
                writer.WriteElementString("BizMsgIdr", kuber.requestPayload.AppHdr.BizMsgIdr);//??? WHAT WIIL BE THE FILE NAME FORMAT ???
                // *Identification of the Message Definition Default 
                writer.WriteElementString("MsgDefIdr", kuber.requestPayload.AppHdr.MsgDefIdr);//DEFAULT
                // *Business Service
                writer.WriteElementString("BizSvc", kuber.requestPayload.AppHdr.BizSvc);//DEFAULT
                // *Creation Date
                writer.WriteElementString("CreDt", currentDateTime);

                writer.WriteEndElement(); // AppHdr
                // Write Document element
                writer.WriteStartElement("Document");
                // Write SysEvtNtfctn element
                writer.WriteStartElement("SysEvtNtfctn");
                writer.WriteStartElement("EvtInf");
                writer.WriteElementString("EvtCd", kuber.requestPayload.Document.SysEvtNtfctn.EvtInf.EvtCd);
                writer.WriteElementString("EvtTm", currentDateTime);
                writer.WriteEndElement(); // EvtInf

                writer.WriteEndElement(); // SysEvtNtfctn

                writer.WriteEndElement(); // Document

                writer.WriteEndElement(); // RequestPayload

                writer.WriteEndDocument();
            }
        }
        public void GenerateRecallXMLData(EKuber eKuber)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EKuber));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            using (TextWriter writer = new StreamWriter("fileName.xml"))
            {
                serializer.Serialize(writer, eKuber,ns);
            }
        }
    }
}
