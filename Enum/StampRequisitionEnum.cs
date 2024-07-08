namespace CTS_BE.Enum
{
    public enum StampRequisitionStatusEnum
    {
        ForwardedToStampCleck = 30,

        ForwardedToTreasuryOfficer = 31,
        RejectByStampClerk = 32,

        WaitingForPayment = 33,
        RejectByTreasuryOfficer = 34,

        WaitingForTreasuryOfficerVerification = 35,
        VerifiedByTreasuryOfficer = 36,

        DeliveredToVendor = 37
    }

}
