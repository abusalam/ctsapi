namespace CTS_BE.PensionEnum
{
    // [Flags]
    public enum PensionStatusFlag
    {
        PpoSuspended,
        PpoRunning,
        PpoApproved,
        PpoBankAccountApproved,
        FirstPensionBillApproved,
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
    }
}