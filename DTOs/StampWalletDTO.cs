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
        public decimal Denomination { get; set; }

        [Required]
        public short ClearBalance { get; set; }
    }
    
    public class StampWalletBalanceDTO
    {

        public short ClearBalance { get; set; }
    }
}