namespace CTS_BE.Enum
{
    public enum APIResponseStatus
    {
        Success = 1,
        Warning = 2,
        Error = 3,
        Info = 4,
    }
    public enum BillStatus
    {
        ForwardedToTreasury = 99,
    }
    public enum TokenStatus
    {
        BillReceived = 1,
        FrowardbyDealingAssistant = 2,
        ObjectedbyDealingAssistant = 3,
        FrowardbyAccountant = 4,
        ObjectedbyAccountant = 5,
        FrowardbyTreasuryOfficer = 6,
        ObjectedbyTreasuryOfficer = 7,
        BillClear = 8,
        RetrunMemoGenerated = 9
    }
    public enum BTAmountType
    {
        AG=1,
        Treasury=2,
    }
}
