using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UI.Code.ViewModel.VisualForm;

namespace UI.Code.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class VisualUserControleForVisual : UserControl
    {
        public event EventHandler BackClick;
        public event EventHandler ClickSave;
        // public event EventHandler ClickCancel;
        public event EventHandler ClickEdit;
        public event EventHandler ClickSearch;

        public event EventHandler ClickVisualReorder;
        public event EventHandler DoubleClickOnImage;
        // public event EventHandler ClickEdit;
        public VisualUserControleForVisual()
        {
            InitializeComponent();
            btnEdit.Click += BtnEdit_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            //btnBack.Click += BtnBack_Click;

            btnDataEdit.Click += BtnDataEdit_Click;
            btnDataSave.Click += BtnDataSave_Click; ;
            btnDataCancel.Click += BtnDataCancel_Click; ;
            btnDataDelete.Click += BtnDataDelete_Click;
            //    btnSearch.Click += BtnSearch_Click;
            btnImageDelete.Click += BtnImageDelete_Click;
            btnImageClose.Click += BtnImageClose_Click;
            this.Loaded += VisualUserControle_Loaded;
            btnPrv.Click += BtnPrv_Click;
            btnNext.Click += BtnNext_Click;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext;
            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                var selectedImage = childImageShow.DataContext as VisualProjectLocationPhoto;
                var img = plPhotos.SkipWhile(x => x != selectedImage).Skip(1).DefaultIfEmpty(plPhotos[0]).FirstOrDefault();
                childImageShow.DataContext = img;

            }

            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {

                var selectedImage = childImageShow.DataContext as VisualBuildingLocationPhoto;
                var img = vbPhotos.SkipWhile(x => x != selectedImage).Skip(1).DefaultIfEmpty(vbPhotos[0]).FirstOrDefault();
                childImageShow.DataContext = img;

            }

            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                var selectedImage = childImageShow.DataContext as VisualApartmentLocationPhoto;
                var img = baPhotos.SkipWhile(x => x != selectedImage).Skip(1).DefaultIfEmpty(baPhotos[0]).FirstOrDefault();
                childImageShow.DataContext = img;

            }
        }

        private void BtnPrv_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext;
            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                var selectedImage = childImageShow.DataContext as VisualProjectLocationPhoto;
                var img = plPhotos.TakeWhile(x => x != selectedImage).DefaultIfEmpty(plPhotos[plPhotos.Count - 1]).LastOrDefault();
                childImageShow.DataContext = img;

            }

            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                var selectedImage = childImageShow.DataContext as VisualBuildingLocationPhoto;
                var img = vbPhotos.TakeWhile(x => x != selectedImage).DefaultIfEmpty(vbPhotos[vbPhotos.Count - 1]).LastOrDefault();
                childImageShow.DataContext = img;

            }

            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                var selectedImage = childImageShow.DataContext as VisualApartmentLocationPhoto;
                var img = baPhotos.TakeWhile(x => x != selectedImage).DefaultIfEmpty(baPhotos[baPhotos.Count - 1]).LastOrDefault();
                childImageShow.DataContext = img;

            }
        }

        private void VisualDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext;

            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                VisualProjectLocationViewModel db = vm as VisualProjectLocationViewModel;
                var obj = ((ListBoxItem)sender).DataContext as VisualProjectLocation;

                db.GetImagesCommand.Execute(obj);
                //Abhinov
                VisualReportEditor.DataContext = new EditVisualReportViewModel(obj, db.Title);

            }
            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                VisualBuildingLocationViewModel db = vm as VisualBuildingLocationViewModel;
                var obj = ((ListBoxItem)sender).DataContext as VisualBuildingLocation;
                db.GetImagesCommand.Execute(obj);
                //Abhinov
                VisualReportEditor.DataContext = new EditVisualReportViewModel(obj, db.Title);
            }
            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                VisualApartmentViewModel db = vm as VisualApartmentViewModel;
                var obj = ((ListBoxItem)sender).DataContext as VisualBuildingApartment;
                db.GetImagesCommand.Execute(obj);
                //Abhinov
                VisualReportEditor.DataContext = new EditVisualReportViewModel(obj, db.Title);
            }

        }
        private void lboxViusal_Drop(object sender, DragEventArgs e)
        {
            var vm = this.DataContext;

            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                VisualProjectLocationViewModel db = vm as VisualProjectLocationViewModel;
                if (sender is ListBoxItem)
                {
                    VisualProjectLocation droppedData = e.Data.GetData(typeof(VisualProjectLocation)) as VisualProjectLocation;
                    VisualProjectLocation target = ((ListBoxItem)(sender)).DataContext as VisualProjectLocation;

                    int removedIdx = lboxVisual.Items.IndexOf(droppedData);
                    int targetIdx = lboxVisual.Items.IndexOf(target);
                    if (removedIdx != -1)
                    {
                        if (removedIdx < targetIdx)
                        {
                            db.Items.Insert(targetIdx + 1, droppedData);
                            db.Items.RemoveAt(removedIdx);
                        }
                        else
                        {
                            int remIdx = removedIdx + 1;
                            if (db.Items.Count + 1 > remIdx)
                            {
                                db.Items.Insert(targetIdx, droppedData);
                                db.Items.RemoveAt(remIdx);
                            }
                        }
                    }
                }
            }
            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                VisualBuildingLocationViewModel db = vm as VisualBuildingLocationViewModel;
                if (sender is ListBoxItem)
                {
                    VisualBuildingLocation droppedData = e.Data.GetData(typeof(VisualBuildingLocation)) as VisualBuildingLocation;
                    VisualBuildingLocation target = ((ListBoxItem)(sender)).DataContext as VisualBuildingLocation;

                    int removedIdx = lboxVisual.Items.IndexOf(droppedData);
                    int targetIdx = lboxVisual.Items.IndexOf(target);
                    if (removedIdx != -1)
                    {
                        if (removedIdx < targetIdx)
                        {
                            db.Items.Insert(targetIdx + 1, droppedData);
                            db.Items.RemoveAt(removedIdx);
                        }
                        else
                        {
                            int remIdx = removedIdx + 1;
                            if (db.Items.Count + 1 > remIdx)
                            {
                                db.Items.Insert(targetIdx, droppedData);
                                db.Items.RemoveAt(remIdx);
                            }
                        }
                    }
                }
            }
            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                VisualApartmentViewModel db = vm as VisualApartmentViewModel;
                if (sender is ListBoxItem)
                {
                    VisualBuildingApartment droppedData = e.Data.GetData(typeof(VisualBuildingApartment)) as VisualBuildingApartment;
                    VisualBuildingApartment target = ((ListBoxItem)(sender)).DataContext as VisualBuildingApartment;

                    int removedIdx = lboxVisual.Items.IndexOf(droppedData);
                    int targetIdx = lboxVisual.Items.IndexOf(target);
                    if (removedIdx != -1)
                    {
                        if (removedIdx < targetIdx)
                        {
                            db.Items.Insert(targetIdx + 1, droppedData);
                            db.Items.RemoveAt(removedIdx);
                        }
                        else
                        {
                            int remIdx = removedIdx + 1;
                            if (db.Items.Count + 1 > remIdx)
                            {
                                db.Items.Insert(targetIdx, droppedData);
                                db.Items.RemoveAt(remIdx);
                            }
                        }
                    }
                }
            }
        }
        private void VisualUserControle_Loaded(object sender, RoutedEventArgs e)
        {
            btnVisualCancel.Visibility = btnVisualSave.Visibility = Visibility.Collapsed;
            btnVisualEdit.Visibility = Visibility.Visible;
            // btnVisualSave.Visibility = btnVisualCancel.Visibility = Visibility.Collapsed;
            childImageShow.Visibility = Visibility.Collapsed;
            childImageShow.Close();

        }

        private void BtnImageClose_Click(object sender, RoutedEventArgs e)
        {
            childImageShow.DataContext = null;
            childImageShow.Visibility = Visibility.Collapsed;
            childImageShow.Close();
            //if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            //{
            //    VisualProjectLocationViewModel vmObj = this.DataContext as VisualProjectLocationViewModel;
            //    vmObj.DeleteCommand.Execute(obj); 
            //}
        }

        private void BtnImageDelete_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext;
            //childImageShow.DataContext = obj;
            //childImageShow.Visibility = Visibility.Visible;
            //childImageShow.Show();
            MessageBoxResult res = MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {

                if (vm.GetType() == typeof(VisualProjectLocationViewModel))
                {
                    var obj = childImageShow.DataContext as VisualProjectLocationPhoto;

                    VisualProjectLocationViewModel vmObj = this.DataContext as VisualProjectLocationViewModel;
                    vmObj.DeleteCommand.Execute(obj);
                    childImageShow.DataContext = null;
                    childImageShow.Visibility = Visibility.Collapsed;
                    childImageShow.Close();
                }
                if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
                {
                    var obj = childImageShow.DataContext as VisualBuildingLocationPhoto;

                    VisualBuildingLocationViewModel vmObj = this.DataContext as VisualBuildingLocationViewModel;
                    vmObj.DeleteCommand.Execute(obj);
                    childImageShow.DataContext = null;
                    childImageShow.Visibility = Visibility.Collapsed;
                    childImageShow.Close();
                }
                if (vm.GetType() == typeof(VisualApartmentViewModel))
                {
                    var obj = childImageShow.DataContext as VisualApartmentLocationPhoto;

                    VisualApartmentViewModel vmObj = this.DataContext as VisualApartmentViewModel;
                    vmObj.DeleteCommand.Execute(obj);
                    childImageShow.DataContext = null;
                    childImageShow.Visibility = Visibility.Collapsed;
                    childImageShow.Close();

                }

            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

            var eventHandler = this.ClickSearch;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }

        }

        void lbox_PreviewMouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (IsDrop == true)
            {
                if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
                {
                    ListBoxItem draggedItem = sender as ListBoxItem;
                    DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                    // draggedItem.IsSelected = true;
                }
            }
        }
        public void lboxDataBinding_Drop(object sender, DragEventArgs e)
        {
            var vm = this.DataContext;

            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                VisualProjectLocationViewModel db = vm as VisualProjectLocationViewModel;
                if (sender is ListBoxItem)
                {
                    VisualProjectLocationPhoto droppedData = e.Data.GetData(typeof(VisualProjectLocationPhoto)) as VisualProjectLocationPhoto;
                    VisualProjectLocationPhoto target = ((ListBoxItem)(sender)).DataContext as VisualProjectLocationPhoto;

                    int removedIdx = lbox.Items.IndexOf(droppedData);
                    int targetIdx = lbox.Items.IndexOf(target);
                    if (removedIdx != -1)
                    {
                        if (removedIdx < targetIdx)
                        {
                            db.Images.Insert(targetIdx + 1, droppedData);
                            db.Images.RemoveAt(removedIdx);
                        }
                        else
                        {
                            int remIdx = removedIdx + 1;
                            if (db.Images.Count + 1 > remIdx)
                            {
                                db.Images.Insert(targetIdx, droppedData);
                                db.Images.RemoveAt(remIdx);
                            }
                        }
                    }
                }
            }
            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                VisualBuildingLocationViewModel db = vm as VisualBuildingLocationViewModel;
                if (sender is ListBoxItem)
                {
                    VisualBuildingLocationPhoto droppedData = e.Data.GetData(typeof(VisualBuildingLocationPhoto)) as VisualBuildingLocationPhoto;
                    VisualBuildingLocationPhoto target = ((ListBoxItem)(sender)).DataContext as VisualBuildingLocationPhoto;

                    int removedIdx = lbox.Items.IndexOf(droppedData);
                    int targetIdx = lbox.Items.IndexOf(target);
                    if (removedIdx != -1)
                    {
                        if (removedIdx < targetIdx)
                        {
                            db.Images.Insert(targetIdx + 1, droppedData);
                            db.Images.RemoveAt(removedIdx);
                        }
                        else
                        {
                            int remIdx = removedIdx + 1;
                            if (db.Images.Count + 1 > remIdx)
                            {
                                db.Images.Insert(targetIdx, droppedData);
                                db.Images.RemoveAt(remIdx);
                            }
                        }
                    }
                }
            }
            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                VisualApartmentViewModel db = vm as VisualApartmentViewModel;
                if (sender is ListBoxItem)
                {
                    VisualApartmentLocationPhoto droppedData = e.Data.GetData(typeof(VisualApartmentLocationPhoto)) as VisualApartmentLocationPhoto;
                    VisualApartmentLocationPhoto target = ((ListBoxItem)(sender)).DataContext as VisualApartmentLocationPhoto;

                    int removedIdx = lbox.Items.IndexOf(droppedData);
                    int targetIdx = lbox.Items.IndexOf(target);
                    if (removedIdx != -1)
                    {
                        if (removedIdx < targetIdx)
                        {
                            db.Images.Insert(targetIdx + 1, droppedData);
                            db.Images.RemoveAt(removedIdx);
                        }
                        else
                        {
                            int remIdx = removedIdx + 1;
                            if (db.Images.Count + 1 > remIdx)
                            {
                                db.Images.Insert(targetIdx, droppedData);
                                db.Images.RemoveAt(remIdx);
                            }
                        }
                    }
                }
            }
        }


        private void BtnDataDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDataCancel_Click(object sender, RoutedEventArgs e)
        {
            btnDataSave.Visibility = btnDataCancel.Visibility = Visibility.Collapsed;
            btnDataEdit.Visibility = Visibility.Visible;
        }

        private void BtnDataSave_Click(object sender, RoutedEventArgs e)
        {
            btnDataSave.Visibility = btnDataCancel.Visibility = Visibility.Collapsed;
            btnDataEdit.Visibility = Visibility.Visible;
        }

        private void BtnDataEdit_Click(object sender, RoutedEventArgs e)
        {
            btnDataSave.Visibility = btnDataCancel.Visibility = Visibility.Visible;
            btnDataEdit.Visibility = Visibility.Collapsed;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.BackClick;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //var eventHandler = this.ClickCancel;

            //if (eventHandler != null)
            //{
            //    eventHandler(this, e);
            //}
            IsDrop = false;
            btnSave.Visibility = btnCancel.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
        }
        public bool IsDrop { get; set; }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.ClickSave;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
            IsDrop = false;
            btnSave.Visibility = btnCancel.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //var eventHandler = this.ClickEdit;

            //if (eventHandler != null)
            //{
            //    eventHandler(this, e);
            //}
            IsDrop = true;
            btnSave.Visibility = btnCancel.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Collapsed;

        }

        private void CbProType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public List<VisualBuildingLocationPhoto> vbPhotos { get; set; }
        public List<VisualApartmentLocationPhoto> baPhotos { get; set; }
        public List<VisualProjectLocationPhoto> plPhotos { get; set; }
        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext;
            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                var obj = ((ListBoxItem)sender).DataContext as VisualProjectLocationPhoto;
                var viewModel = this.DataContext as VisualProjectLocationViewModel;

                plPhotos = viewModel.Images.ToList();
                childImageShow.DataContext = obj;
                childImageShow.Visibility = Visibility.Visible;
                childImageShow.Show();

            }

            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                var obj = ((ListBoxItem)sender).DataContext as VisualBuildingLocationPhoto;
                var viewModel = this.DataContext as VisualBuildingLocationViewModel;
                vbPhotos = viewModel.Images.ToList();
                childImageShow.DataContext = obj;
                childImageShow.Visibility = Visibility.Visible;
                childImageShow.Show();
            }

            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                var obj = ((ListBoxItem)sender).DataContext as VisualApartmentLocationPhoto;
                var viewModel = this.DataContext as VisualApartmentViewModel;
                baPhotos = viewModel.Images.ToList();
                childImageShow.DataContext = obj;
                childImageShow.Visibility = Visibility.Visible;
                childImageShow.Show();
            }

            //if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            //{
            //    VisualProjectLocationViewModel vmObj = this.DataContext as VisualProjectLocationViewModel;
            //    vmObj.DeleteCommand.Execute(obj); 
            //}

            //var obj = ((ListBoxItem)sender).DataContext as Project;
            //vm.SelectedItemCommand.Execute(obj);
        }

        private void btnVisualEdit_Click(object sender, RoutedEventArgs e)
        {
            IsDrop = true;
            btnVisualSave.Visibility = btnVisualCancel.Visibility = Visibility.Visible;
            btnVisualEdit.Visibility = Visibility.Collapsed;
        }

        private void btnVisualSave_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.ClickVisualReorder;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
            IsDrop = false;
            btnVisualSave.Visibility = btnVisualCancel.Visibility = Visibility.Collapsed;
            btnVisualEdit.Visibility = Visibility.Visible;
        }

        private void btnVisualCancel_Click(object sender, RoutedEventArgs e)
        {
            btnVisualCancel.Visibility = btnVisualSave.Visibility = Visibility.Collapsed;
            btnVisualEdit.Visibility = Visibility.Visible;
        }

        private void btnEditVisualReport_Click(object sender, RoutedEventArgs e)
        {
            VisualReportEditor.Visibility = Visibility.Visible;
        }
        //abhinov
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ErrorModel err = new ErrorModel();
            VisualReportEditor.Visibility = Visibility.Collapsed;
            var vm = this.DataContext;

            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                VisualProjectLocationViewModel db = vm as VisualProjectLocationViewModel;

                var dc = VisualReportEditor.DataContext as EditVisualReportViewModel;
                dc.ProjLocation.AdditionalConsideration = dc.AdditionalConsideration;
                dc.ProjLocation.Name = dc.Title;
                dc.ProjLocation.VisualReview = dc.RadioList_VisualReviewItems.Where(c => c.IsChecked == true).Single().Name;
                dc.ProjLocation.AnyVisualSign = dc.RadioList_AnyVisualSignItems.Where(c => c.IsChecked == true).Single().Name;

                dc.ProjLocation.FurtherInasive = dc.RadioList_FurtherInasiveItems.Where(c => c.IsChecked == true).Single().Name;

                dc.ProjLocation.ConditionAssessment = dc.RadioList_ConditionAssessment.Where(c => c.IsChecked == true).Single().Name;

                dc.ProjLocation.LifeExpectancyEEE = dc.RadioList_LifeExpectancyEEE.Where(c => c.IsChecked == true).Single().Name;

                dc.ProjLocation.LifeExpectancyLBC = dc.RadioList_LifeExpectancyLBC.Where(c => c.IsChecked == true).Single().Name;

                dc.ProjLocation.LifeExpectancyAWE = dc.RadioList_LifeExpectancyAWE.Where(c => c.IsChecked == true).Single().Name;

                db.SelectedItem = dc.ProjLocation;
                //Call save here.
                Response response = new Response();
                if (App.IsInvasive == false)
                    response = await db.VisualProjectLocationService.UpdateItemAsync(db.SelectedItem);

                if (response.Status == ApiResult.Success)
                {

                    //bool isComplet = await Task.Run(() => LongOperationGetImage(SelectedItem.Id.ToString()));
                }
                else
                {
                    err.Status = "Error";
                    err.Message = response.Message;
                }

            }
            else if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                VisualBuildingLocationViewModel db = vm as VisualBuildingLocationViewModel;

                var dc = VisualReportEditor.DataContext as EditVisualReportViewModel;
                dc.BuildingLocation.AdditionalConsideration = dc.AdditionalConsideration;
                dc.BuildingLocation.Name = dc.Title;
                dc.BuildingLocation.VisualReview = dc.RadioList_VisualReviewItems.Where(c => c.IsChecked == true).Single().Name;
                dc.BuildingLocation.AnyVisualSign = dc.RadioList_AnyVisualSignItems.Where(c => c.IsChecked == true).Single().Name;

                dc.BuildingLocation.FurtherInasive = dc.RadioList_FurtherInasiveItems.Where(c => c.IsChecked == true).Single().Name;

                dc.BuildingLocation.ConditionAssessment = dc.RadioList_ConditionAssessment.Where(c => c.IsChecked == true).Single().Name;

                dc.BuildingLocation.LifeExpectancyEEE = dc.RadioList_LifeExpectancyEEE.Where(c => c.IsChecked == true).Single().Name;

                dc.BuildingLocation.LifeExpectancyLBC = dc.RadioList_LifeExpectancyLBC.Where(c => c.IsChecked == true).Single().Name;

                dc.BuildingLocation.LifeExpectancyAWE = dc.RadioList_LifeExpectancyAWE.Where(c => c.IsChecked == true).Single().Name;

                db.SelectedItem = dc.BuildingLocation;
                //Call save here.
                Response response = new Response();
                if (App.IsInvasive == false)
                    response = await db.VisualFormBuildingLocationDataStore.UpdateItemAsync(db.SelectedItem);

                if (response.Status == ApiResult.Success)
                {

                    //bool isComplet = await Task.Run(() => LongOperationGetImage(SelectedItem.Id.ToString()));
                }
                else
                {
                    err.Status = "Error";
                    err.Message = response.Message;
                }

            }

            else if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                VisualApartmentViewModel db = vm as VisualApartmentViewModel;

                var dc = VisualReportEditor.DataContext as EditVisualReportViewModel;
                dc.AptLocation.AdditionalConsideration = dc.AdditionalConsideration;
                dc.AptLocation.Name = dc.Title;
                dc.AptLocation.VisualReview = dc.RadioList_VisualReviewItems.Where(c => c.IsChecked == true).Single().Name;
                dc.AptLocation.AnyVisualSign = dc.RadioList_AnyVisualSignItems.Where(c => c.IsChecked == true).Single().Name;

                dc.AptLocation.FurtherInasive = dc.RadioList_FurtherInasiveItems.Where(c => c.IsChecked == true).Single().Name;

                dc.AptLocation.ConditionAssessment = dc.RadioList_ConditionAssessment.Where(c => c.IsChecked == true).Single().Name;

                dc.AptLocation.LifeExpectancyEEE = dc.RadioList_LifeExpectancyEEE.Where(c => c.IsChecked == true).Single().Name;

                dc.AptLocation.LifeExpectancyLBC = dc.RadioList_LifeExpectancyLBC.Where(c => c.IsChecked == true).Single().Name;

                dc.AptLocation.LifeExpectancyAWE = dc.RadioList_LifeExpectancyAWE.Where(c => c.IsChecked == true).Single().Name;

                db.SelectedItem = dc.AptLocation;
                //Call save here.
                Response response = new Response();
                if (App.IsInvasive == false)
                    response = await db.VisualFormApartmentDataStore.UpdateItemAsync(db.SelectedItem);

                if (response.Status == ApiResult.Success)
                {

                    //bool isComplet = await Task.Run(() => LongOperationGetImage(SelectedItem.Id.ToString()));
                }
                else
                {
                    err.Status = "Error";
                    err.Message = response.Message;
                }

            }
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
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    pics.Add(System.IO.Path.GetFullPath(filename));

               
                var passed= await UploadGallary(pics);
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
            parameters.Add("UserID", App.LogUser.Id.ToString());


            if (App.IsInvasive == true)
            {

                parameters.Add("IsInvaiveImage", "TRUE");
                // parameters.Add("IsInvasive", "FALSE");
            }
            else
            {
                parameters.Add("IsInvaiveImage", null);
                //  parameters.Add("IsInvasive", "FALSE");

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
            parameters.Add("UserID", App.LogUser.Id.ToString());


            if (App.IsInvasive == true)
            {

                parameters.Add("IsInvaiveImage", "TRUE");
                // parameters.Add("IsInvasive", "FALSE");
            }
            else
            {
                parameters.Add("IsInvaiveImage", null);
                //  parameters.Add("IsInvasive", "FALSE");

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
            parameters.Add("ImageDescription", "False");
            parameters.Add("UserID", App.LogUser.Id.ToString());


            if (App.IsInvasive == true)
            {

                parameters.Add("IsInvaiveImage", "TRUE");
                // parameters.Add("IsInvasive", "FALSE");
            }
            else
            {
                parameters.Add("IsInvaiveImage", null);
                //  parameters.Add("IsInvasive", "FALSE");

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

        private void closeme_Click(object sender, RoutedEventArgs e)
        {
            VisualReportEditor.Visibility = Visibility.Collapsed;
        }
    }
}
