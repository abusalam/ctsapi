namespace CTS_BE.PensionEnum
{
    [Flags]
    public enum PensionStatusFlag
    {
        PpoApproved,
        FirstPensionBillApproved,
        PpoRunning,
        PpoSuspended,
        PpoBankAccountApproved,
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