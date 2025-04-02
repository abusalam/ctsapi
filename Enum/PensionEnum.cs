namespace CTS_BE.PensionEnum
{
    // [Flags]
    public enum PensionStatusFlag
    {
        PpoApproved,
        FirstPensionBillApproved,
        PpoRunning,
        PpoSuspended,
        PpoClosed,
    }

    public enum PensionStatusReassonFlag
    {
        Others,
        LifeCertificateSubmitted,
        LifeCertificateNotSubmitted,
        Death,
    }

    /// <summary>
    /// P - Percentage; A - Amount;
    /// </summary>
    public sealed class BreakupRateType
    {
        public const char Amount = 'A';
        public const char Percentage = 'P';
    }

    public sealed class BillType
    {
        public const char FirstBill = 'F';
        public const char RegularBill = 'R';
        public const char ArrearBill = 'A';
    }

    /// <summary>
    /// P - Payment; D - Deduction;
    /// </summary>
    public sealed class BreakupComponentType
    {
        public const char Payment = 'P';
        public const char Deduction = 'D';
    }
}
