using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProgressTestTask.Domain.Entity;
using ProgressTestTask.Domain.ViewModels;
using ProgressTestTask.Service.Interfaces;

namespace ProgressTestTask.Controllers
{
    public class VisitController : Controller
    {
        private readonly IVisitService _visitService;

        private readonly IPatientService _petientService;

        private readonly IMKB10Service _MKB10Service;

        public VisitController(IVisitService visitService, IPatientService petientService, IMKB10Service mKB10Service)
        {
            _visitService = visitService;
            _petientService = petientService;
            _MKB10Service = mKB10Service;
        }

        // Отображает страницу посещений
        public IActionResult GetVisits()
        {
            return View();
        }

        // Возвращает данные всех визитов в формате JSON
        public async Task<string> GetData()
        {
            var response = await _visitService.GetAllVisits();

            return JsonConvert.SerializeObject(response.Data);
        }

        // Получает всех пациентов
        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            var response = await _petientService.GetAllPatients();

            return response.Data;
        }

        // Добавляет новый визит
        [HttpPost]
        public async Task<IActionResult> AddVisit(VisitViewModel visit)
        {
            // Убираем ненужные поля из модели состояния
            ModelState.Remove("DiagnosisName");
            ModelState.Remove("FIO");
            ModelState.Remove("VisitId");

            if (ModelState.IsValid)
            {
                var mkb10 = await _MKB10Service.GetMKB10ByCode(visit.Diagnosis); // Получение диагноза по коду

                // Проверка, существует ли диагноз
                if (mkb10.StatusCode == Domain.Enum.StatusCode.OK && mkb10.Data == null)
                {
                    ModelState.AddModelError("Diagnosis", "Такого диагноза нет, пожалуйста проконсультируйтесь со справочником");
                    return BadRequest(ModelState); // Возврат ошибки, если диагноз не найден
                }
                else if (mkb10.StatusCode != Domain.Enum.StatusCode.OK)
                {
                    return BadRequest(mkb10.Description);
                }

                var response = await _visitService.Create(visit);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Ok(response.Description);
                }
                return BadRequest(response.Description);
            }
            return BadRequest(ModelState);
        }
    }
}
