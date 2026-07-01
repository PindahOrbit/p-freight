using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("loads")]
[Index("OrganisationId", "Status", Name = "idx_loads_org_status")]
public partial class Load
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("organisation_id")]
    public string OrganisationId { get; set; } = null!;

    [Column("customer_id")]
    public string CustomerId { get; set; } = null!;

    [Column("pickup_address")]
    public string PickupAddress { get; set; } = null!;

    [Column("pickup_lat")]
    public double? PickupLat { get; set; }

    [Column("pickup_lng")]
    public double? PickupLng { get; set; }

    [Column("dropoff_address")]
    public string DropoffAddress { get; set; } = null!;

    [Column("dropoff_lat")]
    public double? DropoffLat { get; set; }

    [Column("dropoff_lng")]
    public double? DropoffLng { get; set; }

    [Column("cargo_type")]
    public string CargoType { get; set; } = null!;

    [Column("weight")]
    public double? Weight { get; set; }

    [Column("special_notes")]
    public string? SpecialNotes { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "DATETIME")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "DATETIME")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Loads")]
    public virtual User Customer { get; set; } = null!;

    [InverseProperty("Load")]
    public virtual ICollection<LoadBid> LoadBids { get; set; } = new List<LoadBid>();

    [InverseProperty("Load")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    [ForeignKey("OrganisationId")]
    [InverseProperty("Loads")]
    public virtual Organisation Organisation { get; set; } = null!;

    [InverseProperty("Load")]
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
