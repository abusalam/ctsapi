using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("uploaded_files", Schema = "cts_pension")]
public partial class UploadedFile
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("file_path")]
    [StringLength(500)]
    public string FilePath { get; set; } = null!;

    [Column("file_name")]
    [StringLength(500)]
    public string FileName { get; set; } = null!;

    [Column("file_mime_type")]
    [StringLength(100)]
    public string FileMimeType { get; set; } = null!;

    [Column("contents")]
    public byte[] Contents { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool ActiveFlag { get; set; }

    [InverseProperty("EppoFile")]
    public virtual ICollection<EppoReceipt> EppoReceiptEppoFiles { get; set; } = new List<EppoReceipt>();

    [InverseProperty("PhotoFile")]
    public virtual ICollection<EppoReceipt> EppoReceiptPhotoFiles { get; set; } = new List<EppoReceipt>();

    [InverseProperty("SignatureFile")]
    public virtual ICollection<EppoReceipt> EppoReceiptSignatureFiles { get; set; } = new List<EppoReceipt>();

    [InverseProperty("EppoFile")]
    public virtual ICollection<EppoRevision> EppoRevisions { get; set; } = new List<EppoRevision>();
}
