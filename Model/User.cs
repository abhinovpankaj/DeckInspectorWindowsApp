using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Code.Model
{
    //public class User : BindingModel
    //{
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Id { get; set; }
    //    public string UserName { get; set; }

    //   // public string FullName { get; set; }
    //    public string Role { get; set; }
    //    public string Pwd { get; set; }

    //    public string Mobile { get; set; }
    //    public string RoleId { get; set; }

    //    public string CreatedOn { get; set; }

    //   // public DateTime UpdatedOn { get; set; }
    //    private bool _isActive;
    //    public bool IsActive
    //    {
    //        get { return _isActive; }
    //        set
    //        {
    //            SetProperty(ref _isActive, value);

    //        }
    //    }

    //   // private string _fname;

    //    public string FullName { get; set; }
    //    //public string FullName
    //    //{
    //    //    get
    //    //    {
    //    //        return FirstName + " " + LastName;
    //    //    }
    //    //    set
    //    //    {
    //    //        SetProperty(ref _status, value);

    //    //    }

    //    // }
    //    //private string _status;

    //    public string Status;
    //    //{
    //    //    get
    //    //    {
    //    //        if (IsActive == true)
    //    //            return "Activate";
    //    //        else
    //    //            return "Deactivate";
    //    //    }
    //    //    set
    //    //    {
    //    //        SetProperty(ref _status, value);

    //    //    }

    //    //}
    //    //public bool IsActive { get; set; }
    //}
    public class User : BindingModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Pwd { get; set; }
        public string UserName { get; set; }


        public bool IsOriginal { get; set; }


        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string ActiveStatus { get; set; }

        public bool IsAssign { get; set; }
        
        public string IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ErrNo { get; set; }
        public string ErrMsg { get; set; }
    }
}
