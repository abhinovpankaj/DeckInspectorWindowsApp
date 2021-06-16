using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Code.Model
{
    public class CustomRadioItem : BindingModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        private bool _isChk;

        public string GroupName { get; set; }
        public bool IsChecked
        {
            get { return _isChk; }
            set { _isChk = value; OnPropertyChanged("IsSelected"); }
        }
    }
}
