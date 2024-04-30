namespace CTS_BE.Enum
{
    public enum IndentStatus
    {
        NewIndent = 11,
        FrowardToTreasuryOfficer = 12,
        RejectByTreasuryOfficer = 13,
        ApproveByTreasuryOfficer = 14,
        ApproveByDTA = 15,
        RejectByDTA = 16,

    }
    public enum InvoiceStatus
    {
        NewInvoice = 17,
        FrowardToTreasuryOfficer = 18
    }
    public enum ChequeType
    {
        TreasuryCheque = 1,
        Others = 2,
    }
}
