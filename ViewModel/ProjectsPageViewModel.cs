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
    public class ProjectsPageViewModel : BaseViewModel, INavigationAware
    {
        private ObservableCollection<string> _pType;

        public ObservableCollection<string> ProjectTypeList
        {
            get { return _pType; }
            set { _pType = value; OnPropertyChanged("ProjectTypeList"); }
        }
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






        private Project selectedItem;

        public Project SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged("SelectedItem"); }
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

        public DelegateCommand NewCommand => new DelegateCommand(async () => await New());
        public async Task New()
        {
            ProjectType = "Visual Report";
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
        public DelegateCommand ResetCommand => new DelegateCommand(async () => await Reset());
        public async Task Reset()
        {
            SearchText = string.Empty;
            CreatedOn = null;
            IsCompleted = false;
            SelectedeportType = "Running";
            SelectedeportType = "All";
            await Task.Run(() => LongOperation(SearchText, SelectedeportType, null));
        }
        private string _selecteType;

        public string SelectedeportType
        {
            get { return _selecteType; }
            set { _selecteType = value; OnPropertyChanged("SelectedeportType"); }
        }
        public async void ReloadLocation()
        {
            IsBusy = true;
            var s = SearchText;
            var s1 = SelectedeportType;
            var s2 = CreatedOn;

            if (IsCompleted == true)
            {
                SelectedeportType = "Completed";
            }
            else
            {

                SelectedeportType = "All";
            }

            string DateCreated = string.Empty;
            if (CreatedOn != null)
            {
                DateCreated = CreatedOn?.ToString("dd-MMM-yyyy");
            }
            Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync(SearchText, SelectedeportType, DateCreated));
        }

        private string _us;

        public string UserSearch
        {
            get { return _us; }
            set { _us = value; OnPropertyChanged("UserSearch"); }
        }
        private bool _isc;

        public bool IsCompleted
        {
            get { return _isc; }
            set { _isc = value; OnPropertyChanged("IsCompleted"); }
        }

        public DelegateCommand SearchCommand => new DelegateCommand(async () => await Search());
        public async Task Search()
        {
            IsBusy = true;
            var s = SearchText;
            var s1 = SelectedeportType;
            var s2 = CreatedOn;
            string DateCreated = string.Empty;
            if (CreatedOn != null)
            {
                DateCreated = CreatedOn?.ToString("dd-MMM-yyyy");
            }
            if (IsCompleted == true)
            {
                SelectedeportType = "Completed";
            }
            else
            {

                SelectedeportType = "All";
            }

            await Task.Run(() => LongOperation(SearchText, SelectedeportType, DateCreated));

        }
        public DelegateCommand<Project> SelectedItemCommand => new DelegateCommand<Project>(async (Project prm) => await SelectedItemExecute(prm));
        public async Task SelectedItemExecute(Project prm)
        {
            App.IsInvasive = false;
            //if (SelectedItem != null)
            //{
            App.ProjectID = prm.Id;
            var parameters = new NavigationParameters { { "Project", prm } };
            RegionManger.RequestNavigate("MainRegion", "Project", parameters);
            //  }
            //var v = this.SelectedItem;
        }


        public DelegateCommand<Project> InvasiveItemCommand => new DelegateCommand<Project>(async (Project prm) => await InvasiveItemCommandExecute(prm));
        public async Task InvasiveItemCommandExecute(Project prm)
        {
            App.IsInvasive = true;
            App.ProjectID = prm.InvasiveProjectID;
            var parameters = new NavigationParameters { { "Project", prm } };
            RegionManger.RequestNavigate("MainRegion", "Project", parameters);
            //if (SelectedItem != null)
            //{

            //  }
            //var v = this.SelectedItem;
        }
        public DelegateCommand CloseCommand => new DelegateCommand(async () => await Close());
        public async Task Close()
        {
            DetailVisibility = false;
        }
        public DelegateCommand<Project> EditCommand => new DelegateCommand<Project>(async (Project parm) => await Edit(parm));
        public async Task Edit(Project parm)
        {
            // ShowDialog();
            //DetailVisibility = true;
            var parameters = new NavigationParameters { { "Project", parm } };
            RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit", parameters);
        }
        public DelegateCommand<Project> BuildingSelectCommand => new DelegateCommand<Project>(async (Project parm) => await BuildingSelect(parm));
        public async Task BuildingSelect(Project parm)
        {
            DetailVisibility = true;
            //var parameters = new NavigationParameters { { "Title", "New Project" }, { "Type", "New" } };
            // RegionManger.RequestNavigate("MainRegion", "ProjectDetail", parameters);
        }

        private UCFilePageViewModel uCF;

        public UCFilePageViewModel UCFilePageViewModel
        {
            get { return uCF; }
            set { uCF = value; OnPropertyChanged("UCFilePageViewModel"); }
        }
        private bool _isRole;

        public bool IsRoleAdmin
        {
            get { return _isRole; }
            set { _isRole = value;OnPropertyChanged("IsRoleAdmin"); }
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(App.LogUser.RoleName=="Admin")
            {
                IsRoleAdmin = true;
            }
               UCFilePageViewModel = new UCFilePageViewModel(null);
            SelectedItem = null;
            ProjectTypeList = new ObservableCollection<string>();
            ProjectTypeList.Add("Running");
            ProjectTypeList.Add("Completed");
            SelectedeportType = "Running";
            //  ProjectTypeList.Add("All");
            await Task.Run(() => LongOperation(SearchText, string.Empty, string.Empty));

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public async Task<bool> LongOperation(string search, string ProjectType, string CreatedOn)
        {
            IsBusy = true;
            //IsCompleted = false;
            if (IsCompleted == true)
            {
                SelectedeportType = "Completed";
            }
            else
            {

                SelectedeportType = "All";
            }
          
            //TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchText)), null);
            Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync(search, SelectedeportType, CreatedOn));

            IsBusy = false;
            return await Task.FromResult(true);
        }
        public async Task<bool> CallTreeLongOperation(string Id)
        {
            await Task.Run(() => TreeLongOperation(Id));
            return await Task.FromResult(true);
        }
        public async Task<bool> TreeLongOperation(string Id)
        {
            //IsCompleted = false;
            //if (IsCompleted == true)
            //{
            //    SelectedeportType = "Completed";
            //}
            //else
            //{

            //    SelectedeportType = "All";
            //}
            IsBusy = true;
           // TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync()), null);
            Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync("", "", ""));

            IsBusy = false;
            return await Task.FromResult(true);
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
        private bool sbusy;

        public bool IsBusy
        {
            get { return sbusy; }
            set { sbusy = value; OnPropertyChanged("IsBusy"); }
        }
        private ObservableCollection<Project> _projects;
        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set { _projects = value; OnPropertyChanged("Projects"); }
        }

        public async Task<ErrorModel> FinelSaveCompleted(Project item)
        {
            IsBusy = true;
            ErrorModel err = new ErrorModel();

            //AssignLocationAndBuilding item = new AssignLocationAndBuilding();
            //item.Users = UsersAssignList.ToList();
            //item.ParentID = SelectedItem.Id;
            //item.Type = "Project";
            try
            {


                Response result = await projectService.FinelReportUpdateItemAsync(item);
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
        public async Task<ErrorModel> Save()
        {
            IsBusy = true;
            ErrorModel err = new ErrorModel();

            AssignLocationAndBuilding item = new AssignLocationAndBuilding();
            item.Users = UsersAssignList.ToList();
            item.ParentID = SelectedItem.Id;
            item.Type = "Project";
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

        private ObservableCollection<User> _ulist;
        public ObservableCollection<User> UsersAssignList
        {
            get { return _ulist; }
            set { _ulist = value; OnPropertyChanged("UsersAssignList"); }
        }
        public async Task<bool> GetUserListForAssign(string UserID, string ProjectID, string ProjectLocationID, string BuildingID, string Type, string searchText)
        {

            UsersAssignList = new ObservableCollection<User>(await userService.GetUserListForAssign(UserID, ProjectID, ProjectLocationID, BuildingID, Type, searchText));
            return await Task.FromResult(true);
        }
        //public ProjectsPageViewModel() { }
        private readonly new IDialogService _dialogService;
        public ProjectsPageViewModel()
        {

        }
        public ProjectsPageViewModel(IDialogService dialogService)
        {
            IsCompleted = false;
            _dialogService = dialogService;

            Title = "Project(s)";

            ImageHeight= Properties.Settings.Default.ImageHeight;
            ImageWidth = Properties.Settings.Default.ImageWidth;
            ImageQuality = Properties.Settings.Default.ImageQuality;
            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }
        private void ShowDialog()
        {
            var message = "This is a message that should be shown in the dialog.";

            //using the dialog service as-is
            _dialogService.ShowDialog("NotificationDialog", new DialogParameters($"message={message}"), r =>
            {
                if (r.Result == ButtonResult.None)
                    Title = "Result is None";
                else if (r.Result == ButtonResult.OK)
                    Title = "Result is OK";
                else if (r.Result == ButtonResult.Cancel)
                    Title = "Result is Cancel";
                else
                    Title = "I Don't know what you did!?";
            });
        }
        public DelegateCommand<Project> FileCommand => new DelegateCommand<Project>(async (Project p) => await FileExecute(p));
        public async Task FileExecute(Project p)
        {
            //ProjectType = "Visual Report";
            //if (!string.IsNullOrEmpty(ProjectType))
            //{
            //    ErrorMsg = string.Empty;
                var parameters = new NavigationParameters { { "Page", "Page" },{"Project", p } };
               // RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit");
            RegionManger.RequestNavigate("TreeRegion", "FileUC", parameters);
            //}
            //else
            //{
            //    ErrorMsg = "Please Select Report Type";
            //}
        }
        #region ImageFormatting
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public long ImageQuality { get; set; }
        #endregion
    }
}
