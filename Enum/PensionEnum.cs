namespace CTS_BE.PensionEnum
{
    [Flags]
    public enum PensionStatusFlag
    {
        PpoApproved = 1,
        FirstPensionBillGenerated = 2,
        PpoRunning = 4,
        PpoSuspended = 8,
        PpoBankAccountApproved = 16,
    }

    public sealed class PpoStatus
    {
        public const int PpoApproved = 1;
        public const int FirstPensionBillGenerated = 2;
        public const int PpoRunning = 4;
        public const int PpoSuspended = 8;
        public const int PpoBankAccountApproved = 16;
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