using Microsoft.Win32;
using Prism.Services.Dialogs;
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
using UI.Code.Services;
using UI.Code.ViewModel;

namespace UI.Code.View
{
    /// <summary>
    /// Interaction logic for ProjectsPageView.xaml
    /// </summary>
    public partial class BuildingPageView : UserControl
    {
        BuildingViewModel vm;
        public BuildingPageView()
        {
            InitializeComponent();
            UCAddEdit.IsAddress = false;
            UCAddEdit.ClickSave += UCAddEdit_ClickSave;
            UCAddEdit.ClickBack += UCAddEdit_ClickBack;
            UCAddEdit.ClickDel += UCAddEdit_ClickDel;
            UCAddEdit.ClickImageUpload += UCAddEdit_ClickImageUpload;
            vm = this.DataContext as BuildingViewModel;
            //  lboxBuildingLocation.SelectionChanged += LboxBuildingLocation_SelectionChanged;
            //  lboxBuildingApartment.SelectionChanged += LboxBuildingApartment_SelectionChanged;
            //   DataContext = new ProjectsPageViewModel();
            // List<TypeList> projectTypelist = new List<TypeList>();
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual3" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual4" });
            //CbProType.ItemsSource = projectTypelist;
            // this.Loaded += ProjectsPageView_Loaded;
            this.Loaded += BuildingPageView_Loaded;
            UCAddEdit.IsEdit = false;
        }
        private async void UCAddEdit_ClickImageUpload(object sender, EventArgs e)
        {
            //browse select image
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                var localPath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                var response = await DataUploadService.UploadFile(localPath, vm.Project.Name, $"api/Project/AddImage");
                if (response.Status != ApiResult.Fail)
                {
                    vm.ProjectBuilding.ImageUrl = response.Data.ToString();
                }

            }
        }
        private  void UCAddEdit_ClickDel(object sender, EventArgs e)
        {
           
            childDeleteConfirmation.Visibility = Visibility.Visible;
            childDeleteConfirmation.Show();
        }

        private void UCAddEdit_ClickBack(object sender, EventArgs e)
        {
            UCAddEdit.IsEdit=true;
            ProjectBuilding old = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectBuilding>(vm.ObjectString);
            bool b = false;
            if (old.Name == vm.DataModel.Name && old.Description == vm.DataModel.Description)
            {
                b = false;
            }
            else
            {
                b = true;
            }
            if (b == true)
            {
                ErrorModel err = new ErrorModel() { Status = "Warning", Message = "Are you sure you want to discard the changes." };
                childWindowMessageBox.DataContext = err;
                childWindowMessageBox.Visibility = Visibility.Visible;
                //MessageBoxResult res=  MessageBox.Show("Are you sure you want to discard the changes","Warning",MessageBoxButton.YesNo);
                //  if(res==MessageBoxResult.Yes)
                //  {
                //      vm.BackCommand.Execute();
                //  }

            }
            else
            {
                vm.BackCommand.Execute();
            }
        }

        private async void UCAddEdit_ClickSave(object sender, EventArgs e)
        {
            ErrorModel err = await vm.Save();
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
        }

        private void BuildingPageView_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
            childDeleteConfirmation.Visibility = Visibility.Collapsed;
            childDeleteConfirmation.Close();

            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }

        public bool IsDrop { get; set; }
        private void LboxBuildingLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (IsDrop == false)
            //    vm.BuildingLocationSelectedItemCommand.Execute();
        }

        private void LboxBuildingApartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (IsDrop == false)
            //    vm.BuildingApartmentSelectedItemCommand.Execute();
        }

        //private void ProjectsPageView_Loaded(object sender, RoutedEventArgs e)
        //{
        //    childWindowFeedback.Visibility = Visibility.Collapsed;
        //    childWindowFeedback.Close();
        //}

        //private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void lvDataBinding_Drop(object sender, DragEventArgs e)
        //{
        //    ProjectsPageViewModel vm = DataContext as ProjectsPageViewModel;
        //    if (sender is ListBoxItem)
        //    {
        //        Project droppedData = e.Data.GetData(typeof(Project)) as Project;
        //        Project target = ((ListBoxItem)(sender)).DataContext as Project;

        //        int removedIdx = lvDataBinding.Items.IndexOf(droppedData);
        //        int targetIdx = lvDataBinding.Items.IndexOf(target);

        //        if (removedIdx < targetIdx)
        //        {
        //            vm.Projects.Insert(targetIdx + 1, droppedData);
        //            vm.Projects.RemoveAt(removedIdx);
        //        }
        //        else
        //        {
        //            int remIdx = removedIdx + 1;
        //            if (vm.Projects.Count + 1 > remIdx)
        //            {
        //                vm.Projects.Insert(targetIdx, droppedData);
        //                vm.Projects.RemoveAt(remIdx);
        //            }
        //        }
        //    }
        //}
        //void s_PreviewMouseMoveEvent(object sender, MouseEventArgs e)
        //{
        //    //if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
        //    //{
        //    //    ListBoxItem draggedItem = sender as ListBoxItem;
        //    //    DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
        //    //    draggedItem.IsSelected = true;
        //    //}
        //}

        //private void btnNew_Click(object sender, RoutedEventArgs e)
        //{
        //    childWindowFeedback.Visibility = Visibility.Visible;
        //    childWindowFeedback.Show();
        //}

        //private void btnClose_Click(object sender, RoutedEventArgs e)
        //{
        //    childWindowFeedback.Visibility = Visibility.Collapsed;
        //    childWindowFeedback.Close();
        //}


        private void lboxBuildingLocation_Drop(object sender, DragEventArgs e)
        {
            BuildingViewModel vm = DataContext as BuildingViewModel;
            if (sender is ListBoxItem)
            {
                BuildingLocation droppedData = e.Data.GetData(typeof(BuildingLocation)) as BuildingLocation;
                BuildingLocation target = ((ListBoxItem)(sender)).DataContext as BuildingLocation;

                int removedIdx = lboxBuildingLocation.Items.IndexOf(droppedData);
                int targetIdx = lboxBuildingLocation.Items.IndexOf(target);
                if (removedIdx != -1)
                {
                    if (removedIdx < targetIdx)
                    {
                        vm.BuildingLocations.Insert(targetIdx + 1, droppedData);
                        vm.BuildingLocations.RemoveAt(removedIdx);
                    }
                    else
                    {
                        int remIdx = removedIdx + 1;
                        if (vm.BuildingLocations.Count + 1 > remIdx)
                        {
                            vm.BuildingLocations.Insert(targetIdx, droppedData);
                            vm.BuildingLocations.RemoveAt(remIdx);
                        }
                    }
                }
            }
        }
        private void lboxBuildingApartment_Drop(object sender, DragEventArgs e)
        {
         
                BuildingViewModel vm = DataContext as BuildingViewModel;
            if (sender is ListBoxItem)
            {
                BuildingApartment droppedData = e.Data.GetData(typeof(BuildingApartment)) as BuildingApartment;
                BuildingApartment target = ((ListBoxItem)(sender)).DataContext as BuildingApartment;

                int removedIdx = lboxBuildingApartment.Items.IndexOf(droppedData);
                int targetIdx = lboxBuildingApartment.Items.IndexOf(target);
                if (removedIdx != -1)
                {
                    if (removedIdx < targetIdx)
                    {
                        vm.BuildingApartments.Insert(targetIdx + 1, droppedData);
                        vm.BuildingApartments.RemoveAt(removedIdx);
                    }
                    else
                    {
                        int remIdx = removedIdx + 1;
                        if (vm.BuildingApartments.Count + 1 > remIdx)
                        {
                            vm.BuildingApartments.Insert(targetIdx, droppedData);
                            vm.BuildingApartments.RemoveAt(remIdx);
                        }
                    }
                }
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            IsDrop = true;
            btnSave.Visibility = btnCancel.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Collapsed;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BuildingViewModel vm = DataContext as BuildingViewModel;
            ErrorModel err = await vm.ReorderBuildinLocation();
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
            IsDrop = false;
            btnSave.Visibility = btnCancel.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            IsDrop = false;
            btnSave.Visibility = btnCancel.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
        }
        private void btnBuildingEdit_Click(object sender, RoutedEventArgs e)
        {
            IsDrop = true;
            btnBuildingSave.Visibility = btnBuildingCancel.Visibility = Visibility.Visible;
            btnBuildingEdit.Visibility = Visibility.Collapsed;
        }

        private async void btnBuildingSave_Click(object sender, RoutedEventArgs e)
        {
            BuildingViewModel vm = DataContext as BuildingViewModel;
            ErrorModel err = await vm.ReorderBuildingApartment();
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
            IsDrop = false;
            btnBuildingSave.Visibility = btnBuildingCancel.Visibility = Visibility.Collapsed;
            btnBuildingEdit.Visibility = Visibility.Visible;
        }

        private void btnBuildingCancel_Click(object sender, RoutedEventArgs e)
        {
            IsDrop = false;
            btnBuildingSave.Visibility = btnBuildingCancel.Visibility = Visibility.Collapsed;
            btnBuildingEdit.Visibility = Visibility.Visible;
        }
        private async void btnDeleteOk_Click(object sender, RoutedEventArgs e)
        {
            ErrorModel err = await vm.Delete();
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
            childDeleteConfirmation.Visibility = Visibility.Collapsed;
            childDeleteConfirmation.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            childDeleteConfirmation.Visibility = Visibility.Visible;
            childDeleteConfirmation.Show();
        }
        private void btnErrorClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }

        private void btnDeleteClose_Click(object sender, RoutedEventArgs e)
        {
            childDeleteConfirmation.Visibility = Visibility.Collapsed;
            childDeleteConfirmation.Close();
        }
        private void BuildingLocationHandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = ((ListBoxItem)sender).DataContext as BuildingLocation;
            vm.BuildingLocationSelectedItemCommand.Execute(obj);
        }
        private void ApartmentHandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = ((ListBoxItem)sender).DataContext as BuildingApartment;
            vm.BuildingApartmentSelectedItemCommand.Execute(obj);
        }

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAssignClose_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnBackOk_Click(object sender, RoutedEventArgs e)
        {
            UCAddEdit.IsEdit = false;
            vm.BackCommand.Execute();
        }

        private void btnBackClose_Click(object sender, RoutedEventArgs e)
        {
            UCAddEdit.IsEdit = true;
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
        }
    }
}
