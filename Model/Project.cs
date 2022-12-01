using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UI.Code.Services;

namespace UI.Code.Model
{
    public class Project : BindingModel
    {
        public ProjectDocument SelectedDocument { get; 
            set; }
        public ObservableCollection<ProjectDocument> DocumentsList { get; set; } = new ObservableCollection<ProjectDocument>();
        public string Id { get; set; }

        public string Attendent { get; set; }
        public string ProjectType { get; set; }
        public string AssignTo { get; set; }
        public string Name { get; set; }

        private bool _isAvailableOffline;
        public bool IsAvailableOffline
        {
            get
            {
                return _isAvailableOffline;
            }
            set
            {
                if (value!=_isAvailableOffline)
                {
                    _isAvailableOffline = value;
                    OnPropertyChanged("IsAvailableOffline");                    
                }
            }
        }        
        public string Description { get; set; }


        public string Address { get; set; }
        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; OnPropertyChanged("ImageUrl"); }
        }

        // public string CreatedOn { get; set; }

        private string createOn;

        public string CreatedOn
        {
            get { return createOn; }
            set { createOn = value; OnPropertyChanged("CreatedOn"); }
        }

        private bool? _isSingle;
        public bool? IsSingle
        {
            set
            {
                if (_isSingle!=value)
                {
                    _isSingle = value;
                    OnPropertyChanged("IsSingle");
                }
            }
            get
            {
               return _isSingle;
            }
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

        public string Category { get; set; }

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
