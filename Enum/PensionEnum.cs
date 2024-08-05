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

    /// <summary>
    /// P - Percentage; A - Amount;
    /// </summary>
    public sealed class BreakupRateType
    {
        public const char Amount = 'A';
        public const char Percentage = 'P';
    }
}