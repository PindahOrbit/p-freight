using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("reviews")]
public partial class Review
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("organisation_id")]
    public string OrganisationId { get; set; } = null!;

    [Column("trip_id")]
    public string TripId { get; set; } = null!;

    [Column("reviewer_id")]
    public string ReviewerId { get; set; } = null!;

    [Column("reviewee_id")]
    public string RevieweeId { get; set; } = null!;

    [Column("rating")]
    public int Rating { get; set; }

    [Column("comment")]
    public string? Comment { get; set; }

    [Column("created_at", TypeName = "DATETIME")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("OrganisationId")]
    [InverseProperty("Reviews")]
    public virtual Organisation Organisation { get; set; } = null!;

    [ForeignKey("RevieweeId")]
    [InverseProperty("ReviewReviewees")]
    public virtual User Reviewee { get; set; } = null!;

    [ForeignKey("ReviewerId")]
    [InverseProperty("ReviewReviewers")]
    public virtual User Reviewer { get; set; } = null!;

    [ForeignKey("TripId")]
    [InverseProperty("Reviews")]
    public virtual Trip Trip { get; set; } = null!;
}
