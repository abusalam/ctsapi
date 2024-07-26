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
}