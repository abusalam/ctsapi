﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("allotment_transactions", Schema = "bantan")]
public partial class AllotmentTransaction
{
    [Key]
    [Column("allotment_id")]
    public long AllotmentId { get; set; }

    [Column("transaction_id")]
    public long? TransactionId { get; set; }

    [Column("sanction_id")]
    public long? SanctionId { get; set; }

    [Column("memo_number")]
    [StringLength(20)]
    public string? MemoNumber { get; set; }

    [Column("memo_date")]
    public DateOnly? MemoDate { get; set; }

    [Column("from_allotment_id")]
    public long? FromAllotmentId { get; set; }

    [Column("financial_year")]
    [StringLength(9)]
    public string? FinancialYear { get; set; }

    [Column("sender_user_type")]
    public short? SenderUserType { get; set; }

    [Column("sender_sao_ddo_code")]
    [StringLength(12)]
    public string? SenderSaoDdoCode { get; set; }

    [Column("receiver_user_type")]
    public short? ReceiverUserType { get; set; }

    [Column("receiver_sao_ddo_code")]
    [StringLength(12)]
    public string? ReceiverSaoDdoCode { get; set; }

    [Column("dept_code")]
    [StringLength(2)]
    public string? DeptCode { get; set; }

    [Column("demand_no")]
    [StringLength(2)]
    public string? DemandNo { get; set; }

    [Column("major_head")]
    [StringLength(4)]
    public string? MajorHead { get; set; }

    [Column("submajor_head")]
    [StringLength(2)]
    public string? SubmajorHead { get; set; }

    [Column("minor_head")]
    [StringLength(3)]
    public string? MinorHead { get; set; }

    [Column("plan_status")]
    [StringLength(2)]
    public string? PlanStatus { get; set; }

    [Column("scheme_head")]
    [StringLength(3)]
    public string? SchemeHead { get; set; }

    [Column("detail_head")]
    [StringLength(2)]
    public string? DetailHead { get; set; }

    [Column("subdetail_head")]
    [StringLength(2)]
    public string? SubdetailHead { get; set; }

    [Column("voted_charged")]
    [MaxLength(1)]
    public char? VotedCharged { get; set; }

    [Column("budget_alloted_amount")]
    public decimal? BudgetAllotedAmount { get; set; }

    [Column("reappropriated_amount")]
    public decimal? ReappropriatedAmount { get; set; }

    [Column("augment_amount")]
    public decimal? AugmentAmount { get; set; }

    [Column("surrender_amount")]
    public decimal? SurrenderAmount { get; set; }

    [Column("revised_amount")]
    public decimal? RevisedAmount { get; set; }

    [Column("ceiling_amount")]
    public decimal? CeilingAmount { get; set; }

    [Column("provisional_released_amount")]
    public decimal? ProvisionalReleasedAmount { get; set; }

    [Column("actual_released_amount")]
    public decimal? ActualReleasedAmount { get; set; }

    [Column("map_type")]
    public short? MapType { get; set; }

    [Column("sanction_type")]
    public short? SanctionType { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("allotment_date")]
    public DateOnly? AllotmentDate { get; set; }

    [Column("remarks", TypeName = "character varying")]
    public string? Remarks { get; set; }

    [Column("created_by_userid")]
    public long? CreatedByUserid { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_by_userid")]
    public long? UpdatedByUserid { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }
}
