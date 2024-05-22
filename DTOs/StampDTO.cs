using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CTS_BE.Common;
using CTS_BE.DAL.Entities;

namespace CTS_BE.DTOs
{
    public class StampIndentDTO
    {
        public long StampCombinationId { get; set; }
        public string MemoNumber { get; set; } = null!;
        public DateTime MemoDate { get; set; }
        public string? Remarks { get; set; }
        public short Sheet { get; set; }
        public short Label { get; set; }
        public short Quantity { get; set; }
        public decimal Amount { get; set; }
        public short Status { get; set; }
        public short RaisedByTreasury { get; set; }
        public short? RaisedToTreasury { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}