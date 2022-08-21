using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Code.Model
{
    public  class ProjectDocument
    {
        public string DocumentName { get; set; }
        public string UserName { get; set; }
        public string ProjectId { get; set; }
        public DateTime UploadedOn { get; set; }
        public string DocURL { get; set; }
        public string Id { get; set; }
    }
}

