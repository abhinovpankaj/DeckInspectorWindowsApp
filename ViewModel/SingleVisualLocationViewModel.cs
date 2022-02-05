using CommonServiceLocator;
using Newtonsoft.Json;
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
    public class VisualSingleLevelProjectLocationViewModel : BaseViewModel, INavigationAware
    {
        public event EventHandler OnProjectLoadSuccess;
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


        public async Task<ErrorModel> ReorderVisualLocation()
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
                Response result = await VisualProjectLocationService.Reorder(list);

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

            var parameters = new NavigationParameters { { "Project", Project } };
            RegionManger.RequestNavigate("MainRegion", "Projects", parameters);
        }

        public DelegateCommand<VisualProjectLocationPhoto> DeleteCommand => new DelegateCommand<VisualProjectLocationPhoto>(async (VisualProjectLocationPhoto prm) => await Delete(prm));

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
                    Project.ProjectType = "Invasive";
                    var parameters = new NavigationParameters { { "Project", Project } };
                    RegionManger.RequestNavigate("MainRegion", "SingleLevelProject", parameters);
                   

                    IsBusy = false;

                }
                else
                {

                    err.Status = "Success";
                    err.Message = response.Message;
                }

            }
            else
            {
                Project.IsInvasive = Project.InvasiveProjectID != null ? true : false;
                
                var response = await Task.Run(() =>
                  projectService.CreateInvasiveReport(Project)
                );
                if (response.Status == ApiResult.Success)
                {
                    err.Status = "Success";
                    err.Message = response.Message;
                    App.IsInvasive = true;
                    Project.ProjectType = "Invasive";
                    Project.Id = response.ID.ToString();
                    App.ProjectID = Project.Id;
                    OnPropertyChanged("ProjectType");
                    var parameters = new NavigationParameters { { "Project", Project } };
                    RegionManger.RequestNavigate("MainRegion", "SingleLevelProject", parameters);
                    IsBusy = false;
                    
                }
                else
                {
                    err.Status = "Success";
                    err.Message = response.Message;

                }
            }


            return await Task.FromResult(err);
        }
        public async Task DeleteMain()
        {
            ErrorModel err = new ErrorModel();
            Response result = await projectService.DeleteItemAsync(Data);
            if (result.Status == ApiResult.Success)
            {

                err.Status = "Success";
                err.Message = result.Message;
                var parameters = new NavigationParameters { { "Project", Project } };
                RegionManger.RequestNavigate("MainRegion", "Project", parameters);
            }
            else
            {
                err.Status = "Error";
                err.Message = result.Message;
            }

        }
        public async Task Delete(VisualProjectLocationPhoto prm)
        {
            ErrorModel err = new ErrorModel();
            Response result = await VisualProjectLocationPhotoDataStore.DeleteItemAsync(prm);
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
        private string _objStrin;

        public string ObjectString
        {
            get { return _objStrin; }
            set { _objStrin = value; }
        }
        public DelegateCommand<VisualProjectLocation> GetImagesCommand => new DelegateCommand<VisualProjectLocation>(async (VisualProjectLocation parm) => await GetImages(parm));
        public async Task GetImages(VisualProjectLocation parm)
        {
            IsBusy = true;
            if (parm != null)
            {
                SelectedItem = parm;
                IsDataShow = true;
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
            var res = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByVisualProjectLocationId(Id));
            Images = new ObservableCollection<VisualProjectLocationPhoto>(res.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").OrderBy(c => c.SeqNo));
            InvasiveImgs = new ObservableCollection<VisualProjectLocationPhoto>(res.Where(c => c.ImageDescription == "TRUE").OrderBy(c => c.SeqNo));
            ConclusiveImgs = new ObservableCollection<VisualProjectLocationPhoto>(res.Where(c => c.ImageDescription == "CONCLUSIVE").OrderBy(c => c.SeqNo));
            return await Task.FromResult(true);
        }
        private ObservableCollection<VisualProjectLocationPhoto> Imgs;

        public ObservableCollection<VisualProjectLocationPhoto> Images
        {
            get { return Imgs; }
            set { Imgs = value; OnPropertyChanged("Images"); }
        }


        private ObservableCollection<VisualProjectLocationPhoto> _invasiveImgs;

        public ObservableCollection<VisualProjectLocationPhoto> InvasiveImgs
        {
            get { return _invasiveImgs; }
            set { _invasiveImgs = value; OnPropertyChanged("InvasiveImgs"); }
        }

        private ObservableCollection<VisualProjectLocationPhoto> _conclusiveImgs;

        public ObservableCollection<VisualProjectLocationPhoto> ConclusiveImgs
        {
            get { return _conclusiveImgs; }
            set { _conclusiveImgs = value; OnPropertyChanged("ConclusiveImgs"); }
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
           
            bool isComplet = await Task.Run(() => LongOperation(navigationContext));
            if (isComplet)
            {
                IsBusy = false;
                if (OnProjectLoadSuccess!=null)
                {
                    OnProjectLoadSuccess(this, EventArgs.Empty);
                } 
            }
        }
        private ObservableCollection<VisualProjectLocation> _prImages;
        public ObservableCollection<VisualProjectLocation> Items
        {
            get { return _prImages; }
            set { _prImages = value; OnPropertyChanged("Items"); }
        }
        private Project _projectlocations;
        public Project Data
        {
            get { return _projectlocations; }
            set { _projectlocations = value; OnPropertyChanged("Data"); }
        }
        private Project _projectlocationsDataModel;
        public Project DataModel
        {
            get { return _projectlocationsDataModel; }
            set { _projectlocationsDataModel = value; OnPropertyChanged("DataModel"); }
        }
       

        private bool _canInvasiveCreate;

        public bool CanInvasiveCreate
        {
            get { return _canInvasiveCreate; }
            set { _canInvasiveCreate = value; OnPropertyChanged("CanInvasiveCreate"); }
        }
        private string _btnInvasiveText;

        public string BtnInvasiveText
        {
            get { return _btnInvasiveText; }
            set { _btnInvasiveText = value; OnPropertyChanged("BtnInvasiveText"); }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        private VisualProjectLocation _VisualProjectLocation;

        public VisualProjectLocation SelectedItem
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
                        Response result = await projectService.UpdateItemAsync(DataModel);
                        if (result.Status == ApiResult.Success)
                        {

                            var parameters = new NavigationParameters { { "Project", DataModel }, { "Project", Project } };
                            RegionManger.RequestNavigate("MainRegion", "SingleLevelProject", parameters);
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
        public async Task<bool> LongOperation(NavigationContext navigationContext)
        {

            if (navigationContext != null)
            {
           
                Data = DataModel = Project = (Project)navigationContext.Parameters["Project"];
                ObjectString = Newtonsoft.Json.JsonConvert.SerializeObject(DataModel);
            }
            IsDataShow = false;
            SelectedItem = null; 
            
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
                else
                {
                    CanInvasiveCreate = true;
                    BtnInvasiveText = "Invasive";
                }

            }
            else
            {
                CanInvasiveCreate = true;
                BtnInvasiveText = "Refresh";

            }
            
            if (App.IsInvasive)
            {
                if (Data != null)
                    Items = new ObservableCollection<VisualProjectLocation>(await VisualProjectLocationService.GetItemsAsyncByVisualProjectLocationId(App.ProjectID));
                IsInvasiveControlDisable = false;
            }
            else
            {
                IsInvasiveControlDisable = true;
                if (Data != null)
                    Items = new ObservableCollection<VisualProjectLocation>(await VisualProjectLocationService.GetItemsAsyncByVisualProjectLocationId(Data.Id));
            }
               

            return await Task.FromResult(true);

        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }





        private readonly new IDialogService _dialogService;
        public VisualSingleLevelProjectLocationViewModel(IDialogService dialogService)
        {

            _dialogService = dialogService;
            Title = "Single Level Project";


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
