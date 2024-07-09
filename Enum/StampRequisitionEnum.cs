namespace CTS_BE.Enum
{
    public enum StampRequisitionStatusEnum
    {
        ForwardedToStampCleck = 30,

        ForwardedToTreasuryOfficer = 31,
        RejectedByStampClerk = 32,

        WaitingForPayment = 33,
        RejectedByTreasuryOfficer = 34,

        WaitingForTreasuryOfficerVerification = 35,
        VerifiedByTreasuryOfficer = 36,

        DeliveredToVendor = 37
    }

}
