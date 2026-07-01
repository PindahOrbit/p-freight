using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("trips")]
[Index("DriverId", Name = "idx_trips_driver")]
public partial class Trip
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
    public string TruckId { get; set; } = null!;

    [Column("agreed_price")]
    public double AgreedPrice { get; set; }

    [Column("currency")]
    public string? Currency { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("started_at", TypeName = "DATETIME")]
    public DateTime? StartedAt { get; set; }

    [Column("completed_at", TypeName = "DATETIME")]
    public DateTime? CompletedAt { get; set; }

    [Column("created_at", TypeName = "DATETIME")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "DATETIME")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("DriverId")]
    [InverseProperty("Trips")]
    public virtual User Driver { get; set; } = null!;

    [ForeignKey("LoadId")]
    [InverseProperty("Trips")]
    public virtual Load Load { get; set; } = null!;

    [ForeignKey("OrganisationId")]
    [InverseProperty("Trips")]
    public virtual Organisation Organisation { get; set; } = null!;

    [InverseProperty("Trip")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("Trip")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Trip")]
    public virtual ICollection<TripLocation> TripLocations { get; set; } = new List<TripLocation>();

    [ForeignKey("TruckId")]
    [InverseProperty("Trips")]
    public virtual Truck Truck { get; set; } = null!;
}
