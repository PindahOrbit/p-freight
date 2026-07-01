using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("trip_locations")]
[Index("TripId", Name = "idx_trip_locs_trip")]
public partial class TripLocation
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("organisation_id")]
    public string OrganisationId { get; set; } = null!;

    [Column("trip_id")]
    public string TripId { get; set; } = null!;

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }

    [Column("recorded_at", TypeName = "DATETIME")]
    public DateTime? RecordedAt { get; set; }

    [ForeignKey("OrganisationId")]
    [InverseProperty("TripLocations")]
    public virtual Organisation Organisation { get; set; } = null!;

    [ForeignKey("TripId")]
    [InverseProperty("TripLocations")]
    public virtual Trip Trip { get; set; } = null!;
}
