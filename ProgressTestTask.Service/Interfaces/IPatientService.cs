using ProgressTestTask.Domain.ViewModels;
using ProgressTestTask.Domain.Entity;
using ProgressTestTask.Domain.Response;


namespace ProgressTestTask.Service.Interfaces
{
    public interface IPatientService
    {
        Task<IBaseResponse<Patient>> Create(PatientViewModel model);

        Task<BaseResponse<IEnumerable<Patient>>> GetAllPatients();

        //Task<IBaseResponse<PatientViewModel>> GetPatient(string login);

        Task<IBaseResponse<bool>> DeletePatient(Guid id);

        Task<IBaseResponse<Patient>> Edit(Guid id, PatientViewModel model);

        Task<BaseResponse<PatientDetailViewModel>> GetDetail(string id);
    }
}
