using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.Domain.ViewModels
{
    public class VisitViewModel
    {
        public string VisitId { get; set; }

        [Required(ErrorMessage = "Дата посещения обязательна для заполнения")]
        [DataType(DataType.Date, ErrorMessage = "Дата посещения обязательна для заполнения")]
        [Display(Name = "Дата посещения")]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Диагноз обязателен для заполнения")]
        [StringLength(36, ErrorMessage = "Не должно содержаться более 36 символов")]
        [Display(Name = "Диагноз (МКБ10)")]
        public string Diagnosis { get; set; }

        public string DiagnosisName { get; set; }

        [Required(ErrorMessage = "Пациент обязателен для заполнения")]
        [StringLength(36)]
        public String PatientId { get; set; }

        public String FIO { get; set; }
    }
}
