using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.Domain.Enum
{
    public enum StatusCode
    {
        PatientNotFound = 1,
        VisitNotFound = 2,
        MKB10NotFound = 3,
        PatientAlreadyExists = 4,

        OK = 200,
        InternalServerError = 500
    }
}
