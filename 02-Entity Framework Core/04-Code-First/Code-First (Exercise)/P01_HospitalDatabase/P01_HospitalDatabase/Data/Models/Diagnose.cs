using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{
    public class Diagnose
    {
        public int DiagnoseId { get; set; }

        public string Name { get; set; }

        public string Comments { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }
    }
}
