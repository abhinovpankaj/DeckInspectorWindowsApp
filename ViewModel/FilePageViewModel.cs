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
    public class FilePageViewModel : BaseViewModel, INavigationAware
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
            SearchTree = string.Empty;
            SearchText = string.Empty;
            CreatedOn = null;
            IsCompleted = false;
            SelectedeportType = "Running";
            SelectedeportType = "All";
            await Task.Run(() => LongOperation(string.Empty, SearchText, SelectedeportType, null));
            TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);
           
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

        public DelegateCommand<string> SearchCommand => new DelegateCommand<string>(async (string treeID) => await Search(treeID));
        public async Task Search(string treeID)
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

            await Task.Run(() => LongOperationSearch(treeID, SearchText, SelectedeportType, DateCreated));

        }
        private List<string> _tt;

        public List<string> TreeTitle
        {
            get { return _tt; }
            set { _tt = value; OnPropertyChanged("TreeTitle"); }
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

        private bool _page;

        public bool ControlVisible
        {
            get { return _page; }
            set { _page = value; OnPropertyChanged("ControlVisible"); }
        }
        private Project project;

        public Project MoveProject
        {
            get { return project; }
            set { project = value; OnPropertyChanged("MoveProject"); }
        }

        public async  void OnNavigatedTo(NavigationContext navigationContext)
        {
            string Page = (string)navigationContext.Parameters["Page"];
           
            
            if (string.IsNullOrEmpty(Page))
            {
                ControlVisible = true;
            }
            else
            {
                MoveProject = (Project)navigationContext.Parameters["Project"];

                ControlVisible = false;
            }
            SearchTree = string.Empty;
            SearchText = string.Empty;
            CreatedOn = null;
            IsCompleted = false;
            SelectedeportType = "Running";
            SelectedeportType = "All";
            await Task.Run(() => LongOperation(string.Empty, SearchText, SelectedeportType, null));
            TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public async Task<bool> LongOperationSearch(string treeID, string search, string ProjectType, string CreatedOn)
        {
            SelectedItem = null;
            ProjectTypeList = new ObservableCollection<string>();
            ProjectTypeList.Add("Running");
            ProjectTypeList.Add("Completed");
            SelectedeportType = "Running";
            //  ProjectTypeList.Add("All");
            //await Task.Run(() => LongOperation(string.Empty,string.Empty, string.Empty, string.Empty));
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
           // TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);
            if(!string.IsNullOrEmpty(treeID))
            Projects = new ObservableCollection<Project>(await projectService.TreeGetItemsAsync(treeID, search, ProjectType, CreatedOn));
            

            IsBusy = false;
            return await Task.FromResult(true);
        }
        public async Task<bool> LongOperation(string treeID,string search, string ProjectType, string CreatedOn)
        {
            SelectedItem = null;
            ProjectTypeList = new ObservableCollection<string>();
            ProjectTypeList.Add("Running");
            ProjectTypeList.Add("Completed");
            SelectedeportType = "Running";
            //  ProjectTypeList.Add("All");
            //await Task.Run(() => LongOperation(string.Empty,string.Empty, string.Empty, string.Empty));
            //IsCompleted = false;
            //if (IsCompleted == true)
            //{
            //    SelectedeportType = "Completed";
            //}
            //else
            //{

            //    SelectedeportType = "All";
            //}

            mainTree = new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree));

            IsBusy = true;
            TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);
            if (!string.IsNullOrEmpty(treeID))
            {
                Projects = new ObservableCollection<Project>(await projectService.TreeGetItemsAsync(string.Empty, search, ProjectType, CreatedOn));
            }

            IsBusy = false;
            return await Task.FromResult(true);
        }
        public async Task<bool> GetMainTree()
        {

            mainTree = new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree));
            return await Task.FromResult(true);
        }
        private ObservableCollection<Organization> mtree;

        public ObservableCollection<Organization> mainTree
        {
            get { return mtree; }
            set { mtree = value;OnPropertyChanged("mainTree"); }
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
            //TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);
            Projects = new ObservableCollection<Project>(await projectService.TreeGetItemsAsync(Id, "", "", ""));
            TreeTitle.Clear();
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
        //public FilePageViewModel() { }
        private readonly new IDialogService _dialogService;
        public FilePageViewModel(IDialogService dialogService)
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

        private ObservableCollection<Organization> _treeItems;
        public ObservableCollection<Organization> TreeItems
        {
            get { return _treeItems; }
            set { _treeItems = value; OnPropertyChanged("TreeItems"); }
        }
        private ObservableCollection<Organization> MakeTree(ObservableCollection<Organization> list, Organization parentNode)
        {
            ObservableCollection<Organization> treeViewList = new ObservableCollection<Organization>();
            var nodes = list.Where(x => parentNode == null ? x.Parent_ID == string.Empty : x.Parent_ID == parentNode.Id);
            foreach (var node in nodes)
            {
                Organization newNode = new Organization();
                newNode.Id = node.Id;
                newNode.NodeTitle = node.NodeTitle;
                newNode.Parent_ID = node.Parent_ID;
                newNode.IsLock = node.IsLock;
                if (parentNode == null)
                {
                  //  newNode.Path();
                    //newNode.NodeCode = newNode.NodeTitle;
                    treeViewList.Add(newNode);
                }
                else
                {
                   // newNode.NodeCode += newNode.NodeCode + "/"+ newNode.NodeTitle;
                    parentNode.NodeItem.Add(newNode);
                }
                MakeTree(list, newNode);
            }
            return treeViewList;
        }
       
        private string _ts;

        public string SearchTree
        {
            get { return _ts; }
            set { _ts = value; OnPropertyChanged("SearchTree"); }
        }

        public DelegateCommand SearcTreeCommand => new DelegateCommand(async () => await SearcTreeCommandExecute());

        public async Task SearcTreeCommandExecute()
        {
            TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);

            // DetailVisibility = false;
        }

        public DelegateCommand RefreshTreeCommand => new DelegateCommand(async () => await RefreshTreeCommandExecute());

        public async Task RefreshTreeCommandExecute()
        {
            await Reset();
           
            // DetailVisibility = false;
        }
        private Organization _org;

        public Organization Organization
        {
            get { return _org; }
            set { _org = value; OnPropertyChanged("Organization"); }
        }
        public async Task<ErrorModel> DeleteNode(Organization node)
        {
            ErrorModel err = new ErrorModel();
            await treeService.DeleteItemAsync(node);
            TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);
            return await Task.FromResult(err);

        }

        public async Task<ErrorModel> Delete_CheakStatus_Node(Organization node)
        {
            ErrorModel err = new ErrorModel();
            Response res= await treeService.Delete_Cheack_ItemAsync(node);
            err.Status = res.Data.ToString();
            err.Message = res.Message;
            // TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);
            return await Task.FromResult(err);

        }
        public async void LoadTree()
        {

            TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);

        }
        public async Task<ErrorModel> AddNewNode(Organization node, string NodeText)
        {
            IsBusy = true;
            ErrorModel err = new ErrorModel();

            if (!string.IsNullOrEmpty(NodeText))
            {
                //if (string.IsNullOrEmpty(Organization.NodeTitle))
                //{
                //    err.Status = "Validation";
                //    err.Message = "Please Enter Name";

                //    return await Task.FromResult(err);
                //}
                try
                {
                   
                    if (node.NodeCode == "Edit")
                    {
                        node.NodeTitle = NodeText;
                        await treeService.UpdateItemAsync(node);
                        TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);
                    }
                    else
                    {
                        if(node.IsLock==false)
                        {

                        }
                        Organization org = new Organization();

                        //if (string.IsNullOrEmpty(Organization.Id))
                        //{
                        // Organization item = new Organization();
                        org.NodeTitle = NodeText;
                        org.Id = Guid.NewGuid().ToString();
                        org.Parent_ID = node.Id;
                        org.level = node.level;
                        //obj.NodeItem.Add(org);
                        Response res=  await treeService.AddItemAsync(org);
                        err.Status =res.Status.ToString();
                        err.Message = res.Message;
                        // TreeItems = new ObservableCollection<Organization>(await treeService.GetItemsAsync());
                        TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchTree)), null);
                    }
                    //}
                    //else
                    //{
                    //    Organization org = new Organization();
                    //    //if (string.IsNullOrEmpty(Organization.Id))
                    //    //{
                    //    // Organization item = new Organization();
                    //    org.NodeTitle = NodeText;
                    //    org.Id = Guid.NewGuid().ToString();
                    //    org.Parent_ID = ParentID;

                    //    await treeService.AddItemAsync(org);
                    //    // obj.NodeItem.Add(org);
                    //    TreeItems = new ObservableCollection<Organization>(await treeService.GetItemsAsync());
                    //}


                    //}
                    //else
                    //{
                    //Response result = await projectService.UpdateItemAsync(DataModel);
                    //if (result.Status == ApiResult.Success)
                    //{

                    //    //App.ProjectID = Project.Id;
                    //    //ErrorMsg = result.Message;
                    //    var parameters = new NavigationParameters { { "Project", DataModel } };
                    //    RegionManger.RequestNavigate("MainRegion", "Project", parameters);
                    //    err.Status = "Success";
                    //    err.Message = result.Message;
                    //}
                    //else
                    //{
                    //    err.Status = "Error";
                    //    err.Message = result.Message;
                    //    //   ErrorMsg = result.Message;
                    //}
                    //   }

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
        public async Task<ErrorModel> Move(OrganizationDetail org)
        {
            ErrorModel err = new ErrorModel();




            try
            {

                Response result = await projectService.MoveItemAsync(org);
                if (result.Status == ApiResult.Success)
                {

                    //App.ProjectID = Project.Id;
                    //ErrorMsg = result.Message;
                    //var parameters = new NavigationParameters { { "Project", DataModel } };
                    //RegionManger.RequestNavigate("MainRegion", "Project", parameters);
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
            catch (Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;

            }



            return await Task.FromResult(err);
        }
        public DelegateCommand<Project> FileCommand => new DelegateCommand<Project>(async (Project p) => await FileExecute(p));
        public async Task FileExecute(Project p)
        {
           // MoveProject = p;
            //ProjectType = "Visual Report";
            //if (!string.IsNullOrEmpty(ProjectType))
            //{
            //    ErrorMsg = string.Empty;
            var parameters = new NavigationParameters { { "Page", "Page" }, { "Project", p } };
            // RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit");
            RegionManger.RequestNavigate("TreeRegionMain", "FileUC", parameters);
            //}
            //else
            //{
            //    ErrorMsg = "Please Select Report Type";
            //}
        }
        //public DelegateCommand<Project> FileCommand => new DelegateCommand<Project>(async (Project p) => await FileExecute(p));
        //public async Task FileExecute(Project p)
        //{
        //    //ProjectType = "Visual Report";
        //    //if (!string.IsNullOrEmpty(ProjectType))
        //    //{
        //    //    ErrorMsg = string.Empty;
        //    var parameters = new NavigationParameters { { "Page", "Page" }, { "Project", p } };
        //    // RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit");
        //    RegionManger.RequestNavigate("TreeRegionMain", "File", parameters);
        //    //}
        //    //else
        //    //{
        //    //    ErrorMsg = "Please Select Report Type";
        //    //}
        //}
    }
}
