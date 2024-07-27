using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[PrimaryKey("OperatorCode", "OperatorName", "PaymentHoa", "ReceiptHoa", "TreasuryCode")]
[Table("operator_master", Schema = "lf_pl_master")]
[Index("OpId", Name = "op_id_unique", IsUnique = true)]
public partial class OperatorMaster
{
    [Key]
    [Column("operator_code")]
    public short OperatorCode { get; set; }

    [Key]
    [Column("operator_name", TypeName = "character varying")]
    public string OperatorName { get; set; } = null!;

    [Column("operator_type")]
    [StringLength(2)]
    public string OperatorType { get; set; } = null!;

    [Key]
    [Column("payment_hoa", TypeName = "character varying")]
    public string PaymentHoa { get; set; } = null!;

    [Key]
    [Column("receipt_hoa", TypeName = "character varying")]
    public string ReceiptHoa { get; set; } = null!;

    [Key]
    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("treasury_name", TypeName = "character varying")]
    public string TreasuryName { get; set; } = null!;

    [Column("op_id")]
    public int OpId { get; set; }

    public virtual ICollection<Advice1> Advice1s { get; set; } = new List<Advice1>();

    public virtual ICollection<Advice> Advices { get; set; } = new List<Advice>();
}
