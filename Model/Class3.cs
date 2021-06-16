using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Code.Model
{
    public class AssignLocationAndBuilding
    {
        public string Type { get; set; }
        public List<User> Users { get; set; }
        //public List<ProjectLocation> ProjectLocations { get; set; }
        //public List<ProjectBuilding> ProjectBuildings { get; set; }
        public string ParentID { get; set; }
    }
}
