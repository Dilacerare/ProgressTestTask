using Microsoft.EntityFrameworkCore;
using ProgressTestTask.DAL.Interfaces;
using ProgressTestTask.Domain.Entity;
using ProgressTestTask.Domain.Enum;
using ProgressTestTask.Domain.Response;
using ProgressTestTask.Domain.ViewModels;
using ProgressTestTask.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ProgressTestTask.Service.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IBaseRepository<Patient> _patientRepository;
        private readonly IBaseRepository<Visit> _visitRepository;
        private readonly IBaseRepository<MKB10> _MKB10Repository;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IBaseRepository<Patient> patientRepository,
                              IBaseRepository<Visit> visitRepository,
                              IBaseRepository<MKB10> MKB10Repository,
                              ILogger<PatientService> logger)
        {
            _patientRepository = patientRepository;
            _visitRepository = visitRepository;
            _MKB10Repository = MKB10Repository;
            _logger = logger;
        }

        // Создание пациента
        public async Task<IBaseResponse<Patient>> Create(PatientViewModel model)
        {
            try
            {
                var patient = new Patient()
                {
                    PatientId = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Patronymic = model.Patronymic,
                    DateOfBirth = model.DateOfBirth,
                    Phone = model.Phone
                };

                await _patientRepository.Create(patient);

                _logger.LogInformation($"[PatientService.Create] Пациент {patient.PatientId} добавлен.");

                return new BaseResponse<Patient>()
                {
                    Data = patient,
                    Description = "Пациент добавлен",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[PatientService.Create] Ошибка: {ex.Message}");
                return new BaseResponse<Patient>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        // Удаление пациента по ID
        public async Task<IBaseResponse<bool>> DeletePatient(Guid id)
        {
            try
            {
                var patient = await _patientRepository.GetAll().FirstOrDefaultAsync(x => x.PatientId == id);
                if (patient == null)
                {
                    _logger.LogWarning($"[PatientService.DeletePatient] Пациент с ID {id} не найден.");
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.PatientNotFound,
                        Data = false
                    };
                }
                await _patientRepository.Delete(patient);
                _logger.LogInformation($"[PatientService.DeletePatient] Пациент с ID {id} удален.");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[PatientService.DeletePatient] Ошибка: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        // Редактирование пациента по ID
        public async Task<IBaseResponse<Patient>> Edit(Guid id, PatientViewModel model)
        {
            try
            {
                var patient = await _patientRepository.GetAll().FirstOrDefaultAsync(x => x.PatientId == id);
                if (patient == null)
                {
                    _logger.LogWarning($"[PatientService.Edit] Пациент с ID {id} не найден.");
                    return new BaseResponse<Patient>()
                    {
                        Description = "Пациент не найден",
                        StatusCode = StatusCode.PatientNotFound
                    };
                }

                // Обновление данных пациента
                patient.FirstName = model.FirstName;
                patient.LastName = model.LastName;
                patient.Patronymic = model.Patronymic;
                patient.DateOfBirth = model.DateOfBirth;
                patient.Phone = model.Phone;

                await _patientRepository.Update(patient);
                _logger.LogInformation($"[PatientService.Edit] Пациент с ID {id} обновлен.");

                return new BaseResponse<Patient>()
                {
                    Data = patient,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[PatientService.Edit] Ошибка: {ex.Message}");
                return new BaseResponse<Patient>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        // Получение списка всех пациентов
        public async Task<BaseResponse<IEnumerable<Patient>>> GetAllPatients()
        {
            try
            {
                var patients = await _patientRepository.GetAll().ToListAsync();
                _logger.LogInformation($"[PatientService.GetAllPatients] Получено {patients.Count} пациентов.");

                return new BaseResponse<IEnumerable<Patient>>()
                {
                    Data = patients,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[PatientService.GetAllPatients] Ошибка: {ex.Message}");
                return new BaseResponse<IEnumerable<Patient>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        // Получение подробной информации о пациенте
        public async Task<BaseResponse<PatientDetailViewModel>> GetDetail(string id)
        {
            try
            {
                Guid guid = Guid.Parse(id);

                var patient = await _patientRepository.GetAll().Where(x => x.PatientId == guid).FirstOrDefaultAsync();

                if (patient == null)
                {
                    _logger.LogWarning($"[PatientService.GetDetail] Пациент с ID {id} не найден.");
                    return new BaseResponse<PatientDetailViewModel>()
                    {
                        StatusCode = StatusCode.PatientNotFound,
                        Description = "Пациент не найден."
                    };
                }

                var visits = await _visitRepository.GetAll().Where(x => x.PatientId == patient.PatientId).ToListAsync();
                var visitViewModels = new List<VisitViewModel>();

                if (visits != null)
                {
                    var fio = GetFIO(patient);
                    var diagnosisNameTask = visits.Select(visit => DiagnosisName(visit.Diagnosis));
                    var diagnosisNames = await Task.WhenAll(diagnosisNameTask);

                    // Создание VisitViewModel для каждого посещения
                    visitViewModels = visits.Select((visit, index) => new VisitViewModel
                    {
                        VisitId = visit.VisitId.ToString(),
                        PatientId = visit.PatientId.ToString(),
                        Diagnosis = visit.Diagnosis,
                        VisitDate = visit.VisitDate,
                        FIO = fio,
                        DiagnosisName = diagnosisNames[index]
                    }).ToList();
                }

                var detail = new PatientDetailViewModel()
                {
                    PatientId = patient.PatientId.ToString(),
                    LastName = patient.LastName,
                    FirstName = patient.FirstName,
                    Patronymic = patient.Patronymic,
                    DateOfBirth = patient.DateOfBirth,
                    Phone = patient.Phone,
                    Visits = visitViewModels
                };

                _logger.LogInformation($"[PatientService.GetDetail] Подробности пациента с ID {id} успешно получены.");
                return new BaseResponse<PatientDetailViewModel>()
                {
                    Data = detail,
                    StatusCode = StatusCode.OK
                };
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "[PatientService.GetDetail] Некорректный формат ID.");
                return new BaseResponse<PatientDetailViewModel>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Некорректный формат PatientId."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[PatientService.GetDetail] Ошибка: {ex.Message}");
                return new BaseResponse<PatientDetailViewModel>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        // Формирование полного имени пациента
        private string GetFIO(Patient patient)
        {
            return patient != null ? $"{patient.LastName} {patient.FirstName} {patient.Patronymic}" : "Неизвестно";
        }

        // Получение названия диагноза по коду
        private async Task<string> DiagnosisName(string code)
        {
            try
            {
                var diagnosis = await _MKB10Repository.GetAll().Where(x => x.Code == code).FirstOrDefaultAsync();
                return diagnosis != null ? diagnosis.Name : "Неизвестно";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[PatientService.DiagnosisName] Ошибка при получении названия диагноза: {ex.Message}");
                return "Ошибка при получении названия диагноза";
            }
        }
    }
}
