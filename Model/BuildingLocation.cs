using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UI.Code.Model
{
    public class BuildingLocation : BindingModel

    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<BuildingCommonLocationImages> BuildingCommonLocationImages { get; set; }
        private string _image;

        public string ImageUrl
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("ImageUrl"); }
        }

       

        public string EmployeeName { get; set; }

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
        private bool _isd;

        public bool IsDelete
        {
            get { return _isd; }
            set { _isd = value; OnPropertyChanged("IsDeleted"); }
        }

        public Guid UserId { get;  set; }
    }
}
