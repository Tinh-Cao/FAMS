using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class Student : BaseEntity
{
    public string? FullName { get; set; }

    public string? UnsignFullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? Dob { get; set; }

    public bool? Gender { get; set; }

    public string? University { get; set; }

    public string? Major { get; set; }

    public string? Skill { get; set; }

    public decimal? YearOfGraduation { get; set; }

    public double? Gpa { get; set; }

    public string? StudentCode { get; set; }

    public string? EnrollmentArea { get; set; }

    public string? Recruiter { get; set; }

    public string? Status { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public string? Image { get; set; } = "";

    public virtual ICollection<StudentCertificate> StudentCertificates { get; set; } = new List<StudentCertificate>();

    public virtual ICollection<EmailSendStudent> EmailSendStudents { get; set; } = new List<EmailSendStudent>();

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();

    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
}
