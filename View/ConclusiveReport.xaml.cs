using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.Code.Model;
using UI.Code.Services;
using UI.Code.ViewModel;

namespace UI.Code.View
{
    /// <summary>
    /// Interaction logic for ConclusiveReport.xaml
    /// </summary>
    public partial class ConclusiveReport : UserControl
    {
        public ConclusiveReport()
        {
            InitializeComponent();
            
        }

        
        private async void addPicBtn_Click(object sender, RoutedEventArgs e)
        {
            List<string> pics = new List<string>();
            var vm = this.DataContext;
            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                var myVM = vm as VisualProjectLocationViewModel;
                this.Dispatcher.Invoke(() => {
                    myVM.IsBusy = true;
                });
            }

            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                var myVM = vm as VisualBuildingLocationViewModel;
                this.Dispatcher.Invoke(() => {
                    myVM.IsBusy = true;
                });

            }

            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                var myVM = vm as VisualApartmentViewModel;
                this.Dispatcher.Invoke(() => {
                    myVM.IsBusy = true;
                });

            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    pics.Add(System.IO.Path.GetFullPath(filename));


                var passed = await UploadGallary(pics);
            }
            else
            {
                if (vm.GetType() == typeof(VisualProjectLocationViewModel))
                {
                    var myVM = vm as VisualProjectLocationViewModel;
                    this.Dispatcher.Invoke(() => {
                        myVM.IsBusy = false;
                    });
                }

                if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
                {
                    var myVM = vm as VisualBuildingLocationViewModel;
                    this.Dispatcher.Invoke(() => {
                        myVM.IsBusy = false;
                    });

                }

                if (vm.GetType() == typeof(VisualApartmentViewModel))
                {
                    var myVM = vm as VisualApartmentViewModel;
                    this.Dispatcher.Invoke(() => {
                        myVM.IsBusy = false;
                    });

                }
            }
        }

        public async Task<bool> UploadGallary(List<string> images)
        {
            var vm = this.DataContext;
            List<MultiImage> locImages = new List<MultiImage>();
            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                var myVM = vm as VisualProjectLocationViewModel;


                foreach (var item in images)
                {

                    locImages.Add(new MultiImage() { Id = Guid.NewGuid().ToString(), Image = item, ParentId = myVM.SelectedItem.ProjectLocationId, Status = "New", IsServerData = false });
                }


                //bool result = await UploadFromGallary(myVM.SelectedItem.Name, "/api/ProjectLocationImage/AddEdit?ParentId=" + myVM.SelectedItem.ProjectLocationId + "&UserId=" + App.LogUser.Id.ToString(), images);
                Response result = await UploadFromGallary(myVM.SelectedItem, "api/VisualProjectLocation/Edit", locImages);

                await myVM.GetImages(myVM.SelectedItem);
            }
            else if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                var myVM = vm as VisualBuildingLocationViewModel;
                this.Dispatcher.Invoke(() => {
                    myVM.IsBusy = true;
                });
                foreach (var item in images)
                {

                    locImages.Add(new MultiImage() { Id = Guid.NewGuid().ToString(), Image = item, ParentId = myVM.SelectedItem.BuildingLocationId, Status = "New", IsServerData = false });
                }


                Response result = await UploadFromGallary(myVM.SelectedItem, "api/VisualBuildingLocation/Edit", locImages);

                await myVM.GetImages(myVM.SelectedItem);
            }
            else if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                var myVM = vm as VisualApartmentViewModel;
                this.Dispatcher.Invoke(() => {
                    myVM.IsBusy = true;
                });
                foreach (var item in images)
                {

                    locImages.Add(new MultiImage() { Id = Guid.NewGuid().ToString(), Image = item, ParentId = myVM.SelectedItem.BuildingApartmentId, Status = "New", IsServerData = false });
                }


                Response result = await UploadFromGallary(myVM.SelectedItem, "api/VisualBuildingApartment/Edit", locImages);

                await myVM.GetImages(myVM.SelectedItem);
            }

            return true;
        }
        public async Task<Response> UploadFromGallary(VisualBuildingApartment visualLocation, String endpointUrl, IEnumerable<MultiImage> list)
        {


            Response result = new Response();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", visualLocation.Id.ToString());
            parameters.Add("Name", visualLocation.Name);
            parameters.Add("BuildingApartmentId", visualLocation.BuildingApartmentId);
            parameters.Add("AdditionalConsideration", visualLocation.AdditionalConsideration);

            parameters.Add("ExteriorElements", visualLocation.ExteriorElements);
            parameters.Add("WaterProofingElements", visualLocation.WaterProofingElements);
            parameters.Add("ConditionAssessment", visualLocation.ConditionAssessment);
            parameters.Add("VisualReview", visualLocation.VisualReview);

            parameters.Add("AnyVisualSign", visualLocation.AnyVisualSign);
            parameters.Add("FurtherInasive", visualLocation.FurtherInasive);
            parameters.Add("LifeExpectancyEEE", visualLocation.LifeExpectancyEEE);
            parameters.Add("LifeExpectancyLBC", visualLocation.LifeExpectancyLBC);

            parameters.Add("LifeExpectancyAWE", visualLocation.LifeExpectancyAWE);
            parameters.Add("ImageDescription", visualLocation.ImageDescription);

            parameters.Add("IsPostInvasiveRepairsRequired", visualLocation.IsPostInvasiveRepairsRequired.ToString());
            parameters.Add("ConclusiveComments", visualLocation.ConclusiveComments);
            parameters.Add("IsInvasiveRepairComplete", visualLocation.IsInvasiveRepairComplete.ToString());
            parameters.Add("IsInvasiveRepairApproved", visualLocation.IsInvasiveRepairApproved.ToString());
            parameters.Add("ConclusiveLifeExpEEE", visualLocation.ConclusiveLifeExpEEE);
            parameters.Add("ConclusiveLifeExpLBC", visualLocation.ConclusiveLifeExpLBC);
            parameters.Add("ConclusiveLifeExpAWE", visualLocation.ConclusiveLifeExpAWE);
            parameters.Add("ConclusiveAdditionalConcerns", visualLocation.ConclusiveAdditionalConcerns);



            parameters.Add("UserID", App.LogUser.Id.ToString());

          


            if (App.IsInvasive == true)
            {

                parameters.Add("IsInvaiveImage", "CONCLUSIVE");
                
            }
            else
            {
                parameters.Add("IsInvaiveImage", null);
               
            }



            //  result = await HttpUtil.VisualDataAdd(item.Name, "api/VisualProjectLocation/Add", parameters, ImageList);

            HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(App.AppUrl);

                using (var formData = new MultipartFormDataContent())
                {

                    int Index = 1000;
                    foreach (MultiImage img in list)
                    {
                        Index++;
                        if (img.IsServerData == false && img.Status != "FromServer")
                        {
                            string ServerFileName = "New_" + img.Id;

                            string[] array = ServerFileName.Split('_');
                            string operation = array[0];
                            string searchId = array[1];
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                        }
                        if (img.IsServerData == true && img.Status == "Update")
                        {
                            string ServerFileName = "Update_" + img.Id;
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                        }
                        if (img.IsServerData == true && img.Status == "Delete")
                        {
                            string ServerFileName = "Delete_" + img.Id;
                            var sevenThousandItems = new byte[7000];
                            formData.Add(new ByteArrayContent(sevenThousandItems), Index.ToString(), ServerFileName);
                        }

                    }
                    // var extension = Path.GetExtension(img);
                    formData.Add(DictionaryItems, "Model");


                    var response = client.PostAsync(endpointUrl, formData).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Response>(responseBody);
                    return await Task.FromResult(result);
                }

            }
        }
        public async Task<Response> UploadFromGallary(VisualBuildingLocation visualLocation, String endpointUrl, IEnumerable<MultiImage> list)
        {

            Response result = new Response();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", visualLocation.Id.ToString());
            parameters.Add("Name", visualLocation.Name);
            parameters.Add("BuildingLocationId", visualLocation.BuildingLocationId);
            parameters.Add("AdditionalConsideration", visualLocation.AdditionalConsideration);

            parameters.Add("ExteriorElements", visualLocation.ExteriorElements);
            parameters.Add("WaterProofingElements", visualLocation.WaterProofingElements);
            parameters.Add("ConditionAssessment", visualLocation.ConditionAssessment);
            parameters.Add("VisualReview", visualLocation.VisualReview);

            parameters.Add("AnyVisualSign", visualLocation.AnyVisualSign);
            parameters.Add("FurtherInasive", visualLocation.FurtherInasive);
            parameters.Add("LifeExpectancyEEE", visualLocation.LifeExpectancyEEE);
            parameters.Add("LifeExpectancyLBC", visualLocation.LifeExpectancyLBC);

            parameters.Add("LifeExpectancyAWE", visualLocation.LifeExpectancyAWE);
            parameters.Add("ImageDescription", visualLocation.ImageDescription);

            parameters.Add("IsPostInvasiveRepairsRequired", visualLocation.IsPostInvasiveRepairsRequired.ToString());
            parameters.Add("ConclusiveComments", visualLocation.ConclusiveComments);
            parameters.Add("IsInvasiveRepairComplete", visualLocation.IsInvasiveRepairComplete.ToString());
            parameters.Add("IsInvasiveRepairApproved", visualLocation.IsInvasiveRepairApproved.ToString());
            parameters.Add("ConclusiveLifeExpEEE", visualLocation.ConclusiveLifeExpEEE);
            parameters.Add("ConclusiveLifeExpLBC", visualLocation.ConclusiveLifeExpLBC);
            parameters.Add("ConclusiveLifeExpAWE", visualLocation.ConclusiveLifeExpAWE);
            parameters.Add("ConclusiveAdditionalConcerns", visualLocation.ConclusiveAdditionalConcerns);

            parameters.Add("UserID", App.LogUser.Id.ToString());


            if (App.IsInvasive == true)
            {

                parameters.Add("IsInvaiveImage", "CONCLUSIVE");
                
            }
            else
            {
                parameters.Add("IsInvaiveImage", null);
                
            }



            //  result = await HttpUtil.VisualDataAdd(item.Name, "api/VisualProjectLocation/Add", parameters, ImageList);

            HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(App.AppUrl);

                using (var formData = new MultipartFormDataContent())
                {

                    int Index = 1000;
                    foreach (MultiImage img in list)
                    {
                        Index++;
                        if (img.IsServerData == false && img.Status != "FromServer")
                        {
                            string ServerFileName = "New_" + img.Id;

                            string[] array = ServerFileName.Split('_');
                            string operation = array[0];
                            string searchId = array[1];
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                        }
                        if (img.IsServerData == true && img.Status == "Update")
                        {
                            string ServerFileName = "Update_" + img.Id;
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                        }
                        if (img.IsServerData == true && img.Status == "Delete")
                        {
                            string ServerFileName = "Delete_" + img.Id;
                            var sevenThousandItems = new byte[7000];
                            formData.Add(new ByteArrayContent(sevenThousandItems), Index.ToString(), ServerFileName);
                        }

                    }
                    // var extension = Path.GetExtension(img);
                    formData.Add(DictionaryItems, "Model");


                    var response = client.PostAsync(endpointUrl, formData).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Response>(responseBody);
                    return await Task.FromResult(result);
                }

            }
        }
        public async Task<Response> UploadFromGallary(VisualProjectLocation visualLocation, String endpointUrl, IEnumerable<MultiImage> list)
        {


            Response result = new Response();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", visualLocation.Id.ToString());
            parameters.Add("Name", visualLocation.Name);
            parameters.Add("ProjectLocationId", visualLocation.ProjectLocationId);
            parameters.Add("AdditionalConsideration", visualLocation.AdditionalConsideration);

            parameters.Add("ExteriorElements", visualLocation.ExteriorElements);
            parameters.Add("WaterProofingElements", visualLocation.WaterProofingElements);
            parameters.Add("ConditionAssessment", visualLocation.ConditionAssessment);
            parameters.Add("VisualReview", visualLocation.VisualReview);

            parameters.Add("AnyVisualSign", visualLocation.AnyVisualSign);
            parameters.Add("FurtherInasive", visualLocation.FurtherInasive);
            parameters.Add("LifeExpectancyEEE", visualLocation.LifeExpectancyEEE);
            parameters.Add("LifeExpectancyLBC", visualLocation.LifeExpectancyLBC);

            parameters.Add("LifeExpectancyAWE", visualLocation.LifeExpectancyAWE);
            parameters.Add("ImageDescription", visualLocation.ImageDescription);

            parameters.Add("IsPostInvasiveRepairsRequired", visualLocation.IsPostInvasiveRepairsRequired.ToString());
            parameters.Add("ConclusiveComments", visualLocation.ConclusiveComments);
            parameters.Add("IsInvasiveRepairComplete", visualLocation.IsInvasiveRepairComplete.ToString());
            parameters.Add("IsInvasiveRepairApproved", visualLocation.IsInvasiveRepairApproved.ToString());
            parameters.Add("ConclusiveLifeExpEEE", visualLocation.ConclusiveLifeExpEEE);
            parameters.Add("ConclusiveLifeExpLBC", visualLocation.ConclusiveLifeExpLBC);
            parameters.Add("ConclusiveLifeExpAWE", visualLocation.ConclusiveLifeExpAWE);

            parameters.Add("ConclusiveAdditionalConcerns", visualLocation.ConclusiveAdditionalConcerns);
            parameters.Add("UserID", App.LogUser.Id.ToString());


            if (App.IsInvasive == true)
            {

                parameters.Add("IsInvaiveImage", "CONCLUSIVE");
            }
            else
            {
                parameters.Add("IsInvaiveImage", null);
                
            }



            //  result = await HttpUtil.VisualDataAdd(item.Name, "api/VisualProjectLocation/Add", parameters, ImageList);

            HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(App.AppUrl);

                using (var formData = new MultipartFormDataContent())
                {

                    int Index = 1000;
                    foreach (MultiImage img in list)
                    {
                        Index++;
                        if (img.IsServerData == false && img.Status != "FromServer")
                        {
                            string ServerFileName = "New_" + img.Id;

                            string[] array = ServerFileName.Split('_');
                            string operation = array[0];
                            string searchId = array[1];
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                        }
                        if (img.IsServerData == true && img.Status == "Update")
                        {
                            string ServerFileName = "Update_" + img.Id;
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                        }
                        if (img.IsServerData == true && img.Status == "Delete")
                        {
                            string ServerFileName = "Delete_" + img.Id;
                            var sevenThousandItems = new byte[7000];
                            formData.Add(new ByteArrayContent(sevenThousandItems), Index.ToString(), ServerFileName);
                        }

                    }
                    // var extension = Path.GetExtension(img);
                    formData.Add(DictionaryItems, "Model");


                    var response = client.PostAsync(endpointUrl, formData).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Response>(responseBody);
                    return await Task.FromResult(result);
                }

            }
        }

        
        private async void SaveConclusiveData_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext;
            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                var myVM = vm as VisualProjectLocationViewModel;
                var ds = new VisualProjectLocationService();
                var response= await ds.UpdateItemAsync(myVM.SelectedItem);
                
            }

            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                var myVM = vm as VisualBuildingLocationViewModel;
                var ds = new VisualFormBuildingLocationDataStore();
                var response = await ds.UpdateItemAsync(myVM.SelectedItem);

            }

            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                var myVM = vm as VisualApartmentViewModel;
                var ds = new VisualFormApartmentDataStore();
                var response = await ds.UpdateItemAsync(myVM.SelectedItem);
            }
        }
    }
}
