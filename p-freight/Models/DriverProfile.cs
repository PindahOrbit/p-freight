using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("driver_profiles")]
public partial class DriverProfile
{
    [Key]
    [Column("user_id")]
    public string UserId { get; set; } = null!;

    [Column("organisation_id")]
    public string OrganisationId { get; set; } = null!;

    [Column("license_number")]
    public string? LicenseNumber { get; set; }

    [Column("is_verified", TypeName = "BOOLEAN")]
    public bool? IsVerified { get; set; }

    [Column("rating")]
    public double? Rating { get; set; }

    [Column("total_trips")]
    public int? TotalTrips { get; set; }

    [Column("current_location_lat")]
    public double? CurrentLocationLat { get; set; }

    [Column("current_location_lng")]
    public double? CurrentLocationLng { get; set; }

    [Column("is_online", TypeName = "BOOLEAN")]
    public bool? IsOnline { get; set; }

    [Column("created_at", TypeName = "DATETIME")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "DATETIME")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("OrganisationId")]
    [InverseProperty("DriverProfiles")]
    public virtual Organisation Organisation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("DriverProfile")]
    public virtual User User { get; set; } = null!;
}
