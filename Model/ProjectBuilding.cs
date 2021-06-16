using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UI.Code.Model
{
    public class ProjectBuilding : BindingModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string AssignTo { get; set; }
        public bool IsAssign { get; set; }
        public string Description { get; set; }
        public ObservableCollection<BuildingLocation> BuildingLocations { get; set; }
        public ObservableCollection<BuildingApartment> BuildingApartments { get; set; }

        private string _bimage;

        public string ImageUrl
        {
            get { return _bimage; }
            set { _bimage = value; OnPropertyChanged("ImageUrl"); }
        }

       
        public string EmployeeName { get; set; }
        public int SeqNo { get; set; }
        public string Attendent { get; set; }

        private string createdOn;

        public string CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; OnPropertyChanged("CreatedOn"); }
        }


        private string unm;

        public string Username
        {
            get { return unm; }
            set { unm = value; OnPropertyChanged("Username"); }
        }
        public Guid UserId { get;  set; }
        public bool IsDelete { get;  set; }
    }
}
