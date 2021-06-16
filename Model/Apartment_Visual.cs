using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UI.Code.Model
{
    public class Apartment_Visual : BindingModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ExteriorElements { get; set; }


        public string ApartmentId { get; set; }
        public string WaterProofingElements { get; set; }
        public string ConditionAssessment { get; set; }
        public string VisualReview { get; set; }
        public string AnyVisualSign { get; set; }
        public string FurtherInasive { get; set; }
        public string AdditionalConsideration { get; set; }
        public string LifeExpectancyEEE { get; set; }
        public string LifeExpectancyLBC { get; set; }
        public string LifeExpectancyAWE { get; set; }

        private string _image;

        public string Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("Image"); }
        }
        public string CreatedOn { get; set; }

        public string EmployeeName { get; set; }

        public string Attendent { get; set; }
    }
}
