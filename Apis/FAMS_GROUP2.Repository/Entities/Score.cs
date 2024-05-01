using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class Score : BaseEntity
{

    public int? StudentId { get; set; }

    public int? ClassId { get; set; }

    public double? Quiz1 { get; set; }

    public double? Quiz2 { get; set; }

    public double? Quiz3 { get; set; }

    public double? Quiz4 { get; set; }

    public double? Quiz5 { get; set; }

    public double? Quiz6 { get; set; }

    /// <summary>
    /// Average off Q1 - Q6
    /// </summary>
    public double? QuizAvg { get; set; }

    public double? QuizFinal { get; set; }

    public double? Asm1 { get; set; }

    public double? Asm2 { get; set; }

    public double? Asm3 { get; set; }

    public double? Asm4 { get; set; }

    public double? Asm5 { get; set; }

    /// <summary>
    /// Avarage of Assignment
    /// </summary>
    public double? AsmAvg { get; set; }

    public double? PracticeFinal { get; set; }

    public double? Audit { get; set; }

    public double? Gpamodule { get; set; }


    public int? LevelModule { get; set; }

    public string? Status { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Class? Class { get; set; }
}
