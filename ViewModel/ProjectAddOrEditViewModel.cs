using CommonServiceLocator;
using Newtonsoft.Json;
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
    public class ProjectAddOrEditViewModel : BaseViewModel, INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }
        private string _error;

        public string ErrorMsg
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("ErrorMsg"); }
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
            RegionManger.RequestNavigate("MainRegion", "ProjectDetail", new NavigationParameters("New"));
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }
        //  public DelegateCommand SaveCommand => new DelegateCommand(async () => await Save());
        public void Go()
        {
            if (Project.Category=="SingleLevel")
            {

                var parameters = new NavigationParameters { { "Project", Project } };
                RegionManger.RequestNavigate("MainRegion", "SingleLevelProject", parameters);
            }
            else
                RegionManger.RequestNavigate("MainRegion", "Project");

        }
        public async Task<ErrorModel> Save()
        {
            ErrorModel err = new ErrorModel();
            
            if (Project != null)
            {
                if (Project.IsSingle==null)
                {
                    err.Status = "Validation";
                    err.Message = "Please Select Project Category";

                    return await Task.FromResult(err);
                }
                else
                {
                    Project.Category = (bool)Project.IsSingle ? "SingleLevel" : "MultiLevel";
                }
                if (string.IsNullOrEmpty(Project.Name))
                {
                    err.Status = "Validation";
                    err.Message = "Please Enter Name";
                   
                    return await Task.FromResult(err);
                }
                try
                {
                    Project.UserId =new Guid( App.LogUser.Id);
                    if (string.IsNullOrEmpty(Project.Id))
                    {
                        Response result = await projectService.AddItemAsync(Project);
                        if (result.Status == ApiResult.Success)
                        {
                             App.ProjectID = result.ID;
                            Project.Id = result.ID;
                            err.Status = "Success";
                            err.Message = result.Message;
                            
                        }
                        else
                        {
                            err.Status = "Error";
                            err.Message = result.Message;
                        }
                    }
                    else
                    {
                        Response result = await projectService.UpdateItemAsync(Project);
                        if (result.Status == ApiResult.Success)
                        {
                            App.ProjectID = Project.Id;
                            ErrorMsg = result.Message;
                            RegionManger.RequestNavigate("MainRegion", "Project");
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
            return await Task.FromResult(err);
        }
        public DelegateCommand BackCommand => new DelegateCommand(async () => await Back());
        public async Task Back()
        {
            RegionManger.RequestNavigate("MainRegion", "Projects");
        }

        public async Task OnLoaded()
        {
         //   Users = new ObservableCollection<User>(await userService.GetItemsAsync());
        }
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged("Users"); }
        }

        private Project _project;

        public Project Project
        {
            get { return _project; }

            set { _project = value; OnPropertyChanged("Project"); }

        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsBusy = true;
          
            Type = (string)navigationContext.Parameters["Type"];
            Project = (Project)navigationContext.Parameters["Project"];
            if (Project == null)
            {
                Title = "New Project";
               
                Project = new Project();
               
                Project.Username = App.LogUser.FullName;
                Project.CreatedOn = DateTime.Now.ToString("dd,MMM yyyy");
                Project.ProjectType = Type;
            }
            else
            {
                Title = "Edit Project";
                Type = Project.ProjectType;
            }
            IsBusy = false;
        }

        //public async Task<bool> LongOperation(NavigationContext navigationContext)
        //{
        //    return await Task.FromResult(true);
        //}

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public ProjectAddOrEditViewModel()
        {
            Title = "Project(s)";

            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }

    }
}
