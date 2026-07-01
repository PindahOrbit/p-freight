using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("trucks")]
public partial class Truck
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("organisation_id")]
    public string OrganisationId { get; set; } = null!;

    [Column("owner_id")]
    public string OwnerId { get; set; } = null!;

    [Column("truck_type")]
    public string TruckType { get; set; } = null!;

    [Column("registration_number")]
    public string RegistrationNumber { get; set; } = null!;

    [Column("capacity_weight")]
    public double? CapacityWeight { get; set; }

    [Column("is_verified", TypeName = "BOOLEAN")]
    public bool? IsVerified { get; set; }

    [Column("created_at", TypeName = "DATETIME")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "DATETIME")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Truck")]
    public virtual ICollection<LoadBid> LoadBids { get; set; } = new List<LoadBid>();

    [ForeignKey("OrganisationId")]
    [InverseProperty("Trucks")]
    public virtual Organisation Organisation { get; set; } = null!;

    [ForeignKey("OwnerId")]
    [InverseProperty("Trucks")]
    public virtual User Owner { get; set; } = null!;

    [InverseProperty("Truck")]
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
