using CommonServiceLocator;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Model;

namespace UI.Code.ViewModel
{
    public class UserAddOrEditViewModel : BaseViewModel,INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        IRegionManager RegionManger
        {
            get
            {
                return (IRegionManager)Prism.Ioc.ContainerLocator.Container.Resolve(typeof(IRegionManager));
            }
        }

        private ObservableCollection<User> _users;
        public DelegateCommand OnLoadedCommand => new DelegateCommand(async () => await OnLoaded());
        public DelegateCommand NewCommand => new DelegateCommand(async () => await New());
        public async Task New()
        {
            RegionManger.RequestNavigate("MainRegion", "ProjectDetail",new NavigationParameters("New"));
        }
        public DelegateCommand BackCommand => new DelegateCommand(async () => await Back());
        public async Task Back()
        {
            RegionManger.RequestNavigate("MainRegion", "Users");
        }
        private bool sbusy;

        public bool IsBusy
        {
            get { return sbusy; }
            set { sbusy = value; OnPropertyChanged("IsBusy"); }
        }
        public async Task<ErrorModel> Save()
        {
            IsBusy = true;
            ErrorModel err = new ErrorModel();

            if (User != null)
            {
               
                if (string.IsNullOrEmpty(User.FirstName))
                {
                    err.Status = "Validation";
                    err.Message = "Please Enter Name";

                    return await Task.FromResult(err);
                }
                try
                {
                    if (string.IsNullOrEmpty(User.Id))
                    {
                        Response result = await userService.AddItemAsync(User);
                        if (result.Status == ApiResult.Success)
                        {
                            //Response getObj = await projectService.GetItemAsync(result.ID);
                            //if (getObj.Status == ApiResult.Success)
                            //{
                            //    //App.ProjectID = result.ID;
                            //    //  Project project = JsonConvert.DeserializeObject<Project>(getObj.Data.ToString());
                            //    // var parameters = new NavigationParameters { { "ProjectID", result.ID } };
                            //    RegionManger.RequestNavigate("MainRegion", "Users");
                            //}
                            err.Status = "Success";
                            err.Message = result.Message;
                            // RegionManger.RequestNavigate("MainRegion", "Projects");
                        }
                        else
                        {
                            err.Status = "Error";
                            err.Message = result.Message;
                        }
                    }
                    else
                    {
                        Response result = await userService.UpdateItemAsync(User);
                        if (result.Status == ApiResult.Success)
                        {
                           
                          //  RegionManger.RequestNavigate("MainRegion", "Users");
                            err.Status = "Success";
                            err.Message = result.Message;
                        }
                        else
                        {
                            err.Status = "Error";
                            err.Message = result.Message;
                            //   ErrorMsg = result.Message;
                        }
                    }

                }
                catch (Exception ex)
                {
                    err.Status = "Error";
                    err.Message = ex.Message;

                }

            }
            IsBusy = false;
            return await Task.FromResult(err);
        }
        public async Task OnLoaded()
        {
          //  Users = new ObservableCollection<User>(await userService.GetItemsAsync());
        }
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged("Users"); }
        }

        private User _user;

        public User  User
        {
            get { return _user; }
            set { _user = value;OnPropertyChanged("User"); }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }
        private ObservableCollection<string> _actveDeActive;

        public ObservableCollection<string> ActiveList
        {
            get { return _actveDeActive; }
            set { _actveDeActive = value; OnPropertyChanged("ActiveList"); }
        }
        private string myVar;

        public string SelectedType
        {
            get { return myVar; }
            set { myVar = value; OnPropertyChanged("SelectedType"); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsBusy = true;
            ActiveList = new ObservableCollection<string>();
            ActiveList.Add("Active");
            ActiveList.Add("Inactive");
            //Type = (string)navigationContext.Parameters["Type"];
            User = (User)navigationContext.Parameters["User"];
            if(User == null)
            {
             //   SelectedType = "Activate";
                Title = "New User";
                User = new User();
                SelectedType = "Active";

            }
            else
            {
                Title = "Edit User";
             //   SelectedType = User.Status;
                //   Type = Project.ProjectType;
            }
            IsBusy = false;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public UserAddOrEditViewModel()
        {
            Title = "Project(s)";

          //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }
        
    }
}
