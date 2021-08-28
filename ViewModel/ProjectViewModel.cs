using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Model;
using UI.Code.View.Dialog;

namespace UI.Code.ViewModel
{
    public class ProjectViewModel : BaseViewModel, INavigationAware
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
        private DateTime ? _date;

        public DateTime ? CreatedOn
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged("CreatedOn"); }
        }

        


       

        private Project _project;

        public Project Project
        {
            get { return _project; }
            set { _project = value;  }
        }
        //OnPropertyChanged("Project");
        private Project _projectDataModel;

        public Project DataModel
        {
            get { return _projectDataModel; }
            set { _projectDataModel = value; OnPropertyChanged("DataModel"); }
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
        public DelegateCommand BackCommand => new DelegateCommand(async () => await Back());
        public async Task Back()
        {
            RegionManger.RequestNavigate("MainRegion", "Projects");
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
            var s1 = SelectedType;
            var s2 = CreatedOn;
        }
        

        public DelegateCommand CloseCommand => new DelegateCommand(async () => await Close());
        public async Task Close()
        {
            DetailVisibility = false;
        }
        private ProjectBuilding _projectBuilding;

        public ProjectBuilding ProjectBuilding
        {
            get { return _projectBuilding; }
            set { _projectBuilding = value; OnPropertyChanged("ProjectBuilding"); }
        }


       
        public DelegateCommand<ProjectLocation> EditLocationCommand => new DelegateCommand<ProjectLocation>(async (ProjectLocation parm) => await EditLocation(parm));
        public async Task EditLocation(ProjectLocation parm)
        {
            // ShowDialog();
            //DetailVisibility = true;
            var parameters = new NavigationParameters { { "Title", "Edit Project" }, { "Project", Project } ,{ "ProjectLocation", parm } };
            RegionManger.RequestNavigate("MainRegion", "ProjectLocationAddEdit", parameters);
        }
        public DelegateCommand NewLocationItemCommand => new DelegateCommand(async () => await NewLocation());
        public async Task NewLocation()
        {
            var parameters = new NavigationParameters { { "Title", "New Building Common Location" },{ "BuildingLocation", null },{ "Project", Project} };
            RegionManger.RequestNavigate("MainRegion", "ProjectLocationAddEdit", parameters);
            //  RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit", parameters);
            //if (ProjectBuilding != null)
            //{
            //    var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
                
            //}
        }
      //  public DelegateCommand DeleteCommand => new DelegateCommand(async () => await Delete());
        public async Task<ErrorModel> Delete()
        {
            ErrorModel err = new ErrorModel();
            Project.IsDelete = true;
            try
            {
                Response result = await projectService.DeleteItemAsync(Project);
                if (result.Status == ApiResult.Success)
                {
                    RegionManger.RequestNavigate("MainRegion", "Projects");
                    err.Status = "Success";
                    err.Message = result.Message;
                }
                else
                {
                    err.Status = "Error";
                    err.Message = result.Message;
                }
            }
            catch(Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;
            }
            return await Task.FromResult(err);
            
        }
       
        public async Task<ErrorModel> ReorderProjectLocation()
        {
            ErrorModel err = new ErrorModel();
            try
            {
                List<ReorderList> list = new List<ReorderList>();
                int index = 0;
                foreach (var item in ProjectLocations)
                {
                    index = index + 1;
                    list.Add(new ReorderList() { Id = item.Id, SeqNo = index });
                }
                Response result = await projectLocationService.Reorder(list);
               
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
            catch(Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;
            }
            return await Task.FromResult(err);
        }

        public async Task<ErrorModel> ReorderProjectBuilding()
        {
            ErrorModel err = new ErrorModel();
            try
            {
                List<ReorderList> list = new List<ReorderList>();
                int index = 0;
                foreach (var item in ProjectBuildings)
                {
                    index = index + 1;
                    list.Add(new ReorderList() { Id = item.Id, SeqNo = index });
                }
                Response result = await ProjectBuildingDataStore.Reorder(list);

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
        public DelegateCommand<ProjectBuilding> EditBuildingCommand => new DelegateCommand<ProjectBuilding>(async (ProjectBuilding parm) => await EditBuilding(parm));
        public async Task EditBuilding(ProjectBuilding parm)
        {
            // ShowDialog();
            //DetailVisibility = true;
            var parameters = new NavigationParameters { { "Title", "Edit Project" }, { "Project", Project }, { "ProjectBuilding", parm } };
            RegionManger.RequestNavigate("MainRegion", "BuildingAddOrEdit", parameters);
        }
        public DelegateCommand NewBuildingItemCommand => new DelegateCommand(async () => await NewBuilding());
        public async Task NewBuilding()
        {
            var parameters = new NavigationParameters { { "Title", "New Project Common Location" }, { "ProjectLocation", null }, { "Project", Project } };
            RegionManger.RequestNavigate("MainRegion", "BuildingAddOrEdit", parameters);
            //  RegionManger.RequestNavigate("MainRegion", "ProjectBuildingAddOrEdit", parameters);
            //if (ProjectBuilding != null)
            //{
            //    var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };

            //}
        }
        //public DelegateCommand ProjBuildingSelectedItemCommand => new DelegateCommand(async () => await ProjBuildingSelectedItem());
        //public async Task ProjBuildingSelectedItem()
        //{

        //    if (ProjectBuilding != null)
        //    {
        //        var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
        //        RegionManger.RequestNavigate("MainRegion", "Building", parameters);
        //    }
        //}
        public DelegateCommand<ProjectBuilding> ProjBuildingSelectedItemCommand => new DelegateCommand<ProjectBuilding>(async (ProjectBuilding parm) => await ProjBuildingSelectedItem(parm));
        public async Task ProjBuildingSelectedItem(ProjectBuilding parm)
        {
            //if (ProjectBuilding != null)
            //{
                var parameters = new NavigationParameters { { "ProjectBuilding", parm }, { "Project", Project } };
                RegionManger.RequestNavigate("MainRegion", "Building", parameters);
           // }
        }
        public DelegateCommand<ProjectLocation> ProjLocationSelectedItemCommand => new DelegateCommand<ProjectLocation>(async (ProjectLocation parm) => await ProjLocationSelectedItem(parm));
        public async Task ProjLocationSelectedItem(ProjectLocation parm)
        {
            //if (ProjectLocation != null)
            //{
                if (Project.ProjectType == "Visual Report")
                {
                    var parameters = new NavigationParameters { { "ProjectLocation", parm }, { "Project", Project } };
                    RegionManger.RequestNavigate("MainRegion", "VisualProjectLocation", parameters);
                }
                else
                {
                    var parameters = new NavigationParameters { { "ProjectLocation", parm }, { "Project", Project } };
                // RegionManger.RequestNavigate("MainRegion", "DetailLocation", parameters);
                     RegionManger.RequestNavigate("MainRegion", "InvasiveVisualProjectLocationView", parameters);
                }
              

            //}
            //await Task.Run(() =>
            //{

            //});

        }
        private ProjectLocation _projectLocation;

        public ProjectLocation ProjectLocation
        {
            get { return _projectLocation; }
            set { _projectLocation = value; OnPropertyChanged("ProjectLocation"); }
        }
       
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsBusy =true;

            bool status=   await Task.Run(() => LongOperation(navigationContext));
            if(status == true)
            {
                IsBusy = false;
            }
          
        }
        private ObservableCollection<ProjectLocation> _projectlocations;
        public ObservableCollection<ProjectLocation> ProjectLocations
        {
            get { return _projectlocations; }
            set { _projectlocations = value; OnPropertyChanged("ProjectLocations"); }
        }

        private ObservableCollection<ProjectBuilding> _pbuild;
        public ObservableCollection<ProjectBuilding> ProjectBuildings
        {
            get { return _pbuild; }
            set { _pbuild = value; OnPropertyChanged("ProjectBuildings"); }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true; 
        }
        private bool sbusy;

        public bool IsBusy
        {
            get { return sbusy; }
            set { sbusy = value; OnPropertyChanged("IsBusy"); }
        }
        private string _InvasiveText;

        private bool _canInvasiveCreate;

        public bool CanInvasiveCreate
        {
            get { return _canInvasiveCreate; }
            set { _canInvasiveCreate = value; OnPropertyChanged("CanInvasiveCreate"); }
        }
        private bool _isCreateOrRefreshInvasive;

        public bool IsCreateOrRefreshInvasive
        {
            get { return _isCreateOrRefreshInvasive; }
            set { _isCreateOrRefreshInvasive = value; OnPropertyChanged("IsCreateOrRefreshInvasive"); }
        }
        private string _btnInvasiveText;

        public string BtnInvasiveText
        {
            get { return _btnInvasiveText; }
            set { _btnInvasiveText = value; OnPropertyChanged("BtnInvasiveText"); }
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
                        Response result = await projectService.UpdateItemAsync(DataModel);
                        if (result.Status == ApiResult.Success)
                        {

                            //App.ProjectID = Project.Id;
                            //ErrorMsg = result.Message;
                            var parameters = new NavigationParameters { { "Project", DataModel } };
                            RegionManger.RequestNavigate("MainRegion", "Project", parameters);
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
        public bool Compare(object obj1, object obj2)
        {
            if (obj1 == null || obj2 == null)
            {
                return false;
            }
            if (!obj1.GetType().Equals(obj2.GetType()))
            {
                return false;
            }

            Type type = obj1.GetType();
            if (type.IsPrimitive || typeof(string).Equals(type))
            {
                return obj1.Equals(obj2);
            }
            if (type.IsArray)
            {
                Array first = obj1 as Array;
                Array second = obj2 as Array;
                var en = first.GetEnumerator();
                int i = 0;
                while (en.MoveNext())
                {
                    if (!Compare(en.Current, second.GetValue(i)))
                        return false;
                    i++;
                }
            }
            else
            {
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                {
                    var val = pi.GetValue(obj1);
                    var tval = pi.GetValue(obj2);
                    if (!Compare(val, tval))
                        return false;
                }
                foreach (FieldInfo fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                {
                    var val = fi.GetValue(obj1);
                    var tval = fi.GetValue(obj2);
                    if (!Compare(val, tval))
                        return false;
                }
            }
            return true;
        }
        private string _objStrin;

        public string ObjectString
        {
            get { return _objStrin; }
            set { _objStrin = value; }
        }
        public async Task<ErrorModel> CreateInvasive()
        {
            ErrorModel err = new ErrorModel();
            IsBusy = true;

            if (App.IsInvasive == true)
            {
                Project.IsInvasive = true;
                Project.Id = Project.InvasiveProjectID;
                var response = await Task.Run(() =>
                  projectService.CreateInvasiveReport(Project)
                );
                if (response.Status == ApiResult.Success)
                {
                    err.Status = "Success";
                    err.Message = response.Message;
                    App.IsInvasive = true;
                    Project.Id = response.ID.ToString();
                    var parameters = new NavigationParameters { { "Project", Project } };
                    RegionManger.RequestNavigate("MainRegion", "Project", parameters);
                    //if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(ProjectDetail))
                    //{
                    //   await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = Project } });
                    //}
                 
                    IsBusy = false;

                    //  await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    
                    err.Status = "Success";
                    err.Message = response.Message;
                }

            }
            else
            {
                Project.IsInvasive = false;
                // Project.Id = Project.InvasiveProjectID;
                var response = await Task.Run(() =>
                  projectService.CreateInvasiveReport(Project)
                );
                if (response.Status == ApiResult.Success)
                {
                    err.Status = "Success";
                    err.Message = response.Message;
                    App.IsInvasive = true;
                    Project.Id = response.ID.ToString();
                    // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = Project } });
                    var parameters = new NavigationParameters { { "Project", Project } };
                    RegionManger.RequestNavigate("MainRegion", "Project", parameters);
                    IsBusy = false;
                    //  await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    err.Status = "Success";
                    err.Message = response.Message;

                }
            }
         

            return await Task.FromResult(err);
        }
        public async Task<bool> LongOperation(NavigationContext navigationContext)
        {
            IsInvasiveControlDisable = true;

            // IsBusy =true;
            Response result = await projectService.GetItemAsync(App.ProjectID);
            if (result.Status == ApiResult.Success)
            {
                Project= DataModel= JsonConvert.DeserializeObject<Project>(result.Data.ToString());
                ObjectString = Newtonsoft.Json.JsonConvert.SerializeObject(Project);

                if (Project.ProjectType != "Invasive")
                {
                    if (Project.IsInvaisveExist == true)
                    {
                        CanInvasiveCreate = true;
                        BtnInvasiveText = "Invasive";
                    }
                    if (!string.IsNullOrEmpty(Project.InvasiveProjectID))
                    {
                        CanInvasiveCreate = false;
                        BtnInvasiveText = "Invasive";

                    }
                }
                else
                {
                     CanInvasiveCreate = true;
                     BtnInvasiveText = "Refresh";
                    
                }
                // Project = (Project)navigationContext.Parameters["Project"];
            }
                
            ProjectLocations = new ObservableCollection<ProjectLocation>(await projectLocationService.GetItemsAsyncByProjectID(App.ProjectID));
            ProjectBuildings = new ObservableCollection<ProjectBuilding>(await ProjectBuildingDataStore.GetItemsAsyncByProjectID(App.ProjectID));
            // IsBusy = false;
            if (App.IsInvasive)
            {
                IsInvasiveControlDisable = false;
            }
            else
            {
                IsInvasiveControlDisable = true;
            }

            return await Task.FromResult(true);
            //Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync());
        }
        private bool _isInvasiveControlDisable;

        public bool IsInvasiveControlDisable
        {
            get { return _isInvasiveControlDisable; }
            set { _isInvasiveControlDisable = value; OnPropertyChanged("IsInvasiveControlDisable"); }
        }
        public async void ReloadLocation()
        {
          
            ProjectLocations = new ObservableCollection<ProjectLocation>(await projectLocationService.GetItemsAsyncByProjectID(App.ProjectID));
        }
        public async void ReloadBuilding()
        {
            ProjectBuildings = new ObservableCollection<ProjectBuilding>(await ProjectBuildingDataStore.GetItemsAsyncByProjectID(App.ProjectID));
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

        public async Task<ErrorModel> AssignLocation()
        {
            IsBusy = true;
            ErrorModel err = new ErrorModel();

            AssignLocationAndBuilding item = new AssignLocationAndBuilding();
            item.Users = UsersAssignList.ToList();
            item.ParentID = ProjectLocation.Id;
            item.Type = "Location";
            try
            {


                Response result = await userService.Assign(item);
                if (result.Status == ApiResult.Success)
                {

                    err.Status = "Success";
                    err.Message = result.Message;
                }
                // RegionManger.RequestNavigate("MainRegion", "Projects");

            }

            catch (Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;

            }

            IsBusy = false;
            return await Task.FromResult(err);
        }
        public async Task<ErrorModel> AssignBuilding()
        {
            IsBusy = true;
            ErrorModel err = new ErrorModel();

            AssignLocationAndBuilding item = new AssignLocationAndBuilding();
            item.Users = UsersAssignList.ToList();
            item.ParentID = ProjectBuilding.Id;
            item.Type = "Building";
            try
            {


                Response result = await userService.Assign(item);
                if (result.Status == ApiResult.Success)
                {

                    err.Status = "Success";
                    err.Message = result.Message;
                }
                // RegionManger.RequestNavigate("MainRegion", "Projects");

            }

            catch (Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;

            }

            IsBusy = false;
            return await Task.FromResult(err);
        }
        private string _us;

        public string UserSearch
        {
            get { return _us; }
            set { _us = value; OnPropertyChanged("UserSearch"); }
        }
        private ObservableCollection<User> _ulist;
        public ObservableCollection<User> UsersAssignList
        {
            get { return _ulist; }
            set { _ulist = value; OnPropertyChanged("UsersAssignList"); }
        }
        public async Task<bool> GetUserListForAssign(string UserID, string ProjectID, string ProjectLocationID, string BuildingID, string Type,string searchText)
        {

            UsersAssignList = new ObservableCollection<User>(await userService.GetUserListForAssign(UserID, ProjectID, ProjectLocationID, BuildingID, Type, searchText));
            return await Task.FromResult(true);
        }

        public ProjectViewModel(IDialogService dialogService)
        {
           
           
            Title = "Project Detail";
          

            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }
       
    }
}
