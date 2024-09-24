using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProgressTestTask.Domain.Entity;
using ProgressTestTask.Domain.ViewModels;
using ProgressTestTask.Models;
using ProgressTestTask.Service.Interfaces;
using System.Diagnostics;
using System.Security.Claims;
using static System.Reflection.Metadata.BlobBuilder;

namespace ProgressTestTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IPatientService _petientService;

        public HomeController(ILogger<HomeController> logger,
            IPatientService patientService)
        {
            _logger = logger;
            _petientService = patientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetPatients()
        {
            var response = await _petientService.GetAllPatients();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<string> GetData()
        {
            var response = await _petientService.GetAllPatients();
            
            return JsonConvert.SerializeObject(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientViewModel patient)
        {
            if (ModelState.IsValid)
            {
                // Логика добавления пациента (например, добавление в базу данных)
                var response = await _petientService.Create(patient);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {

                    return RedirectToAction("GetPatients", "Home");
                }
                return BadRequest(ModelState);
            }
            return BadRequest(ModelState);
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}
