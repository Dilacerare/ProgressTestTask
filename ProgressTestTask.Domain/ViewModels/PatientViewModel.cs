using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.Domain.ViewModels
{
    public class PatientViewModel
    {

        [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
        [StringLength(50, ErrorMessage = "Фамилия не должна превышать более 50 символов")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Имя обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Имя не должно превышать более 50 символов")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Отчество не должно содержаться более 50 символов")]
        public string? Patronymic { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Дата рождения обязательна для заполнения")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Номер телефона обязателен для заполнения")]
        [StringLength(18)]
        [RegularExpression(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$", ErrorMessage = "Номер телефона должен быть в формате +7 (999) 999-99-99")]
        public string Phone { get; set; }
    }
}
