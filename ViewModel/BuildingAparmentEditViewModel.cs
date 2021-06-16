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
    public class BuildingAparmentEditViewModel : BaseViewModel, INavigationAware
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

        //  public DelegateCommand SaveCommand => new DelegateCommand(async () => await Save());
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }
        public async Task<ErrorModel> Save()
        {

            ErrorModel err = new ErrorModel();

            if (BuildingApartment != null)
            {
                if (string.IsNullOrEmpty(BuildingApartment.Name))
                {
                    err.Status = "Validation";
                    err.Message = "Please Enter Name";

                    return await Task.FromResult(err);
                }
                try
                {
                    if (string.IsNullOrEmpty(BuildingApartment.Id))
                    {
                        Response result = await BuildingApartmentDataStore.AddItemAsync(BuildingApartment);
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
                            // RegionManger.RequestNavigate("MainRegion", "Building");
                            BackCall();
                        }
                        else
                        {
                            err.Status = "Error";
                            err.Message = result.Message;
                        }
                    }
                    else
                    {
                        Response result = await BuildingApartmentDataStore.UpdateItemAsync(BuildingApartment);
                        if (result.Status == ApiResult.Success)
                        {
                            //  App.ProjectID = Project.Id;
                            // ErrorMsg = result.Message;
                            // RegionManger.RequestNavigate("MainRegion", "Building");
                            BackCall();
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
        //public async Task Save()
        //{
        //    if (BuildingApartment != null)
        //    {
        //        try
        //        {
        //            if (string.IsNullOrEmpty(BuildingApartment.Id))
        //            {
        //                Response result = await BuildingApartmentDataStore.AddItemAsync(BuildingApartment);
        //                if (result.Status == ApiResult.Success)
        //                {
        //                    ErrorMsg = result.Message;
        //                    //   RegionManger.RequestNavigate("MainRegion", "Building");
        //                    BackCall();
        //                }
        //                else
        //                {
        //                    ErrorMsg = result.Message;
        //                }
        //            }
        //            else
        //            {
        //                Response result = await BuildingApartmentDataStore.UpdateItemAsync(BuildingApartment);
        //                if (result.Status == ApiResult.Success)
        //                {
        //                    ErrorMsg = result.Message;
        //                    BackCall();
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
        public void BackCall()
        {
            var parameters = new NavigationParameters { { "Project", Project }, { "ProjectBuilding", ProjectBuilding }, { "BuildingApartment", BuildingApartment } };


            //Project = (Project)navigationContext.Parameters["Project"];
            //ProjectBuilding = (ProjectBuilding)navigationContext.Parameters["ProjectBuilding"];
            //BuildingLocation = (BuildingLocation)navigationContext.Parameters["BuildingLocation"];
            RegionManger.RequestNavigate("MainRegion", "Building", parameters);
        }
        public DelegateCommand BackCommand => new DelegateCommand(async () => await Back());
        public async Task Back()
        {
            //var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }};
            BackCall();
            //   RegionManger.RequestNavigate("MainRegion", "Building");
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


        private BuildingApartment _ploc;

        public BuildingApartment BuildingApartment
        {
            get { return _ploc; }
            set { _ploc = value; OnPropertyChanged("BuildingApartment"); }

        }


        private ProjectBuilding _ploc1;

        public ProjectBuilding ProjectBuilding
        {
            get { return _ploc1; }
            set { _ploc1 = value; OnPropertyChanged("ProjectBuilding"); }

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
            BuildingApartment = (BuildingApartment)navigationContext.Parameters["BuildingApartment"];
            if (BuildingApartment == null)
            {
                Title = "New Apartment";
                BuildingApartment = new BuildingApartment();

                BuildingApartment.Username = App.LogUser.FullName;
                BuildingApartment.CreatedOn = DateTime.Now.ToString("dd,MMM yyyy");
                BuildingApartment.BuildingId = ProjectBuilding.Id;
            }
            else
            {
                Title = "Edit Building Apartment";
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

        public BuildingAparmentEditViewModel()
        {
            Title = "Apartment";

            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }


    }
}
