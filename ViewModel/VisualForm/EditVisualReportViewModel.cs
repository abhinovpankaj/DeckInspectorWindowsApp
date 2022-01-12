using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Code.Model;

namespace UI.Code.ViewModel.VisualForm
{
    public class EditVisualReportViewModel : BaseViewModel
    {
        private ObservableCollection<string> _exteriorElements;//= new ObservableCollection<string>();
        private string _additionalConsideration;
        public string AdditionalConsideration
        {
            get
            {
                return _additionalConsideration;
            }
            set
            {
                _additionalConsideration = value;
                OnPropertyChanged("AdditionalConsideration");
            }
        }
        private ObservableCollection<CheckboxItem> _selectedExteriorElements = new ObservableCollection<CheckboxItem>()
        {
            new CheckboxItem() {  Name = "Decks", IsSelected = false },
            new CheckboxItem() {  Name = "Porches / Entry", IsSelected = false },
            new CheckboxItem() {  Name = "Stairs", IsSelected = false },
            new CheckboxItem() {  Name = "Stairs Landing", IsSelected = false },
            new CheckboxItem() {  Name = "Walkways", IsSelected = false },
            new CheckboxItem() {  Name = "Railings", IsSelected = false },
            new CheckboxItem() { Name = "Integrations", IsSelected = false },
            new CheckboxItem() {  Name = "Door Threshold", IsSelected = false },
            new CheckboxItem() {  Name = "Stucco Interface", IsSelected = false }
        };

        public ObservableCollection<CheckboxItem> SelectedExteriorElements
        {
            get { return _selectedExteriorElements; }
            set
            {
                _selectedExteriorElements = value;
                OnPropertyChanged("SelectedExteriorElements");
                CountExteriorElements = _selectedExteriorElements.Where(x => x.IsSelected).Count().ToString();
            }
        }

        private ObservableCollection<CheckboxItem> _selectedWaterproofElements = new ObservableCollection<CheckboxItem>()
        {
            new CheckboxItem() { Name = "Flashings", IsSelected = false },
            new CheckboxItem() {  Name = "Waterproofing", IsSelected = false },
            new CheckboxItem() {  Name = "Coatings", IsSelected = false },
            new CheckboxItem() { Name = "Sealants", IsSelected = false },
        };

        public ObservableCollection<CheckboxItem> SelectedWaterproofElements
        {
            get { return _selectedWaterproofElements; }
            set
            {
                _selectedWaterproofElements = value;
                OnPropertyChanged("SelectedWaterproofElements");
                CountWaterProofingElements = _selectedWaterproofElements.Where(x => x.IsSelected).Count().ToString();
            }
        }


