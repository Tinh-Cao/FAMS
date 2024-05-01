using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class Account : BaseEntity
{
    public string? UnsignFullName { get; set; } = "";

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; } = "";

    public string? PhoneNumber { get; set; }

    public DateTime? Dob { get; set; }

    public bool? Gender { get; set; }

    public string? UnsignAddress { get; set; } = "";

    public string? Address { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public int? RoleId { get; set; }

    public string? Image { get; set; } = "";

    public virtual ICollection<ClassAccount> ClassAccountAdmins { get; set; } = new List<ClassAccount>();

    public virtual ICollection<ClassAccount> ClassAccountTrainers { get; set; } = new List<ClassAccount>();

    public virtual ICollection<EmailSend> EmailSends { get; set; } = new List<EmailSend>();

    public virtual Role? Role { get; set; }
}
