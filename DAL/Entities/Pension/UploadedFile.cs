using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

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
    [StringLength(500)]
    public string FileMimeType { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool? ActiveFlag { get; set; }

    [InverseProperty("PhotoFile")]
    public virtual ICollection<Nominee> NomineePhotoFiles { get; set; } = new List<Nominee>();

    [InverseProperty("SignatureFile")]
    public virtual ICollection<Nominee> NomineeSignatureFiles { get; set; } = new List<Nominee>();

    [InverseProperty("PhotoFile")]
    public virtual ICollection<Pensioner> PensionerPhotoFiles { get; set; } = new List<Pensioner>();

    [InverseProperty("SignatureFile")]
    public virtual ICollection<Pensioner> PensionerSignatureFiles { get; set; } = new List<Pensioner>();
}
