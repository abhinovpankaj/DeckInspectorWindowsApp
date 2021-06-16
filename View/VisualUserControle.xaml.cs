using System;
using System.Collections.Generic;
using System.Linq;
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
using UI.Code.ViewModel;

namespace UI.Code.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class VisualUserControle : UserControl
    {
        public event EventHandler BackClick;
        public event EventHandler ClickSave;
        // public event EventHandler ClickCancel;
        public event EventHandler ClickEdit;
        public event EventHandler ClickSearch;

        public event EventHandler InvasiveClickSave;

        public event EventHandler InvasiveClickEdit;

        public event EventHandler ClickVisualReorder;
        public event EventHandler DoubleClickOnImage;
        // public event EventHandler ClickEdit;
        public VisualUserControle()
        {
            InitializeComponent();
            btnEdit.Click += BtnEdit_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnSaveInvasive.Click += BtnSaveInvasive_Click;
            btnCancelInvasive.Click += BtnCancelInvasive_Click;
            btnEditInvasive.Click += BtnEditInvasive_Click;
            //btnBack.Click += BtnBack_Click;

            btnDataEdit.Click += BtnDataEdit_Click;
            btnDataSave.Click += BtnDataSave_Click; ;
            btnDataCancel.Click += BtnDataCancel_Click; ;
            btnDataDelete.Click += BtnDataDelete_Click;
            //    btnSearch.Click += BtnSearch_Click;
            btnImageDelete.Click += BtnImageDelete_Click;
            btnImageClose.Click += BtnImageClose_Click;
            this.Loaded += VisualUserControle_Loaded;
            rbTab1.IsChecked = true;
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
        public List<VisualBuildingLocationPhoto> vbPhotos { get; set; }
        public List<VisualApartmentLocationPhoto> baPhotos { get; set; }
        public List<VisualProjectLocationPhoto> plPhotos { get; set; }
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

        private void BtnEditInvasive_Click(object sender, RoutedEventArgs e)
        {
            IsDrop = true;
            btnSaveInvasive.Visibility = btnCancelInvasive.Visibility = Visibility.Visible;
            btnEditInvasive.Visibility = Visibility.Collapsed;
        }

        private void BtnCancelInvasive_Click(object sender, RoutedEventArgs e)
        {
            btnCancelInvasive.Visibility = btnSaveInvasive.Visibility = Visibility.Collapsed;
            btnEditInvasive.Visibility = Visibility.Visible;
        }

        private void BtnSaveInvasive_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.InvasiveClickSave;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
            IsDrop = false;
            btnSaveInvasive.Visibility = btnCancelInvasive.Visibility = Visibility.Collapsed;
            btnEditInvasive.Visibility = Visibility.Visible;
        }

        private void VisualDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext;

            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                VisualProjectLocationViewModel db = vm as VisualProjectLocationViewModel;
                var obj = ((ListBoxItem)sender).DataContext as VisualProjectLocation;
                db.GetImagesCommand.Execute(obj);
            }
            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                VisualBuildingLocationViewModel db = vm as VisualBuildingLocationViewModel;
                var obj = ((ListBoxItem)sender).DataContext as VisualBuildingLocation;
                db.GetImagesCommand.Execute(obj);
            }
            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                VisualApartmentViewModel db = vm as VisualApartmentViewModel;
                var obj = ((ListBoxItem)sender).DataContext as VisualBuildingApartment;
                db.GetImagesCommand.Execute(obj);
            }
            //if (FTitleDes.Text == "Yes")
            //{
            //    FTitle.Foreground = FTitleDes.Foreground = new SolidColorBrush(Colors.DarkRed);
            //}
            //else
            //{
            //    FTitle.Foreground = FTitleDes.Foreground = new SolidColorBrush(Colors.DarkGreen);
            //}
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

        public void lboxInvasiveDataBinding_Drop(object sender, DragEventArgs e)
        {
            var vm = this.DataContext;

            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                VisualProjectLocationViewModel db = vm as VisualProjectLocationViewModel;
                if (sender is ListBoxItem)
                {
                    VisualProjectLocationPhoto droppedData = e.Data.GetData(typeof(VisualProjectLocationPhoto)) as VisualProjectLocationPhoto;
                    VisualProjectLocationPhoto target = ((ListBoxItem)(sender)).DataContext as VisualProjectLocationPhoto;

                    int removedIdx = lboxInvasive.Items.IndexOf(droppedData);
                    int targetIdx = lboxInvasive.Items.IndexOf(target);
                    if (removedIdx != -1)
                    {
                        if (removedIdx < targetIdx)
                        {
                            db.InvasiveImgs.Insert(targetIdx + 1, droppedData);
                            db.InvasiveImgs.RemoveAt(removedIdx);
                        }
                        else
                        {
                            int remIdx = removedIdx + 1;
                            if (db.InvasiveImgs.Count + 1 > remIdx)
                            {
                                db.InvasiveImgs.Insert(targetIdx, droppedData);
                                db.InvasiveImgs.RemoveAt(remIdx);
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

                    int removedIdx = lboxInvasive.Items.IndexOf(droppedData);
                    int targetIdx = lboxInvasive.Items.IndexOf(target);
                    if (removedIdx != -1)
                    {
                        if (removedIdx < targetIdx)
                        {
                            db.InvasiveImgs.Insert(targetIdx + 1, droppedData);
                            db.InvasiveImgs.RemoveAt(removedIdx);
                        }
                        else
                        {
                            int remIdx = removedIdx + 1;
                            if (db.InvasiveImgs.Count + 1 > remIdx)
                            {
                                db.InvasiveImgs.Insert(targetIdx, droppedData);
                                db.InvasiveImgs.RemoveAt(remIdx);
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

                    int removedIdx = lboxInvasive.Items.IndexOf(droppedData);
                    int targetIdx = lboxInvasive.Items.IndexOf(target);
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
                            if (db.InvasiveImgs.Count + 1 > remIdx)
                            {
                                db.InvasiveImgs.Insert(targetIdx, droppedData);
                                db.InvasiveImgs.RemoveAt(remIdx);
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
            btnSaveInvasive.Visibility = btnCancelInvasive.Visibility = Visibility.Collapsed;
            btnEditInvasive.Visibility = Visibility.Visible;
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


        private void HandleDoubleClick_Invasive(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext;
            if (vm.GetType() == typeof(VisualProjectLocationViewModel))
            {
                var obj = ((ListBoxItem)sender).DataContext as VisualProjectLocationPhoto;
                var viewModel = this.DataContext as VisualProjectLocationViewModel;

                plPhotos = viewModel.InvasiveImgs.ToList();


                childImageShow.DataContext = obj;

                childImageShow.Visibility = Visibility.Visible;
                childImageShow.Show();
            }

            if (vm.GetType() == typeof(VisualBuildingLocationViewModel))
            {
                var obj = ((ListBoxItem)sender).DataContext as VisualBuildingLocationPhoto;
                var viewModel = this.DataContext as VisualBuildingLocationViewModel;


                vbPhotos = viewModel.InvasiveImgs.ToList();

                childImageShow.DataContext = obj;
                childImageShow.Visibility = Visibility.Visible;
                childImageShow.Show();
            }

            if (vm.GetType() == typeof(VisualApartmentViewModel))
            {
                var obj = ((ListBoxItem)sender).DataContext as VisualApartmentLocationPhoto;
                var viewModel = this.DataContext as VisualApartmentViewModel;



                baPhotos = viewModel.InvasiveImgs.ToList();
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
        //private void BtnDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    var eventHandler = this.Click;

        //    if (eventHandler != null)
        //    {
        //        eventHandler(this, e);
        //    }
        //}

    }
}
