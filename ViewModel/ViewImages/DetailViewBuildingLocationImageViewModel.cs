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
    public class DetailViewBuildingLocationImageViewModel : BaseViewModel, INavigationAware
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
        private Project _project;

        public Project Project
        {
            get { return _project; }
            set { _project = value; OnPropertyChanged("Project"); }
        }
        private ProjectBuilding _pb;

        public ProjectBuilding ProjectBuilding
        {
            get { return _pb; }
            set { _pb = value; OnPropertyChanged("ProjectBuilding"); }
        }
        public DelegateCommand BackCommand => new DelegateCommand(async () => await Back());
        public async Task Back()
        {
            var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project },{ "BuildingLocation",Data } };
            RegionManger.RequestNavigate("MainRegion", "Building", parameters);
            //RegionManger.NavigationService.Journal.GoForward()
            // RegionManger.Regions["MainRegion"].NavigationService.Journal.GoBack();
            //   RegionManger.RequestNavigate("MainRegion", "Project");

        }
        public DelegateCommand NewCommand => new DelegateCommand(async () => await New());
        public async Task New()
        {
           
        }
        public DelegateCommand SearchCommand => new DelegateCommand(async () => await Search());
        public async Task Search()
        {
            var s = SearchText;
            var s1 = SelectedType;
            var s2 = CreatedOn;
        }
        

        public DelegateCommand CloseCommand => new DelegateCommand(async () => await Close());
        public async Task Close()
        {
           
        }
      

       
        private ObservableCollection<BindingModel> bindingModels;
        public ObservableCollection<BindingModel> BindingModels
        {
            get { return bindingModels; }
            set { bindingModels = value; OnPropertyChanged("BindingModels"); }
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Project = (Project)navigationContext.Parameters["Project"];
            //  ProjectBuildings = new ObservableCollection<ProjectBuilding>(await ProjectBuildingDataStore.GetItemsAsyncByProjectID(Project.Id));
            await Task.Run(() => LongOperation(navigationContext));
          
        }
        private ObservableCollection<BuildingCommonLocationImages> _prImages;
        public ObservableCollection<BuildingCommonLocationImages> Items
        {
            get { return _prImages; }
            set { _prImages = value; OnPropertyChanged("Items"); }
        }
        private BuildingLocation _bl;
        public BuildingLocation Data
        {
            get { return _bl; }
            set { _bl = value; OnPropertyChanged("Data"); }
        }
        private BuildingLocation _blDataModel;
        public BuildingLocation DataModel
        {
            get { return _blDataModel; }
            set { _blDataModel = value; OnPropertyChanged("DataModel"); }
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
        public async void LongOperation(NavigationContext navigationContext)
        {
            ProjectBuilding = (ProjectBuilding)navigationContext.Parameters["ProjectBuilding"];
            Project = (Project)navigationContext.Parameters["Project"];
            Data =DataModel= (BuildingLocation)navigationContext.Parameters["BuildingLocation"];
            ObjectString = Newtonsoft.Json.JsonConvert.SerializeObject(DataModel);
            if (Data!=null)
            Items = new ObservableCollection<BuildingCommonLocationImages>(await BuildingCommonLocationImagesDataStore.GetItemsAsyncByBuildingId(Data.Id));

            //   Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync());
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }



        public async Task<ErrorModel> Delete()
        {
            ErrorModel err = new ErrorModel();

            if (Data != null)
            {

                try
                {

                    Response result = await BuildingLocationDataStore.DeleteItemAsync(Data);
                    if (result.Status == ApiResult.Success)
                    {
                        await Back();
                        err.Status = "Success";
                        err.Message = result.Message;




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
        public DetailViewBuildingLocationImageViewModel(IDialogService dialogService)
        {
           
           
            Title = "Building Common Location";
          

            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }
        private string _objStrin;

        public string ObjectString
        {
            get { return _objStrin; }
            set { _objStrin = value; }
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

                        //Response result = await projectService.AddItemAsync(Project);
                        //if (result.Status == ApiResult.Success)
                        //{
                        //    Response getObj = await projectService.GetItemAsync(result.ID);
                        //    if (getObj.Status == ApiResult.Success)
                        //    {
                        //        App.ProjectID = result.ID;
                        //        //  Project project = JsonConvert.DeserializeObject<Project>(getObj.Data.ToString());
                        //        // var parameters = new NavigationParameters { { "ProjectID", result.ID } };
                        //        RegionManger.RequestNavigate("MainRegion", "Project");
                        //    }
                        //    err.Status = "Success";
                        //    err.Message = result.Message;
                        //    // RegionManger.RequestNavigate("MainRegion", "Projects");
                        //}
                        //else
                        //{
                        //    err.Status = "Error";
                        //    err.Message = result.Message;
                        //}
                    }
                    else
                    {
                        Response result = await BuildingLocationDataStore.UpdateItemAsync(DataModel);
                        if (result.Status == ApiResult.Success)
                        {
                            var parameters = new NavigationParameters { { "BuildingLocation", DataModel }, { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
                            RegionManger.RequestNavigate("MainRegion", "DetailBuildingLocation", parameters);
                            //App.ProjectID = Project.Id;
                            //ErrorMsg = result.Message;
                            //  var parameters = new NavigationParameters { { "ProjectBuilding", ProjectBuilding }, { "Project", Project } };
                            // RegionManger.RequestNavigate("MainRegion", "Building", parameters);

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
        public DelegateCommand<BuildingCommonLocationImages> DeleteCommand => new DelegateCommand<BuildingCommonLocationImages>(async (BuildingCommonLocationImages prm) => await Delete(prm));
        public async Task Delete(BuildingCommonLocationImages prm)
        {
            ErrorModel err = new ErrorModel();
            Response result = await BuildingCommonLocationImagesDataStore.DeleteItemAsync(prm);
            if (result.Status == ApiResult.Success)
            {

                if (Data != null)
                {
                    Items = new ObservableCollection<BuildingCommonLocationImages>(await BuildingCommonLocationImagesDataStore.GetItemsAsyncByBuildingId(Data.Id));
                }
            }
            else
            {
                err.Status = "Error";
                err.Message = result.Message;
            }

        }

        public async Task<ErrorModel> Reorder()
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
                Response result = await BuildingCommonLocationImagesDataStore.Reorder(list);

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
    }
}