        public ObservableCollection<string> ExteriorElements
        {
            get { return _exteriorElements; }
            set
            {
                if (_exteriorElements != value)
                {
                    _exteriorElements = value;
                    OnPropertyChanged("ExteriorElements");
                    switch (LocationType)
                    {
                        case 0:
                            ProjLocation.ExteriorElements = string.Join(",", value);
                            break;
                        case 1:
                            BuildingLocation.ExteriorElements = string.Join(",", value);
                            break;
                        case 2:
                            AptLocation.ExteriorElements = string.Join(",", value);
                            break;
                        default:
                            break;
                    }
                }


            }
        }
        private ObservableCollection<string> _wpe;
        public ObservableCollection<string> WaterProofingElements
        {
            get { return _wpe; }
            set
            {
                if (_wpe != value)
                {
                    _wpe = value;
                    OnPropertyChanged("WaterProofingElements");
                    switch (LocationType)
                    {
                        case 0:
                            ProjLocation.WaterProofingElements = string.Join(",", value);
                            break;
                        case 1:
                            BuildingLocation.WaterProofingElements = string.Join(",", value);
                            break;
                        case 2:
                            AptLocation.WaterProofingElements = string.Join(",", value);
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        private async Task Save()
        {


        }
        private async Task<Response> SaveLoad()
        {
            Response response = new Response();
            try
            {
                string errorMessage = string.Empty;
                if (string.IsNullOrEmpty(VisualForm.Name))
                {
                    errorMessage += "\nName is required\n";
                }
                if (string.IsNullOrEmpty(UnitPhotoCount) || UnitPhotoCount == "0")
                {
                    errorMessage += "\nUnit photo required\n";
                }
                if (ExteriorElements.Count == 0)
                {
                    errorMessage += "\nExterior Elements photo required\n";
                }
                if (WaterProofingElements.Count == 0)
                {
                    errorMessage += "\nWaterProofing Elements required\n";
                }
                if (RadioList_VisualReviewItems.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nVisual Review required\n";
                }
                if (RadioList_AnyVisualSignItems.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nAny visual signs of leaksrequired\n";
                }
                if (RadioList_FurtherInasiveItems.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nFurther Invasive Review Required Yes/No required\n";
                }
                if (RadioList_ConditionAssessment.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nCondition Assessment Required Yes/No required\n";
                }
                if (RadioList_LifeExpectancyEEE.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nLife Expectancy Exterior Elevated Elements (EEE) required\n";
                }
                if (RadioList_LifeExpectancyLBC.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nLife Expectancy Load Bearing Componenets (LBC) required\n";
                }
                if (RadioList_LifeExpectancyAWE.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nLife Expectancy Associated Waterproofing Elements (AWE) required\n";
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    response.Message = errorMessage;
                    response.Status = ApiResult.Fail;
                }
                else
                {

                    VisualForm.ExteriorElements = string.Join(",", ExteriorElements.ToArray());
                    VisualForm.WaterProofingElements = string.Join(",", WaterProofingElements.ToArray());

                    VisualForm.VisualReview = RadioList_VisualReviewItems.Where(c => c.IsChecked == true).Single().Name;
                    VisualForm.AnyVisualSign = RadioList_AnyVisualSignItems.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.FurtherInasive = RadioList_FurtherInasiveItems.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.ConditionAssessment = RadioList_ConditionAssessment.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.LifeExpectancyEEE = RadioList_LifeExpectancyEEE.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.LifeExpectancyLBC = RadioList_LifeExpectancyLBC.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.LifeExpectancyAWE = RadioList_LifeExpectancyAWE.Where(c => c.IsChecked == true).Single().Name;

                   
                }
            }

            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = ApiResult.Fail;

            }
            return await Task.FromResult(response);

        }

        public ObservableCollection<CustomRadioItem> RadioList_VisualReviewItems { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_AnyVisualSignItems { get; set; }

        public ObservableCollection<CustomRadioItem> RadioList_FurtherInasiveItems { get; set; }


        public ObservableCollection<CustomRadioItem> RadioList_ConditionAssessment { get; set; }

        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyEEE { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyLBC { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyAWE { get; set; }

        // public VisualFormProjectLocation MyProperty { get; set; }
        private ProjectLocation_Visual visualForm;

        public ProjectLocation_Visual VisualForm
        {
            get { return visualForm; }
            set { visualForm = value; OnPropertyChanged("VisualForm"); }
        }

        private void FillVisualReportViewModel()
        {
            RadioList_VisualReviewItems = new ObservableCollection<CustomRadioItem>();
            RadioList_VisualReviewItems.Add(new CustomRadioItem() { ID = 1, Name = "Good", IsChecked = false, GroupName = "VR" });
            RadioList_VisualReviewItems.Add(new CustomRadioItem() { ID = 2, Name = "Bad", IsChecked = false, GroupName = "VR" });
            RadioList_VisualReviewItems.Add(new CustomRadioItem() { ID = 3, Name = "Fair", IsChecked = false, GroupName = "VR" });

            RadioList_AnyVisualSignItems = new ObservableCollection<CustomRadioItem>();
            RadioList_AnyVisualSignItems.Add(new CustomRadioItem() { ID = 1, Name = "Yes", IsChecked = false, GroupName = "AVS" });
            RadioList_AnyVisualSignItems.Add(new CustomRadioItem() { ID = 2, Name = "No", IsChecked = false, GroupName = "AVS" });


            RadioList_FurtherInasiveItems = new ObservableCollection<CustomRadioItem>();
            RadioList_FurtherInasiveItems.Add(new CustomRadioItem() { ID = 1, Name = "Yes", IsChecked = false, GroupName = "FIR" });
            RadioList_FurtherInasiveItems.Add(new CustomRadioItem() { ID = 2, Name = "No", IsChecked = false, GroupName = "FIR" });

            RadioList_ConditionAssessment = new ObservableCollection<CustomRadioItem>();
            RadioList_ConditionAssessment.Add(new CustomRadioItem() { ID = 1, Name = "Pass", IsChecked = false, GroupName = "CA" });
            RadioList_ConditionAssessment.Add(new CustomRadioItem() { ID = 2, Name = "Fail", IsChecked = false, GroupName = "CA" });
            RadioList_ConditionAssessment.Add(new CustomRadioItem() { ID = 3, Name = "Future Inspection", IsChecked = false, GroupName = "CA" });

            RadioList_LifeExpectancyEEE = new ObservableCollection<CustomRadioItem>();
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsChecked = false, GroupName = "EEE" });
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsChecked = false, GroupName = "EEE" });
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsChecked = false, GroupName = "EEE" });
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsChecked = false, GroupName = "EEE" });



