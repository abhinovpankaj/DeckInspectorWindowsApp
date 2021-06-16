using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Model;
using UI.Code.Services;
using Unity;

namespace UI.Code.ViewModel
{
    public  class BaseViewModel : INotifyPropertyChanged
    {


        private string _logUsername;

        public string LogUserName
        {
            get { return _logUsername; }
            set { _logUsername = value; OnPropertyChanged("LogUserName"); }
        }


        private bool _NavBarVisible;

        public bool NavBarVisible
        {
            get { return _NavBarVisible; }
            set { _NavBarVisible = value;OnPropertyChanged("NavBarVisible"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public BaseViewModel()
        {
          
            var container = new UnityContainer();
            userService = container.Resolve<UserService>();
            projectService = container.Resolve<ProjectService>();
            projectLocationService= container.Resolve<ProjectLocationService>();
            projectCommonLocationImagesService = container.Resolve<ProjectCommonLocationImagesService>();

            ProjectBuildingDataStore = container.Resolve<ProjectBuildingDataStore>();
            BuildingLocationDataStore = container.Resolve<BuildingLocationDataStore>();
            BuildingApartmentDataStore = container.Resolve<BuildingApartmentDataStore>();

            VisualProjectLocationService = container.Resolve<VisualProjectLocationService>();
            VisualFormBuildingLocationDataStore = container.Resolve<VisualFormBuildingLocationDataStore>();
            VisualFormApartmentDataStore = container.Resolve<VisualFormApartmentDataStore>();

            VisualProjectLocationPhotoDataStore = container.Resolve<VisualProjectLocationPhotoDataStore>();
            VisualBuildingLocationPhotoDataStore = container.Resolve<VisualBuildingLocationPhotoDataStore>();
            VisualApartmentLocationPhotoDataStore = container.Resolve<VisualApartmentLocationPhotoDataStore>();

            //   ProjectCommonLocationImagesService = container.Resolve<ProjectCommonLocationImagesService>();
            BuildingApartmentImagesDataStore = container.Resolve<BuildingApartmentImagesDataStore>();
            BuildingCommonLocationImagesDataStore = container.Resolve<BuildingCommonLocationImagesDataStore>();


            treeService = container.Resolve<TreeService>();
            //_dialogService = new DialogService(); ;
        }
       

        private VisualProjectLocation _selectedType;

        public VisualProjectLocation SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = value; OnPropertyChanged("SelectedType"); }
        }
        public IDialogService _dialogService { get; set; }
       // public DialogService dialogService { get; set; }
        public UserService userService { get; set; }
        public ProjectService projectService { get; set; }
        public ProjectCommonLocationImagesService projectCommonLocationImagesService { get; set; }
        public TreeService treeService { get; set; }
        public ProjectLocationService projectLocationService { get; set; }
        public ProjectBuildingDataStore ProjectBuildingDataStore { get; set; }
        public BuildingLocationDataStore BuildingLocationDataStore { get; set; }
        public BuildingApartmentDataStore BuildingApartmentDataStore { get; set; }

        public VisualProjectLocationPhotoDataStore VisualProjectLocationPhotoDataStore { get; set; }
        public VisualBuildingLocationPhotoDataStore VisualBuildingLocationPhotoDataStore { get; set; }
        public VisualApartmentLocationPhotoDataStore VisualApartmentLocationPhotoDataStore { get; set; }
        // public ProjectCommonLocationImagesService ProjectCommonLocationImagesService { get; set; }
        public BuildingApartmentImagesDataStore BuildingApartmentImagesDataStore { get; set; }
        public BuildingCommonLocationImagesDataStore BuildingCommonLocationImagesDataStore { get; set; }

        public VisualProjectLocationService VisualProjectLocationService { get; set; }
        public VisualFormBuildingLocationDataStore VisualFormBuildingLocationDataStore { get; set; }
        public VisualFormApartmentDataStore VisualFormApartmentDataStore { get; set; }
        public virtual bool IsNew { get; protected set; }

        public virtual bool IsDirty { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
