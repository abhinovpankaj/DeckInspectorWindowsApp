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
    public class LocationDetailPageViewModel : BaseViewModel, INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        private Project selectedItem;

        public Project SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged("SelectedItem"); }
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



        public DelegateCommand NewCommand => new DelegateCommand(async () => await New());
        public async Task New()
        {
            var parameters = new NavigationParameters { { "Title", "New Project" },{ "Type","New"} };
            RegionManger.RequestNavigate("MainRegion", "ProjectDetail", parameters);
        }
        private ObservableCollection<ProjectCommonLocationImages> _projectlocationsimg;
        public ObservableCollection<ProjectCommonLocationImages> ProjectCommonLocationImages
        {
            get { return _projectlocationsimg; }
            set { _projectlocationsimg = value; OnPropertyChanged("ProjectCommonLocationImages"); }
        }
        public DelegateCommand SelectedItemCommand => new DelegateCommand(async () => await SelectedItemExecute());
        public async Task SelectedItemExecute()
        {
            var v = this.SelectedItem;
        }

        public DelegateCommand CloseCommand => new DelegateCommand(async () => await Close());
        public async Task Close()
        {
            RegionManger.RequestNavigate("DetailRegion", "DummyView");
            //RegionManger.Regions.Remove("LocationDetail");
            //var theView = RegionManger.Regions["DetailRegion"].GetView("LocationDetail");
            //RegionManger.Regions["DetailRegion"].Remove(theView);
            //if (RegionManger.Views.Contains(this))
            //{
            //    RegionManger.Remove(this);
            //}DummyView
            //var region = RegionManger.Regions["DetailRegion"];
            //var view = region.Views.Single(v => v.GetType().Name == "LocationDetail");
            //region.Remove(view);
        }
        public DelegateCommand<Project> LocationSelectCommand => new DelegateCommand<Project>(async (Project parm) => await LocationSelect(parm));
        public async Task LocationSelect(Project parm)
        {
            DetailVisibility = true;
            //var parameters = new NavigationParameters { { "Title", "New Project" }, { "Type", "New" } };
            // RegionManger.RequestNavigate("MainRegion", "ProjectDetail", parameters);
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
            ProjectLocation ProjectLocation = (ProjectLocation)navigationContext.Parameters["ProjectLocation"];
            if (ProjectLocation!=null)
                await Task.Run(() => LongOperation(ProjectLocation));
            
           // Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync());
        }
        public async void LongOperation(ProjectLocation p)
        {
            ProjectCommonLocationImages = new ObservableCollection<ProjectCommonLocationImages>(await projectCommonLocationImagesService.GetItemsAsyncByProjectLocationId(p.Id));
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           // throw new NotImplementedException();
        }

        private ObservableCollection<Project> _projects;
        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set { _projects = value; OnPropertyChanged("Projects"); }
        }

     
      
       
        public LocationDetailPageViewModel()
        {
            Title = "Project(s)";

          //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }
        
    }
}
