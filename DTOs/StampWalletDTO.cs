using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CTS_BE.Common;
using CTS_BE.DAL.Entities;

namespace CTS_BE.DTOs
{
    //public class StampWalletDTO
    //{
    //    public long Id { get; set; }
    //    public string? TreasuryCode { get; set; }
    //    public decimal Denomination { get; set; }
    //    public short ClearBalance { get; set; }
    //    public short LedgerBalance { get; set; }
    //}
    
    public class StampWalletInsertDTO
    {
        [Required]
        public string? TreasuryCode { get; set; }

        [Required]
        public long CombinationId { get; set; }

        [Required]
        public short AddSheet { get; set; }
        [Required] 
        public short AddLabel { get; set; }
    }
    
    public class StampWalletBalanceDTO
    {
        public short SheetLedgerBalance { get; set; }
        public short LabelLedgerBalance { get; set; }
        public string Category { get; set; }
        public decimal Denomination { get; set; }
    }
}