            RadioList_LifeExpectancyLBC = new ObservableCollection<CustomRadioItem>();
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsChecked = false, GroupName = "LBC" });
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsChecked = false, GroupName = "LBC" });
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsChecked = false, GroupName = "LBC" });
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsChecked = false, GroupName = "LBC" });


            RadioList_LifeExpectancyAWE = new ObservableCollection<CustomRadioItem>();
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsChecked = false, GroupName = "AWE" });
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsChecked = false, GroupName = "AWE" });
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsChecked = false, GroupName = "AWE" });
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsChecked = false, GroupName = "AWE" });



        }
        public EditVisualReportViewModel()
        {

            FillVisualReportViewModel();
           
        }

        public VisualProjectLocation ProjLocation { get; set; }
        public VisualBuildingLocation BuildingLocation { get; set; }
        public VisualBuildingApartment AptLocation { get; set; }

        public int LocationType { get; set; }
        public EditVisualReportViewModel(VisualProjectLocation parm, string title)
        {
            ProjLocation = parm;

            LocationType = 0;
            FillVisualReportViewModel();
            this.ExteriorElements = new ObservableCollection<string>(parm.ExteriorElements.Split(',').ToList());

            this.WaterProofingElements = new ObservableCollection<string>(parm.WaterProofingElements.Split(',').ToList());

            this.CountExteriorElements = this.ExteriorElements.Count.ToString();
            this.CountWaterProofingElements = this.WaterProofingElements.Count.ToString();
            this.RadioList_VisualReviewItems.Where(c => c.Name == parm.VisualReview).Single().IsChecked = true;
            this.RadioList_AnyVisualSignItems.Where(c => c.Name == parm.AnyVisualSign).Single().IsChecked = true;
            this.RadioList_FurtherInasiveItems.Where(c => c.Name == parm.FurtherInasive).Single().IsChecked = true;
            this.RadioList_ConditionAssessment.Where(c => c.Name == parm.ConditionAssessment).Single().IsChecked = true;
            this.RadioList_LifeExpectancyEEE.Where(c => c.Name == parm.LifeExpectancyEEE).Single().IsChecked = true;
            this.RadioList_LifeExpectancyLBC.Where(c => c.Name == parm.LifeExpectancyLBC).Single().IsChecked = true;
            this.RadioList_LifeExpectancyAWE.Where(c => c.Name == parm.LifeExpectancyAWE).Single().IsChecked = true;
            Title = parm.Name;
            AdditionalConsideration = parm.AdditionalConsideration;

        }

        public EditVisualReportViewModel(VisualBuildingApartment parm, string title)
        {
            AptLocation = parm;
            LocationType = 2;
            FillVisualReportViewModel();
            this.ExteriorElements = new ObservableCollection<string>(parm.ExteriorElements.Split(',').ToList());
            this.WaterProofingElements = new ObservableCollection<string>(parm.WaterProofingElements.Split(',').ToList());

            this.WaterProofingElements = new ObservableCollection<string>(parm.WaterProofingElements.Split(',').ToList());

            this.CountExteriorElements = this.ExteriorElements.Count.ToString();
            this.CountWaterProofingElements = this.WaterProofingElements.Count.ToString();
            this.RadioList_VisualReviewItems.Where(c => c.Name == parm.VisualReview).Single().IsChecked = true;
            this.RadioList_AnyVisualSignItems.Where(c => c.Name == parm.AnyVisualSign).Single().IsChecked = true;
            this.RadioList_FurtherInasiveItems.Where(c => c.Name == parm.FurtherInasive).Single().IsChecked = true;
            this.RadioList_ConditionAssessment.Where(c => c.Name == parm.ConditionAssessment).Single().IsChecked = true;
            this.RadioList_LifeExpectancyEEE.Where(c => c.Name == parm.LifeExpectancyEEE).Single().IsChecked = true;
            this.RadioList_LifeExpectancyLBC.Where(c => c.Name == parm.LifeExpectancyLBC).Single().IsChecked = true;
            this.RadioList_LifeExpectancyAWE.Where(c => c.Name == parm.LifeExpectancyAWE).Single().IsChecked = true;
            //UnitPhotoCount = count.ToString();
            Title = parm.Name;
            AdditionalConsideration = parm.AdditionalConsideration;
        }

        public EditVisualReportViewModel(VisualBuildingLocation parm, string title)
        {
            BuildingLocation = parm;
            LocationType = 1;
            FillVisualReportViewModel();
            this.ExteriorElements = new ObservableCollection<string>(parm.ExteriorElements.Split(',').ToList());
            this.WaterProofingElements = new ObservableCollection<string>(parm.WaterProofingElements.Split(',').ToList());
            this.CountExteriorElements = this.ExteriorElements.Count.ToString();
            this.CountWaterProofingElements = this.WaterProofingElements.Count.ToString();

            this.WaterProofingElements = new ObservableCollection<string>(parm.WaterProofingElements.Split(',').ToList());

            this.RadioList_VisualReviewItems.Where(c => c.Name == parm.VisualReview).Single().IsChecked = true;
            this.RadioList_AnyVisualSignItems.Where(c => c.Name == parm.AnyVisualSign).Single().IsChecked = true;
            this.RadioList_FurtherInasiveItems.Where(c => c.Name == parm.FurtherInasive).Single().IsChecked = true;
            this.RadioList_ConditionAssessment.Where(c => c.Name == parm.ConditionAssessment).Single().IsChecked = true;
            this.RadioList_LifeExpectancyEEE.Where(c => c.Name == parm.LifeExpectancyEEE).Single().IsChecked = true;
            this.RadioList_LifeExpectancyLBC.Where(c => c.Name == parm.LifeExpectancyLBC).Single().IsChecked = true;
            this.RadioList_LifeExpectancyAWE.Where(c => c.Name == parm.LifeExpectancyAWE).Single().IsChecked = true;
            //UnitPhotoCount = count.ToString();
            Title = parm.Name;
            AdditionalConsideration = parm.AdditionalConsideration;
        }

        private string _countExteriorElements;

        public string CountExteriorElements
        {
            get { return _countExteriorElements; }
            set { _countExteriorElements = value; OnPropertyChanged("CountExteriorElements"); }
        }

        private string _countWaterProofingElements;

        public string CountWaterProofingElements
        {
            get { return _countWaterProofingElements; }
            set { _countWaterProofingElements = value; OnPropertyChanged("CountWaterProofingElements"); }
        }


        private string _unitPhotCount;

        public string UnitPhotoCount
        {
            get { return _unitPhotCount; }
            set { _unitPhotCount = value; OnPropertyChanged("UnitPhotoCount"); }
        }
        private string _InvunitPhotCount;

        public string InvasiveUnitPhotoCount
        {
            get { return _InvunitPhotCount; }
            set { _InvunitPhotCount = value; OnPropertyChanged("InvasiveUnitPhotoCount"); }
        }

        //  public ObservableCollection<VisualProjectLocationPhoto> UnitPhotos { get; set; }
        private ObservableCollection<VisualProjectLocationPhoto> _unitPhoto;

        public ObservableCollection<VisualProjectLocationPhoto> UnitPhotos
        {
            get { return _unitPhoto; }
            set { _unitPhoto = value; OnPropertyChanged("UnitPhotos"); }
        }




        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }


        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }


        private bool _isprojectLoc;

        public bool IsVisualProjectLocatoion
        {
            get { return _isprojectLoc; }
            set { _isprojectLoc = value; OnPropertyChanged("IsVisualProjectLocatoion"); }
        }
        private bool _isbuildingLoc;
        public bool IsVisualBuilding
        {
            get { return _isbuildingLoc; }
            set { _isbuildingLoc = value; OnPropertyChanged("IsVisualBuilding"); }
        }


        private bool _isA;

        public bool IsVisualApartment
        {
            get { return _isA; }
            set { _isA = value; OnPropertyChanged("IsVisualApartment"); }
        }

        public ICommand ChooseExteriorCommand => new DelegateCommand(async () => await ChooseExteriorCommandCommandExecute());
        private async Task ChooseExteriorCommandCommandExecute()
        {
            //await Shell.Current.Navigation.PushModalAsync(new PopUpCheakListBox() { BindingContext = new PopUpCheakListBoxViewModel() { CheakBoxSelectedItems = ExteriorElements } });
        }

        public ICommand ChooseWaterproofingCommand => new DelegateCommand(async () => await ChooseWaterproofingCommandCommandExecute());
        private async Task ChooseWaterproofingCommandCommandExecute()
        {
            //await Shell.Current.Navigation.PushModalAsync(new PopUpCheakListBoxWaterProofing() { BindingContext = new PopUpCheakListBoxWaterproofingViewModel() { CheakBoxSelectedItems = WaterProofingElements } });
        }
        //private async Task<string> TakePictureFromLibrary()
        //{
        //    IsBusy = true;
        //    var file = await CrossMedia.Current.PickPhotoAsync
        //        (new PickMediaOptions()
        //        {
        //            SaveMetaData = true,
        //            PhotoSize = PhotoSize.MaxWidthHeight,
        //            CompressionQuality = App.CompressionQuality


        //        }); ;

        //    IsBusy = false;
        //    if (file == null)
        //        return null;

        //    return file.Path;

        //}

    }
}
