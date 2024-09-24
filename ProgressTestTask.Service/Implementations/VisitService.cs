using Microsoft.EntityFrameworkCore;
using ProgressTestTask.DAL.Interfaces;
using ProgressTestTask.Domain.Entity;
using ProgressTestTask.Domain.Enum;
using ProgressTestTask.Domain.Response;
using ProgressTestTask.Domain.ViewModels;
using ProgressTestTask.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProgressTestTask.Service.Implementations
{
    public class VisitService : IVisitService
    {
        private readonly IBaseRepository<Visit> _visitRepository;
        private readonly IBaseRepository<Patient> _patientRepository;
        private readonly IBaseRepository<MKB10> _MKB10Repository;
        private readonly ILogger<VisitService> _logger;

        public VisitService(
            IBaseRepository<Visit> visitRepository,
            IBaseRepository<Patient> patientRepository,
            IBaseRepository<MKB10> MKB10Repository,
            ILogger<VisitService> logger)
        {
            _visitRepository = visitRepository;
            _patientRepository = patientRepository;
            _MKB10Repository = MKB10Repository;
            _logger = logger;
        }


        // Создает новую запись о визите
        public async Task<IBaseResponse<Visit>> Create(VisitViewModel model)
        {
            try
            {
                var visit = new Visit()
                {
                    VisitId = Guid.NewGuid(),
                    PatientId = Guid.Parse(model.PatientId),
                    Diagnosis = model.Diagnosis,
                    VisitDate = model.VisitDate
                };

                await _visitRepository.Create(visit);

                _logger.LogInformation($"[VisitService.Create] Приём создан: {visit.VisitId}");

                return new BaseResponse<Visit>()
                {
                    Data = visit,
                    Description = "Приём добавлен",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[VisitService.Create] ошибка: {ex.Message}");
                return new BaseResponse<Visit>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        // Удаляет запись о визите по идентификатору
        public async Task<IBaseResponse<bool>> DeleteVisit(Guid id)
        {
            try
            {
                var visit = await _visitRepository.GetAll().FirstOrDefaultAsync(x => x.VisitId == id);
                if (visit == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.VisitNotFound,
                        Data = false
                    };
                }
                await _visitRepository.Delete(visit);
                _logger.LogInformation($"[VisitService.DeleteVisit] Приём удален: {visit.VisitId}");

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[VisitService.DeleteVisit] ошибка: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        // Обновляет запись о визите
        public async Task<IBaseResponse<Visit>> Edit(Guid id, VisitViewModel model)
        {
            try
            {
                var visit = await _visitRepository.GetAll().FirstOrDefaultAsync(x => x.VisitId == id);
                if (visit == null)
                {
                    return new BaseResponse<Visit>()
                    {
                        Description = "Приём не найден",
                        StatusCode = StatusCode.VisitNotFound
                    };
                }

                // Обновляем свойства приёма
                visit.PatientId = Guid.Parse(model.PatientId);
                visit.Diagnosis = model.Diagnosis;
                visit.VisitDate = model.VisitDate;

                await _visitRepository.Update(visit);
                _logger.LogInformation($"[VisitService.Edit] Визит обновлен: {visit.VisitId}");

                return new BaseResponse<Visit>()
                {
                    Data = visit,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[VisitService.Edit] ошибка: {ex.Message}");
                return new BaseResponse<Visit>()
                {
                    Description = $"Ошибка при обновлении визита: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        // Получает все записи о приёмах
        public async Task<BaseResponse<IEnumerable<VisitViewModel>>> GetAllVisits()
        {
            try
            {
                var visits = await _visitRepository.GetAll().ToListAsync();

                var fioTasks = visits.Select(visit => GetFIO(visit.PatientId));
                var diagnosisNameTasks = visits.Select(visit => DiagnosisName(visit.Diagnosis));

                // Выполняем все запросы FIO параллельно
                var fios = await Task.WhenAll(fioTasks);
                var diagnosisNames = await Task.WhenAll(diagnosisNameTasks);

                // Создание VisitViewModel
                var visitViewModels = visits.Select((visit, index) => new VisitViewModel
                {
                    VisitId = visit.VisitId.ToString(),
                    PatientId = visit.PatientId.ToString(),
                    Diagnosis = visit.Diagnosis,
                    VisitDate = visit.VisitDate,
                    FIO = fios[index], // Используем результат из fios
                    DiagnosisName = diagnosisNames[index] // Используем результат из diagnosisNames
                }).ToList();

                _logger.LogInformation($"[VisitService.GetAllVisits] Получено элементов: {visitViewModels.Count}");
                return new BaseResponse<IEnumerable<VisitViewModel>>()
                {
                    Data = visitViewModels,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[VisitService.GetAllVisits] ошибка: {ex.Message}");
                return new BaseResponse<IEnumerable<VisitViewModel>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        // Получает ФИО пациента по его идентификатору
        private async Task<string> GetFIO(Guid id)
        {
            try
            {
                var patient = await _patientRepository.GetAll()
                                                .FirstOrDefaultAsync(x => x.PatientId == id);

                if (patient != null)
                {
                    return $"{patient.LastName} {patient.FirstName} {patient.Patronymic}";
                }
                else
                {
                    return "Неизвестно";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[VisitService.GetFIO] ошибка при получении ФИО: {ex.Message}");
                return "Ошибка при получении ФИО";
            }
        }

        // Получает название диагноза по коду
        private async Task<string> DiagnosisName(string code)
        {
            try
            {
                var diagnosis = await _MKB10Repository.GetAll()
                                                .FirstOrDefaultAsync(x => x.Code == code);

                if (diagnosis != null)
                {
                    return diagnosis.Name;
                }
                else
                {
                    return "Неизвестно";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[VisitService.DiagnosisName] ошибка при получении названия диагноза: {ex.Message}");
                return "Ошибка при получении названия диагноза";
            }
        }
    }
}
