using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("load_bids")]
[Index("LoadId", Name = "idx_load_bids_load")]
public partial class LoadBid
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("organisation_id")]
    public string OrganisationId { get; set; } = null!;

    [Column("load_id")]
    public string LoadId { get; set; } = null!;

    [Column("driver_id")]
    public string DriverId { get; set; } = null!;

    [Column("truck_id")]
    public string? TruckId { get; set; }

    [Column("offer_amount")]
    public double OfferAmount { get; set; }

    [Column("currency")]
    public string? Currency { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "DATETIME")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "DATETIME")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("DriverId")]
    [InverseProperty("LoadBids")]
    public virtual User Driver { get; set; } = null!;

    [ForeignKey("LoadId")]
    [InverseProperty("LoadBids")]
    public virtual Load Load { get; set; } = null!;

    [ForeignKey("OrganisationId")]
    [InverseProperty("LoadBids")]
    public virtual Organisation Organisation { get; set; } = null!;

    [ForeignKey("TruckId")]
    [InverseProperty("LoadBids")]
    public virtual Truck? Truck { get; set; }
}
