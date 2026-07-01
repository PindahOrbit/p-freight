using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("users")]
[Index("NormalizedEmail", Name = "EmailIndex")]
[Index("NormalizedUserName", Name = "UserNameIndex", IsUnique = true)]
[Index("OrganisationId", Name = "idx_users_org")]
public partial class User
{
    [Key]
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public int EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public int PhoneNumberConfirmed { get; set; }

    public int TwoFactorEnabled { get; set; }

    public string? LockoutEnd { get; set; }

    public int LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    [Column("organisation_id")]
    public string? OrganisationId { get; set; }

    [InverseProperty("User")]
    public virtual DriverProfile? DriverProfile { get; set; }

    [InverseProperty("Driver")]
    public virtual ICollection<LoadBid> LoadBids { get; set; } = new List<LoadBid>();

    [InverseProperty("Customer")]
    public virtual ICollection<Load> Loads { get; set; } = new List<Load>();

    [InverseProperty("Receiver")]
    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    [InverseProperty("Sender")]
    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    [ForeignKey("OrganisationId")]
    [InverseProperty("Users")]
    public virtual Organisation? Organisation { get; set; }

    [InverseProperty("Payee")]
    public virtual ICollection<Payment> PaymentPayees { get; set; } = new List<Payment>();

    [InverseProperty("Payer")]
    public virtual ICollection<Payment> PaymentPayers { get; set; } = new List<Payment>();

    [InverseProperty("Reviewee")]
    public virtual ICollection<Review> ReviewReviewees { get; set; } = new List<Review>();

    [InverseProperty("Reviewer")]
    public virtual ICollection<Review> ReviewReviewers { get; set; } = new List<Review>();

    [InverseProperty("Driver")]
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();

    [InverseProperty("Owner")]
    public virtual ICollection<Truck> Trucks { get; set; } = new List<Truck>();

    [InverseProperty("User")]
    public virtual ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();

    [InverseProperty("User")]
    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    [InverseProperty("User")]
    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
