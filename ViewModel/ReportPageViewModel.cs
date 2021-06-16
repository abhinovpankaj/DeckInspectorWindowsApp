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
    public class ReportPageViewModel : BaseViewModel, INavigationAware
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


        IRegionManager RegionManger
        {
            get
            {
                return (IRegionManager)Prism.Ioc.ContainerLocator.Container.Resolve(typeof(IRegionManager));
            }
        }



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
            ProjectLocations = new ObservableCollection<ProjectLocation>(await projectLocationService.GetItemsAsyncByProjectID("1"));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           // throw new NotImplementedException();
        }

        private ObservableCollection<ProjectLocation> _projectlocations;
        public ObservableCollection<ProjectLocation> ProjectLocations
        {
            get { return _projectlocations; }
            set { _projectlocations = value; OnPropertyChanged("ProjectLocations"); }
        }

       


        public ReportPageViewModel()
        {
            Title = "Project(s)";

          //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }
        
    }
}
