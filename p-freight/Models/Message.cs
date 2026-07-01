using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[Table("messages")]
public partial class Message
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("organisation_id")]
    public string OrganisationId { get; set; } = null!;

    [Column("load_id")]
    public string LoadId { get; set; } = null!;

    [Column("sender_id")]
    public string SenderId { get; set; } = null!;

    [Column("receiver_id")]
    public string ReceiverId { get; set; } = null!;

    [Column("content")]
    public string Content { get; set; } = null!;

    [Column("is_read", TypeName = "BOOLEAN")]
    public bool? IsRead { get; set; }

    [Column("created_at", TypeName = "DATETIME")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("LoadId")]
    [InverseProperty("Messages")]
    public virtual Load Load { get; set; } = null!;

    [ForeignKey("OrganisationId")]
    [InverseProperty("Messages")]
    public virtual Organisation Organisation { get; set; } = null!;

    [ForeignKey("ReceiverId")]
    [InverseProperty("MessageReceivers")]
    public virtual User Receiver { get; set; } = null!;

    [ForeignKey("SenderId")]
    [InverseProperty("MessageSenders")]
    public virtual User Sender { get; set; } = null!;
}
