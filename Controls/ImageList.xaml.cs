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

namespace UI.Code.Controls
{
    /// <summary>
    /// Interaction logic for ImageList.xaml
    /// </summary>
    public partial class ImageList : UserControl
    {
        public event EventHandler Click;
        public event EventHandler ClickReorderSave;
        public event EventHandler ClickReorderCancel;
        public event EventHandler ClickReorderEdit;
        public ImageList()
        {
            InitializeComponent();
           // btnDelete.Click += BtnDelete_Click;
            btnReorderEdit.Click += BtnEdit_Click;
            btnReorderSave.Click += BtnSave_Click;
            btnReorderCancel.Click += BtnCancel_Click;
            this.Loaded += ImageList_Loaded;
            btnImageDelete.Click += BtnImageDelete_Click;
            btnImageClose.Click += BtnImageClose_Click;
        }

        private void ImageList_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
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
            
           

            ErrorModel err = new ErrorModel() { Message = "Are you sure you want to delete?", Status = "Warning" };
            childWindowMessageBox.DataContext = err;
            childWindowMessageBox.Visibility = Visibility.Visible;
            childWindowMessageBox.Show();
            //MessageBoxResult res = MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButton.YesNo);
            //if (res == MessageBoxResult.Yes)
            //{

               

            //}
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.ClickReorderCancel;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
            IsDrop = false;
            btnReorderSave.Visibility = btnReorderCancel.Visibility = Visibility.Collapsed;
            btnReorderEdit.Visibility = Visibility.Visible;
        }
        public bool IsDrop { get; set; }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.ClickReorderSave;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
            IsDrop = false;
            btnReorderSave.Visibility = btnReorderCancel.Visibility = Visibility.Collapsed;
            btnReorderEdit.Visibility = Visibility.Visible;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.ClickReorderEdit;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
            IsDrop = true;
            btnReorderSave.Visibility = btnReorderCancel.Visibility = Visibility.Visible;
            btnReorderEdit.Visibility = Visibility.Collapsed;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.Click;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        public void lboxDataBinding_Drop(object sender, DragEventArgs e)
        {

            var vm = this.DataContext ;
            if (vm.GetType() == typeof(DetailViewProjectLocationImageViewModel))
            {
                DetailViewProjectLocationImageViewModel db = vm as DetailViewProjectLocationImageViewModel;
                if (sender is ListBoxItem)
                {
                    ProjectCommonLocationImages droppedData = e.Data.GetData(typeof(ProjectCommonLocationImages)) as ProjectCommonLocationImages;
                    ProjectCommonLocationImages target = ((ListBoxItem)(sender)).DataContext as ProjectCommonLocationImages;

                    int removedIdx = lbox.Items.IndexOf(droppedData);
                    int targetIdx = lbox.Items.IndexOf(target);
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
            else if(vm.GetType() == typeof(DetailViewBuildingLocationImageViewModel))
            {
                DetailViewBuildingLocationImageViewModel db = vm as DetailViewBuildingLocationImageViewModel;
                if (sender is ListBoxItem)
                {
                    BuildingCommonLocationImages droppedData = e.Data.GetData(typeof(BuildingCommonLocationImages)) as BuildingCommonLocationImages;
                    BuildingCommonLocationImages target = ((ListBoxItem)(sender)).DataContext as BuildingCommonLocationImages;

                    int removedIdx = lbox.Items.IndexOf(droppedData);
                    int targetIdx = lbox.Items.IndexOf(target);
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
            else if(vm.GetType() == typeof(DetailViewBuildingApartmentImageViewModel))
            {
                DetailViewBuildingApartmentImageViewModel db = vm as DetailViewBuildingApartmentImageViewModel;
                if (sender is ListBoxItem)
                {
                    BuildingApartmentImages droppedData = e.Data.GetData(typeof(BuildingApartmentImages)) as BuildingApartmentImages;
                    BuildingApartmentImages target = ((ListBoxItem)(sender)).DataContext as BuildingApartmentImages;

                    int removedIdx = lbox.Items.IndexOf(droppedData);
                    int targetIdx = lbox.Items.IndexOf(target);
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
            /// Type obj=sender.GetType();
           // var vm = this.DataContext as DataContext.GetType().Name;
           
            
        }
        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext;
            if (vm.GetType() == typeof(DetailViewProjectLocationImageViewModel))
            {
                var obj = ((ListBoxItem)sender).DataContext as ProjectCommonLocationImages;

                childImageShow.DataContext = obj;
                childImageShow.Visibility = Visibility.Visible;
                childImageShow.Show();
            }

            if (vm.GetType() == typeof(DetailViewBuildingLocationImageViewModel))
            {
                var obj = ((ListBoxItem)sender).DataContext as BuildingCommonLocationImages;

                childImageShow.DataContext = obj;
                childImageShow.Visibility = Visibility.Visible;
                childImageShow.Show();
            }

            if (vm.GetType() == typeof(DetailViewBuildingApartmentImageViewModel))
            {
                var obj = ((ListBoxItem)sender).DataContext as BuildingApartmentImages;

                childImageShow.DataContext = obj;
                childImageShow.Visibility = Visibility.Visible;
                childImageShow.Show();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBackClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
        }

        private void btnBackOk_Click(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
            var vm = this.DataContext;
            if (vm.GetType() == typeof(DetailViewProjectLocationImageViewModel))
            {
                var obj = childImageShow.DataContext as ProjectCommonLocationImages;

                DetailViewProjectLocationImageViewModel vmObj = this.DataContext as DetailViewProjectLocationImageViewModel;
                vmObj.DeleteCommand.Execute(obj);
                childImageShow.DataContext = null;
                childImageShow.Visibility = Visibility.Collapsed;
                childImageShow.Close();
            }
            if (vm.GetType() == typeof(DetailViewBuildingLocationImageViewModel))
            {
                var obj = childImageShow.DataContext as BuildingCommonLocationImages;

                DetailViewBuildingLocationImageViewModel vmObj = this.DataContext as DetailViewBuildingLocationImageViewModel;
                vmObj.DeleteCommand.Execute(obj);
                childImageShow.DataContext = null;
                childImageShow.Visibility = Visibility.Collapsed;
                childImageShow.Close();
            }
            if (vm.GetType() == typeof(DetailViewBuildingApartmentImageViewModel))
            {
                var obj = childImageShow.DataContext as BuildingApartmentImages;

                DetailViewBuildingApartmentImageViewModel vmObj = this.DataContext as DetailViewBuildingApartmentImageViewModel;
                vmObj.DeleteCommand.Execute(obj);
                childImageShow.DataContext = null;
                childImageShow.Visibility = Visibility.Collapsed;
                childImageShow.Close();

            }
        }
    }
  
}
