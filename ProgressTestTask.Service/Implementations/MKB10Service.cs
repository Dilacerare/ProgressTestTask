using Microsoft.EntityFrameworkCore;
using ProgressTestTask.DAL.Interfaces;
using ProgressTestTask.Domain.Entity;
using ProgressTestTask.Domain.Enum;
using ProgressTestTask.Domain.Response;
using ProgressTestTask.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProgressTestTask.Service.Implementations
{
    public class MKB10Service : IMKB10Service
    {
        private readonly IBaseRepository<MKB10> _MKB10Repository;
        private readonly ILogger<MKB10Service> _logger;

        public MKB10Service(IBaseRepository<MKB10> MKB10Repository, ILogger<MKB10Service> logger)
        {
            _MKB10Repository = MKB10Repository;
            _logger = logger;
        }

        // Метод для получения всех корневых элементов (без родителя)
        public async Task<BaseResponse<IEnumerable<MKB10>>> GetBaseMKB10()
        {
            try
            {
                var MKB10 = await _MKB10Repository.GetAll().Where(x => x.Parent_id == null).ToListAsync();

                _logger.LogInformation($"[MKB10Service.GetBaseMKB10] Получено {MKB10.Count} элементов.");

                return new BaseResponse<IEnumerable<MKB10>>()
                {
                    Data = MKB10,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[MKB10Service.GetBaseMKB10] Ошибка: {ex.Message}");

                return new BaseResponse<IEnumerable<MKB10>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        // Метод для получения дочерних элементов по ID родителя
        public async Task<BaseResponse<IEnumerable<MKB10>>> GetChildMKB10(int id)
        {
            try
            {
                var MKB10 = await _MKB10Repository.GetAll().Where(x => x.Parent_id == id).ToListAsync();

                _logger.LogInformation($"[MKB10Service.GetChildMKB10] Для Parent_id={id} получено {MKB10.Count} элементов.");

                return new BaseResponse<IEnumerable<MKB10>>()
                {
                    Data = MKB10,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[MKB10Service.GetChildMKB10] Ошибка: {ex.Message}");

                return new BaseResponse<IEnumerable<MKB10>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<MKB10>> GetMKB10ByCode(string code)
        {
            try
            {
                var MKB10 = await _MKB10Repository.GetAll().Where(x => x.Code == code).FirstOrDefaultAsync();

                if (MKB10 != null)
                {
                    _logger.LogInformation($"[MKB10Service.GetMKB10ByCode] Найден элемент с кодом {code}.");
                }
                else
                {
                    _logger.LogWarning($"[MKB10Service.GetMKB10ByCode] Элемент с кодом {code} не найден.");
                }

                return new BaseResponse<MKB10>()
                {
                    Data = MKB10,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[MKB10Service.GetMKB10ByCode] Ошибка: {ex.Message}");

                return new BaseResponse<MKB10>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }
    }
}
