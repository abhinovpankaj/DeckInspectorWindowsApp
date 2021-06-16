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
    public class BuildingAddOrEditViewModel : BaseViewModel, INavigationAware
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

        //public DelegateCommand SaveCommand => new DelegateCommand(async () => await Save());
        //public async Task Save()
        //{
        //    if (ProjectBuilding != null)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrEmpty(ProjectBuilding.Id))
        //            {
        //                Response result = await ProjectBuildingDataStore.AddItemAsync(ProjectBuilding);
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
        //                Response result = await ProjectBuildingDataStore.UpdateItemAsync(ProjectBuilding);
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

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }

        public async Task<ErrorModel> Save()
        {
            
            ErrorModel err = new ErrorModel();

            if (ProjectBuilding != null)
            {
                if (string.IsNullOrEmpty(ProjectBuilding.Name))
                {
                    err.Status = "Validation";
                    err.Message = "Please Enter Name";

                    return await Task.FromResult(err);
                }
                try
                {
                    if (string.IsNullOrEmpty(ProjectBuilding.Id))
                    {
                        Response result = await ProjectBuildingDataStore.AddItemAsync(ProjectBuilding);
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
                        Response result = await ProjectBuildingDataStore.UpdateItemAsync(ProjectBuilding);
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
           
            return await Task.FromResult(err);
        }
        public DelegateCommand BackCommand => new DelegateCommand(async () => await Back());
        public async Task Back()
        {
            var parameters = new NavigationParameters { { "Project", Project } };

            RegionManger.RequestNavigate("MainRegion", "Project", parameters);
        }

        public async Task OnLoaded()
        {
            //Users = new ObservableCollection<User>(await userService.GetItemsAsync());
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


        private ProjectBuilding _ploc;

        public ProjectBuilding ProjectBuilding
        {
            get { return _ploc; }
            set { _ploc = value; OnPropertyChanged("ProjectBuilding"); }

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
            ProjectBuilding = (ProjectBuilding)navigationContext.Parameters["ProjectBuilding"];
            if (ProjectBuilding == null)
            {
                Title = "New Building";
                ProjectBuilding = new ProjectBuilding();
              
                ProjectBuilding.Username = App.LogUser.FullName;
                ProjectBuilding.CreatedOn = DateTime.Now.ToString("dd,MMM yyyy");
                ProjectBuilding.ProjectId = Project.Id;
            }
            else
            {
                Title = "Edit Building";
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

        public BuildingAddOrEditViewModel()
        {
            Title = "Project(s)";

            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }

    }
}
