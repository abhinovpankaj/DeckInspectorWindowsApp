using System;
using System.Collections.Generic;
using System.Text;

namespace UI.Code.Model
{
    public class ProjectCommonLocationImages : BindingModel
    {
        public string Id { get; set; }
        public string ProjectLocationId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }
        private string _image;

        public string ImageUrl
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("ImageUrl"); }
        }
        public string CreatedOn { get; set; }

        public bool IsDelete { get; set; }
        public string EmployeeName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Attendent { get; set; }

        public string Username { get; set; }
    }
}
