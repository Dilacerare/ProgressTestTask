using ProgressTestTask.Domain.Entity;
using ProgressTestTask.Domain.Response;
using ProgressTestTask.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.Service.Interfaces
{
    public interface IVisitService
    {
        Task<IBaseResponse<Visit>> Create(VisitViewModel model);

        Task<BaseResponse<IEnumerable<VisitViewModel>>> GetAllVisits();

        //Task<IBaseResponse<PatientViewModel>> GetPatient(string login);

        Task<IBaseResponse<bool>> DeleteVisit(Guid id);

        Task<IBaseResponse<Visit>> Edit(Guid id, VisitViewModel model);
    }
}
