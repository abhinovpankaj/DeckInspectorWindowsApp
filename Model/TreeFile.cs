using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Code.Model
{
    public class Organization : BindingModel,IEquatable<Organization>
    {
        public Organization()
        {
            
            this.NodeItem = new ObservableCollection<Organization>();
            //Path = new List<string>();
        }
        public string UserID { get; set; }
        public string Id { get; set; }
        public string Parent_ID { get; set; }
        public string NodeCode { get; set; }
        public string NodeTitle { get; set; }
        public bool IsEnable { get; set; }
       // public bool IsLock { get; set; }
        public bool IsDelete { get; set; }
        public bool IsSystem { get; set; }


        private bool _isloc;

        public bool IsLock
        {
            get { return _isloc; }
            set { _isloc = value; OnPropertyChanged("IsLock"); }
        }

        public int level { get; set; }

        //private List<string> vs;

        //public List<string> Path
        //{
        //    get { return vs; }
        //    set { vs = value; }
        //}

        public override bool Equals(object other)
        {
            return Equals(other as Organization);
        }

        public bool Equals(Organization otherItem)
        {
            if (otherItem == null)
            {
                return false;
            }
            return otherItem.Id == Id && otherItem.NodeTitle == NodeTitle;
        }

        public override int GetHashCode()
        {
            int hash = 19;
            hash = hash * 31 + (NodeTitle == null ? 0 : NodeTitle.GetHashCode());
            hash = hash * 31 + Id.GetHashCode();
            return hash;
        }

        public ObservableCollection<Organization> NodeItem { get; set; }
    }

}
