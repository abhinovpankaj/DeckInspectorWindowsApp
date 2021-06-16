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
    public class ProjectLocationAddOrEditViewModel : BaseViewModel, INavigationAware
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

        public async Task<ErrorModel> Save()
        {
            //IsBusy = true;
            ErrorModel err = new ErrorModel();

            if (ProjectLocation != null)
            {
                if (string.IsNullOrEmpty(ProjectLocation.Name))
                {
                    err.Status = "Validation";
                    err.Message = "Please Enter Name";

                    return await Task.FromResult(err);
                }
                try
                {
                    if (string.IsNullOrEmpty(ProjectLocation.Id))
                    {
                        Response result = await projectLocationService.AddItemAsync(ProjectLocation);
                        if (result.Status == ApiResult.Success)
                        {
                            //Response getObj = await projectLocationService.GetItemAsync(result.ID);
                            //if (getObj.Status == ApiResult.Success)
                            //{
                            //    App.ProjectID = result.ID;
                            //    //  Project project = JsonConvert.DeserializeObject<Project>(getObj.Data.ToString());
                            //    // var parameters = new NavigationParameters { { "ProjectID", result.ID } };
                            //    RegionManger.RequestNavigate("MainRegion", "Project");
                            //}
                            err.Status = "Success";
                            err.Message = result.Message;
                            RegionManger.RequestNavigate("MainRegion", "Project");
                        }
                        else
                        {
                            err.Status = "Error";
                            err.Message = result.Message;
                        }
                    }
                    else
                    {
                        Response result = await projectLocationService.UpdateItemAsync(ProjectLocation);
                        if (result.Status == ApiResult.Success)
                        {
                          //  App.ProjectID = Project.Id;
                           // ErrorMsg = result.Message;
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
           // IsBusy = false;
            return await Task.FromResult(err);
        }
        // public DelegateCommand SaveCommand => new DelegateCommand(async () => await Save());
        //public async Task Save()
        //{
        //    if (ProjectLocation != null)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrEmpty(ProjectLocation.Id))
        //            {
        //                Response result = await projectLocationService.AddItemAsync(ProjectLocation);
        //                if (result.Status == ApiResult.Success)
        //                {
        //                    ErrorMsg = result.Message;
        //                    RegionManger.RequestNavigate("MainRegion", "Project");
        //                }
        //                else
        //                {
        //                    ErrorMsg = result.Message;
        //                }
        //            }
        //            else
        //            {
        //                Response result = await projectLocationService.UpdateItemAsync(ProjectLocation);
        //                if (result.Status == ApiResult.Success)
        //                {
        //                    ErrorMsg = result.Message;
        //                    RegionManger.RequestNavigate("MainRegion", "Project");
        //                }
        //                else
        //                {
        //                    ErrorMsg = result.Message;
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorMsg = ex.Message;

        //        }

        //    }
        //    else
        //    {

        //    }
        //}
        public DelegateCommand BackCommand => new DelegateCommand(async () => await Back());
        public async Task Back()
        {
            var parameters = new NavigationParameters {  { "Project", Project } };
          
            RegionManger.RequestNavigate("MainRegion", "Project", parameters);
        }

        public async Task OnLoaded()
        {
           // Users = new ObservableCollection<User>(await userService.GetItemsAsync());
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


        private ProjectLocation _ploc;

        public ProjectLocation ProjectLocation
        {
            get { return _ploc; }
            set { _ploc = value; OnPropertyChanged("ProjectLocation"); }

        }
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Project = (Project)navigationContext.Parameters["Project"];
            ProjectLocation = (ProjectLocation)navigationContext.Parameters["ProjectLocation"];
            if (ProjectLocation == null)
            {
                Title = "New Project Common Location";
                ProjectLocation = new ProjectLocation();
                ProjectLocation.Username = App.LogUser.FullName;
                ProjectLocation.CreatedOn = DateTime.Now.ToString("dd,MMM yyyy");
                ProjectLocation.ProjectId = Project.Id;
               // ProjectLocation.UserId = App.LogUser.Id;
                   

            }
            else
            {
                Title = "Edit Project";
              // Type = Project.ProjectType;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public ProjectLocationAddOrEditViewModel()
        {
            Title = "Project(s)";

            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }

    }
}
