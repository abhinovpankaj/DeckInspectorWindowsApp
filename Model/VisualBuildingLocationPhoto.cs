using System;
using System.Collections.Generic;
using System.Text;

namespace UI.Code.Model
{
    public class VisualBuildingLocationPhoto
    {
        public Guid Id { get; set; }
        public string VisualBuildingId { get; set; }
        public string ImageUrl { get; set; }

        public string CreatedOn { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        public bool IsDelete { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public int SeqNo { get; set; }
        public string Username { get; set; }
    }
}
