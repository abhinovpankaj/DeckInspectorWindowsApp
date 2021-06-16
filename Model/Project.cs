using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UI.Code.Model
{
    public class Project : BindingModel
    {

        public string Id { get; set; }

        public string Attendent { get; set; }
        public string ProjectType { get; set; }
        public string AssignTo { get; set; }
        public string Name { get; set; }


        public string Description { get; set; }


        public string Address { get; set; }
        public string ImageUrl { get; set; }

        // public string CreatedOn { get; set; }

        private string createOn;

        public string CreatedOn
        {
            get { return createOn; }
            set { createOn = value; OnPropertyChanged("CreatedOn"); }
        }

        public bool IsOriginal { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        public bool IsDelete { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }

        private string username;
        public string InvasiveProjectID { get; set; }
        public bool IsInvaisveExist { get; set; }
        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged("Username"); }
        }


        private bool _isRoleAdmin;

        public bool IsRoleAdmin
        {
            get { 
                    if (App.LogUser.RoleName == "Admin")
                    return true;
                else
                    return false;
                   }
           
        }


        


        private bool _fr;
        public bool FinelReport
        {
            get { return _fr; }
            set { _fr = value; OnPropertyChanged("FinelReport"); }
        }

        private bool _frprogress;
        public bool FinelReportProgress
        {
            get { return _frprogress; }
            set { _frprogress = value; OnPropertyChanged("FinelReportProgress"); }
        }
        private bool _inv;


        public bool IsInvasive
        {
            get { return _inv; }
            set { _inv = value; OnPropertyChanged("IsInvasive"); }
        }
    }
}
