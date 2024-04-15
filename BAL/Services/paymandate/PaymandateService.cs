using System.Xml;
using CTS_BE.BAL.Interfaces.paymandate;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;

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
        public void GenerateXML()
        {
            string filePath = "example.xml";
            string fileName = "";
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = System.Text.Encoding.UTF8
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
                writer.WriteElementString("Id", "125");//???
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
                writer.WriteElementString("MmbId", "RBI");//DEFAULT "RBI"
                writer.WriteEndElement(); // ClrSysMmbId
                writer.WriteEndElement(); // FinInstnId
                writer.WriteEndElement(); // FIId
                writer.WriteEndElement(); // To

                writer.WriteElementString("BizMsgIdr", fileName);
                writer.WriteElementString("MsgDefIdr", "pain.001.001.08");//DEFAULT
                writer.WriteElementString("BizSvc", "CustomerCreditTransferInitiationV08");//DEFAULT
                writer.WriteElementString("CreDt", currentDateTime);

                writer.WriteEndElement(); // AppHdr

                // Write Document element
                writer.WriteStartElement("Document");

                // Write CstmrCdtTrfInitn element
                writer.WriteStartElement("CstmrCdtTrfInitn");

                // Write GrpHdr element
                writer.WriteStartElement("GrpHdr");
                writer.WriteElementString("MsgId", fileName);
                writer.WriteElementString("CreDtTm", currentDateTime);
                writer.WriteStartElement("Authstn");
                writer.WriteElementString("Prtry", "ALL");
                writer.WriteEndElement(); // Authstn
                writer.WriteElementString("NbOfTxs", "2");//TODO::Change it to the length of transactions
                writer.WriteElementString("CtrlSum", "41701.00");//TODO:: Change it to Total Amount

                // Write InitgPty element
                writer.WriteStartElement("InitgPty");
                writer.WriteElementString("Nm", "JHARKHAND");//TODO:: Change it to Name of the party initiates
                writer.WriteStartElement("Id");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");
                writer.WriteElementString("Id", "125");//???
                writer.WriteEndElement(); // Othr
                writer.WriteEndElement(); // OrgId
                writer.WriteEndElement(); // Id
                writer.WriteStartElement("CtctDtls");
                writer.WriteElementString("EmailAdr", "test@gmail.com");//TODO:: Change it to email
                writer.WriteEndElement(); // CtctDtls
                writer.WriteEndElement(); // InitgPty

                writer.WriteEndElement(); // GrpHdr

                // Write PmtInf element
                writer.WriteStartElement("PmtInf");
                writer.WriteElementString("PmtInfId", "0125CKP0002100020240210202402");//TODO:: Change it to Payment Information Identification
                writer.WriteElementString("PmtMtd", "TRF");//?DEFAULT?
                writer.WriteElementString("BtchBookg", "true");//DEFAULT
                writer.WriteElementString("NbOfTxs", "2");//TODO:: Change it to Number Of Transactions
                writer.WriteElementString("CtrlSum", "41701.00");//TODO:: Change it to Total Amount
                writer.WriteStartElement("PmtTpInf");
                writer.WriteElementString("InstrPrty", "HIGH");//TODO:: Change it to Instruction Priority
                writer.WriteStartElement("SvcLvl");
                writer.WriteElementString("Prtry", "NEFT");//DEFAULT
                writer.WriteEndElement(); // SvcLvl
                writer.WriteEndElement(); // PmtTpInf
                writer.WriteElementString("ReqdExctnDt", "2024-02-10");//? WITCH DATE?

                // Write Dbtr element
                writer.WriteStartElement("Dbtr");
                //*Name by which a party is known and which is usually used to identify that party
                writer.WriteElementString("Nm", "FINANCE DEPT GoJH");//TODO:: Change it to Name
                writer.WriteStartElement("PstlAdr");
                //*Identification of a division
                writer.WriteElementString("Dept", "CKP");//TODO:: Change it to Department
                //*Identification of a sub-division
                writer.WriteElementString("SubDept", "CKPRWD001");//TODO:: Change it to Sub Department
                writer.WriteEndElement(); // PstlAdr
                writer.WriteStartElement("Id");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");
                //*UDCH Code of Debtor as assigned by RBI (or) Advice Sender UDCH Code in case of IGAA
                writer.WriteElementString("Id", "125");//??
                writer.WriteEndElement(); // Othr
                writer.WriteEndElement(); // OrgId
                writer.WriteEndElement(); // Id
                writer.WriteEndElement(); // Dbtr

                // Write DbtrAcct element
                writer.WriteStartElement("DbtrAcct");
                writer.WriteStartElement("Id");
                writer.WriteStartElement("Othr");
                //*Account Number of Debtor as assigned by RBI (or) Debtor UDCH Code in case of IGAA
                writer.WriteElementString("Id", "01602501045");//??
                writer.WriteEndElement(); // Othr
                writer.WriteEndElement(); // Id
                writer.WriteEndElement(); // DbtrAcct

                // Write DbtrAgt element
                writer.WriteStartElement("DbtrAgt");
                writer.WriteStartElement("FinInstnId");
                writer.WriteStartElement("ClrSysMmbId");
                //*IFSC Code of RBI PAD (or) Optional in case of IGAA
                writer.WriteElementString("MmbId", "RBIS0GOJHEP");//TODO:: Change it to Member Identification
                writer.WriteEndElement(); // ClrSysMmbId
                writer.WriteEndElement(); // FinInstnId
                writer.WriteEndElement(); // DbtrAgt

                // Write CdtTrfTxInf elements
                for (int i = 0; i < 2; i++)
                {
                    writer.WriteStartElement("CdtTrfTxInf");
                    writer.WriteStartElement("PmtId");
                    writer.WriteElementString("InstrId", (i == 0) ? "DRNPAYEE03891A21010022024CKP" : "20RCHR00508E1DNA21010022024CKP");
                    /**
                    * * Unique identification assigned by the initiating party to unambiguously identify the transaction. 
                    * * This identification is passed on, unchanged, throughout the entire end-to-end chain.
                    * *Transaction Id format : 
                    * !Type(2)+UDCH Code(4)+Reserved Field(12)+Date of Authorization(2) YY+Julian Date (3)+Sequence Number(6)
                    */
                    writer.WriteElementString("EndToEndId", (i == 0) ? "EP012500000000039824041001576" : "EP012500000000039824041001577");
                    writer.WriteEndElement(); // PmtId
                    writer.WriteStartElement("Amt");
                    writer.WriteStartElement("InstdAmt");
                    /**
                    * *Amount of money to be moved between the debtor and creditor, 
                    * *before deduction of charges,as ordered by the initiating party.
                    */
                    writer.WriteElementString("Amt", (i == 0) ? "40979.00" : "722.00");
                    writer.WriteElementString("CcyOfTrf", "INR");//DEFAULT
                    writer.WriteEndElement(); // InstdAmt
                    writer.WriteEndElement(); // Amt
                    writer.WriteStartElement("CdtrAgt");
                    writer.WriteStartElement("FinInstnId");
                    writer.WriteStartElement("ClrSysMmbId");
                    writer.WriteElementString("MmbId", (i == 0) ? "SBIN0001672" : "RBIS0GSTPMT");
                    writer.WriteEndElement(); // ClrSysMmbId
                    writer.WriteEndElement(); // FinInstnId
                    writer.WriteStartElement("BrnchId");
                    writer.WriteElementString("Id", (i == 0) ? "SBIN0001672" : "RBIS0GSTPMT");
                    writer.WriteEndElement(); // BrnchId
                    writer.WriteEndElement(); // CdtrAgt
                    writer.WriteStartElement("Cdtr");
                    writer.WriteElementString("Nm", (i == 0) ? "RAIDER SECURITY SERVICES PVT LTD" : "GST");
                    writer.WriteEndElement(); // Cdtr
                    writer.WriteStartElement("CdtrAcct");
                    writer.WriteStartElement("Id");
                    writer.WriteStartElement("Othr");
                    writer.WriteElementString("Id", (i == 0) ? "33544440518" : "24022000007067");
                    writer.WriteEndElement(); // Othr
                    writer.WriteEndElement(); // Id
                    writer.WriteStartElement("Tp");
                    writer.WriteElementString("Cd", (i == 0) ? "10" : "10");
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
    }
}
