using FAMS_GROUP2.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ScoreModels
{
    public class ScoreViewModel
    {
        public string? FullName { get; set; }

        public int? StudentId { get; set; }

        public int? ClassId { get; set; }

        public double? Quiz1 { get; set; }

        public double? Quiz2 { get; set; }

        public double? Quiz3 { get; set; }

        public double? Quiz4 { get; set; }

        public double? Quiz5 { get; set; }

        public double? Quiz6 { get; set; }

        public double? QuizAvg { get; set; }

        public double? QuizFinal { get; set; }

        public double? Asm1 { get; set; }

        public double? Asm2 { get; set; }

        public double? Asm3 { get; set; }

        public double? Asm4 { get; set; }

        public double? Asm5 { get; set; }

        public double? AsmAvg { get; set; }

        public double? PracticeFinal { get; set; }

        public double? Audit { get; set; }

        public double? Gpamodule { get; set; }

        public int? LevelModule { get; set; }

        public string? Status { get; set; }

        public bool? IsDelete { get; set; }
    }
}
