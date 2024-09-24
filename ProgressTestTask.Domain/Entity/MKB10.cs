using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTestTask.Domain.Entity
{
    public class MKB10
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Parent_id { get; set; }
        public string? Parent_code { get; set; }
        public int Node_count { get; set; }
        public string? Additional_info {get; set;}
    }
}
