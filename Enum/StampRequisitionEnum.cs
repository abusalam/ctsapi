namespace CTS_BE.Enum
{
    public enum StampRequisitionStatusEnum
    {
        ForwardedToStampCleck = 30,

        ForwardedToTreasuryOfficer = 31,
        RejectedByStampClerk = 32,

        WaitingForPayment = 33,
        RejectedByTreasuryOfficer = 34,

        WaitingForPaymentVerification = 35,
        WaitingForDelivery = 36,

        DeliveredToVendor = 37
    }

}
