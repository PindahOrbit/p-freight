using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("payments")]
public partial class Payment
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("organisation_id")]
    public string OrganisationId { get; set; } = null!;

    [Column("trip_id")]
    public string TripId { get; set; } = null!;

    [Column("payer_id")]
    public string PayerId { get; set; } = null!;

    [Column("payee_id")]
    public string PayeeId { get; set; } = null!;

    [Column("amount")]
    public double Amount { get; set; }

    [Column("currency")]
    public string? Currency { get; set; }

    [Column("payment_method")]
    public string? PaymentMethod { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "DATETIME")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "DATETIME")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("OrganisationId")]
    [InverseProperty("Payments")]
    public virtual Organisation Organisation { get; set; } = null!;

    [ForeignKey("PayeeId")]
    [InverseProperty("PaymentPayees")]
    public virtual User Payee { get; set; } = null!;

    [ForeignKey("PayerId")]
    [InverseProperty("PaymentPayers")]
    public virtual User Payer { get; set; } = null!;

    [ForeignKey("TripId")]
    [InverseProperty("Payments")]
    public virtual Trip Trip { get; set; } = null!;
}
