using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.Domain.ViewModels
{
    public class PatientDetailViewModel
    {
        public string PatientId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string? Patronymic { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; }

        public List<VisitViewModel> Visits { get; set; }
    }
}
