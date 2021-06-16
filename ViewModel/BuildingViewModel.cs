using CommonServiceLocator;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Model;
using UI.Code.View.Dialog;

namespace UI.Code.ViewModel
{
    public class BuildingViewModel : BaseViewModel, INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        private string _search;

        public string SearchText
        {
            get { return _search; }
            set { _search = value; OnPropertyChanged("SearchText"); }
        }
        private DateTime? _date;

        public DateTime? CreatedOn
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged("CreatedOn"); }
        }








        private string _projectType;

        public string ProjectType
        {
            get { return _projectType; }
            set { _projectType = value; OnPropertyChanged("ProjectType"); }
        }
        private bool _isDetailShow;

        public bool DetailVisibility
        {
            get { return _isDetailShow; }
            set { _isDetailShow = value; OnPropertyChanged("DetailVisibility"); }
        }


        IRegionManager RegionManger
        {
            get
            {
                return (IRegionManager)Prism.Ioc.ContainerLocator.Container.Resolve(typeof(IRegionManager));
            }
        }


        private string error;

        public string ErrorMsg
        {
            get { return error; }
            set { error = value; OnPropertyChanged("ErrorMsg"); }
        }
        private Project _project;
        public Project Project
        {
            get { return _project; }
            set { _project = value; OnPropertyChanged("Project"); }
        }
        public DelegateCommand BackCommand => new DelegateCommand(async () => await Back());
        public async Task Back()
        {

            var parameters = new NavigationParameters { { "Project", Project }, { "ProjectBuilding", ProjectBuilding } };
            RegionManger.RequestNavigate("MainRegion", "Project", parameters);
            // RegionManger.RequestNavigate("MainRegion", "Project");
        }
        //public DelegateCommand BuildingSelectedItem => new DelegateCommand(async () => await BuildingSelected());
        //public async Task BuildingSelected()
        //{
        //    RegionManger.RequestNavigate("MainRegion", "Detail");
        //}

        private BuildingLocation _bloca;

        public BuildingLocation BuildingLocation
        {
            get { return _bloca; }
            set { _bloca = value; OnPropertyChanged("BuildingLocation"); }
        }
        private BuildingApartment _ba;

        public BuildingApartment BuildingApartment
        {
            get { return _ba; }
            set { _ba = value; OnPropertyChanged("BuildingApartment"); }
        }
        private ProjectBuilding _pb;

        public ProjectBuilding ProjectBuilding
        {
            get { return _pb; }
            set { _pb = value; OnPropertyChanged("ProjectBuilding"); }
        }
        private ProjectBuilding _pb1;
        public ProjectBuilding DataModel
        {
            get { return _pb1; }
            set { _pb1 = value; OnPropertyChanged("DataModel"); }
        }
        public DelegateCommand NewBuildingLocationItemCommand => new DelegateCommand(async () => await NewBuildingLocation());
        public async Task NewBuildingLocation()
        {
            var parameters = new NavigationParameters { { "Title", "New Building Common Location" }, { "BuildingLocation", null }, { "Project", Project }, { "ProjectBuilding", ProjectBuilding } };
            RegionManger.RequestNavigate("MainRegion", "BuildingLocationAddOrEdit", parameters);
            //  RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit", parameters);
            //if (ProjectBuilding != null)
            //{
            //    var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };

            //}
        }
        public DelegateCommand<BuildingLocation> EditBuildingLocationCommand => new DelegateCommand<BuildingLocation>(async (BuildingLocation parm) => await EditBuildingLocation(parm));
        public async Task EditBuildingLocation(BuildingLocation parm)
        {

            var parameters = new NavigationParameters { { "Title", "Edit Project" }, { "Project", Project }, { "ProjectBuilding", ProjectBuilding }, { "BuildingLocation", parm } };
            RegionManger.RequestNavigate("MainRegion", "BuildingLocationAddOrEdit", parameters);
        }
        public DelegateCommand NewApartmentItemCommand => new DelegateCommand(async () => await NewApartment());
        public async Task NewApartment()
        {
            var parameters = new NavigationParameters { { "Title", "New Project Common Location" }, { "ProjectLocation", null }, { "Project", Project }, { "ProjectBuilding", ProjectBuilding } };
            RegionManger.RequestNavigate("MainRegion", "BuildingApartmentAddOrEdit", parameters);
            //  RegionManger.RequestNavigate("MainRegion", "ProjectBuildingAddOrEdit", parameters);
            //if (ProjectBuilding != null)
            //{
            //    var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };

            //}
        }
        //public DelegateCommand DeleteCommand => new DelegateCommand(async () => await Delete());
        //public async Task Delete()
        //{
        //    ProjectBuilding.IsDelete = true;
        //    Response response = await ProjectBuildingDataStore.DeleteItemAsync(ProjectBuilding);
        //    if (response.Status == ApiResult.Success)
        //    {
        //        RegionManger.RequestNavigate("MainRegion", "Project");
        //    }
        //    //  RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit", parameters);
        //    //if (ProjectBuilding != null)
        //    //{
        //    //    var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };

        //    //}
        //}

        private bool sbusy;

        public bool IsBusy
        {
            get { return sbusy; }
            set { sbusy = value; OnPropertyChanged("IsBusy"); }
        }
        public async Task<ErrorModel> Delete()
        {
            ErrorModel err = new ErrorModel();
            ProjectBuilding.IsDelete = true;
            try
            {
                Response result = await ProjectBuildingDataStore.DeleteItemAsync(ProjectBuilding);
                if (result.Status == ApiResult.Success)
                {
                    RegionManger.RequestNavigate("MainRegion", "Project");
                    err.Status = "Success";
                    err.Message = result.Message;
                }
                else
                {
                    err.Status = "Error";
                    err.Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;
            }
            return await Task.FromResult(err);
            //  RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit", parameters);
            //if (ProjectBuilding != null)
            //{
            //    var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };

            //}
        }
        public DelegateCommand<BuildingApartment> EditBuildingApartmentCommand => new DelegateCommand<BuildingApartment>(async (BuildingApartment parm) => await EditBuildingApartment(parm));
        public async Task EditBuildingApartment(BuildingApartment parm)
        {

            var parameters = new NavigationParameters { { "Title", "Edit Project" }, { "Project", Project }, { "ProjectBuilding", ProjectBuilding }, { "BuildingApartment", parm } };
            RegionManger.RequestNavigate("MainRegion", "BuildingApartmentAddOrEdit", parameters);
        }
        public DelegateCommand<BuildingLocation> BuildingLocationSelectedItemCommand => new DelegateCommand<BuildingLocation>(async (BuildingLocation parm) => await BuildingLocationSelectedItem(parm));
        public async Task BuildingLocationSelectedItem(BuildingLocation parm)
        {
            //if (BuildingLocation != null)
            //{
            if (Project.ProjectType == "Visual Report")
            {
                var parameters = new NavigationParameters { { "BuildingLocation", parm }, { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
                RegionManger.RequestNavigate("MainRegion", "VisualBuildingLocation", parameters);
            }
            else
            {
                var parameters = new NavigationParameters { { "BuildingLocation", parm }, { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
                RegionManger.RequestNavigate("MainRegion", "InvasiveVisualBuildingLocationView", parameters);
            }
                
            //}
        }
        public DelegateCommand<BuildingApartment> BuildingApartmentSelectedItemCommand => new DelegateCommand<BuildingApartment>(async (BuildingApartment parm) => await BuildingApartmentSelectedItem(parm));
        public async Task BuildingApartmentSelectedItem(BuildingApartment parm)
        {
            if (Project.ProjectType == "Visual Report")
            {
                var parameters = new NavigationParameters { { "BuildingApartment", parm }, { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
                RegionManger.RequestNavigate("MainRegion", "VisualApartmentView", parameters);
            }
            else
            {
                var parameters = new NavigationParameters { { "BuildingApartment", parm }, { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
                RegionManger.RequestNavigate("MainRegion", "InvasiveVisualApartmentView", parameters);
            }
         
            //}
        }
        public DelegateCommand NewCommand => new DelegateCommand(async () => await New());
        public async Task New()
        {
            if (!string.IsNullOrEmpty(ProjectType))
            {
                ErrorMsg = string.Empty;
                var parameters = new NavigationParameters { { "Title", "New Project" }, { "Type", ProjectType } };
                RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit", parameters);
            }
            else
            {
                ErrorMsg = "Please Select Report Type";
            }
        }
        public DelegateCommand SearchCommand => new DelegateCommand(async () => await Search());
        public async Task Search()
        {
            var s = SearchText;
          //  var s1 = SelectedType;
            var s2 = CreatedOn;
        }


        public DelegateCommand CloseCommand => new DelegateCommand(async () => await Close());
        public async Task Close()
        {
            DetailVisibility = false;
        }


        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsInvasiveControlDisable = true;
            if (App.IsInvasive)
            {
                IsInvasiveControlDisable = false;
            }
            else
            {
                IsInvasiveControlDisable = true;
            }
            IsBusy = true;
            Project = (Project)navigationContext.Parameters["Project"];
            bool res = await Task.Run(() => LongOperation(navigationContext));
            if (res == true)
            {
                IsBusy = false;
            }
            //  await Task.Run(() => LongOperationApartment(navigationContext));

        }
        private ObservableCollection<BuildingLocation> _bloc;
        public ObservableCollection<BuildingLocation> BuildingLocations
        {
            get { return _bloc; }
            set { _bloc = value; OnPropertyChanged("BuildingLocations"); }
        }

        private ObservableCollection<BuildingApartment> _pbuild;
        public ObservableCollection<BuildingApartment> BuildingApartments
        {
            get { return _pbuild; }
            set { _pbuild = value; OnPropertyChanged("BuildingApartments"); }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public async Task<ErrorModel> ReorderBuildinLocation()
        {
            ErrorModel err = new ErrorModel();
            try
            {
                List<ReorderList> list = new List<ReorderList>();
                int index = 0;
                foreach (var item in BuildingLocations)
                {
                    index = index + 1;
                    list.Add(new ReorderList() { Id = item.Id, SeqNo = index });
                }
                Response result = await BuildingLocationDataStore.Reorder(list);

                if (result.Status == ApiResult.Success)
                {

                    err.Status = "Success";
                    err.Message = result.Message;
                }
                else
                {
                    err.Status = "Error";
                    err.Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;
            }
            return await Task.FromResult(err);
        }
        public async Task<ErrorModel> Save()
        {
            ErrorModel err = new ErrorModel();

            if (DataModel != null)
            {
                if (string.IsNullOrEmpty(DataModel.Name))
                {
                    err.Status = "Validation";
                    err.Message = "Please Enter Name";

                    return await Task.FromResult(err);
                }
                try
                {
                    if (string.IsNullOrEmpty(DataModel.Id))
                    {

                        //Response result = await projectService.AddItemAsync(Project);
                        //if (result.Status == ApiResult.Success)
                        //{
                        //    Response getObj = await projectService.GetItemAsync(result.ID);
                        //    if (getObj.Status == ApiResult.Success)
                        //    {
                        //        App.ProjectID = result.ID;
                        //        //  Project project = JsonConvert.DeserializeObject<Project>(getObj.Data.ToString());
                        //        // var parameters = new NavigationParameters { { "ProjectID", result.ID } };
                        //        RegionManger.RequestNavigate("MainRegion", "Project");
                        //    }
                        //    err.Status = "Success";
                        //    err.Message = result.Message;
                        //    // RegionManger.RequestNavigate("MainRegion", "Projects");
                        //}
                        //else
                        //{
                        //    err.Status = "Error";
                        //    err.Message = result.Message;
                        //}
                    }
                    else
                    {
                        Response result = await ProjectBuildingDataStore.UpdateItemAsync(DataModel);
                        if (result.Status == ApiResult.Success)
                        {

                            //App.ProjectID = Project.Id;
                            //ErrorMsg = result.Message;
                            var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
                            RegionManger.RequestNavigate("MainRegion", "Building", parameters);
                            
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
        public async Task<ErrorModel> ReorderBuildingApartment()
        {
            ErrorModel err = new ErrorModel();
            try
            {
                List<ReorderList> list = new List<ReorderList>();
                int index = 0;
                foreach (var item in BuildingApartments)
                {
                    index = index + 1;
                    list.Add(new ReorderList() { Id = item.Id, SeqNo = index });
                }
                Response result = await BuildingApartmentDataStore.Reorder(list);

                if (result.Status == ApiResult.Success)
                {

                    err.Status = "Success";
                    err.Message = result.Message;
                }
                else
                {
                    err.Status = "Error";
                    err.Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;
            }
            return await Task.FromResult(err);
        }
        private string _objStrin;

        public string ObjectString
        {
            get { return _objStrin; }
            set { _objStrin = value; }
        }
        public async Task<bool> LongOperation(NavigationContext navigationContext)
        {
            Project = (Project)navigationContext.Parameters["Project"];
            ProjectBuilding = DataModel= (ProjectBuilding)navigationContext.Parameters["ProjectBuilding"];
            ObjectString = Newtonsoft.Json.JsonConvert.SerializeObject(ProjectBuilding);
            if (ProjectBuilding != null)
            {
                BuildingLocations = new ObservableCollection<BuildingLocation>(await BuildingLocationDataStore.GetItemsAsyncByBuildingId(ProjectBuilding.Id));
                BuildingApartments = new ObservableCollection<BuildingApartment>(await BuildingApartmentDataStore.GetItemsAsyncByBuildingId(ProjectBuilding.Id));
            }
            //    BuildingApartments = new ObservableCollection<BuildingApartment>(await BuildingApartmentDataStore.GetItemsAsyncByBuildingId(ProjectBuilding.Id));
            //}
            return await Task.FromResult(true);
        }
        private bool _isInvasiveControlDisable;

        public bool IsInvasiveControlDisable
        {
            get { return _isInvasiveControlDisable; }
            set { _isInvasiveControlDisable = value; OnPropertyChanged("IsInvasiveControlDisable"); }
        }
        public async void LongOperationApartment(NavigationContext navigationContext)
        {
            Project = (Project)navigationContext.Parameters["Project"];
            ProjectBuilding=DataModel = (ProjectBuilding)navigationContext.Parameters["ProjectBuilding"];
            if (ProjectBuilding != null)
            {

            }
           
            //}
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        private ObservableCollection<Project> _projects;
        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set { _projects = value; OnPropertyChanged("Projects"); }
        }

       


        public BuildingViewModel(IDialogService dialogService)
        {


            Title = "Building Detail";


            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }

    }
}
