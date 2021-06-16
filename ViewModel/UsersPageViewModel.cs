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
    public class UsersPageViewModel : BaseViewModel, INavigationAware
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

        


       

        private User selectedItem;

        public User SelectedItem
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

            RegionManger.RequestNavigate("MainRegion", "UserAddEdit");
            //if (!string.IsNullOrEmpty(ProjectType))
            //{
            //    ErrorMsg = string.Empty;
            //    var parameters = new NavigationParameters { { "Title", "New Project" }, { "Type", ProjectType } };
            //    RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit", parameters);
            //}
            //else
            //{
            //    ErrorMsg = "Please Select Report Type";
            //}
        }
        public DelegateCommand SearchCommand => new DelegateCommand(async () => await Search());
        public async Task Search()
        {
            bool? IsActive = null;
            var s = SearchText;
            var s1 = SelectedType;
            if(SelectedType=="All")
            {
                IsActive = null;
            }
            if (SelectedType == "Active")
            {
                IsActive = false;
            }
            if (SelectedType == "Inactive")
            {
                IsActive = true;
            }
            var s2 = CreatedOn;
            await Task.Run(() => LongOperation(IsActive, SearchText));
        }
        private string _selType;

        public string SelectedType
        {
            get { return _selType; }
            set { _selType = value;OnPropertyChanged("SelectedType"); }
        }

        public DelegateCommand SelectedItemCommand => new DelegateCommand(async () => await SelectedItemExecute());
        public async Task SelectedItemExecute()
        {
            var v = this.SelectedItem;
        }
        private ObservableCollection<string> _actveDeActive;

        public ObservableCollection<string> ActiveList
        {
            get { return _actveDeActive; }
            set { _actveDeActive = value; OnPropertyChanged("ActiveList"); }
        }
        public DelegateCommand CloseCommand => new DelegateCommand(async () => await Close());
        public async Task Close()
        {
            DetailVisibility = false;
        }
        public DelegateCommand<User> EditCommand => new DelegateCommand<User>(async (User parm) => await Edit(parm));
        public async Task Edit(User parm)
        {
           // ShowDialog();
            //DetailVisibility = true;
             var parameters = new NavigationParameters { { "User", parm }};
             RegionManger.RequestNavigate("MainRegion", "UserAddEdit", parameters);
        }
        public DelegateCommand<Project> BuildingSelectCommand => new DelegateCommand<Project>(async (Project parm) => await BuildingSelect(parm));
        public async Task BuildingSelect(Project parm)
        {
            DetailVisibility = true;
            //var parameters = new NavigationParameters { { "Title", "New Project" }, { "Type", "New" } };
            // RegionManger.RequestNavigate("MainRegion", "ProjectDetail", parameters);
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            await Task.Run(() => LongOperation(null, string.Empty));
        }
        private bool sbusy;

        public bool IsBusy
        {
            get { return sbusy; }
            set { sbusy = value; OnPropertyChanged("IsBusy"); }
        }
        public async Task<bool> LongOperation(bool ? IsActive, string searchText)
        {
            IsBusy = true;
            Users = new ObservableCollection<User>(await userService.GetItemsAsync(IsActive, searchText));
            IsBusy = false;
            return await Task.FromResult(true);
        }
        public DelegateCommand ResetCommand => new DelegateCommand(async () => await Reset());
        public async Task Reset()
        {
            SearchText = string.Empty;
            CreatedOn = null;
            SelectedType = null;
            await Task.Run(() => LongOperation(null, string.Empty));
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        private ObservableCollection<User> _user;
        public ObservableCollection<User> Users
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("Users"); }
        }




        //public ProjectsPageViewModel() { }
        private readonly new IDialogService _dialogService;
        public UsersPageViewModel(IDialogService dialogService)
        {
            ActiveList = new ObservableCollection<string>();
            ActiveList.Add("All");
            ActiveList.Add("Active");
            ActiveList.Add("Inactive");
            
            _dialogService =dialogService;
           
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
