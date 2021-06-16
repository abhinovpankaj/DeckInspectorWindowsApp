using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Code.Model
{
    public class Response
    {
        public string ID { get; set; }
        public object Data { get; set; }
        public ApiResult Status { get; set; }
        public string Message { get; set; }
        public int TotalCount { get; set; }
    }
    public enum ApiResult
    {

        Fail = 0,
        Success = 1

    }
}
