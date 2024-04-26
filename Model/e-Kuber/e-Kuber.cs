namespace CTS_BE.Model.e_Kuber
{
    public class EKuber
    {
        public RequestPayload? requestPayload { get; set; }
    }

    public class RequestPayload
    {
        public AppHdr? AppHdr { get; set; }
        public Document? Document { get; set; }
    }

    public class AppHdr
    {
        public Fr? Fr { get; set; }
        public To? To { get; set; }
        public string? BizMsgIdr { get; set; }
        public string? MsgDefIdr { get; set; }
        public string? BizSvc { get; set; }
        public DateTime CreDt { get; set; }
    }

    public class Fr
    {
        public OrgId? OrgId { get; set; }
    }

    public class To
    {
        public FIId? FIId { get; set; }
    }

    public class OrgId
    {
        public Id? Id { get; set; }
        public Othr? Othr { get; set; }
    }

    public class Id
    {
        public OrgId? OrgId { get; set; }
        public Othr? Othr { get; set; }
    }
    public class Othr
    {
        public string? Id { get; set; }
    }
    public class FIId
    {
        public FinInstnId? FinInstnId { get; set; }
    }

    public class FinInstnId
    {
        public ClrSysMmbId? ClrSysMmbId { get; set; }
    }

    public class ClrSysMmbId
    {
        public string? MmbId { get; set; }
    }

    public class Document
    {
        public CstmrCdtTrfInitn? CstmrCdtTrfInitn { get; set; }
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
        public InstdAmt? InstdAmt {get;set;}
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
