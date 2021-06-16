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
    public class AssigeProjectViewModel : BaseViewModel, INavigationAware
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
        private bool _isvis;

        public bool IsVisibile
        {
            get { return _isvis; }
            set { _isvis = value; OnPropertyChanged("IsVisibile"); }
        }
        public async Task<ErrorModel> Save()
        {
            IsBusy = true;
            ErrorModel err = new ErrorModel();

            AssignLocationAndBuilding item = new AssignLocationAndBuilding();
            //item.ProjectLocations = ProjectLocations.ToList();
            //item.ProjectBuildings = ProjectBuildings.ToList();
            //item.UserID = Selecteduser.Id;
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
        public DelegateCommand SearchCommand => new DelegateCommand(async () => await Search());
        public async Task Search()
        {
            IsVisibile = true;
            ProjectLocations = new ObservableCollection<ProjectLocation>(await projectLocationService.GetItemsAsyncByProjectIDAndUser(SelectedItem.Id, Selecteduser.Id));
            ProjectBuildings = new ObservableCollection<ProjectBuilding>(await ProjectBuildingDataStore.GetItemsAsyncByProjectIDAndUser(SelectedItem.Id, Selecteduser.Id));
            var s = SearchText;
            var s1 = SelectedType;
            var s2 = CreatedOn;
        }
        private string _selType;

        public string SelectedType
        {
            get { return _selType; }
            set { _selType = value; OnPropertyChanged("SelectedType"); }
        }
        public DelegateCommand SelectedItemCommand => new DelegateCommand(async () => await SelectedItemExecute());
        public async Task SelectedItemExecute()
        {
            var parameters = new NavigationParameters { { "Project", this.SelectedItem } };
            RegionManger.RequestNavigate("MainRegion", "Project", parameters);
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
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsVisibile = false;

            ProjectTypeList = new ObservableCollection<string>();
            ProjectTypeList.Add("Visual Report");
            ProjectTypeList.Add("Invasive Report");
            ProjectTypeList.Add("Final Report");
            IsBusy = true;
            await Task.Run(() => LongOperation());
            IsBusy = false;

        }
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public async void LongOperation()
        {

            Users = new ObservableCollection<User>(await userService.GetItemsAsync(false, string.Empty));
            Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync(string.Empty, string.Empty, string.Empty));
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
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged("Users"); }
        }

        private User _selecteduser;

        public User Selecteduser
        {
            get { return _selecteduser; }
            set { _selecteduser = value; OnPropertyChanged("Selecteduser"); }
        }


        //public AssigeProjectViewModel.() { }
        private readonly new IDialogService _dialogService;
        public AssigeProjectViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            Title = "Project(s)";


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
    }
}
