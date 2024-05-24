using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CTS_BE.Common;
using CTS_BE.DAL.Entities;

namespace CTS_BE.DTOs
{
    public class StampIndentDTO
    {
        public long StampIndentId { get; set; }
        public string MemoNumber { get; set; } = null!;
        public DateTime MemoDate { get; set; }
        public string? Remarks { get; set; }
        public string RaisedByTreasury { get; set; }
        public string? RaisedToTreasury { get; set; }
        public string StmapCategory { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Denomination { get; set; }
        public short LabelPerSheet { get; set; }
        public short Sheet { get; set; }
        public short Label { get; set; }
        public short Quantity { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public short Status { get; set; }
    }
}