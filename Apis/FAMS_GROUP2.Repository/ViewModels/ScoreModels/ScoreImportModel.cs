using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FAMS_GROUP2.Repositories.ViewModels.ScoreModels
{
    public class ScoreImportModel
    {
        [Required(ErrorMessage = "ClassID is required!")]
        [Display(Name = "Class ID")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Student Code is required!")]
        [Display(Name = "Student Code")]
        public string? StudentCode { get; set; }

        [Display(Name = "Quiz1")]
        [Range(0, 10)]
        public double? Quiz1 { get; set; }

        [Display(Name = "Quiz2")]
        [Range(0, 10)]
        public double? Quiz2 { get; set; } = null;

        [Display(Name = "Quiz3")]
        [Range(0, 10)]
        public double? Quiz3 { get; set; } = null;

        [Display(Name = "Quiz4")]
        [Range(0, 10)]
        public double? Quiz4 { get; set; } = null;

        [Display(Name = "Quiz5")]
        [Range(0, 10)]
        public double? Quiz5 { get; set; } = null;

        [Display(Name = "Quiz6")]
        [Range(0, 10)]
        public double? Quiz6 { get; set; } = null;

        [JsonIgnore]
        public double? QuizAvg { get; set; } = null;

        [Display(Name = "QuizFinal")]
        [Range(0, 10)]
        public double? QuizFinal { get; set; } = null;

        [Display(Name = "Asm1")]
        [Range(0, 10)]
        public double? Asm1 { get; set; } = null;

        [Display(Name = "Asm2")]
        [Range(0, 10)]
        public double? Asm2 { get; set; } = null;

        [Display(Name = "Asm3")]
        [Range(0, 10)]
        public double? Asm3 { get; set; } = null;

        [Display(Name = "Asm4")]
        [Range(0, 10)]
        public double? Asm4 { get; set; } = null;

        [Display(Name = "Asm5")]
        [Range(0, 10)]
        public double? Asm5 { get; set; } = null;

        [JsonIgnore]
        public double? AsmAvg { get; set; } = null;

        [Display(Name = "PracticeFinal")]
        [Range(0, 10)]
        public double? PracticeFinal { get; set; } = null;

        [Display(Name = "Audit")]
        [Range(5, 10)]
        public double? Audit { get; set; } = null;

        [JsonIgnore]
        public double? Gpamodule { get; set; } = null;

        [JsonIgnore]
        public int? LevelModule { get; set; } = null;

        [JsonIgnore]
        public string? Status { get; set; } = null;
    }
}
