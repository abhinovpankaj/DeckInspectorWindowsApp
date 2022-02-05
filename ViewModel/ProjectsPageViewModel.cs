using CommonServiceLocator;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UI.Code.Model;
using UI.Code.View.Dialog;

namespace UI.Code.ViewModel
{
    public class ProjectsPageViewModel : BaseViewModel, INavigationAware
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
            SearchText = string.Empty;
            CreatedOn = null;
            IsCompleted = false;
            SelectedeportType = "Running";
            SelectedeportType = "All";
            await Task.Run(() => LongOperation(SearchText, SelectedeportType, null));
        }
        private string _selecteType;

        public string SelectedeportType
        {
            get { return _selecteType; }
            set { _selecteType = value; OnPropertyChanged("SelectedeportType"); }
        }
        public async void ReloadLocation(bool isActive=false)
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
            Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync(SearchText, SelectedeportType, DateCreated,isActive));
            IsBusy = false;
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
        private bool _isProjectDeleted;
        public bool IsProjectDeleted
        {
            get { return _isProjectDeleted; }
            set { _isProjectDeleted = value; OnPropertyChanged("IsProjectDeleted"); }
        }

        public DelegateCommand SearchCommand => new DelegateCommand(async () => await Search());
        public async Task Search()
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

            await Task.Run(() => LongOperation(SearchText, SelectedeportType, DateCreated,IsProjectDeleted));

        }
        public DelegateCommand<Project> SelectedItemCommand => new DelegateCommand<Project>(async (Project prm) => await SelectedItemExecute(prm));
        public async Task SelectedItemExecute(Project prm)
        {
            App.IsInvasive = false;
            
            App.ProjectID = prm.Id;
            if (prm.Category=="SingleLevel")
            {
                var parameters = new NavigationParameters { { "Project", prm } };
                RegionManger.RequestNavigate("MainRegion", "SingleLevelProject", parameters);
            }
            else
            {
                var parameters = new NavigationParameters { { "Project", prm } };
                RegionManger.RequestNavigate("MainRegion", "Project", parameters);
            }
            
            
        }


        public DelegateCommand<Project> InvasiveItemCommand => new DelegateCommand<Project>(async (Project prm) => await InvasiveItemCommandExecute(prm));
        public async Task InvasiveItemCommandExecute(Project prm)
        {
            App.IsInvasive = true;
            App.ProjectID = prm.InvasiveProjectID;
            if (prm.Category == "SingleLevel")
            {
                prm.ProjectType = "Invasive";
                var parameters = new NavigationParameters { { "Project", prm } };
                RegionManger.RequestNavigate("MainRegion", "SingleLevelProject", parameters);
            }
            else
            {
                var parameters = new NavigationParameters { { "Project", prm } };
                RegionManger.RequestNavigate("MainRegion", "Project", parameters);
            }
            //var parameters = new NavigationParameters { { "Project", prm } };
            //RegionManger.RequestNavigate("MainRegion", "Project", parameters);
            ////if (SelectedItem != null)
            ////{

            ////  }
            ////var v = this.SelectedItem;
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

        private UCFilePageViewModel uCF;

        public UCFilePageViewModel UCFilePageViewModel
        {
            get { return uCF; }
            set { uCF = value; OnPropertyChanged("UCFilePageViewModel"); }
        }
        private bool _isRole;

        public bool IsRoleAdmin
        {
            get { return _isRole; }
            set { _isRole = value;OnPropertyChanged("IsRoleAdmin"); }
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(App.LogUser.RoleName=="Admin")
            {
                IsRoleAdmin = true;
            }
               UCFilePageViewModel = new UCFilePageViewModel(null);
            SelectedItem = null;
            ProjectTypeList = new ObservableCollection<string>();
            ProjectTypeList.Add("Running");
            ProjectTypeList.Add("Completed");
            SelectedeportType = "Running";
            //  ProjectTypeList.Add("All");
            await Task.Run(() => LongOperation(SearchText, string.Empty, string.Empty));

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public bool Showhide { get; set; }
        public async Task<bool> LongOperation(string search, string ProjectType, string CreatedOn, bool isProjectDeleted=false)
        {
            IsBusy = true;
            //IsCompleted = false;
            if (IsCompleted == true)
            {
                SelectedeportType = "Completed";
            }
            else
            {

                SelectedeportType = "All";
            }
          
            //TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync(SearchText)), null);
            Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync(search, SelectedeportType, CreatedOn,isProjectDeleted));

            IsBusy = false;
            Showhide = IsProjectDeleted;
            OnPropertyChanged("Showhide");
            return await Task.FromResult(true);
            
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
           // TreeItems = MakeTree(new ObservableCollection<Organization>(await treeService.GetItemAsync()), null);
            Projects = new ObservableCollection<Project>(await projectService.GetItemsAsync("", "", ""));

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
        //public ProjectsPageViewModel() { }
        private readonly new IDialogService _dialogService;
        public ProjectsPageViewModel()
        {

        }
        public ProjectsPageViewModel(IDialogService dialogService)
        {
            IsCompleted = false;
            _dialogService = dialogService;

            Title = "Project(s)";

            
            ImageQuality = Properties.Settings.Default.ImageQuality;
            Factor = Properties.Settings.Default.Factor;
            ReportMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(8000));
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

        private void ShowDialog(string message)
        {

            ReportMessageQueue.Enqueue(message);
        }

        public DelegateCommand<Project> FileCommand => new DelegateCommand<Project>(async (Project p) => await FileExecute(p));
        public async Task FileExecute(Project p)
        {
            //ProjectType = "Visual Report";
            //if (!string.IsNullOrEmpty(ProjectType))
            //{
            //    ErrorMsg = string.Empty;
                var parameters = new NavigationParameters { { "Page", "Page" },{"Project", p } };
               // RegionManger.RequestNavigate("MainRegion", "ProjectAddOrEdit");
            RegionManger.RequestNavigate("TreeRegion", "FileUC", parameters);
            //}
            //else
            //{
            //    ErrorMsg = "Please Select Report Type";
            //}
        }
        #region ImageFormatting
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public long ImageQuality { get; set; }

        public int Factor { get; set; }
        public SnackbarMessageQueue ReportMessageQueue { get; private set; }
        #endregion



        #region reporting

        #region Invasive

        public async Task WordInvasive(long quality, int height, int width, string projectID = "11D6DFDB-EF89-42E4-A127-7565CCE65DC0", string company = "DI", string Type = "Word")
        {
            Response result = await projectService.GetItemAsync(projectID);
            
            Project project = JsonConvert.DeserializeObject<Project>(result.Data.ToString());

            if (project.Category=="SingleLevel")
            {
                await WordInvasiveSingleLevel(quality, height, width, projectID, company, Type);
                return;
            }
            int factor = height;
            IsReportInProgress = System.Windows.Visibility.Visible;
            string fname = string.Empty;
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads\\";
            if (Directory.Exists(download + "\\compressed"))
            {
                Directory.Delete(download + "\\compressed", true);
            }
            Directory.CreateDirectory(download + "\\compressed");
            NotificationUI("Report Creation Started.");
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Report\\";
            string html = System.IO.File.ReadAllText(path + "Invasive.html", iso);

            try
            {


                StringBuilder mainHtml = new StringBuilder();

                mainHtml.Append("<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset='utf-8'/> <title></title>" +

                    "</head><body>");
                if (project != null)
                {
                    html = html.Replace("{projectName}", project.Name);
                    html = html.Replace("{projectAdd}", project.Address);
                    html = html.Replace("{projectDes}", project.Description);
                    html = html.Replace("{projectDate}", project.CreatedOn);

                    //get the Project Image
                    if (project.ImageUrl != null)
                    {

                        html = html.Replace("{projectImagePath}", project.ImageUrl);
                    }



                    if (company == "DI")
                    {
                        html = html.Replace("{projectCreated}", "Deck Inspectors");
                    }
                    else
                        html = html.Replace("{projectCreated}", "WICR Waterproofing and Construction");
                    //html = html.Replace("{projectCreated}", project.Username);

                    IEnumerable<ProjectBuilding> buildingList = await ProjectBuildingDataStore.GetItemsAsyncByProjectID(projectID);
                    IEnumerable<ProjectLocation> ProjectLocationList = await projectLocationService.GetItemsAsyncByProjectID(projectID);


                    html = html.Replace("{heading}", "Invasive Inspection Report");

                    mainHtml.Append(html);
                    if (buildingList.Count() != 0)//Check Building Exists
                    {

                        foreach (var building in buildingList)////open foreach building
                        {

                            // html_Header = string.Empty;
                            //APARTMENT
                            StatusMessage = "Fetching details for Apartments...";
                            IEnumerable<BuildingApartment> buildingapartmentList = await BuildingApartmentDataStore.GetItemsAsyncByBuildingId(building.Id);
                            if (buildingapartmentList.Count() != 0)
                            {
                                foreach (var apartment in buildingapartmentList)////open building apartment list
                                {


                                    IEnumerable<VisualBuildingApartment> visualBuildingApartment = await VisualFormApartmentDataStore.GetVisualBuildingApartmentByBuildingApartmentId(apartment.Id);
                                    if (visualBuildingApartment.Count() != 0)
                                    {
                                        foreach (var visualApt in visualBuildingApartment)
                                        {
                                            //Table_B_Apartment
                                            string html_Header = System.IO.File.ReadAllText(path + "\\VisualDetail.html", iso);
                                            html_Header = html_Header.Replace("{BuildingName}", building.Name);
                                            html_Header = html_Header.Replace("{AptName}", apartment.Name);
                                            html_Header = html_Header.Replace("{LocName}", visualApt.Name);

                                            html_Header = html_Header.Replace("{TitleName}", "Building Name");
                                            html_Header = html_Header.Replace("{TitleApt}", "Apartment Name");
                                            html_Header = html_Header.Replace("{TitleLoc}", "Location Name");

                                            html_Header = html_Header.Replace("{ExteriorElements}", visualApt.ExteriorElements.Replace(",", ", ").Replace("Thershold", "Threshold"));
                                            html_Header = html_Header.Replace("{WaterProofingElements}", visualApt.WaterProofingElements.Replace(",", ", "));



                                            if (visualApt.VisualReview == "Bad")
                                            {
                                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"red\">&nbsp;" + visualApt.VisualReview + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"green\">&nbsp;" + visualApt.VisualReview + "</font></p>");
                                            if (visualApt.AnyVisualSign == "Yes")
                                            {
                                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"red\">&nbsp;" + visualApt.AnyVisualSign + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"green\">&nbsp;" + visualApt.AnyVisualSign + "</font></p>");

                                            if (visualApt.FurtherInasive == "Yes")
                                            {
                                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"red\">&nbsp;" + visualApt.FurtherInasive + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"green\">&nbsp;" + visualApt.FurtherInasive + "</font></p>");
                                            if (visualApt.ConditionAssessment == "Fail")
                                            {
                                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"red\">&nbsp;" + visualApt.ConditionAssessment + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"green\">&nbsp;" + visualApt.ConditionAssessment + "</font></p>");


                                            html_Header = html_Header.Replace("{Additional}", visualApt.AdditionalConsideration.Replace("\n", "<br/>"));

                                            if (visualApt.LifeExpectancyEEE == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualApt.LifeExpectancyEEE + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualApt.LifeExpectancyEEE + "</p>");

                                            if (visualApt.LifeExpectancyLBC == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualApt.LifeExpectancyLBC + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualApt.LifeExpectancyLBC + "</p>");

                                            if (visualApt.LifeExpectancyAWE == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualApt.LifeExpectancyAWE + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualApt.LifeExpectancyAWE + "</p>");


                                            html_Header = html_Header.Replace("{detailheading}", "Invasive Inspection Details");
                                            mainHtml.Append(html_Header);

                                            string html_VisualImages = System.IO.File.ReadAllText(path + "\\VisualImage.html", iso);

                                            IEnumerable<VisualApartmentLocationPhoto> imageApartment = await VisualApartmentLocationPhotoDataStore.GetVisualBuildingApartmentImageByVisualApartmentId(visualApt.Id.ToString());
                                            var visualimageApartment = imageApartment.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();

                                            StringBuilder visual_Images_apartment = new StringBuilder();
                                            int apCount = 0;
                                            if (visualimageApartment.Count() <= factor)
                                            {

                                                visual_Images_apartment.Append(trWithStyle);
                                                foreach (var item in visualimageApartment)
                                                {
                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);

                                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                                }
                                                visual_Images_apartment.Append("</tr>");
                                            }
                                            else
                                            {
                                                visual_Images_apartment.Append(trWithStyle);
                                                foreach (var item in visualimageApartment)
                                                {

                                                    if ((apCount % factor) == 0)
                                                    {

                                                        visual_Images_apartment.Append("</tr>");
                                                        visual_Images_apartment.Append(trWithStyle);
                                                    }

                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                                    apCount++;
                                                }
                                                visual_Images_apartment.Append("</tr>");
                                            }
                                            html_VisualImages = html_VisualImages.Replace("{VisualImages}", visual_Images_apartment.ToString()).Replace("<tr></tr>", "");
                                            html_VisualImages = html_VisualImages.Replace("{imageCount}", factor.ToString());
                                            mainHtml.Append(html_VisualImages);


                                            List<VisualApartmentLocationPhoto> InvasiveimageApartment = imageApartment.Where(c => c.ImageDescription == "TRUE").ToList();


                                            //Invasive Images
                                            string html_InvasiveImages = System.IO.File.ReadAllText(path + "\\VisualInvasiveImage.html", iso);



                                            StringBuilder Inv_Images_apartment = new StringBuilder();
                                            if (InvasiveimageApartment.Count <= factor)
                                            {

                                                Inv_Images_apartment.Append(trWithStyle);
                                                foreach (var item in InvasiveimageApartment)
                                                {

                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                                    Inv_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                                }
                                                Inv_Images_apartment.Append("</tr>");
                                            }
                                            else
                                            {
                                                int apCountInv = 0;
                                                Inv_Images_apartment.Append(trWithStyle);
                                                foreach (var item in InvasiveimageApartment)
                                                {


                                                    if ((apCountInv % factor) == 0)
                                                    {

                                                        Inv_Images_apartment.Append("</tr>");
                                                        Inv_Images_apartment.Append(trWithStyle);
                                                    }
                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);

                                                    Inv_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                                    apCountInv++;
                                                }
                                                Inv_Images_apartment.Append("</tr>");
                                            }



                                            html_InvasiveImages = html_InvasiveImages.Replace("{InvasiveDescrition}", visualApt.ImageDescription.Replace("\n", "<br/>")).Replace("{InvasiveImages}", Inv_Images_apartment.ToString()).Replace("<tr></tr>", "");
                                            html_InvasiveImages = html_InvasiveImages.Replace("{imageCount}", factor.ToString());

                                            mainHtml.Append(html_InvasiveImages);

                                            //Add Conclusive Part 
                                            string ConclusiveHtml = await getConclusiveHTMLContent(visualApt, quality, factor);
                                            mainHtml.Append(ConclusiveHtml);

                                        }

                                    }

                                }//close building apartment list
                            }

                        }//close foreach building

                        foreach (var building in buildingList)////open foreach building
                        {

                            IEnumerable<BuildingLocation> buildingLocationList = await BuildingLocationDataStore.GetItemsAsyncByBuildingId(building.Id);
                            if (buildingLocationList.Count() != 0)
                            {
                                StatusMessage = "Fetching details for Building locations...";
                                ////open building location list
                                foreach (var blocation in buildingLocationList)
                                {


                                    IEnumerable<VisualBuildingLocation> visualBuildingLocationList = await VisualFormBuildingLocationDataStore.GetVisualBuildingLocationByBuildingLocationId(blocation.Id);
                                    if (visualBuildingLocationList.Count() != 0)
                                    {
                                        foreach (var visualbuildingLocation in visualBuildingLocationList)
                                        {

                                            //Table_B_Apartment
                                            string html_Header = System.IO.File.ReadAllText(path + "\\VisualDetail.html", iso);
                                            html_Header = html_Header.Replace("{BuildingName}", building.Name);
                                            html_Header = html_Header.Replace("{AptName}", blocation.Name);
                                            html_Header = html_Header.Replace("{LocName}", visualbuildingLocation.Name);

                                            html_Header = html_Header.Replace("{TitleName}", "Building Name");
                                            html_Header = html_Header.Replace("{TitleApt}", "Building Common Location Name");
                                            html_Header = html_Header.Replace("{TitleLoc}", "Location Name");

                                            html_Header = html_Header.Replace("{ExteriorElements}", visualbuildingLocation.ExteriorElements.Replace(",", ", ").Replace("Thershold", "Threshold"));
                                            html_Header = html_Header.Replace("{WaterProofingElements}", visualbuildingLocation.WaterProofingElements.Replace(",", ", "));

                                            if (visualbuildingLocation.VisualReview == "Bad")
                                            {
                                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.VisualReview + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.VisualReview + "</font></p>");
                                            if (visualbuildingLocation.AnyVisualSign == "Yes")
                                            {
                                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.AnyVisualSign + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.AnyVisualSign + "</font></p>");

                                            if (visualbuildingLocation.FurtherInasive == "Yes")
                                            {
                                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.FurtherInasive + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.FurtherInasive + "</font></p>");
                                            if (visualbuildingLocation.ConditionAssessment == "Fail")
                                            {
                                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.ConditionAssessment + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.ConditionAssessment + "</font></p>");


                                            //html_Header = html_Header.Replace("{Additional}", visualbuildingLocation.AdditionalConsideration);
                                            html_Header = html_Header.Replace("{Additional}", visualbuildingLocation.AdditionalConsideration.Replace("\n", "<br/>"));

                                            if (visualbuildingLocation.LifeExpectancyEEE == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.LifeExpectancyEEE + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualbuildingLocation.LifeExpectancyEEE + "</p>");

                                            if (visualbuildingLocation.LifeExpectancyLBC == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.LifeExpectancyLBC + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualbuildingLocation.LifeExpectancyLBC + "</p>");

                                            if (visualbuildingLocation.LifeExpectancyAWE == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.LifeExpectancyAWE + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualbuildingLocation.LifeExpectancyAWE + "</p>");


                                            html_Header = html_Header.Replace("{detailheading}", "Invasive Inspection Details");
                                            mainHtml.Append(html_Header);



                                            IEnumerable<VisualBuildingLocationPhoto> imageApartment = await VisualBuildingLocationPhotoDataStore.GetVisualBuildingLocationImageByVisualBuildingId(visualbuildingLocation.Id.ToString());
                                            var visualimageApartment = imageApartment.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();

                                            string html_VisualImages = System.IO.File.ReadAllText(path + "\\VisualImage.html", iso);

                                            StringBuilder visual_Images_apartment = new StringBuilder();
                                            int apCount = 0;
                                            if (visualimageApartment.Count() <= factor)
                                            {

                                                visual_Images_apartment.Append(trWithStyle);
                                                foreach (var item in visualimageApartment)
                                                {
                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                                    // visual_Images_apartment.Append("<td><img  width=\"100%\"   src='" + newCompressedPath + "'/></td>");
                                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                                }
                                                visual_Images_apartment.Append("</tr>");
                                            }
                                            else
                                            {
                                                visual_Images_apartment.Append(trWithStyle);
                                                foreach (var item in visualimageApartment)
                                                {

                                                    if ((apCount % factor) == 0)
                                                    {

                                                        visual_Images_apartment.Append("</tr>");
                                                        visual_Images_apartment.Append(trWithStyle);
                                                    }
                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);

                                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                                    apCount++;
                                                }
                                                visual_Images_apartment.Append("</tr>");
                                            }
                                            html_VisualImages = html_VisualImages.Replace("{VisualImages}", visual_Images_apartment.ToString()).Replace("<tr></tr>", "");
                                            html_VisualImages = html_VisualImages.Replace("{imageCount}", factor.ToString());
                                            mainHtml.Append(html_VisualImages);



                                            List<VisualBuildingLocationPhoto> InvasiveimageApartment = imageApartment.Where(c => c.ImageDescription == "TRUE").ToList();

                                            string html_InvasiveImages = System.IO.File.ReadAllText(path + "\\VisualInvasiveImage.html", iso);

                                            StringBuilder Inv_Images_apartment = new StringBuilder();
                                            if (InvasiveimageApartment.Count <= factor)
                                            {

                                                Inv_Images_apartment.Append(trWithStyle);
                                                foreach (var item in InvasiveimageApartment)
                                                {


                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                                    Inv_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                                }
                                                Inv_Images_apartment.Append("</tr>");
                                            }
                                            else
                                            {
                                                int apCountInv = 0;
                                                Inv_Images_apartment.Append(trWithStyle);
                                                foreach (var item in InvasiveimageApartment)
                                                {

                                                    if ((apCountInv % factor) == 0)
                                                    {

                                                        Inv_Images_apartment.Append("</tr>");
                                                        Inv_Images_apartment.Append(trWithStyle);
                                                    }
                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                                    Inv_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                                    apCountInv++;
                                                }
                                                Inv_Images_apartment.Append("</tr>");
                                            }



                                            html_InvasiveImages = html_InvasiveImages.Replace("{InvasiveDescrition}", blocation.Description.Replace("\n", "<br/>")).Replace("{InvasiveImages}", Inv_Images_apartment.ToString()).Replace("<tr></tr>", "");
                                            html_InvasiveImages = html_InvasiveImages.Replace("{imageCount}", factor.ToString());
                                            mainHtml.Append(html_InvasiveImages);

                                            //add Conclusive Part
                                            string ConclusiveHtml = await getConclusiveHTMLContent(visualbuildingLocation, quality, factor);
                                            mainHtml.Append(ConclusiveHtml);


                                        }

                                    }

                                }//close building apartment list
                            }
                        }

                    }
                    if (ProjectLocationList.Count() != 0)
                    {
                        StatusMessage = "Fetching details for Project locations...";
                        foreach (var projectlocation in ProjectLocationList)////open project location list
                        {


                            IEnumerable<VisualProjectLocation> visualprojectLocation = await VisualProjectLocationService.GetItemsAsyncByVisualProjectLocationId(projectlocation.Id);
                            if (visualprojectLocation.Count() != 0)
                            {
                                foreach (var visualPloc in visualprojectLocation)
                                {
                                    string html_Header = System.IO.File.ReadAllText(path + "\\VisualDeatilForProjectLocation.html", iso);
                                    html_Header = html_Header.Replace("{BuildingName}", projectlocation.Name);
                                    html_Header = html_Header.Replace("{AptName}", projectlocation.Name);
                                    html_Header = html_Header.Replace("{LocName}", visualPloc.Name);

                                    // html_Header = html_Header.Replace("{TitleName}", "Project Common Location Name");
                                    html_Header = html_Header.Replace("{TitleApt}", "Project Common Location Name");
                                    html_Header = html_Header.Replace("{TitleLoc}", "Location Name");

                                    html_Header = html_Header.Replace("{ExteriorElements}", visualPloc.ExteriorElements.Replace(",", ", ").Replace("Thershold", "Threshold"));
                                    html_Header = html_Header.Replace("{WaterProofingElements}", visualPloc.WaterProofingElements.Replace(",", ", "));

                                    if (visualPloc.VisualReview == "Bad")
                                    {
                                        html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"red\">&nbsp;" + visualPloc.VisualReview + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"green\">&nbsp;" + visualPloc.VisualReview + "</font></p>");
                                    if (visualPloc.AnyVisualSign == "Yes")
                                    {
                                        html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"red\">&nbsp;" + visualPloc.AnyVisualSign + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"green\">&nbsp;" + visualPloc.AnyVisualSign + "</font></p>");

                                    if (visualPloc.FurtherInasive == "Yes")
                                    {
                                        html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"red\">&nbsp;" + visualPloc.FurtherInasive + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"green\">&nbsp;" + visualPloc.FurtherInasive + "</font></p>");
                                    if (visualPloc.ConditionAssessment == "Fail")
                                    {
                                        html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"red\">&nbsp;" + visualPloc.ConditionAssessment + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"green\">&nbsp;" + visualPloc.ConditionAssessment + "</font></p>");

                                    //html_Header = html_Header.Replace("{Additional}", visualPloc.AdditionalConsideration);
                                    html_Header = html_Header.Replace("{Additional}", visualPloc.AdditionalConsideration.Replace("\n", "<br/>"));

                                    if (visualPloc.LifeExpectancyEEE == "0-1 Years")
                                    {
                                        html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyEEE + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualPloc.LifeExpectancyEEE + "</p>");

                                    if (visualPloc.LifeExpectancyLBC == "0-1 Years")
                                    {
                                        html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyLBC + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualPloc.LifeExpectancyLBC + "</p>");

                                    if (visualPloc.LifeExpectancyAWE == "0-1 Years")
                                    {
                                        html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyAWE + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualPloc.LifeExpectancyAWE + "</p>");

                                    mainHtml.Append(html_Header);




                                    IEnumerable<VisualProjectLocationPhoto> imageApartment = await VisualProjectLocationPhotoDataStore.GetItemsAsyncByVisualProjectLocationId(visualPloc.Id.ToString());
                                    var visualimageApartment = imageApartment.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();
                                    string html_VisualImages = System.IO.File.ReadAllText(path + "\\VisualImage.html", iso);

                                    StringBuilder visual_Images_apartment = new StringBuilder();
                                    int apCount = 0;
                                    if (visualimageApartment.Count() <= factor)
                                    {

                                        visual_Images_apartment.Append(trWithStyle);
                                        foreach (var item in visualimageApartment)
                                        {
                                            string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                            var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                            visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                        }
                                        visual_Images_apartment.Append("</tr>");
                                    }
                                    else
                                    {
                                        visual_Images_apartment.Append(trWithStyle);
                                        foreach (var item in visualimageApartment)
                                        {

                                            if ((apCount % factor) == 0)
                                            {

                                                visual_Images_apartment.Append("</tr>");
                                                visual_Images_apartment.Append(trWithStyle);
                                            }
                                            string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                            var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);

                                            visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                            apCount++;
                                        }
                                        visual_Images_apartment.Append("</tr>");
                                    }
                                    html_VisualImages = html_VisualImages.Replace("{VisualImages}", visual_Images_apartment.ToString()).Replace("<tr></tr>", "");
                                    html_VisualImages = html_VisualImages.Replace("{imageCount}", factor.ToString());
                                    mainHtml.Append(html_VisualImages);
                                    //Invasive


                                    List<VisualProjectLocationPhoto> InvasiveimageApartment = imageApartment.Where(c => c.ImageDescription == "TRUE").ToList();
                                    string html_InvasiveImages = System.IO.File.ReadAllText(path + "\\VisualInvasiveImage.html", iso);



                                    StringBuilder Inv_Images_apartment = new StringBuilder();

                                    if (InvasiveimageApartment.Count <= factor)
                                    {

                                        Inv_Images_apartment.Append(trWithStyle);
                                        foreach (var item in InvasiveimageApartment)
                                        {


                                            string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                            var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                            Inv_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                        }
                                        Inv_Images_apartment.Append("</tr>");
                                    }
                                    else
                                    {
                                        int apCountInv = 0;
                                        Inv_Images_apartment.Append(trWithStyle);
                                        foreach (var item in InvasiveimageApartment)
                                        {
                                            if ((apCountInv % factor) == 0)
                                            {

                                                Inv_Images_apartment.Append("</tr>");
                                                Inv_Images_apartment.Append(trWithStyle);
                                            }
                                            string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                            var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                            Inv_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                            apCountInv++;
                                        }
                                        Inv_Images_apartment.Append("</tr>");
                                    }

                                    html_InvasiveImages = html_InvasiveImages.Replace("{InvasiveDescrition}", visualPloc.ImageDescription.Replace("\n", "<br/>")).Replace("{InvasiveImages}", Inv_Images_apartment.ToString()).Replace("<tr></tr>", "");
                                    html_InvasiveImages = html_InvasiveImages.Replace("{imageCount}", factor.ToString());
                                    mainHtml.Append(html_InvasiveImages);

                                    string conclusiveHTML = await getConclusiveHTMLContent(visualPloc, quality, factor);
                                    mainHtml.Append(conclusiveHTML);
                                }

                            }

                        }
                    }

                }

                mainHtml.Append("</body></html>");
                //to save html file.
                var projPath = path + "\\" + "ProjectReport.html";
                using (StreamWriter sr = File.CreateText(projPath))
                {
                    sr.Write(mainHtml.ToString());
                    sr.Close();

                }
                StatusMessage = "Creating document...";
                using (Stream fileStreamPath = GenerateStreamFromString(mainHtml.ToString()))
                {
                    //Creates a new instance of WordDocument
                    WordDocument document = new WordDocument();

                    document.Open(fileStreamPath, FormatType.Html);
                    //var destPath = HttpContext.Current.Server.MapPath("~/Report/" + "report123.docx");
                    ////save document locally.
                    //document.Save(destPath);

                    if (company == "WICR")
                    {
                        foreach (WSection section in document.Sections)
                        {
                            // Add a new table to the header
                            IWParagraph headerPar = section.HeadersFooters.Header.AddParagraph();

                            RowFormat format = new RowFormat();
                            format.HorizontalAlignment = RowAlignment.Center;
                            // Setting Single table border style.
                            format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;


                            string s = path + "wicrlogo.jpg";

                            headerPar.AppendPicture(System.Drawing.Image.FromFile(s));
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            //Set Image size.
                            (headerPar.Items[0] as WPicture).Width = 132.5f;
                            (headerPar.Items[0] as WPicture).Height = 44.75f;
                            (headerPar.Items[0] as WPicture).HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            // Inserting text to the table second cell.
                            WParagraph headerPar1 = new WParagraph(document);
                            IWTextRange txt = headerPar1.AppendText("State Contractor’s License No.: 745936\nVisit our website: www.WICR.net \n888-388-9427");
                            txt.CharacterFormat.FontSize = 6;
                            txt.CharacterFormat.Bold = false;
                            txt.CharacterFormat.FontName = "Arial Narrow";
                            txt.CharacterFormat.TextColor = System.Drawing.ColorTranslator.FromHtml("#8EAADB");
                            txt.CharacterFormat.CharacterSpacing = 1.4f;
                            headerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;
                            section.HeadersFooters.Header.Paragraphs.Add(headerPar1);
                            // Add a footer paragraph text to the document.
                            WParagraph footerPar = new WParagraph(document);
                            footerPar.ParagraphFormat.Tabs.AddTab(523f, TabJustification.Centered, TabLeader.NoLeader);
                            // Add text.

                            IWTextRange firstText = footerPar.AppendText("www.WICR.net");
                            firstText.CharacterFormat.TextColor = Color.BlueViolet;
                            // Add page and Number of pages field to the document.
                            WParagraph footerPar1 = new WParagraph(document);
                            footerPar1.AppendText("\tPage ");
                            footerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;

                            IWField ff = footerPar1.AppendField("Page", FieldType.FieldPage);

                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar);
                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar1);

                            #region Page Number Settings
                            section.PageSetup.RestartPageNumbering = true;
                            section.PageSetup.PageStartingNumber = 1;
                            section.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;
                            #endregion Page Number Settings

                        }
                    }
                    else
                    {
                        foreach (WSection section in document.Sections)
                        {



                            // Add a new table to the header
                            // IWTable table = section.HeadersFooters.Header.AddTable();
                            IWParagraph headerPar = section.HeadersFooters.Header.AddParagraph();
                            RowFormat format = new RowFormat();
                            format.HorizontalAlignment = RowAlignment.Center;
                            // Setting Single table border style.
                            format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;

                            string s = path + "decklogo.jpg";

                            headerPar.AppendPicture(System.Drawing.Image.FromFile(s));
                            //Set Image size.
                            (headerPar.Items[0] as WPicture).Width = 132.5f;
                            (headerPar.Items[0] as WPicture).Height = 44.75f;
                            (headerPar.Items[0] as WPicture).HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            // Inserting text to the table second cell.
                            // headerPar = table[0, 0].AddParagraph() as WParagraph;
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                            // Add a footer paragraph text to the document.
                            WParagraph footerPar = new WParagraph(document);

                            //footerPar.ParagraphFormat.Tabs.AddTab(523f, TabJustification.Centered, TabLeader.NoLeader);
                            // Add text.
                            footerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            IWTextRange firstText = footerPar.AppendText("WWW.DECKINSPECTORS.COM");
                            firstText.CharacterFormat.TextColor = Color.Orange;
                            // Add page and Number of pages field to the document.
                            WParagraph footerPar1 = new WParagraph(document);
                            footerPar1.AppendText("\tPage ");
                            footerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;

                            IWField ff = footerPar1.AppendField("Page", FieldType.FieldPage);

                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar);
                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar1);
                            #region Page Number Settings
                            section.PageSetup.RestartPageNumbering = true;
                            section.PageSetup.PageStartingNumber = 1;
                            section.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;

                            #endregion Page Number Settings

                        }
                    }



                    if (Type == "Word")
                    {

                        if (company == "DI")
                        {
                            fname = "Deck_Invasive" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".docx";
                        }
                        else
                        {
                            fname = "WICR_Invasive" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".docx";
                        }
                        document.Save(download + fname, FormatType.Docx);


                    }
                    else
                    {
                        if (company == "DI")
                        {
                            fname = "Deck_Invasive" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".pdf";
                        }
                        else
                        {
                            fname = "WICR_Invasive" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".pdf";
                        }

                        DocToPDFConverter converter = new DocToPDFConverter();
                        PdfDocument pdfDocument = converter.ConvertToPDF(document);
                        converter.Dispose();

                        pdfDocument.Save(download + fname);
                        pdfDocument.Close();
                    }
                    document.Close();
                }

                NotificationUI("Report Created Successfully, please navigate to Downlaods folder to access it.");
                Process.Start(download + fname);
            }
            catch (Exception ex)
            {
                NotificationUI("Report Creation failed, please try again later," + ex.Message);
            }
            IsReportInProgress = System.Windows.Visibility.Collapsed;
        }

        private async Task<string> getConclusiveHTMLContent(VisualProjectLocation visualPloc, long quality, int factor)
        {

            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Report\\";
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            string conclusiveHtml = System.IO.File.ReadAllText(path + "\\NoConclusive.html", iso);
            if (!visualPloc.IsPostInvasiveRepairsRequired)
            {
                conclusiveHtml = conclusiveHtml.Replace("{Comments}", "Post Invasive repairs NOT required");
                return conclusiveHtml;
            }

            if (!visualPloc.IsInvasiveRepairApproved)
            {
                conclusiveHtml = conclusiveHtml.Replace("{Comments}", "Post Invasive repairs required, Owner has NOT agreed for Conclusive report.");
                return conclusiveHtml;
            }

            if (!visualPloc.IsInvasiveRepairComplete)
            {
                conclusiveHtml = conclusiveHtml.Replace("{Comments}", "Post Invasive repairs required, Conclusive analysis NOT Complete.");
                return conclusiveHtml;
            }


            string html_Header = System.IO.File.ReadAllText(path + "\\ConclusiveDetails.html", iso);

            html_Header = html_Header.Replace("{ConclusiveApproved}", visualPloc.IsInvasiveRepairComplete.ToString() == "True" ? "Yes" : "No");


            if (visualPloc.ConclusiveLifeExpEEE == "0-1 Years")
            {
                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualPloc.ConclusiveLifeExpEEE + "</font></p>");
            }
            else if (visualPloc.ConclusiveLifeExpEEE == "4-7 Years" || visualPloc.ConclusiveLifeExpEEE == "7+ Years")
                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"green\">&nbsp;" + visualPloc.ConclusiveLifeExpEEE + "</font></p>");
            else
                html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualPloc.ConclusiveLifeExpEEE + "</p>");


            if (visualPloc.ConclusiveLifeExpLBC == "0-1 Years")
            {
                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualPloc.ConclusiveLifeExpLBC + "</font></p>");
            }
            else if (visualPloc.ConclusiveLifeExpLBC == "4-7 Years" || visualPloc.ConclusiveLifeExpLBC == "7+ Years")
                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"green\">&nbsp;" + visualPloc.ConclusiveLifeExpLBC + "</font></p>");
            else
                html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualPloc.ConclusiveLifeExpLBC + "</p>");


            if (visualPloc.ConclusiveLifeExpAWE == "0-1 Years")
            {
                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualPloc.ConclusiveLifeExpAWE + "</font></p>");
            }
            else if (visualPloc.ConclusiveLifeExpAWE == "4-7 Years" || visualPloc.ConclusiveLifeExpAWE == "7+ Years")
            {
                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"green\">&nbsp;" + visualPloc.ConclusiveLifeExpAWE + "</font></p>");
            }
            else
                html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualPloc.ConclusiveLifeExpAWE + "</p>");


            html_Header = html_Header.Replace("{AdditonalConsiderations}", "<p>&nbsp;" + visualPloc.ConclusiveAdditionalConcerns.Replace("\n", "<br/>") + "</p>");

            string html_ConclusiveImages = System.IO.File.ReadAllText(path + "\\ConclusiveImages.html", iso);

            IEnumerable<VisualProjectLocationPhoto> imageApartment = await VisualProjectLocationPhotoDataStore.GetItemsAsyncByVisualProjectLocationId(visualPloc.Id.ToString());
            imageApartment = imageApartment.Where(c => c.ImageDescription == "CONCLUSIVE").ToList();

            StringBuilder conclusive_Images_apartment = new StringBuilder();
            int apCount = 0;
            if (imageApartment.Count() <= factor)
            {

                conclusive_Images_apartment.Append(trWithStyle);
                foreach (var item in imageApartment)
                {
                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);

                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                    conclusive_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                }
                conclusive_Images_apartment.Append("</tr>");
            }
            else
            {
                conclusive_Images_apartment.Append(trWithStyle);
                foreach (var item in imageApartment)
                {

                    if ((apCount % factor) == 0)
                    {

                        conclusive_Images_apartment.Append("</tr>");
                        conclusive_Images_apartment.Append(trWithStyle);
                    }
                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                    conclusive_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                    apCount++;
                }
                conclusive_Images_apartment.Append("</tr>");
            }

            html_ConclusiveImages = html_ConclusiveImages.Replace("{ConclusiveImages}", conclusive_Images_apartment.ToString()).Replace("<tr></tr>", "");
            html_ConclusiveImages = html_ConclusiveImages.Replace("{imageCount}", factor.ToString());
            return html_Header + html_ConclusiveImages;
        }

        private async Task<string> getConclusiveHTMLContent(VisualBuildingLocation visualbuildingLocation, long quality, int factor)
        {

            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Report\\";
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            string conclusiveHtml = System.IO.File.ReadAllText(path + "\\NoConclusive.html", iso);
            if (!visualbuildingLocation.IsPostInvasiveRepairsRequired)
            {
                conclusiveHtml = conclusiveHtml.Replace("{Comments}", "Post Invasive repairs NOT required");
                return conclusiveHtml;
            }

            if (!visualbuildingLocation.IsInvasiveRepairApproved)
            {
                conclusiveHtml = conclusiveHtml.Replace("{Comments}", "Post Invasive repairs required, Owner has NOT agreed for Conclusive report.");
                return conclusiveHtml;
            }

            if (!visualbuildingLocation.IsInvasiveRepairComplete)
            {
                conclusiveHtml = conclusiveHtml.Replace("{Comments}", "Post Invasive repairs required, Conclusive analysis NOT Complete.");
                return conclusiveHtml;
            }


            string html_Header = System.IO.File.ReadAllText(path + "\\ConclusiveDetails.html", iso);

            html_Header = html_Header.Replace("{ConclusiveApproved}", visualbuildingLocation.IsInvasiveRepairComplete.ToString() == "True" ? "Yes" : "No");


            if (visualbuildingLocation.ConclusiveLifeExpEEE == "0-1 Years")
            {
                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.ConclusiveLifeExpEEE + "</font></p>");
            }
            else if (visualbuildingLocation.ConclusiveLifeExpEEE == "4-7 Years" || visualbuildingLocation.ConclusiveLifeExpEEE == "7+ Years")
                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.ConclusiveLifeExpEEE + "</font></p>");
            else
                html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualbuildingLocation.ConclusiveLifeExpEEE + "</p>");


            if (visualbuildingLocation.ConclusiveLifeExpLBC == "0-1 Years")
            {
                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.ConclusiveLifeExpLBC + "</font></p>");
            }
            else if (visualbuildingLocation.ConclusiveLifeExpLBC == "4-7 Years" || visualbuildingLocation.ConclusiveLifeExpLBC == "7+ Years")
                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.ConclusiveLifeExpLBC + "</font></p>");
            else
                html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualbuildingLocation.ConclusiveLifeExpLBC + "</p>");


            if (visualbuildingLocation.ConclusiveLifeExpAWE == "0-1 Years")
            {
                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.ConclusiveLifeExpAWE + "</font></p>");
            }
            else if (visualbuildingLocation.ConclusiveLifeExpAWE == "4-7 Years" || visualbuildingLocation.ConclusiveLifeExpAWE == "7+ Years")
            {
                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.ConclusiveLifeExpAWE + "</font></p>");
            }
            else
                html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualbuildingLocation.ConclusiveLifeExpAWE + "</p>");


            html_Header = html_Header.Replace("{AdditonalConsiderations}", "<p>&nbsp;" + visualbuildingLocation.ConclusiveAdditionalConcerns.Replace("\n", "<br/>") + "</p>");

            string html_ConclusiveImages = System.IO.File.ReadAllText(path + "\\ConclusiveImages.html", iso);

            IEnumerable<VisualBuildingLocationPhoto> imageApartment = await VisualBuildingLocationPhotoDataStore.GetVisualBuildingLocationImageByVisualBuildingId(visualbuildingLocation.Id.ToString());
            imageApartment = imageApartment.Where(c => c.ImageDescription == "CONCLUSIVE").ToList();

            StringBuilder conclusive_Images_apartment = new StringBuilder();
            int apCount = 0;
            if (imageApartment.Count() <= factor)
            {

                conclusive_Images_apartment.Append(trWithStyle);
                foreach (var item in imageApartment)
                {
                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);

                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                    conclusive_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                }
                conclusive_Images_apartment.Append("</tr>");
            }
            else
            {
                conclusive_Images_apartment.Append(trWithStyle);
                foreach (var item in imageApartment)
                {

                    if ((apCount % factor) == 0)
                    {

                        conclusive_Images_apartment.Append("</tr>");
                        conclusive_Images_apartment.Append(trWithStyle);
                    }
                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                    conclusive_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                    apCount++;
                }
                conclusive_Images_apartment.Append("</tr>");
            }

            html_ConclusiveImages = html_ConclusiveImages.Replace("{ConclusiveImages}", conclusive_Images_apartment.ToString()).Replace("<tr></tr>", "");
            html_ConclusiveImages = html_ConclusiveImages.Replace("{imageCount}", factor.ToString());
            return html_Header + html_ConclusiveImages;
        }

        private async Task<string> getConclusiveHTMLContent(VisualBuildingApartment visualApt, long quality, int factor)
        {

            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Report\\";
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            string conclusiveHtml = System.IO.File.ReadAllText(path + "\\NoConclusive.html", iso);
            if (!visualApt.IsPostInvasiveRepairsRequired)
            {
                conclusiveHtml = conclusiveHtml.Replace("{Comments}", "Post Invasive repairs NOT required");
                return conclusiveHtml;
            }

            if (!visualApt.IsInvasiveRepairApproved)
            {
                conclusiveHtml = conclusiveHtml.Replace("{Comments}", "Post Invasive repairs required, Owner has NOT agreed for Conclusive report.");
                return conclusiveHtml;
            }

            if (!visualApt.IsInvasiveRepairComplete)
            {
                conclusiveHtml = conclusiveHtml.Replace("{Comments}", "Post Invasive repairs required, Conclusive analysis NOT Complete.");
                return conclusiveHtml;
            }


            string html_Header = System.IO.File.ReadAllText(path + "\\ConclusiveDetails.html", iso);

            html_Header = html_Header.Replace("{ConclusiveApproved}", visualApt.IsInvasiveRepairComplete.ToString() == "True" ? "Yes" : "No");


            if (visualApt.ConclusiveLifeExpEEE == "0-1 Years")
            {
                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualApt.ConclusiveLifeExpEEE + "</font></p>");
            }
            else if (visualApt.ConclusiveLifeExpEEE == "4-7 Years" || visualApt.ConclusiveLifeExpEEE == "7+ Years")
                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"green\">&nbsp;" + visualApt.ConclusiveLifeExpEEE + "</font></p>");
            else
                html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualApt.ConclusiveLifeExpEEE + "</p>");


            if (visualApt.ConclusiveLifeExpLBC == "0-1 Years")
            {
                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualApt.ConclusiveLifeExpLBC + "</font></p>");
            }
            else if (visualApt.ConclusiveLifeExpLBC == "4-7 Years" || visualApt.ConclusiveLifeExpLBC == "7+ Years")
                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"green\">&nbsp;" + visualApt.ConclusiveLifeExpLBC + "</font></p>");
            else
                html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualApt.ConclusiveLifeExpLBC + "</p>");


            if (visualApt.ConclusiveLifeExpAWE == "0-1 Years")
            {
                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualApt.ConclusiveLifeExpAWE + "</font></p>");
            }
            else if (visualApt.ConclusiveLifeExpAWE == "4-7 Years" || visualApt.ConclusiveLifeExpAWE == "7+ Years")
            {
                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"green\">&nbsp;" + visualApt.ConclusiveLifeExpAWE + "</font></p>");
            }
            else
                html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualApt.ConclusiveLifeExpAWE + "</p>");

            html_Header = html_Header.Replace("{AdditonalConsiderations}", "<p>&nbsp;" + visualApt.ConclusiveAdditionalConcerns.Replace("\n", "<br/>") + "</p>");

            string html_ConclusiveImages = System.IO.File.ReadAllText(path + "\\ConclusiveImages.html", iso);

            IEnumerable<VisualApartmentLocationPhoto> imageApartment = await VisualApartmentLocationPhotoDataStore.GetVisualBuildingApartmentImageByVisualApartmentId(visualApt.Id.ToString());
            imageApartment = imageApartment.Where(c => c.ImageDescription == "CONCLUSIVE").ToList();

            StringBuilder conclusive_Images_apartment = new StringBuilder();
            int apCount = 0;
            if (imageApartment.Count() <= factor)
            {

                conclusive_Images_apartment.Append(trWithStyle);
                foreach (var item in imageApartment)
                {
                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                    conclusive_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                }
                conclusive_Images_apartment.Append("</tr>");
            }
            else
            {
                conclusive_Images_apartment.Append(trWithStyle);
                foreach (var item in imageApartment)
                {

                    if ((apCount % factor) == 0)
                    {

                        conclusive_Images_apartment.Append("</tr>");
                        conclusive_Images_apartment.Append(trWithStyle);
                    }
                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                    conclusive_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                    apCount++;
                }
                conclusive_Images_apartment.Append("</tr>");
            }

            html_ConclusiveImages = html_ConclusiveImages.Replace("{ConclusiveImages}", conclusive_Images_apartment.ToString()).Replace("<tr></tr>", "");
            html_ConclusiveImages = html_ConclusiveImages.Replace("{imageCount}", factor.ToString());

            return html_Header + html_ConclusiveImages;
        }

        #endregion
        public async Task WordVisual(long quality, int height, int width, string projectID = "11D6DFDB-EF89-42E4-A127-7565CCE65DC0", string company = "DI", string Type = "Word")
        {
            Response result = await projectService.GetItemAsync(projectID);
            Project project = JsonConvert.DeserializeObject<Project>(result.Data.ToString());
            if (project.Category=="SingleLevel")
            {
               await WordVisualSingleLevel(quality, height, width, projectID, company, Type);
                return;
            }

            IsReportInProgress = System.Windows.Visibility.Visible;
            int factor = height;
            string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads\\";

            string fname = string.Empty;

            if (Directory.Exists(download+ "\\compressed"))
            {
                Directory.Delete(download + "\\compressed",true);
            }
            Directory.CreateDirectory(download + "\\compressed");
            NotificationUI("Report Creation Started.");

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Report\\";
            string html = System.IO.File.ReadAllText(path + "\\Invasive.html", iso);
            
            //if (result.Status == ApiResult.Success)
            //{
                
            //}
            StringBuilder mainHtml = new StringBuilder();
            //     StringBuilder buildingApartmentHtml = new StringBuilder();
            mainHtml.Append("<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset='utf-8' /> <title></title></head><body>");

            try
            {

                if (project != null)
                {
                    html = html.Replace("{projectName}", project.Name);
                    html = html.Replace("{projectAdd}", project.Address);
                    html = html.Replace("{projectDes}", project.Description);
                    html = html.Replace("{projectDate}", project.CreatedOn);
                    //get the Project Image
                    if (project.ImageUrl != null)
                    {

                        html = html.Replace("{projectImagePath}", project.ImageUrl);
                    }
                    //html = html.Replace("{projectCreated}", project.Username);
                    if (company == "DI")
                    {
                        html = html.Replace("{projectCreated}", "Deck Inspectors");
                    }
                    else
                        html = html.Replace("{projectCreated}", "WICR Waterproofing and Construction");

                    
                    IEnumerable<ProjectBuilding> buildingList = await ProjectBuildingDataStore.GetItemsAsyncByProjectID(projectID);
                    IEnumerable<ProjectLocation> ProjectLocationList = await projectLocationService.GetItemsAsyncByProjectID(projectID);
                    //string html_Header = string.Empty;
                    html = html.Replace("{heading}", "Visual Inspection Report");


                    mainHtml.Append(html);
                    if (buildingList.Count() != 0)//Check Building Exists
                    {

                        foreach (var building in buildingList)////open foreach building
                        {

                            // html_Header = string.Empty;
                            //APARTMENT

                            IEnumerable<BuildingApartment> buildingapartmentList = await BuildingApartmentDataStore.GetItemsAsyncByBuildingId(building.Id);
                            if (buildingapartmentList.Count() != 0)
                            {
                                foreach (var apartment in buildingapartmentList)////open building apartment list
                                {

                                    StatusMessage = "Fetching details for Apartments...";                                  
                                    IEnumerable<VisualBuildingApartment> visualBuildingApartment = await VisualFormApartmentDataStore.GetVisualBuildingApartmentByBuildingApartmentId(apartment.Id);
                                    if (visualBuildingApartment.Count() != 0)
                                    {
                                        foreach (var visualApt in visualBuildingApartment)
                                        {
                                            //Table_B_Apartment
                                            string html_Header = System.IO.File.ReadAllText(path + "\\" + "VisualDetail.html", iso);
                                            html_Header = html_Header.Replace("{BuildingName}", building.Name);
                                            html_Header = html_Header.Replace("{AptName}", apartment.Name);
                                            html_Header = html_Header.Replace("{LocName}", visualApt.Name);

                                            html_Header = html_Header.Replace("{TitleName}", "Building Name");
                                            html_Header = html_Header.Replace("{TitleApt}", "Apartment Name");
                                            html_Header = html_Header.Replace("{TitleLoc}", "Location Name");

                                            html_Header = html_Header.Replace("{ExteriorElements}", visualApt.ExteriorElements.Replace(",", ", ").Replace("Thershold", "Threshold"));
                                            html_Header = html_Header.Replace("{WaterProofingElements}", visualApt.WaterProofingElements.Replace(",", ", "));

                                            if (visualApt.VisualReview == "Bad")
                                            {
                                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"red\">&nbsp;" + visualApt.VisualReview + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"green\">&nbsp;" + visualApt.VisualReview + "</font></p>");
                                            if (visualApt.AnyVisualSign == "Yes")
                                            {
                                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"red\">&nbsp;" + visualApt.AnyVisualSign + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"green\">&nbsp;" + visualApt.AnyVisualSign + "</font></p>");

                                            if (visualApt.FurtherInasive == "Yes")
                                            {
                                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"red\">&nbsp;" + visualApt.FurtherInasive + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"green\">&nbsp;" + visualApt.FurtherInasive + "</font></p>");
                                            if (visualApt.ConditionAssessment == "Fail")
                                            {
                                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"red\">&nbsp;" + visualApt.ConditionAssessment + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"green\">&nbsp;" + visualApt.ConditionAssessment + "</font></p>");


                                            //html_Header = html_Header.Replace("{Additional}", visualApt.AdditionalConsideration);
                                            html_Header = html_Header.Replace("{Additional}", visualApt.AdditionalConsideration.Replace("\n", "<br/>"));
                                            if (visualApt.LifeExpectancyEEE == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualApt.LifeExpectancyEEE + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualApt.LifeExpectancyEEE + "</p>");

                                            if (visualApt.LifeExpectancyLBC == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualApt.LifeExpectancyLBC + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualApt.LifeExpectancyLBC + "</p>");

                                            if (visualApt.LifeExpectancyAWE == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualApt.LifeExpectancyAWE + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualApt.LifeExpectancyAWE + "</p>");



                                            html_Header = html_Header.Replace("{detailheading}", "Visual Inspection Details");
                                            mainHtml.Append(html_Header);

                                            string html_VisualImages = System.IO.File.ReadAllText(path + "\\" + "VisualImage.html", iso);


                                            IEnumerable<VisualApartmentLocationPhoto> imageApartment = await VisualApartmentLocationPhotoDataStore.GetVisualBuildingApartmentImageByVisualApartmentId(visualApt.Id.ToString());
                                            // imageApartment = imageApartment.Where(c => c.ImageDescription != "TRUE").ToList();

                                            StringBuilder visual_Images_apartment = new StringBuilder();
                                            int apCount = 0;
                                            if (imageApartment.Count() <= factor)
                                            {

                                                visual_Images_apartment.Append(trWithStyle);
                                                foreach (var item in imageApartment)
                                                {
                                                    
                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                                }
                                                visual_Images_apartment.Append("</tr>");
                                            }
                                            else
                                            {
                                                visual_Images_apartment.Append(trWithStyle);
                                                foreach (var item in imageApartment)
                                                {
                                                    if ((apCount % factor) == 0)
                                                    {

                                                        visual_Images_apartment.Append("</tr>");
                                                        visual_Images_apartment.Append(trWithStyle);
                                                    }
                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                                    apCount++;
                                                }
                                                visual_Images_apartment.Append("</tr>");
                                            }
                                            html_VisualImages = html_VisualImages.Replace("{VisualImages}", visual_Images_apartment.ToString()).Replace("<tr></tr>", "");
                                            html_VisualImages = html_VisualImages.Replace("{imageCount}", factor.ToString());
                                            mainHtml.Append(html_VisualImages);

                                        }

                                    }

                                }//close building apartment list
                            }

                        }
                        
                        //close foreach building
                        foreach (var building in buildingList)////open foreach building
                        {

                            IEnumerable<BuildingLocation> buildingLocationList = await BuildingLocationDataStore.GetItemsAsyncByBuildingId(building.Id);
                            if (buildingLocationList.Count() != 0)
                            {
                                foreach (var blocation in buildingLocationList)////open building apartment list
                                {

                                    StatusMessage = "Fetching details for Building Locations...";
                                    IEnumerable<VisualBuildingLocation> visualBuildingLocationList = await VisualFormBuildingLocationDataStore.GetVisualBuildingLocationByBuildingLocationId(blocation.Id);
                                    if (visualBuildingLocationList.Count() != 0)
                                    {
                                        foreach (var visualbuildingLocation in visualBuildingLocationList)
                                        {

                                            //Table_B_Apartment
                                            string html_Header = System.IO.File.ReadAllText(path + "\\" + "VisualDetail.html", iso);
                                            html_Header = html_Header.Replace("{BuildingName}", building.Name);
                                            html_Header = html_Header.Replace("{AptName}", blocation.Name);
                                            html_Header = html_Header.Replace("{LocName}", visualbuildingLocation.Name);

                                            html_Header = html_Header.Replace("{TitleName}", "Building Name");
                                            html_Header = html_Header.Replace("{TitleApt}", "Building Common Location Name");
                                            html_Header = html_Header.Replace("{TitleLoc}", "Location Name");

                                            html_Header = html_Header.Replace("{ExteriorElements}", visualbuildingLocation.ExteriorElements.Replace(",", ", ").Replace("Thershold", "Threshold"));
                                            html_Header = html_Header.Replace("{WaterProofingElements}", visualbuildingLocation.WaterProofingElements.Replace(",", ", "));

                                            if (visualbuildingLocation.VisualReview == "Bad")
                                            {
                                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.VisualReview + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.VisualReview + "</font></p>");
                                            if (visualbuildingLocation.AnyVisualSign == "Yes")
                                            {
                                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.AnyVisualSign + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.AnyVisualSign + "</font></p>");

                                            if (visualbuildingLocation.FurtherInasive == "Yes")
                                            {
                                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.FurtherInasive + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.FurtherInasive + "</font></p>");
                                            if (visualbuildingLocation.ConditionAssessment == "Fail")
                                            {
                                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.ConditionAssessment + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"green\">&nbsp;" + visualbuildingLocation.ConditionAssessment + "</font></p>");

                                            //html_Header = html_Header.Replace("{Additional}", visualbuildingLocation.AdditionalConsideration);
                                            html_Header = html_Header.Replace("{Additional}", visualbuildingLocation.AdditionalConsideration.Replace("\n", "<br/>"));
                                            if (visualbuildingLocation.LifeExpectancyEEE == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.LifeExpectancyEEE + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualbuildingLocation.LifeExpectancyEEE + "</p>");

                                            if (visualbuildingLocation.LifeExpectancyLBC == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.LifeExpectancyLBC + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualbuildingLocation.LifeExpectancyLBC + "</p>");

                                            if (visualbuildingLocation.LifeExpectancyAWE == "0-1 Years")
                                            {
                                                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualbuildingLocation.LifeExpectancyAWE + "</font></p>");
                                            }
                                            else
                                                html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualbuildingLocation.LifeExpectancyAWE + "</p>");


                                            html_Header = html_Header.Replace("{detailheading}", "Visual Inspection Details");
                                            mainHtml.Append(html_Header);



                                            IEnumerable<VisualBuildingLocationPhoto> imageApartment = await VisualBuildingLocationPhotoDataStore.GetVisualBuildingLocationImageByVisualBuildingId(visualbuildingLocation.Id.ToString());
                                            //  imageApartment = imageApartment.Where(c => c.ImageDescription != "TRUE").ToList();

                                            string html_VisualImages = System.IO.File.ReadAllText(path + "\\" + "VisualImage.html", iso);

                                            StringBuilder visual_Images_apartment = new StringBuilder();
                                            int apCount = 0;
                                            if (imageApartment.Count() <= factor)
                                            {

                                                visual_Images_apartment.Append(trWithStyle);
                                                foreach (var item in imageApartment)
                                                {
                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                                }
                                                visual_Images_apartment.Append("</tr>");
                                            }
                                            else
                                            {
                                                visual_Images_apartment.Append(trWithStyle);
                                                foreach (var item in imageApartment)
                                                {

                                                    if ((apCount % factor) == 0)
                                                    {

                                                        visual_Images_apartment.Append("</tr>");
                                                        visual_Images_apartment.Append("<tr>");
                                                    }
                                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                                    apCount++;
                                                }
                                                visual_Images_apartment.Append("</tr>");
                                            }
                                            html_VisualImages = html_VisualImages.Replace("{VisualImages}", visual_Images_apartment.ToString()).Replace("<tr></tr>", "");
                                            html_VisualImages = html_VisualImages.Replace("{imageCount}", factor.ToString());
                                            mainHtml.Append(html_VisualImages);

                                        }

                                    }

                                }//close building apartment list
                            }
                        }
                       
                    }
                   
                    if (ProjectLocationList.Count() != 0)
                    {
                        foreach (var projectlocation in ProjectLocationList)////open building apartment list
                        {

                            StatusMessage = "Fetching details for Project Locations...";
                            IEnumerable<VisualProjectLocation> visualprojectLocation = await VisualProjectLocationService.GetItemsAsyncByVisualProjectLocationId(projectlocation.Id);
                            if (visualprojectLocation.Count() != 0)
                            {
                                foreach (var visualPloc in visualprojectLocation)
                                {
                                    string html_Header = System.IO.File.ReadAllText(path + "\\" + "VisualDeatilForProjectLocation.html", iso);
                                    html_Header = html_Header.Replace("{BuildingName}", projectlocation.Name);
                                    html_Header = html_Header.Replace("{AptName}", projectlocation.Name);
                                    html_Header = html_Header.Replace("{LocName}", visualPloc.Name);

                                    // html_Header = html_Header.Replace("{TitleName}", "Project Common Location Name");
                                    html_Header = html_Header.Replace("{TitleApt}", "Project Common Location Name");
                                    html_Header = html_Header.Replace("{TitleLoc}", "Location Name");

                                    html_Header = html_Header.Replace("{ExteriorElements}", visualPloc.ExteriorElements.Replace(",", ", ").Replace("Thershold", "Threshold"));
                                    html_Header = html_Header.Replace("{WaterProofingElements}", visualPloc.WaterProofingElements.Replace(",", ", "));

                                    if (visualPloc.VisualReview == "Bad")
                                    {
                                        html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"red\">&nbsp;" + visualPloc.VisualReview + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"green\">&nbsp;" + visualPloc.VisualReview + "</font></p>");
                                    if (visualPloc.AnyVisualSign == "Yes")
                                    {
                                        html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"red\">&nbsp;" + visualPloc.AnyVisualSign + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"green\">&nbsp;" + visualPloc.AnyVisualSign + "</font></p>");

                                    if (visualPloc.FurtherInasive == "Yes")
                                    {
                                        html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"red\">&nbsp;" + visualPloc.FurtherInasive + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"green\">&nbsp;" + visualPloc.FurtherInasive + "</font></p>");
                                    if (visualPloc.ConditionAssessment == "Fail")
                                    {
                                        html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"red\">&nbsp;" + visualPloc.ConditionAssessment + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"green\">&nbsp;" + visualPloc.ConditionAssessment + "</font></p>");


                                    // html_Header = html_Header.Replace("{Additional}", visualPloc.AdditionalConsideration);
                                    html_Header = html_Header.Replace("{Additional}", visualPloc.AdditionalConsideration.Replace("\n", "<br/>"));
                                    if (visualPloc.LifeExpectancyEEE == "0-1 Years")
                                    {
                                        html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyEEE + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualPloc.LifeExpectancyEEE + "</p>");

                                    if (visualPloc.LifeExpectancyLBC == "0-1 Years")
                                    {
                                        html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyLBC + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualPloc.LifeExpectancyLBC + "</p>");

                                    if (visualPloc.LifeExpectancyAWE == "0-1 Years")
                                    {
                                        html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyAWE + "</font></p>");
                                    }
                                    else
                                        html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualPloc.LifeExpectancyAWE + "</p>");

                                    mainHtml.Append(html_Header);




                                    //  List<VisualProjectLocationImage> imageBLocation = ImgprojectLocation.GetVisualProjectLocationImageByVisualLocationId(visualPloc.Id);
                                    IEnumerable<VisualProjectLocationPhoto> imageApartment = await VisualProjectLocationPhotoDataStore.GetItemsAsyncByVisualProjectLocationId(visualPloc.Id.ToString());
                                    imageApartment = imageApartment.Where(c => c.ImageDescription != "TRUE").ToList();
                                    string html_VisualImages = System.IO.File.ReadAllText(path + "\\" + "VisualImage.html", iso);

                                    StringBuilder visual_Images_apartment = new StringBuilder();
                                    int apCount = 0;
                                    if (imageApartment.Count() <= factor)
                                    {

                                        visual_Images_apartment.Append(trWithStyle);
                                        foreach (var item in imageApartment)
                                        {
                                            string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                            var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                            visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                        }
                                        visual_Images_apartment.Append("</tr>");
                                    }
                                    else
                                    {
                                        visual_Images_apartment.Append(trWithStyle);
                                        foreach (var item in imageApartment)
                                        {

                                            if ((apCount % factor) == 0)
                                            {

                                                visual_Images_apartment.Append("</tr>");
                                                visual_Images_apartment.Append("<tr>");
                                            }
                                            string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                            var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                            visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                            apCount++;
                                        }
                                        visual_Images_apartment.Append("</tr>");
                                    }
                                    html_VisualImages = html_VisualImages.Replace("{VisualImages}", visual_Images_apartment.ToString()).Replace("<tr></tr>", "");
                                    html_VisualImages = html_VisualImages.Replace("{imageCount}", factor.ToString());
                                    mainHtml.Append(html_VisualImages);
                                   

                                }

                            }

                        }
                    }

                   

                }

                mainHtml.Append("</body></html>");
                //to save html file.
                var projPath = path + "\\" + "ProjectReport.html";
                using (StreamWriter sr = File.CreateText(projPath))
                {
                    sr.Write(mainHtml.ToString());
                    sr.Close();

                }
                StatusMessage = "Creating the document...";
                using (Stream fileStreamPath = GenerateStreamFromString(mainHtml.ToString()))
                {

                    WordDocument document = new WordDocument();
                    //document.HTMLImportSettings.ImageNodeVisited += OpenImage;
                    document.Open(projPath, FormatType.Html);
                    //document.HTMLImportSettings.ImageNodeVisited -= OpenImage;
                    var destPath = "report123.docx";

                    //save document locally.
                    //document.Save(destPath);
                    if (company == "WICR")
                    {
                        foreach (WSection section in document.Sections)
                        {

                            // Add a new paragraph for header to the document.
                            //IWParagraph headerPar = new WParagraph(document);

                            // Add a new table to the header
                            IWParagraph headerPar = section.HeadersFooters.Header.AddParagraph();

                            RowFormat format = new RowFormat();
                            format.HorizontalAlignment = RowAlignment.Center;
                            // Setting Single table border style.
                            format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;

                            // Inserting table with a row and two columns.
                            // table.ResetCells(1, 2, format, 200);

                            // Inserting logo image to the table first cell.
                            //headerPar = table[0, 0].AddParagraph() as WParagraph;

                            // string s = ResolveApplicationDataPath("Northwind_logo.png", "Images\\DocIO");
                            string s = path + "wicrlogo.jpg";

                            headerPar.AppendPicture(System.Drawing.Image.FromFile(s));
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            //Set Image size.
                            (headerPar.Items[0] as WPicture).Width = 132.5f;
                            (headerPar.Items[0] as WPicture).Height = 44.75f;
                            (headerPar.Items[0] as WPicture).HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            // Inserting text to the table second cell.
                            WParagraph headerPar1 = new WParagraph(document);
                            IWTextRange txt = headerPar1.AppendText("State Contractor’s License No.: 745936\nVisit our website: www.WICR.net \n888-388-9427");
                            txt.CharacterFormat.FontSize = 6;
                            txt.CharacterFormat.Bold = false;
                            txt.CharacterFormat.FontName = "Arial Narrow";
                            txt.CharacterFormat.TextColor = System.Drawing.ColorTranslator.FromHtml("#8EAADB");
                            txt.CharacterFormat.CharacterSpacing = 1.4f;
                            headerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;
                            section.HeadersFooters.Header.Paragraphs.Add(headerPar1);
                            // Add a footer paragraph text to the document.
                            WParagraph footerPar = new WParagraph(document);
                            // footerPar.ParagraphFormat.Tabs.AddTab(523f, TabJustification.Centered, TabLeader.NoLeader);
                            footerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            // Add text.

                            IWTextRange firstText = footerPar.AppendText("www.WICR.net");
                            firstText.CharacterFormat.TextColor = Color.BlueViolet;
                            // Add page and Number of pages field to the document.
                            WParagraph footerPar1 = new WParagraph(document);
                            footerPar1.AppendText("\tPage ");
                            footerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;

                            IWField ff = footerPar1.AppendField("Page", FieldType.FieldPage);

                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar);
                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar1);

                            #region Page Number Settings
                            section.PageSetup.RestartPageNumbering = true;
                            section.PageSetup.PageStartingNumber = 1;
                            section.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;
                            #endregion Page Number Settings

                        }
                    }
                    else
                    {
                        foreach (WSection section in document.Sections)
                        {

                            // Add a new paragraph for header to the document.
                            //IWParagraph headerPar = new WParagraph(document);

                            // Add a new table to the header
                            // IWTable table = section.HeadersFooters.Header.AddTable();
                            IWParagraph headerPar = section.HeadersFooters.Header.AddParagraph();
                            RowFormat format = new RowFormat();
                            format.HorizontalAlignment = RowAlignment.Center;
                            // Setting Single table border style.
                            format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;

                            // Inserting table with a row and two columns.
                            //table.ResetCells(1, 1, format, 200);

                            // Inserting logo image to the table first cell.
                            //headerPar = table[0, 0].AddParagraph() as WParagraph;
                            // headerPar.ApplyStyle(BuiltinStyle.DefaultParagraphFont.)
                            // string s = ResolveApplicationDataPath("Northwind_logo.png", "Images\\DocIO");
                            string s = path + "decklogo.jpg";


                            headerPar.AppendPicture(System.Drawing.Image.FromFile(s));
                            //Set Image size.
                            (headerPar.Items[0] as WPicture).Width = 132.5f;
                            (headerPar.Items[0] as WPicture).Height = 44.75f;
                            (headerPar.Items[0] as WPicture).HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            // Inserting text to the table second cell.
                            // headerPar = table[0, 0].AddParagraph() as WParagraph;
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                            // Add a footer paragraph text to the document.
                            WParagraph footerPar = new WParagraph(document);

                            //footerPar.ParagraphFormat.Tabs.AddTab(523f, TabJustification.Centered, TabLeader.NoLeader);
                            // Add text.
                            footerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                            IWTextRange firstText = footerPar.AppendText("WWW.DECKINSPECTORS.COM");
                            firstText.CharacterFormat.TextColor = Color.Orange;
                            // Add page and Number of pages field to the document.
                            WParagraph footerPar1 = new WParagraph(document);
                            footerPar1.AppendText("\tPage ");
                            footerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;

                            IWField ff = footerPar1.AppendField("Page", FieldType.FieldPage);

                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar);
                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar1);
                            #region Page Number Settings
                            section.PageSetup.RestartPageNumbering = true;
                            section.PageSetup.PageStartingNumber = 1;
                            section.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;

                            #endregion Page Number Settings

                        }
                    }


                    
                    if (Type == "Word")
                    {

                        if (company == "DI")
                        {
                            fname = "Deck_Visual_" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".docx";
                        }
                        else
                        {
                            fname = "WICR_Visual_" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".docx";
                        }

                        document.Save(download +fname, FormatType.Docx);


                    }
                    else
                    {
                        if (company == "DI")
                        {
                            fname = "Deck_Visual_" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".pdf";
                        }
                        else
                        {
                            fname = "WICR_Visual_" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".pdf";
                        }
                        
                        DocToPDFConverter converter = new DocToPDFConverter();
                        PdfDocument pdfDocument = converter.ConvertToPDF(document);
                        converter.Dispose();

                        pdfDocument.Save(download + fname);
                        pdfDocument.Close();
                    }
                    document.Close();
                }
                //update the processing status
                NotificationUI("Report Created Successfully, please navigate to Downlaods folder to access it.");
                Process.Start(download + fname);
            }
            
            catch (Exception ex)
            {
                NotificationUI("Report creation failed, please retry." + ex.Message);
            }

            IsReportInProgress = System.Windows.Visibility.Collapsed;
        }

        public void NotificationUI(string message)
        {
            ShowDialog(message);
        }

        private void OpenImage(object sender, ImageNodeVisitedEventArgs args)
        {
            //Read the image from the specified (args.Uri) path
            args.ImageStream = System.IO.File.OpenRead(args.Uri);
        }
        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        string trWithStyle = "<tr>";
        private string trWithStyles()
        {
            return "<tr style=\"font-size: 12px; border: 1px solid #E3F1D5;\">";
        }
        private string getTdWithStyle(int factor)
        {
            string strWid = (100 / factor).ToString();
            return "<td style = \"width: " + strWid + "%; text-align: center;border: thin;\">";
        }

        private string getimgWithStyle(int factor, string fPath)
        {
            string strWid = (750 / factor).ToString();
            string strHeight = Math.Round(750 / factor * 1.25, 0).ToString();
            return "<img style = \"display: block; width:" + strWid + "px ;height:" + strHeight + "px; margin: 0px;\" src = \"" + fPath + "\" /></td>";
        }


        #endregion

        #region ReportStatus
        private string _statusMessage = "Generating Report...";
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (value == _statusMessage) return;
                _statusMessage = value;
                OnPropertyChanged("StatusMessage");
            }
        }
        private System.Windows.Visibility _isReportInProgress= System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility IsReportInProgress
        {
            get => _isReportInProgress;
            set
            {
                if (value == _isReportInProgress) return;
                _isReportInProgress = value;
                OnPropertyChanged("IsReportInProgress");
            }
        }
        private int _statusProgress;
        public int StatusProgress
        {
            get => _statusProgress;
            set
            {
                if (value == _statusProgress) return;
                _statusProgress = value;
                OnPropertyChanged("StatusProgress");
            }
        }

        #endregion

        #region ImageCompressionForDoc
        public string CompressImage(string SourcePathURL,string imgName, long quality)
        {

            string directoryFullPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads\\compressed\\";
            
            string DestPath = directoryFullPath + imgName;
            try
            {
                //DestPath = DestPath.Replace(".png", ".jpeg");
                
                Stream imgStream = GetStreamFromUrl(SourcePathURL);
                
                Image bmp1 = new Bitmap(imgStream);
                FixImageOrientation(bmp1);

                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;

                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, quality);


                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(DestPath, jpgEncoder, myEncoderParameters);
            }
            catch(Exception ex)
            {
                DestPath = SourcePathURL;
            }
            return DestPath;
        }
        private Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new System.Net.WebClient())
                imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private Image FixImageOrientation(Image image)
        {
            const int exifOrientationId = 0x112;
            if (!image.PropertyIdList.Contains(exifOrientationId))
                return image;
            //Gets the specified property item from the image
            var property = image.GetPropertyItem(exifOrientationId);
            var orient = BitConverter.ToInt16(property.Value, 0);
            //Get the rotated or flipped image 
            image = RotateImageSrc(orient, image);
            return image;
        }

        private Image RotateImageSrc(int orient, Image image)
        {
            switch (orient)
            {
                case 1:
                    image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    return image;
                case 2:
                    image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    return image;
                case 3:
                    image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return image;
                case 4:
                    image.RotateFlip(RotateFlipType.Rotate180FlipX);
                    return image;
                case 5:
                    image.RotateFlip(RotateFlipType.Rotate90FlipX);
                    return image;
                case 6:
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return image;
                case 7:
                    image.RotateFlip(RotateFlipType.Rotate270FlipX);
                    return image;
                case 8:
                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    return image;
                default:
                    image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    return image;
            }
        }
        #endregion

        #region singleLevelreporting
        public async Task WordVisualSingleLevel(long quality, int height, int width, string projectID = "11D6DFDB-EF89-42E4-A127-7565CCE65DC0", string company = "DI", string Type = "Word")
        {

            IsReportInProgress = System.Windows.Visibility.Visible;
            int factor = height;
            string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads\\";

            string fname = string.Empty;

            if (Directory.Exists(download + "\\compressed"))
            {
                Directory.Delete(download + "\\compressed", true);
            }
            Directory.CreateDirectory(download + "\\compressed");
            NotificationUI("Report Creation Started.");

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Report\\";
            string html = System.IO.File.ReadAllText(path + "\\Invasive.html", iso);
            Response result = await projectService.GetItemAsync(projectID);
            //if (result.Status == ApiResult.Success)
            //{
            Project project = JsonConvert.DeserializeObject<Project>(result.Data.ToString());
            //}
            StringBuilder mainHtml = new StringBuilder();
            //     StringBuilder buildingApartmentHtml = new StringBuilder();
            mainHtml.Append("<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset='utf-8' /> <title></title></head><body>");

            try
            {

                if (project != null)
                {
                    html = html.Replace("{projectName}", project.Name);
                    html = html.Replace("{projectAdd}", project.Address);
                    html = html.Replace("{projectDes}", project.Description);
                    html = html.Replace("{projectDate}", project.CreatedOn);
                    //get the Project Image
                    if (project.ImageUrl != null)
                    {

                        html = html.Replace("{projectImagePath}", project.ImageUrl);
                    }
                    //html = html.Replace("{projectCreated}", project.Username);
                    if (company == "DI")
                    {
                        html = html.Replace("{projectCreated}", "Deck Inspectors");
                    }
                    else
                        html = html.Replace("{projectCreated}", "WICR Waterproofing and Construction");


                    html = html.Replace("{heading}", "Visual Inspection Report");


                    mainHtml.Append(html);
                   

                    StatusMessage = "Fetching details for Project Locations...";
                    IEnumerable<VisualProjectLocation> visualprojectLocation = await VisualProjectLocationService.GetItemsAsyncByVisualProjectLocationId(project.Id);
                    if (visualprojectLocation.Count() != 0)
                    {
                        foreach (var visualPloc in visualprojectLocation)
                        {
                            string html_Header = System.IO.File.ReadAllText(path + "\\" + "VisualDetailSingleLevelProject.html", iso);
                            //html_Header = html_Header.Replace("{BuildingName}", projectlocation.Name);
                            //html_Header = html_Header.Replace("{AptName}", projectlocation.Name);
                            html_Header = html_Header.Replace("{LocName}", visualPloc.Name);

                            // html_Header = html_Header.Replace("{TitleName}", "Project Common Location Name");
                            html_Header = html_Header.Replace("{TitleApt}", "Project Common Location Name");
                            html_Header = html_Header.Replace("{TitleLoc}", "Location Name");

                            html_Header = html_Header.Replace("{ExteriorElements}", visualPloc.ExteriorElements.Replace(",", ", ").Replace("Thershold", "Threshold"));
                            html_Header = html_Header.Replace("{WaterProofingElements}", visualPloc.WaterProofingElements.Replace(",", ", "));

                            if (visualPloc.VisualReview == "Bad")
                            {
                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"red\">&nbsp;" + visualPloc.VisualReview + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"green\">&nbsp;" + visualPloc.VisualReview + "</font></p>");
                            if (visualPloc.AnyVisualSign == "Yes")
                            {
                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"red\">&nbsp;" + visualPloc.AnyVisualSign + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"green\">&nbsp;" + visualPloc.AnyVisualSign + "</font></p>");

                            if (visualPloc.FurtherInasive == "Yes")
                            {
                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"red\">&nbsp;" + visualPloc.FurtherInasive + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"green\">&nbsp;" + visualPloc.FurtherInasive + "</font></p>");
                            if (visualPloc.ConditionAssessment == "Fail")
                            {
                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"red\">&nbsp;" + visualPloc.ConditionAssessment + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"green\">&nbsp;" + visualPloc.ConditionAssessment + "</font></p>");


                            // html_Header = html_Header.Replace("{Additional}", visualPloc.AdditionalConsideration);
                            html_Header = html_Header.Replace("{Additional}", visualPloc.AdditionalConsideration.Replace("\n", "<br/>"));
                            if (visualPloc.LifeExpectancyEEE == "0-1 Years")
                            {
                                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyEEE + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualPloc.LifeExpectancyEEE + "</p>");

                            if (visualPloc.LifeExpectancyLBC == "0-1 Years")
                            {
                                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyLBC + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualPloc.LifeExpectancyLBC + "</p>");

                            if (visualPloc.LifeExpectancyAWE == "0-1 Years")
                            {
                                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyAWE + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualPloc.LifeExpectancyAWE + "</p>");

                            mainHtml.Append(html_Header);


                            IEnumerable<VisualProjectLocationPhoto> imageApartment = await VisualProjectLocationPhotoDataStore.GetItemsAsyncByVisualProjectLocationId(visualPloc.Id.ToString());
                            imageApartment = imageApartment.Where(c => c.ImageDescription != "TRUE").ToList();
                            string html_VisualImages = System.IO.File.ReadAllText(path + "\\" + "VisualImage.html", iso);

                            StringBuilder visual_Images_apartment = new StringBuilder();
                            int apCount = 0;
                            if (imageApartment.Count() <= factor)
                            {

                                visual_Images_apartment.Append(trWithStyle);
                                foreach (var item in imageApartment)
                                {
                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                }
                                visual_Images_apartment.Append("</tr>");
                            }
                            else
                            {
                                visual_Images_apartment.Append(trWithStyle);
                                foreach (var item in imageApartment)
                                {

                                    if ((apCount % factor) == 0)
                                    {

                                        visual_Images_apartment.Append("</tr>");
                                        visual_Images_apartment.Append("<tr>");
                                    }
                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                    apCount++;
                                }
                                visual_Images_apartment.Append("</tr>");
                            }
                            html_VisualImages = html_VisualImages.Replace("{VisualImages}", visual_Images_apartment.ToString()).Replace("<tr></tr>", "");
                            html_VisualImages = html_VisualImages.Replace("{imageCount}", factor.ToString());
                            mainHtml.Append(html_VisualImages);


                        }

                    }




                }

                mainHtml.Append("</body></html>");
                //to save html file.
                var projPath = path + "\\" + "ProjectReport.html";
                using (StreamWriter sr = File.CreateText(projPath))
                {
                    sr.Write(mainHtml.ToString());
                    sr.Close();

                }
                StatusMessage = "Creating the document...";
                using (Stream fileStreamPath = GenerateStreamFromString(mainHtml.ToString()))
                {

                    WordDocument document = new WordDocument();
                    
                    document.Open(projPath, FormatType.Html);
                    
                   
                    //save document locally.
                    //document.Save(destPath);
                    if (company == "WICR")
                    {
                        foreach (WSection section in document.Sections)
                        {

                            // Add a new paragraph for header to the document.
                            //IWParagraph headerPar = new WParagraph(document);

                            // Add a new table to the header
                            IWParagraph headerPar = section.HeadersFooters.Header.AddParagraph();

                            RowFormat format = new RowFormat();
                            format.HorizontalAlignment = RowAlignment.Center;
                            // Setting Single table border style.
                            format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;

                            string s = path + "wicrlogo.jpg";

                            headerPar.AppendPicture(System.Drawing.Image.FromFile(s));
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            //Set Image size.
                            (headerPar.Items[0] as WPicture).Width = 132.5f;
                            (headerPar.Items[0] as WPicture).Height = 44.75f;
                            (headerPar.Items[0] as WPicture).HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            // Inserting text to the table second cell.
                            WParagraph headerPar1 = new WParagraph(document);
                            IWTextRange txt = headerPar1.AppendText("State Contractor’s License No.: 745936\nVisit our website: www.WICR.net \n888-388-9427");
                            txt.CharacterFormat.FontSize = 6;
                            txt.CharacterFormat.Bold = false;
                            txt.CharacterFormat.FontName = "Arial Narrow";
                            txt.CharacterFormat.TextColor = System.Drawing.ColorTranslator.FromHtml("#8EAADB");
                            txt.CharacterFormat.CharacterSpacing = 1.4f;
                            headerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;
                            section.HeadersFooters.Header.Paragraphs.Add(headerPar1);
                            // Add a footer paragraph text to the document.
                            WParagraph footerPar = new WParagraph(document);
                            // footerPar.ParagraphFormat.Tabs.AddTab(523f, TabJustification.Centered, TabLeader.NoLeader);
                            footerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            // Add text.

                            IWTextRange firstText = footerPar.AppendText("www.WICR.net");
                            firstText.CharacterFormat.TextColor = Color.BlueViolet;
                            // Add page and Number of pages field to the document.
                            WParagraph footerPar1 = new WParagraph(document);
                            footerPar1.AppendText("\tPage ");
                            footerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;

                            IWField ff = footerPar1.AppendField("Page", FieldType.FieldPage);

                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar);
                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar1);

                            #region Page Number Settings
                            section.PageSetup.RestartPageNumbering = true;
                            section.PageSetup.PageStartingNumber = 1;
                            section.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;
                            #endregion Page Number Settings

                        }
                    }
                    else
                    {
                        foreach (WSection section in document.Sections)
                        {

                            // Add a new paragraph for header to the document.
                            //IWParagraph headerPar = new WParagraph(document);

                            // Add a new table to the header
                            // IWTable table = section.HeadersFooters.Header.AddTable();
                            IWParagraph headerPar = section.HeadersFooters.Header.AddParagraph();
                            RowFormat format = new RowFormat();
                            format.HorizontalAlignment = RowAlignment.Center;
                            // Setting Single table border style.
                            format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;

                            // Inserting table with a row and two columns.
                            //table.ResetCells(1, 1, format, 200);

                            // Inserting logo image to the table first cell.
                            //headerPar = table[0, 0].AddParagraph() as WParagraph;
                            // headerPar.ApplyStyle(BuiltinStyle.DefaultParagraphFont.)
                            // string s = ResolveApplicationDataPath("Northwind_logo.png", "Images\\DocIO");
                            string s = path + "decklogo.jpg";


                            headerPar.AppendPicture(System.Drawing.Image.FromFile(s));
                            //Set Image size.
                            (headerPar.Items[0] as WPicture).Width = 132.5f;
                            (headerPar.Items[0] as WPicture).Height = 44.75f;
                            (headerPar.Items[0] as WPicture).HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            // Inserting text to the table second cell.
                            // headerPar = table[0, 0].AddParagraph() as WParagraph;
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                            // Add a footer paragraph text to the document.
                            WParagraph footerPar = new WParagraph(document);

                            //footerPar.ParagraphFormat.Tabs.AddTab(523f, TabJustification.Centered, TabLeader.NoLeader);
                            // Add text.
                            footerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                            IWTextRange firstText = footerPar.AppendText("WWW.DECKINSPECTORS.COM");
                            firstText.CharacterFormat.TextColor = Color.Orange;
                            // Add page and Number of pages field to the document.
                            WParagraph footerPar1 = new WParagraph(document);
                            footerPar1.AppendText("\tPage ");
                            footerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;

                            IWField ff = footerPar1.AppendField("Page", FieldType.FieldPage);

                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar);
                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar1);
                            #region Page Number Settings
                            section.PageSetup.RestartPageNumbering = true;
                            section.PageSetup.PageStartingNumber = 1;
                            section.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;

                            #endregion Page Number Settings

                        }
                    }



                    if (Type == "Word")
                    {

                        if (company == "DI")
                        {
                            fname = "Deck_Visual_" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".docx";
                        }
                        else
                        {
                            fname = "WICR_Visual_" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".docx";
                        }

                        document.Save(download + fname, FormatType.Docx);


                    }
                    else
                    {
                        if (company == "DI")
                        {
                            fname = "Deck_Visual_" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".pdf";
                        }
                        else
                        {
                            fname = "WICR_Visual_" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".pdf";
                        }

                        DocToPDFConverter converter = new DocToPDFConverter();
                        PdfDocument pdfDocument = converter.ConvertToPDF(document);
                        converter.Dispose();

                        pdfDocument.Save(download + fname);
                        pdfDocument.Close();
                    }
                    document.Close();
                }
                //update the processing status
                NotificationUI("Report Created Successfully, please navigate to Downlaods folder to access it.");
                Process.Start(download + fname);
            }

            catch (Exception ex)
            {
                NotificationUI("Report creation failed, please retry." + ex.Message);
            }

            IsReportInProgress = System.Windows.Visibility.Collapsed;
        }

        public async Task WordInvasiveSingleLevel(long quality, int height, int width, string projectID = "11D6DFDB-EF89-42E4-A127-7565CCE65DC0", string company = "DI", string Type = "Word")
        {
            int factor = height;
            IsReportInProgress = System.Windows.Visibility.Visible;
            string fname = string.Empty;
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads\\";
            if (Directory.Exists(download + "\\compressed"))
            {
                Directory.Delete(download + "\\compressed", true);
            }
            Directory.CreateDirectory(download + "\\compressed");
            NotificationUI("Report Creation Started.");
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Report\\";
            string html = System.IO.File.ReadAllText(path + "Invasive.html", iso);

            try
            {


                Response result = await projectService.GetItemAsync(projectID);
                //if (result.Status == ApiResult.Success)
                //{
                Project project = JsonConvert.DeserializeObject<Project>(result.Data.ToString());
                //}
                StringBuilder mainHtml = new StringBuilder();

                mainHtml.Append("<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset='utf-8'/> <title></title>" +

                    "</head><body>");
                if (project != null)
                {
                    html = html.Replace("{projectName}", project.Name);
                    html = html.Replace("{projectAdd}", project.Address);
                    html = html.Replace("{projectDes}", project.Description);
                    html = html.Replace("{projectDate}", project.CreatedOn);

                    //get the Project Image
                    if (project.ImageUrl != null)
                    {

                        html = html.Replace("{projectImagePath}", project.ImageUrl);
                    }



                    if (company == "DI")
                    {
                        html = html.Replace("{projectCreated}", "Deck Inspectors");
                    }
                    else
                        html = html.Replace("{projectCreated}", "WICR Waterproofing and Construction");
                   
                    html = html.Replace("{heading}", "Invasive Inspection Report");

                    mainHtml.Append(html);
                    
                    
                    IEnumerable<VisualProjectLocation> visualprojectLocation = await VisualProjectLocationService.GetItemsAsyncByVisualProjectLocationId(project.Id);
                    if (visualprojectLocation.Count() != 0)
                    {
                        foreach (var visualPloc in visualprojectLocation)
                        {
                            string html_Header = System.IO.File.ReadAllText(path + "\\VisualDetailSingleLevelProject.html", iso);
                            //html_Header = html_Header.Replace("{BuildingName}", projectlocation.Name);
                            //html_Header = html_Header.Replace("{AptName}", projectlocation.Name);
                            html_Header = html_Header.Replace("{LocName}", visualPloc.Name);

                            // html_Header = html_Header.Replace("{TitleName}", "Project Common Location Name");
                            html_Header = html_Header.Replace("{TitleApt}", "Project Common Location Name");
                            html_Header = html_Header.Replace("{TitleLoc}", "Location Name");

                            html_Header = html_Header.Replace("{ExteriorElements}", visualPloc.ExteriorElements.Replace(",", ", ").Replace("Thershold", "Threshold"));
                            html_Header = html_Header.Replace("{WaterProofingElements}", visualPloc.WaterProofingElements.Replace(",", ", "));

                            if (visualPloc.VisualReview == "Bad")
                            {
                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"red\">&nbsp;" + visualPloc.VisualReview + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{VisualReview}", "<p><font color=\"green\">&nbsp;" + visualPloc.VisualReview + "</font></p>");
                            if (visualPloc.AnyVisualSign == "Yes")
                            {
                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"red\">&nbsp;" + visualPloc.AnyVisualSign + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{AnyVisualSignsofleaks}", "<p><font color=\"green\">&nbsp;" + visualPloc.AnyVisualSign + "</font></p>");

                            if (visualPloc.FurtherInasive == "Yes")
                            {
                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"red\">&nbsp;" + visualPloc.FurtherInasive + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{FurtherInvasive}", "<p><font color=\"green\">&nbsp;" + visualPloc.FurtherInasive + "</font></p>");
                            if (visualPloc.ConditionAssessment == "Fail")
                            {
                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"red\">&nbsp;" + visualPloc.ConditionAssessment + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{ConditionAssessment}", "<p><font color=\"green\">&nbsp;" + visualPloc.ConditionAssessment + "</font></p>");

                            //html_Header = html_Header.Replace("{Additional}", visualPloc.AdditionalConsideration);
                            html_Header = html_Header.Replace("{Additional}", visualPloc.AdditionalConsideration.Replace("\n", "<br/>"));

                            if (visualPloc.LifeExpectancyEEE == "0-1 Years")
                            {
                                html_Header = html_Header.Replace("{EEE}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyEEE + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{EEE}", "<p>&nbsp;" + visualPloc.LifeExpectancyEEE + "</p>");

                            if (visualPloc.LifeExpectancyLBC == "0-1 Years")
                            {
                                html_Header = html_Header.Replace("{LBC}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyLBC + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{LBC}", "<p>&nbsp;" + visualPloc.LifeExpectancyLBC + "</p>");

                            if (visualPloc.LifeExpectancyAWE == "0-1 Years")
                            {
                                html_Header = html_Header.Replace("{AWE}", "<p><font color=\"red\">&nbsp;" + visualPloc.LifeExpectancyAWE + "</font></p>");
                            }
                            else
                                html_Header = html_Header.Replace("{AWE}", "<p>&nbsp;" + visualPloc.LifeExpectancyAWE + "</p>");

                            mainHtml.Append(html_Header);




                            IEnumerable<VisualProjectLocationPhoto> imageApartment = await VisualProjectLocationPhotoDataStore.GetItemsAsyncByVisualProjectLocationId(visualPloc.Id.ToString());
                            var visualimageApartment = imageApartment.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();
                            string html_VisualImages = System.IO.File.ReadAllText(path + "\\VisualImage.html", iso);

                            StringBuilder visual_Images_apartment = new StringBuilder();
                            int apCount = 0;
                            if (visualimageApartment.Count() <= factor)
                            {

                                visual_Images_apartment.Append(trWithStyle);
                                foreach (var item in visualimageApartment)
                                {
                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                }
                                visual_Images_apartment.Append("</tr>");
                            }
                            else
                            {
                                visual_Images_apartment.Append(trWithStyle);
                                foreach (var item in visualimageApartment)
                                {

                                    if ((apCount % factor) == 0)
                                    {

                                        visual_Images_apartment.Append("</tr>");
                                        visual_Images_apartment.Append(trWithStyle);
                                    }
                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);

                                    visual_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));
                                    apCount++;
                                }
                                visual_Images_apartment.Append("</tr>");
                            }
                            html_VisualImages = html_VisualImages.Replace("{VisualImages}", visual_Images_apartment.ToString()).Replace("<tr></tr>", "");
                            html_VisualImages = html_VisualImages.Replace("{imageCount}", factor.ToString());
                            mainHtml.Append(html_VisualImages);
                            //Invasive


                            List<VisualProjectLocationPhoto> InvasiveimageApartment = imageApartment.Where(c => c.ImageDescription == "TRUE").ToList();
                            string html_InvasiveImages = System.IO.File.ReadAllText(path + "\\VisualInvasiveImage.html", iso);



                            StringBuilder Inv_Images_apartment = new StringBuilder();

                            if (InvasiveimageApartment.Count <= factor)
                            {

                                Inv_Images_apartment.Append(trWithStyle);
                                foreach (var item in InvasiveimageApartment)
                                {


                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                    Inv_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                }
                                Inv_Images_apartment.Append("</tr>");
                            }
                            else
                            {
                                int apCountInv = 0;
                                Inv_Images_apartment.Append(trWithStyle);
                                foreach (var item in InvasiveimageApartment)
                                {
                                    if ((apCountInv % factor) == 0)
                                    {

                                        Inv_Images_apartment.Append("</tr>");
                                        Inv_Images_apartment.Append(trWithStyle);
                                    }
                                    string lastImage = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                                    var newCompressedPath = CompressImage(item.ImageUrl, lastImage, quality);
                                    Inv_Images_apartment.Append(getTdWithStyle(factor) + getimgWithStyle(factor, newCompressedPath));

                                    apCountInv++;
                                }
                                Inv_Images_apartment.Append("</tr>");
                            }
                            
                            html_InvasiveImages = html_InvasiveImages.Replace("{InvasiveDescrition}", visualPloc.ImageDescription.Replace("\n", "<br/>")).Replace("{InvasiveImages}", Inv_Images_apartment.ToString()).Replace("<tr></tr>", "");
                            html_InvasiveImages = html_InvasiveImages.Replace("{imageCount}", factor.ToString());
                            mainHtml.Append(html_InvasiveImages);

                            string conclusiveHTML = await getConclusiveHTMLContent(visualPloc, quality, factor);
                            mainHtml.Append(conclusiveHTML);
                        }

                    }

                    

                }

                mainHtml.Append("</body></html>");
                //to save html file.
                var projPath = path + "\\" + "ProjectReport.html";
                using (StreamWriter sr = File.CreateText(projPath))
                {
                    sr.Write(mainHtml.ToString());
                    sr.Close();

                }
                StatusMessage = "Creating document...";
                using (Stream fileStreamPath = GenerateStreamFromString(mainHtml.ToString()))
                {
                    //Creates a new instance of WordDocument
                    WordDocument document = new WordDocument();

                    document.Open(fileStreamPath, FormatType.Html);
                    //var destPath = HttpContext.Current.Server.MapPath("~/Report/" + "report123.docx");
                    ////save document locally.
                    //document.Save(destPath);

                    if (company == "WICR")
                    {
                        foreach (WSection section in document.Sections)
                        {
                            // Add a new table to the header
                            IWParagraph headerPar = section.HeadersFooters.Header.AddParagraph();

                            RowFormat format = new RowFormat();
                            format.HorizontalAlignment = RowAlignment.Center;
                            // Setting Single table border style.
                            format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;


                            string s = path + "wicrlogo.jpg";

                            headerPar.AppendPicture(System.Drawing.Image.FromFile(s));
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            //Set Image size.
                            (headerPar.Items[0] as WPicture).Width = 132.5f;
                            (headerPar.Items[0] as WPicture).Height = 44.75f;
                            (headerPar.Items[0] as WPicture).HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            // Inserting text to the table second cell.
                            WParagraph headerPar1 = new WParagraph(document);
                            IWTextRange txt = headerPar1.AppendText("State Contractor’s License No.: 745936\nVisit our website: www.WICR.net \n888-388-9427");
                            txt.CharacterFormat.FontSize = 6;
                            txt.CharacterFormat.Bold = false;
                            txt.CharacterFormat.FontName = "Arial Narrow";
                            txt.CharacterFormat.TextColor = System.Drawing.ColorTranslator.FromHtml("#8EAADB");
                            txt.CharacterFormat.CharacterSpacing = 1.4f;
                            headerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;
                            section.HeadersFooters.Header.Paragraphs.Add(headerPar1);
                            // Add a footer paragraph text to the document.
                            WParagraph footerPar = new WParagraph(document);
                            footerPar.ParagraphFormat.Tabs.AddTab(523f, TabJustification.Centered, TabLeader.NoLeader);
                            // Add text.

                            IWTextRange firstText = footerPar.AppendText("www.WICR.net");
                            firstText.CharacterFormat.TextColor = Color.BlueViolet;
                            // Add page and Number of pages field to the document.
                            WParagraph footerPar1 = new WParagraph(document);
                            footerPar1.AppendText("\tPage ");
                            footerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;

                            IWField ff = footerPar1.AppendField("Page", FieldType.FieldPage);

                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar);
                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar1);

                            #region Page Number Settings
                            section.PageSetup.RestartPageNumbering = true;
                            section.PageSetup.PageStartingNumber = 1;
                            section.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;
                            #endregion Page Number Settings

                        }
                    }
                    else
                    {
                        foreach (WSection section in document.Sections)
                        {



                            // Add a new table to the header
                            // IWTable table = section.HeadersFooters.Header.AddTable();
                            IWParagraph headerPar = section.HeadersFooters.Header.AddParagraph();
                            RowFormat format = new RowFormat();
                            format.HorizontalAlignment = RowAlignment.Center;
                            // Setting Single table border style.
                            format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;

                            string s = path + "decklogo.jpg";

                            headerPar.AppendPicture(System.Drawing.Image.FromFile(s));
                            //Set Image size.
                            (headerPar.Items[0] as WPicture).Width = 132.5f;
                            (headerPar.Items[0] as WPicture).Height = 44.75f;
                            (headerPar.Items[0] as WPicture).HorizontalAlignment = ShapeHorizontalAlignment.Center;
                            // Inserting text to the table second cell.
                            // headerPar = table[0, 0].AddParagraph() as WParagraph;
                            headerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                            // Add a footer paragraph text to the document.
                            WParagraph footerPar = new WParagraph(document);

                            //footerPar.ParagraphFormat.Tabs.AddTab(523f, TabJustification.Centered, TabLeader.NoLeader);
                            // Add text.
                            footerPar.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            IWTextRange firstText = footerPar.AppendText("WWW.DECKINSPECTORS.COM");
                            firstText.CharacterFormat.TextColor = Color.Orange;
                            // Add page and Number of pages field to the document.
                            WParagraph footerPar1 = new WParagraph(document);
                            footerPar1.AppendText("\tPage ");
                            footerPar1.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;

                            IWField ff = footerPar1.AppendField("Page", FieldType.FieldPage);

                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar);
                            section.HeadersFooters.Footer.Paragraphs.Add(footerPar1);
                            #region Page Number Settings
                            section.PageSetup.RestartPageNumbering = true;
                            section.PageSetup.PageStartingNumber = 1;
                            section.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;

                            #endregion Page Number Settings

                        }
                    }



                    if (Type == "Word")
                    {

                        if (company == "DI")
                        {
                            fname = "Deck_Invasive" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".docx";
                        }
                        else
                        {
                            fname = "WICR_Invasive" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".docx";
                        }
                        document.Save(download + fname, FormatType.Docx);


                    }
                    else
                    {
                        if (company == "DI")
                        {
                            fname = "Deck_Invasive" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".pdf";
                        }
                        else
                        {
                            fname = "WICR_Invasive" + project.Name.Replace(" ", "_").ToString() + DateTime.Now.ToString("ddMMMyyyHHmmss") + ".pdf";
                        }

                        DocToPDFConverter converter = new DocToPDFConverter();
                        PdfDocument pdfDocument = converter.ConvertToPDF(document);
                        converter.Dispose();

                        pdfDocument.Save(download + fname);
                        pdfDocument.Close();
                    }
                    document.Close();
                }

                NotificationUI("Report Created Successfully, please navigate to Downlaods folder to access it.");
                Process.Start(download + fname);
            }
            catch (Exception ex)
            {
                NotificationUI("Report Creation failed, please try again later," + ex.Message);
            }
            IsReportInProgress = System.Windows.Visibility.Collapsed;
        }

        #endregion

    }
}
