using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UI.Code.Model
{
    public class BuildingApartment:BindingModel
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<BuildingApartmentImages> BuildingApartmentImages { get; set; }
      //  public string ApartmentImage { get; set; }

        private string _img;

        public string ImageUrl
        {
            get { return _img; }
            set { _img = value;OnPropertyChanged("ImageUrl"); }
        }

      

        public string EmployeeName { get; set; }

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
        public string Attendent { get; set; }
        public Guid UserId { get;  set; }
        private bool _isd;

        public bool IsDelete
        {
            get { return _isd; }
            set { _isd = value; OnPropertyChanged("IsDeleted"); }
        }

    }
}
