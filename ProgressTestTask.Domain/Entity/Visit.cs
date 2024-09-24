using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.Domain.Entity
{
    public class Visit
    {
        public Guid VisitId { get; set; }

        public DateTime VisitDate { get; set; }

        [StringLength(36)]
        public string Diagnosis { get; set; }

        public Guid PatientId { get; set; }
    }
}
