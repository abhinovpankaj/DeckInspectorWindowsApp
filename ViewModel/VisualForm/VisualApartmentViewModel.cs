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
    public class VisualApartmentViewModel : BaseViewModel, INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }
        private bool _isInvasiveControlDisable;

        public bool IsInvasiveControlDisable
        {
            get { return _isInvasiveControlDisable; }
            set { _isInvasiveControlDisable = value; OnPropertyChanged("IsInvasiveControlDisable"); }
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


        public async Task<ErrorModel> Reorder()
        {
            ErrorModel err = new ErrorModel();
            try
            {
                List<ReorderList> list = new List<ReorderList>();
                int index = 0;
                if (App.IsInvasive == true)
                {
                    foreach (var item in InvasiveImgs)
                    {
                        index = index + 1;
                        list.Add(new ReorderList() { Id = item.Id.ToString(), SeqNo = index });
                    }
                }
                else
                {
                    foreach (var item in Images)
                    {
                        index = index + 1;
                        list.Add(new ReorderList() { Id = item.Id.ToString(), SeqNo = index });
                    }
                }
                Response result = await VisualProjectLocationPhotoDataStore.Reorder(list);

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

            var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project }, { "BuildingApartment", Data } };
            RegionManger.RequestNavigate("MainRegion", "Building", parameters);
        }
        public async Task DeleteMain()
        {
            ErrorModel err = new ErrorModel();
            Response result = await BuildingApartmentDataStore.DeleteItemAsync(Data);
            if (result.Status == ApiResult.Success)
            {

                err.Status = "Success";
                err.Message = result.Message;
                var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project }, { "BuildingApartment", Data } };
                RegionManger.RequestNavigate("MainRegion", "Building", parameters);
            }
            else
            {
                err.Status = "Error";
                err.Message = result.Message;
            }

        }
        public DelegateCommand<VisualApartmentLocationPhoto> DeleteCommand => new DelegateCommand<VisualApartmentLocationPhoto>(async (VisualApartmentLocationPhoto prm) => await Delete(prm));
        public async Task Delete(VisualApartmentLocationPhoto prm)
        {
            ErrorModel err = new ErrorModel();
            Response result = await VisualApartmentLocationPhotoDataStore.DeleteItemAsync(prm);
            if (result.Status == ApiResult.Success)
            {
             
                bool isComplet = await Task.Run(() => LongOperationGetImage(SelectedItem.Id.ToString()));
            }
            else
            {
                err.Status = "Error";
                err.Message = result.Message;
            }
            
        }
        public DelegateCommand NewCommand => new DelegateCommand(async () => await New());
        public async Task New()
        {

        }
        public DelegateCommand<VisualBuildingApartment> GetImagesCommand => new DelegateCommand<VisualBuildingApartment>(async (VisualBuildingApartment parm) => await GetImages(parm));
        public async Task GetImages(VisualBuildingApartment parm)
        {
            IsBusy = true;
            if (parm != null)
            {
                IsDataShow = true;
                SelectedItem = parm;
                bool isComplet = await Task.Run(() => LongOperationGetImage(parm.Id.ToString()));
                if (isComplet)
                {
                    IsBusy = false;
                }
            }
            else
            {
                IsDataShow = false;
            }
          
        }

        public async Task<ErrorModel> ReorderVisualApartment()
        {
            ErrorModel err = new ErrorModel();
            try
            {
                List<ReorderList> list = new List<ReorderList>();
                int index = 0;
                foreach (var item in Items)
                {
                    index = index + 1;
                    list.Add(new ReorderList() { Id = item.Id.ToString(), SeqNo = index });
                }
                Response result = await VisualFormApartmentDataStore.Reorder(list);

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
        public DelegateCommand ResetCommand => new DelegateCommand(async () => await Reset());
        public async Task Reset()
        {
            SelectedItem = null;
            IsDataShow = false;
            //  SelectedType = string.Empty;
            //  await Task.Run(() => LongOperation(SearchText, string.Empty, SelectedType));
        }
        private bool _isDataShow;

        public bool IsDataShow
        {
            get { return _isDataShow; }
            set { _isDataShow = value; OnPropertyChanged("IsDataShow"); }
        }

        private async Task<bool> LongOperationGetImage(string Id)
        {
            var res = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetVisualBuildingApartmentImageByVisualApartmentId(Id));
            Images = new ObservableCollection<VisualApartmentLocationPhoto>(res.Where(c => c.ImageDescription != "TRUE"&& c.ImageDescription != "CONCLUSIVE").OrderBy(c => c.SeqNo));
            InvasiveImgs = new ObservableCollection<VisualApartmentLocationPhoto>(res.Where(c => c.ImageDescription == "TRUE").OrderBy(c => c.SeqNo));
            
            ConclusiveImgs = new ObservableCollection<VisualApartmentLocationPhoto>(res.Where(c => c.ImageDescription == "CONCLUSIVE").OrderBy(c => c.SeqNo));
            return await Task.FromResult(true);
        }
        private ObservableCollection<VisualApartmentLocationPhoto> Imgs;

        public ObservableCollection<VisualApartmentLocationPhoto> Images
        {
            get { return Imgs; }
            set { Imgs = value; OnPropertyChanged("Images"); }
        }

        public DelegateCommand CloseCommand => new DelegateCommand(async () => await Close());
        public async Task Close()
        {

        }

        //public DelegateCommand OnLoadedCommand => new DelegateCommand(async () => await Load());
        //public async Task Load()
        //{
        //    if (Data != null)
        //    {
        //        Items = new ObservableCollection<ProjectCommonLocationImages>(await projectCommonLocationImagesService.GetItemsAsyncByProjectLocationId(Data.Id));
        //    }
        //}

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }
        //  public DelegateCommand SaveCommand => new DelegateCommand(async () => await Save());
      
        private ObservableCollection<BindingModel> bindingModels;
        public ObservableCollection<BindingModel> BindingModels
        {
            get { return bindingModels; }
            set { bindingModels = value; OnPropertyChanged("BindingModels"); }
        }
        private Project _project;

        public Project Project
        {
            get { return _project; }
            set { _project = value; OnPropertyChanged("Project"); }
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
            //  ProjectBuildings = new ObservableCollection<ProjectBuilding>(await ProjectBuildingDataStore.GetItemsAsyncByProjectID(Project.Id));
            bool isComplet = await Task.Run(() => LongOperation(navigationContext));
            if (isComplet)
            {
                IsBusy = false;
            }
        }
        private ObservableCollection<VisualBuildingApartment> _prImages;
        public ObservableCollection<VisualBuildingApartment> Items
        {
            get { return _prImages; }
            set { _prImages = value; OnPropertyChanged("Items"); }
        }
        private BuildingApartment _projectlocations;
        public BuildingApartment Data
        {
            get { return _projectlocations; }
            set { _projectlocations = value; OnPropertyChanged("Data"); }
        }
        private BuildingApartment _projectlocationsDataModel;
        
        public BuildingApartment DataModel
        {
            get { return _projectlocationsDataModel; }
            set { _projectlocationsDataModel = value; OnPropertyChanged("DataModel"); }
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
        private VisualBuildingApartment _VisualProjectLocation;

        public VisualBuildingApartment SelectedItem
        {
            get { return _VisualProjectLocation; }
            set { _VisualProjectLocation = value; OnPropertyChanged("SelectedItem"); }
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

                        
                    }
                    else
                    {
                        Response result = await BuildingApartmentDataStore.UpdateItemAsync(DataModel);
                        if (result.Status == ApiResult.Success)
                        {

                            var parameters = new NavigationParameters { { "BuildingApartment", DataModel }, { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
                            RegionManger.RequestNavigate("MainRegion", "VisualApartmentView", parameters);

                            
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
        private ProjectBuilding _pb;

        public ProjectBuilding ProjectBuilding
        {
            get { return _pb; }
            set { _pb = value; OnPropertyChanged("ProjectBuilding"); }
        }

        private BuildingApartment _pbA;

        private string _objStrin;

        public string ObjectString
        {
            get { return _objStrin; }
            set { _objStrin = value; }
        }
        public async Task<bool> LongOperation(NavigationContext navigationContext)
        {
            IsDataShow = false;
            SelectedItem = null;


            Project = (Project)navigationContext.Parameters["Project"];
            ProjectBuilding = (ProjectBuilding)navigationContext.Parameters["ProjectBuilding"];
            Data = DataModel = (BuildingApartment)navigationContext.Parameters["BuildingApartment"];
            ObjectString = Newtonsoft.Json.JsonConvert.SerializeObject(DataModel);
            if (Data != null)
                Items = new ObservableCollection<VisualBuildingApartment>(await VisualFormApartmentDataStore.GetVisualBuildingApartmentByBuildingApartmentId(Data.Id));

            return await Task.FromResult(true);
            
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }





        private readonly new IDialogService _dialogService;
        public VisualApartmentViewModel(IDialogService dialogService)
        {

            _dialogService = dialogService;
            Title = "Apartment";


            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }
        private ObservableCollection<VisualApartmentLocationPhoto> _invasiveImgs;

        public ObservableCollection<VisualApartmentLocationPhoto> InvasiveImgs
        {
            get { return _invasiveImgs; }
            set { _invasiveImgs = value; OnPropertyChanged("InvasiveImgs"); }
        }

        private ObservableCollection<VisualApartmentLocationPhoto> _conclusiveImgs;

        public ObservableCollection<VisualApartmentLocationPhoto> ConclusiveImgs
        {
            get { return _conclusiveImgs; }
            set { _conclusiveImgs = value; OnPropertyChanged("ConclusiveImgs"); }
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
