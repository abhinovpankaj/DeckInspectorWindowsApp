using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Controls;
using UI.Code.Model;

namespace UI.Code.ViewModel
{
    public class LoginPageViewModel: BaseViewModel, INavigationAware
    {
        #region Properties      
        /// <summary>      
        /// This string property will have default text for demo purpose.    
        /// </summary>      
        private string _imGoodByeText = "This is binded from WelcomePageViewModel, Thank you for being part of this Blog!";
        /// <summary>      
        /// This string property will be binded with Textblock on view       
        /// </summary>      
        /// 

        IRegionManager RegionManger
        {
            get
            {
                return (IRegionManager)Prism.Ioc.ContainerLocator.Container.Resolve(typeof(IRegionManager));
            }
        }
        public string ImGoodByeText
        {
            get { return _imGoodByeText; }
            set { _imGoodByeText = value; }
        }
        public event EventHandler LoginCompleted;
        private void RaiseLoginCompletedEvent()
        {
            LoginCompleted?.Invoke(this, EventArgs.Empty);
        }
        public DelegateCommand SubmitCommand { get; private set; }

        private string _unm;

        public string Username
        {
            get { return _unm; }
            set { _unm = value; OnPropertyChanged("Username"); }
        }

        private string _pwd;

        public string Password
        {
            get { return _pwd; }
            set { _pwd = value; OnPropertyChanged("Password"); }
        }
        private bool _isRemember;

        public bool IsRemember
        {
            get { return _isRemember; }
            set { _isRemember = value; OnPropertyChanged("IsRemember"); }
        }

        public LoginPageViewModel()
        {
            SubmitCommand = new DelegateCommand(async () => await Submit());
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error");
            }
        }
        private bool sbusy;

        public bool IsBusy
        {
            get { return sbusy; }
            set { sbusy = value; OnPropertyChanged("IsBusy"); }
        }
        public async Task Submit()
        {
            IsBusy = true;

            // bool status = await Task.Run(() => LongOperation());
            bool status = await  LongOperation();
            if (status == true)
            {
                IsBusy = false;
            }
            
        }

        private async Task<bool> LongOperation()
        {
            App.LogUser = null;
            User user = new User();
            Error = string.Empty;
            if (string.IsNullOrEmpty(Username))
            {

                Error += "Usename field required\n";
            }
            if (string.IsNullOrEmpty(Password))
            {
                Error += "Password field required\n";
            }
            if (string.IsNullOrEmpty(Error))
            {
                User passObj = new User() { UserName = Username, Pwd = Password };
                Response response = (await userService.ValidateLogin(passObj));
                if (response.Status == ApiResult.Success)
                {
                    user = JsonConvert.DeserializeObject<User>(response.Data.ToString());
                    if (user.ErrNo == 1)
                    {
                        if (user.RoleName == "Mobile")
                        {
                            Error = "you are not authorized to access this application";
                            return await Task.FromResult(true);
                        }
                        if (user == null)
                        {
                            Error = "Invalid username and password";
                            return await Task.FromResult(true);

                        }
                        else
                        {
                            if (IsRemember == true)
                            {
                                UI.Code.Properties.Settings.Default.username = Username;
                                UI.Code.Properties.Settings.Default.password = Password;
                                UI.Code.Properties.Settings.Default.IsRemember = IsRemember;

                                UI.Code.Properties.Settings.Default.Save();
                            }
                            
                            App.Role = user.RoleName;
                            App.LogUserName = "Welcome :" + user.UserName;
                            App.LogUser = user;
                            //LogInfo loginfo = new LogInfo();
                           // loginfo.Loaded += Loginfo_Loaded;
                            RegionManger.RequestNavigate("LogInfoRegion", "LogInfo");
                            //App.LogUser = new User() { Id = "B339656A-C220-4ED5-88CF-A7EC500BD71A", FirstName = "John",LastName="Don" };
                            if (user.RoleName == "Admin")
                            {
                                RegionManger.RequestNavigate("NavigationRegion", "UserNav");
                                RegionManger.RequestNavigate("MainRegion", "Projects");
                               // RegionManger.RequestNavigate("MainRegion", "Users");
                            }
                            else if (user.RoleName == "Desktop"||user.RoleName== "Desktop,Mobile")
                            {

                                RegionManger.RequestNavigate("NavigationRegion", "UserNav");
                                RegionManger.RequestNavigate("MainRegion", "Projects");
                            }


                           
                            // RaiseLoginCompletedEvent();
                            return await Task.FromResult(true);
                        }
                    }
                    else
                    {
                        Error = "Invalid username and password";
                        return await Task.FromResult(true);
                    }
                }
                else
                {
                    Error = "Invalid username and password";
                    return await Task.FromResult(true);
                }
                //User user=users.Where(c => c.UserName == Username && c.Pwd == Password).SingleOrDefault();

            }
            return await Task.FromResult(true);
        }

        private void Loginfo_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Submit(object parameter)
        {
            //implement logic
        }

        bool CanSubmit(object parameter)
        {
            return true;
        }
        public async Task ValidateLoginDetails()
        {

            //if (this.UserID == "MS\\vtadiko")
            //{
            //    //Check valid user or not
            //    //database actions
            //    RaiseLoginCompletedEvent();

            //}
            //else
            //{
            //    ErrorMessage = "Invalid User ID and (or) Password entered";
            //}
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Username = UI.Code.Properties.Settings.Default.username;
            Password = UI.Code.Properties.Settings.Default.password;
            IsRemember = UI.Code.Properties.Settings.Default.IsRemember;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        // public Action<object, object> LoginCompleted { get; internal set; }
        #endregion
    }
}
