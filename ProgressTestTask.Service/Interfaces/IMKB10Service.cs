using ProgressTestTask.Domain.Entity;
using ProgressTestTask.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.Service.Interfaces
{
    public interface IMKB10Service
    {
        Task<BaseResponse<IEnumerable<MKB10>>> GetBaseMKB10();

        Task<BaseResponse<IEnumerable<MKB10>>> GetChildMKB10(int id);

        Task<IBaseResponse<MKB10>> GetMKB10ByCode(string code);
    }
}
