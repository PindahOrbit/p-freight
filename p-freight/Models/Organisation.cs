using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("organisations")]
public partial class Organisation
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("country")]
    public string? Country { get; set; }

    [Column("created_at", TypeName = "DATETIME")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "DATETIME")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Organisation")]
    public virtual ICollection<DriverProfile> DriverProfiles { get; set; } = new List<DriverProfile>();

    [InverseProperty("Organisation")]
    public virtual ICollection<LoadBid> LoadBids { get; set; } = new List<LoadBid>();

    [InverseProperty("Organisation")]
    public virtual ICollection<Load> Loads { get; set; } = new List<Load>();

    [InverseProperty("Organisation")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    [InverseProperty("Organisation")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("Organisation")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Organisation")]
    public virtual ICollection<TripLocation> TripLocations { get; set; } = new List<TripLocation>();

    [InverseProperty("Organisation")]
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();

    [InverseProperty("Organisation")]
    public virtual ICollection<Truck> Trucks { get; set; } = new List<Truck>();

    [InverseProperty("Organisation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
