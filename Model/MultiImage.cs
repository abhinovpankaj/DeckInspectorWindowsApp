using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Code.Model
{
    public class MultiImage
    {
        public string Id { get; set; }
        public MultiImage()
        {
            CreateOn = DateTime.Now;
        }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime CreateOn { get; set; }
        public byte[] ImageArray { get; set; }

        public string Status { get; set; }
        public string ParentId { get; set; }


        public bool IsServerData { get; set; }
        public bool IsDelete { get; set; }

    }
}
