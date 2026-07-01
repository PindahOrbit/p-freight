using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace p_freight.Models;

[PrimaryKey("UserId", "LoginProvider", "Name")]
[Table("user_tokens")]
public partial class UserToken
{
    [Key]
    public string UserId { get; set; } = null!;

    [Key]
    public string LoginProvider { get; set; } = null!;

    [Key]
    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserTokens")]
    public virtual User User { get; set; } = null!;
}
