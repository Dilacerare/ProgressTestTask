using Microsoft.AspNetCore.Mvc;
using ProgressTestTask.Service.Interfaces;

namespace ProgressTestTask.Controllers
{
    public class MKB10Controller : Controller
    {
        private readonly IMKB10Service _MKB10Service; 

        public MKB10Controller(IMKB10Service MKB10Service)
        {
            _MKB10Service = MKB10Service;
        }

        // Отображает базовую информацию о MKB10 (без родителя)
        public async Task<IActionResult> Manual()
        {
            var response = await _MKB10Service.GetBaseMKB10();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Index", "Home");
        }

        // Отображает детали конкретного элемента MKB10
        public async Task<IActionResult> Detail(int id)
        {
            var response = await _MKB10Service.GetChildMKB10(id); 
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
