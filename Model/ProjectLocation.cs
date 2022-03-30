using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UI.Code.Model
{
    public class ProjectLocation : BindingModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AssignTo { get; set; }
        public Guid UserId { get; set; }
        public ObservableCollection<ProjectCommonLocationImages> ProjectCommonLocationImages { get; set; }
        private string _image;

        public string ImageUrl
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("ImageUrl"); }
        }


        public bool IsAssign { get; set; }
        public string EmployeeName { get; set; }

        public string Attendent { get; set; }

        public int SeqNo { get; set; }

        private string createdOn;

        public string CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; OnPropertyChanged("CreatedOn"); }
        }


        private bool _isd;

        public bool IsDelete
        {
            get { return _isd; }
            set { _isd = value; OnPropertyChanged("IsDeleted"); }
        }

        private string unm;

        public string Username
        {
            get { return unm; }
            set { unm = value; OnPropertyChanged("Username"); }
        }


    }
}
