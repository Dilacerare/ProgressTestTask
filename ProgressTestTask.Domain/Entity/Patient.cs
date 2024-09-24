using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.Domain.Entity
{
    public class Patient
    {
        public Guid PatientId { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string? Patronymic { get; set; }

        public DateTime DateOfBirth { get; set; }

        [StringLength(18)]
        public string Phone { get; set; }
    }
}
