using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProgressTestTask.Domain.ViewModels;
using ProgressTestTask.Service.Interfaces;
using System.Text;
using System.Xml;

namespace ProgressTestTask.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _petientService;

        public PatientController(IPatientService petientService)
        {
            _petientService = petientService; 
        }

        // Отображает главную страницу пациентов
        public IActionResult Index()
        {
            return View(); 
        }

        // Отображает детали конкретного пациента по его идентификатору
        public async Task<IActionResult> Detail(string id)
        {
            var response = await _petientService.GetDetail(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Index", "Home");
        }

        // Возвращает данные всех пациентов в формате JSON
        public async Task<string> GetData()
        {
            var response = await _petientService.GetAllPatients();

            return JsonConvert.SerializeObject(response.Data);
        }

        // Возвращает детали конкретного пациента в формате JSON
        public async Task<string> GetDetailData(string id)
        {
            var response = await _petientService.GetDetail(id);

            return JsonConvert.SerializeObject(response.Data.Visits);
        }

        // Добавляет нового пациента
        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientViewModel patient)
        {
            if (ModelState.IsValid)
            {
                var response = await _petientService.Create(patient); 
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Ok(response.Description);
                }
                return BadRequest(response.Description);
            }
            return BadRequest(ModelState);
        }

        // Экспортирует детали пациента в XML
        public async Task<IActionResult> ExportToXml(string id)
        {
            var response = await _petientService.GetDetail(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var patientDetail = response.Data; 

                var xml = new System.Xml.Serialization.XmlSerializer(typeof(PatientDetailViewModel));
                using (var stringWriter = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        xml.Serialize(writer, patientDetail); 
                        var xmlContent = stringWriter.ToString(); 

                        return File(Encoding.UTF8.GetBytes(xmlContent), "application/xml", "PatientDetail.xml");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
