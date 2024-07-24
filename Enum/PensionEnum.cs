namespace CTS_BE.PensionEnum
{
    [Flags]
    public enum PensionStatusFlag
    {
        PpoApproved = 1,
        FirstPensionGenerated = 2,
        PpoRunning = 4,
        PpoSuspended = 8,
        
    }
}