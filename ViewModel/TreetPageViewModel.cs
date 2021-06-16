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
{//https://stackoverflow.com/questions/37382285/wpf-c-sharp-treeview-delete-focuse-node

    //Populating TreeView up-to N Levels in C# from Database with Auto Code Generation for New Item at Runtime - CodeProject
    public class TreetPageViewModel : BaseViewModel, INavigationAware
    {

        private bool _isVisual;

        public bool IsVisual
        {
            get { return _isVisual; }
            set { _isVisual = value; OnPropertyChanged("IsVisual"); }
        }

        private bool _isFinalOrInasive;

        public bool IsFinalOrInasive
        {
            get { return _isFinalOrInasive; }
            set { _isFinalOrInasive = value; OnPropertyChanged("IsFinalOrInasive"); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        private ProjectLocation _projectLocationselectedItem;

        public ProjectLocation ProjectLocationSelectedItem
        {
            get { return _projectLocationselectedItem; }
            set { _projectLocationselectedItem = value; OnPropertyChanged("ProjectLocationSelectedItem"); }
        }
        private bool _isDetailShow;

        public bool ProjectLocDetailVisibility
        {
            get { return _isDetailShow; }
            set { _isDetailShow = value; OnPropertyChanged("ProjectLocDetailVisibility"); }
        }

        IRegionManager RegionManger { get { return ServiceLocator.Current.GetInstance<IRegionManager>(); } }



        public DelegateCommand NewCommand => new DelegateCommand(async () => await New());
        public async Task New()
        {
            var parameters = new NavigationParameters { { "ProjectLocationSelectedItem", ProjectLocationSelectedItem } };
            RegionManger.RequestNavigate("MainRegion", "ProjectDetail", parameters);
        }
        
        public DelegateCommand ProjLocationSelectedItemCommand => new DelegateCommand(async () => await ProjLocationSelectedItem());
        public async Task ProjLocationSelectedItem()
        {
            ProjectLocDetailVisibility = true;
            var parameters = new NavigationParameters { { "ProjectLocation", ProjectLocationSelectedItem } };
            // ProjectCommonLocationImages = new ObservableCollection<ProjectCommonLocationImages>(await projectCommonLocationImagesService.GetItemsAsyncByProjectLocationId(ProjectLocationSelectedItem.Id));

            RegionManger.RequestNavigate("DetailRegion", "LocationDetail", parameters);
            // IsVisual = false;
            //IsFinalOrInasive = true;
            // var v = this.ProjectLocationSelectedItem;
        }

        public DelegateCommand CloseCommand => new DelegateCommand(async () => await Close());
        public async Task Close()
        {
            ProjectLocDetailVisibility = false;
        }
        public DelegateCommand<Project> LocationSelectCommand => new DelegateCommand<Project>(async (Project parm) => await LocationSelect(parm));
        public async Task LocationSelect(Project parm)
        {
            ProjectLocDetailVisibility = true;
            //var parameters = new NavigationParameters { { "Title", "New Project" }, { "Type", "New" } };
            // RegionManger.RequestNavigate("MainRegion", "ProjectDetail", parameters);
        }
        public DelegateCommand<Project> BuildingSelectCommand => new DelegateCommand<Project>(async (Project parm) => await BuildingSelect(parm));
        public async Task BuildingSelect(Project parm)
        {
            ProjectLocDetailVisibility = true;
            //var parameters = new NavigationParameters { { "Title", "New Project" }, { "Type", "New" } };
            // RegionManger.RequestNavigate("MainRegion", "ProjectDetail", parameters);
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Organization = new Organization();

            TreeItems = new ObservableCollection<Organization>(await treeService.GetItemAsync());
            //if (TreeItems.Count == 0)
            //{
            //    Organization item = new Organization();
            //    item.NodeTitle = "File(s)";
            //    item.Id = Guid.NewGuid().ToString();
            //    item.Parent_ID = string.Empty;
            //    await treeService.AddItemAsync(item);

            //    TreeItems = new ObservableCollection<Organization>(await treeService.GetItemAsync());
            //}
            // Organization o=new 
            //// ObservableCollection<Organization> items = new ObservableCollection<Organization>();
            // foreach (var item in TreeItems)
            // {
            //    if(item.Id==item.Parent_ID)
            //     {

            //     }
            // }
            TreeItems=MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync()),null);
            //  ProjectLocations = new ObservableCollection<ProjectLocation>(await projectLocationService.GetItemsAsyncByProjectID("1"));
        }
       
        private ObservableCollection<Organization> MakeTree(ObservableCollection<Organization> list, Organization parentNode)
        {
            ObservableCollection<Organization> treeViewList = new ObservableCollection<Organization>();
            var nodes = list.Where(x => parentNode == null ? x.Parent_ID ==string.Empty : x.Parent_ID == parentNode.Id);
            foreach (var node in nodes)
            {
                Organization newNode = new Organization();
                newNode.Id = node.Id;
                newNode.NodeTitle = node.NodeTitle;

                if (parentNode == null)
                {
                    treeViewList.Add(newNode);
                }
                else
                {
                    parentNode.NodeItem.Add(newNode);
                }
                MakeTree(list, newNode);
            }
            return treeViewList;
        }

        private Organization _org;

        public Organization Organization
        {
            get { return _org; }
            set { _org = value; OnPropertyChanged("Organization"); }
        }
        public async Task<ErrorModel> AddNewNode(string ParentID, string NodeText)
        {
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
                    //Organization obj= await treeService.GetItemAsync(ParentID);
                   
                    //if(obj!=null)
                    //{
                        Organization org = new Organization();
                        //if (string.IsNullOrEmpty(Organization.Id))
                        //{
                        // Organization item = new Organization();
                        org.NodeTitle = NodeText;
                        org.Id = Guid.NewGuid().ToString();
                        org.Parent_ID = ParentID;
                        //obj.NodeItem.Add(org);
                        await treeService.AddItemAsync(org);
                        // TreeItems = new ObservableCollection<Organization>(await treeService.GetItemsAsync());
                        TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync()), null);
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

            return await Task.FromResult(err);
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // throw new NotImplementedException();
        }

        private ObservableCollection<Organization> _treeItems;
        public ObservableCollection<Organization> TreeItems
        {
            get { return _treeItems; }
            set { _treeItems = value; OnPropertyChanged("TreeItems"); }
        }




        public TreetPageViewModel()
        {
            Title = "Project(s)";

            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }

    }
}
