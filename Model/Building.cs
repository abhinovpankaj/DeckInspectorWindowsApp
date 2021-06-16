
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UI.Code.Model
{
    public class Building
    {
        public string BuildingImage { get; set; }
        public string BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string BuildingDescription { get; set; }
        public ObservableCollection<Apartment> Apartments { get; set; }

    }
}
