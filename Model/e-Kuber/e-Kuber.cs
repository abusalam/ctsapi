using System.Xml.Serialization;

namespace CTS_BE.Model.e_Kuber
{
    public class EKuber
    {
        public RequestPayload? requestPayload { get; set; }
    }

    [XmlRoot("RequestPayload")]
    public class RequestPayload
    {
        [XmlElement("AppHdr")]
        public AppHdr? AppHdr { get; set; }
        [XmlElement("Document")]
        public Document? Document { get; set; }
    }

    public class AppHdr
    {
        [XmlElement("Fr")]
        public Fr? Fr { get; set; }
        [XmlElement("To")]
        public To? To { get; set; }
        [XmlElement("BizMsgIdr")]
        public string? BizMsgIdr { get; set; }
        [XmlElement("MsgDefIdr")]
        public string? MsgDefIdr { get; set; }
        [XmlElement("BizSvc")]
        public string? BizSvc { get; set; }
        [XmlElement("CreDt")]
        public DateTime CreDt { get; set; }
    }

    public class Fr
    {
        [XmlElement("OrgId")]
        public OrgId? OrgId { get; set; }
    }

    public class To
    {
        [XmlElement("FIId")]
        public FIId? FIId { get; set; }
    }

    public class OrgId
    {
        [XmlElement("Id")]
        public Id? Id { get; set; }
        [XmlElement("Othr")]
        public Othr? Othr { get; set; }
    }

    public class Id
    {
        [XmlElement("OrgId")]
        public OrgId? OrgId { get; set; }
        [XmlElement("Othr")]
        public Othr? Othr { get; set; }
    }
    public class Othr
    {
        [XmlElement("Id")]
        public string? Id { get; set; }
    }
    public class FIId
    {
        [XmlElement("FinInstnId")]
        public FinInstnId? FinInstnId { get; set; }
    }

    public class FinInstnId
    {
        [XmlElement("ClrSysMmbId")]
        public ClrSysMmbId? ClrSysMmbId { get; set; }
    }

    public class ClrSysMmbId
    {
        [XmlElement("MmbId")]
        public string? MmbId { get; set; }
    }

    public class Document
    {
        [XmlElement("CstmrCdtTrfInitn")]
        public CstmrCdtTrfInitn? CstmrCdtTrfInitn { get; set; }
        [XmlElement("CstmrPmtRvsl")]
        public CstmrPmtRvsl? CstmrPmtRvsl { get; set; }
        [XmlElement("SysEvtNtfctn")]
        public SysEvtNtfctn? SysEvtNtfctn { get; set; }
    }
    public class SysEvtNtfctn
    {
        [XmlElement("EvtInf")]
        public EvtInf? EvtInf { get; set; }
    }
    public class EvtInf
    {
        [XmlElement("EvtCd")]
        public string EvtCd { get; set; }
        [XmlElement("EvtTm")]
        public string EvtTm { get; set; }

    }
    public class CstmrPmtRvsl
    {
        [XmlElement("GrpHdr")]
        public GrpHdr? GrpHdr { get; set; }
        [XmlElement("OrgnlGrpInf")]
        public OrgnlGrpInf? OrgnlGrpInf { get; set; }
        [XmlElement("OrgnlPmtInfAndRvsl")]
        public OrgnlPmtInfAndRvsl? OrgnlPmtInfAndRvsl { get; set; }
    }
    public class OrgnlPmtInfAndRvsl
    {
        [XmlElement("TxInf")]
        public TxInf? TxInf { get; set; }
    }
    public class TxInf
    {
        [XmlElement("RvslId")]
        public string RvslId { get; set; }
        [XmlElement("OrgnlInstrId")]
        public int OrgnlInstrId { get; set; }
        [XmlElement("OrgnlEndToEndId")]
        public string OrgnlEndToEndId { get; set; }
        [XmlElement("OrgnlInstdAmt")]
        public decimal OrgnlInstdAmt { get; set; }
        public RvslRsnInf RvslRsnInf { get; set; }
    }
    public class RvslRsnInf
    {
        public Rsn Rsn { get; set; }
        public string AddtlInf { get; set; }
    }
    public class Rsn
    {
        public string Cd { get; set; }
    }
    public class OrgnlGrpInf
    {
        public string OrgnlMsgId { get; set; }
        public string OrgnlMsgNmId { get; set; }
        public DateTime OrgnlCreDtTm { get; set; }
    }
    public class CstmrCdtTrfInitn
    {
        public GrpHdr? GrpHdr { get; set; }
        public PmtInf? PmtInf { get; set; }
    }

    public class GrpHdr
    {
        public string? MsgId { get; set; }
        public DateTime CreDtTm { get; set; }
        public Authstn? Authstn { get; set; }
        public int NbOfTxs { get; set; }
        public decimal CtrlSum { get; set; }
        public bool? GrpRvsl { get; set; }
        public InitgPty? InitgPty { get; set; }
    }
    public class Authstn
    {
        public string? Prtry { get; set; }
    }

    public class InitgPty
    {
        public string? Nm { get; set; }
        public Id? Id { get; set; }
        public ContactDetails? CtctDtls { get; set; }
    }

    public class ContactDetails
    {
        public string? EmailAdr { get; set; }
    }

    public class PmtInf
    {
        public string? PmtInfId { get; set; }
        public string? PmtMtd { get; set; }
        public string BtchBookg { get; set; }
        public int NbOfTxs { get; set; }
        public decimal CtrlSum { get; set; }
        public PaymentTypeInformation? PmtTpInf { get; set; }
        public string ReqdExctnDt { get; set; }
        public Debtor? Dbtr { get; set; }
        public DebtorAccount? DbtrAcct { get; set; }
        public DebtorAgent? DbtrAgt { get; set; }
        public List<CreditTransferTransactionInformation>? CdtTrfTxInf { get; set; }
    }

    public class PaymentTypeInformation
    {
        public string? InstrPrty { get; set; }
        public ServiceLevel? SvcLvl { get; set; }
    }

    public class ServiceLevel
    {
        public string? Prtry { get; set; }
    }

    public class Debtor
    {
        public string? Nm { get; set; }
        public PostalAddress? PstlAdr { get; set; }
        public Id? Id { get; set; }
    }

    public class PostalAddress
    {
        public string? Dept { get; set; }
        public string? SubDept { get; set; }
    }

    public class DebtorAccount
    {
        public Id? Id { get; set; }
    }

    public class DebtorAgent
    {
        public FinInstnId? FinInstnId { get; set; }
    }

    public class CreditTransferTransactionInformation
    {
        public PaymentId? PmtId { get; set; }
        public Amount? Amt { get; set; }
        public CreditorAgent? CdtrAgt { get; set; }
        public Creditor? Cdtr { get; set; }
        public CreditorAccount? CdtrAcct { get; set; }
    }

    public class PaymentId
    {
        public string? InstrId { get; set; }
        public string? EndToEndId { get; set; }
    }

    public class Amount
    {
        public InstdAmt? InstdAmt { get; set; }
    }
    public class InstdAmt
    {
        public decimal Amt { get; set; }
        public string? CcyOfTrf { get; set; }
    }

    public class CreditorAgent
    {
        public FinInstnId? FinInstnId { get; set; }
        public BranchId? BrnchId { get; set; }
    }

    public class BranchId
    {
        public string? Id { get; set; }
    }

    public class Creditor
    {
        public string? Nm { get; set; }
    }

    public class CreditorAccount
    {
        public Id? Id { get; set; }
        public Type? Tp { get; set; }
    }

    public class Type
    {
        public string? Cd { get; set; }
    }

}
