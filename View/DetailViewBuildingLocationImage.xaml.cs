﻿using Prism.Services.Dialogs;
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
    /// Interaction logic for ProjectsPageView.xaml
    /// </summary>
    public partial class DetailViewBuildingLocationImage : UserControl
    {
        DetailViewBuildingLocationImageViewModel vm;
        public DetailViewBuildingLocationImage()
        {
            InitializeComponent();
            imgList.ClickReorderSave += ImgList_ClickReorderSave;
            imgList.Click += ImgList_Click;
            UCAddEdit.ClickSave += UCAddEdit_ClickSave;
            UCAddEdit.ClickBack += UCAddEdit_ClickBack;
            UCAddEdit.ClickDel += UCAddEdit_ClickDel;
            vm = DataContext as DetailViewBuildingLocationImageViewModel;
            //   DataContext = new ProjectsPageViewModel();
            // List<TypeList> projectTypelist = new List<TypeList>();
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual3" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual4" });
            //CbProType.ItemsSource = projectTypelist;
            // this.Loaded += ProjectsPageView_Loaded;
            this.Loaded += DetailViewProjectLocationImage_Loaded;
        }
        private async void ImgList_ClickReorderSave(object sender, EventArgs e)
        {

            ErrorModel err = await vm.Reorder();
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();

        }
        private async void UCAddEdit_ClickDel(object sender, EventArgs e)
        {
            childDeleteConfirmation.Visibility = Visibility.Visible;
            childDeleteConfirmation.Show();
        }

        private void UCAddEdit_ClickBack(object sender, EventArgs e)
        {
            UCAddEdit.IsEdit = true;
              // bool b = UCAddEdit.IsEdit;
              BuildingLocation old = Newtonsoft.Json.JsonConvert.DeserializeObject<BuildingLocation>(vm.ObjectString);
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
        private void DetailViewProjectLocationImage_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
            
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }

        private void ImgList_Click(object sender, EventArgs e)
        {
            childDeleteConfirmation.Visibility = Visibility.Visible;
            childDeleteConfirmation.Show();
        }

        //private void ProjectsPageView_Loaded(object sender, RoutedEventArgs e)
        //{
        //    childWindowFeedback.Visibility = Visibility.Collapsed;
        //    childWindowFeedback.Close();
        //}

        //private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        //{

        //}

        public void lboxDataBinding_Drop(object sender, DragEventArgs e)
        {
            //ProjectsPageViewModel vm = DataContext as ProjectsPageViewModel;
            //if (sender is ListBoxItem)
            //{
            //    Project droppedData = e.Data.GetData(typeof(Project)) as Project;
            //    Project target = ((ListBoxItem)(sender)).DataContext as Project;

            //    int removedIdx = lvDataBinding.Items.IndexOf(droppedData);
            //    int targetIdx = lvDataBinding.Items.IndexOf(target);

            //    if (removedIdx < targetIdx)
            //    {
            //        vm.Projects.Insert(targetIdx + 1, droppedData);
            //        vm.Projects.RemoveAt(removedIdx);
            //    }
            //    else
            //    {
            //        int remIdx = removedIdx + 1;
            //        if (vm.Projects.Count + 1 > remIdx)
            //        {
            //            vm.Projects.Insert(targetIdx, droppedData);
            //            vm.Projects.RemoveAt(remIdx);
            //        }
            //    }
            //}
        }
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


        private void lboxProjectLocationDataBinding_Drop(object sender, DragEventArgs e)
        {
            //ProjectViewModel vm = DataContext as ProjectViewModel;
            //if (sender is ListBoxItem)
            //{
            //    ProjectLocation droppedData = e.Data.GetData(typeof(ProjectLocation)) as ProjectLocation;
            //    ProjectLocation target = ((ListBoxItem)(sender)).DataContext as ProjectLocation;

            //    int removedIdx = lboxProjectLocation.Items.IndexOf(droppedData);
            //    int targetIdx = lboxProjectLocation.Items.IndexOf(target);

            //    if (removedIdx < targetIdx)
            //    {
            //        vm.ProjectLocations.Insert(targetIdx + 1, droppedData);
            //        vm.ProjectLocations.RemoveAt(removedIdx);
            //    }
            //    else
            //    {
            //        int remIdx = removedIdx + 1;
            //        if (vm.ProjectLocations.Count + 1 > remIdx)
            //        {
            //            vm.ProjectLocations.Insert(targetIdx, droppedData);
            //            vm.ProjectLocations.RemoveAt(remIdx);
            //        }
            //    }
            //}
        }
        private void lboxBuildingLocation_Drop(object sender, DragEventArgs e)
        {
            //DetailtViewModel vm = DataContext as DetailtViewModel;
            //if (sender is ListBoxItem)
            //{
            //    ProjectBuilding droppedData = e.Data.GetData(typeof(ProjectBuilding)) as ProjectBuilding;
            //    ProjectBuilding target = ((ListBoxItem)(sender)).DataContext as ProjectBuilding;

            //    int removedIdx = lbox.Items.IndexOf(droppedData);
            //    int targetIdx = lbox.Items.IndexOf(target);

            //    if (removedIdx < targetIdx)
            //    {
            //        vm.ProjectBuildings.Insert(targetIdx + 1, droppedData);
            //        vm.ProjectBuildings.RemoveAt(removedIdx);
            //    }
            //    else
            //    {
            //        int remIdx = removedIdx + 1;
            //        if (vm.ProjectBuildings.Count + 1 > remIdx)
            //        {
            //            vm.ProjectBuildings.Insert(targetIdx, droppedData);
            //            vm.ProjectBuildings.RemoveAt(remIdx);
            //        }
            //    }
            //}
        }
        void lbox_PreviewMouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                // draggedItem.IsSelected = true;
            }
        }
        //private void BtnEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    IsDrop = true;
        //    btnSave.Visibility = btnCancel.Visibility = Visibility.Visible;
        //    btnEdit.Visibility = Visibility.Collapsed;
        //}

        //private async void BtnSave_Click(object sender, RoutedEventArgs e)
        //{
        //    ProjectViewModel vm = DataContext as ProjectViewModel;
        //    ErrorModel err = await vm.Reorder();
        //    childWindowFeedback.DataContext = err;
        //    childWindowFeedback.Visibility = Visibility.Visible;
        //    childWindowFeedback.Show();
        //    IsDrop = false;
        //    btnSave.Visibility = btnCancel.Visibility = Visibility.Collapsed;
        //    btnEdit.Visibility = Visibility.Visible;
        //}

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //IsDrop = false;
            //btnSave.Visibility = btnCancel.Visibility = Visibility.Collapsed;
            //btnEdit.Visibility = Visibility.Visible;
        }

        private void LboxBuildingLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if (IsDrop == false)
            // vm.ProjBuildingSelectedItemCommand.Execute();
        }

        private void btnBuildingEdit_Click(object sender, RoutedEventArgs e)
        {
            //IsDrop = true;
            //btnBuildingSave.Visibility = btnBuildingCancel.Visibility = Visibility.Visible;
            //btnBuildingEdit.Visibility = Visibility.Collapsed;
        }

        private void btnBuildingSave_Click(object sender, RoutedEventArgs e)
        {
            //IsDrop = false;
            //btnBuildingSave.Visibility = btnBuildingCancel.Visibility = Visibility.Collapsed;
            //btnBuildingEdit.Visibility = Visibility.Visible;
        }

        private void btnBuildingCancel_Click(object sender, RoutedEventArgs e)
        {
            //IsDrop = false;
            //btnBuildingSave.Visibility = btnBuildingCancel.Visibility = Visibility.Collapsed;
            //btnBuildingEdit.Visibility = Visibility.Visible;
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
