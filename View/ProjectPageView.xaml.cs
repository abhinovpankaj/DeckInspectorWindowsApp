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
using UI.Code.ViewModel;

namespace UI.Code.View
{
    /// <summary>
    /// Interaction logic for ProjectsPageView.xaml
    /// </summary>
    public partial class ProjectPageView : UserControl


    {
        ProjectViewModel vm;
        public ProjectPageView()
        {
            InitializeComponent();
            UCAddEdit.IsAddress = true;
            UCAddEdit.ClickSave += UCAddEdit_ClickSave;
            UCAddEdit.ClickBack += UCAddEdit_ClickBack;
            UCAddEdit.ClickDel += UCAddEdit_ClickDel;
            UCAddEdit.ClickInvasive += UCAddEdit_ClickInvasive;
            UCAddEdit.ClickExport += UCAddEdit_ClickExport;
            vm = this.DataContext as ProjectViewModel;
            lboxProjectLocation.SelectionChanged += LboxProjectLocation_SelectionChanged;

            
            this.Loaded += ProjectPageView_Loaded;
            assignControl.ClickUserSearch += AssignControl_ClickUserSearch;
            assignControl.ClickUserReset += AssignControl_ClickUserReset;
        }

        private void UCAddEdit_ClickExport(object sender, EventArgs e)
        {
            //string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/ExcelDownload?projectID=" + vm.Project.Id );
        }

        private async void UCAddEdit_ClickInvasive(object sender, EventArgs e)
        {
            ErrorModel err = await vm.CreateInvasive();
            if (err.Status != "Success")
            {
                childWindowFeedback.DataContext = err;
                childWindowFeedback.Visibility = Visibility.Visible;
                childWindowFeedback.Show();
            }
        }

        private async void AssignControl_ClickUserReset(object sender, EventArgs e)
        {
            vm.UserSearch = null;
            vm.IsBusy = true;
           
            if (childWindowAssign.Tag.ToString() == "Location")
            {
                bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.Project.Id, vm.ProjectLocation.Id, string.Empty, "ProjectLocation", vm.UserSearch);
                if (complete == true)
                {

                }
            }
            if (childWindowAssign.Tag.ToString() == "Building")
            {
                bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.Project.Id, string.Empty, vm.ProjectBuilding.Id, "Building", vm.UserSearch);
                if (complete == true)
                {

                }
            }
            vm.IsBusy = false;
        }

        private async void AssignControl_ClickUserSearch(object sender, EventArgs e)
        {
            vm.IsBusy = true;

            if (childWindowAssign.Tag.ToString() == "Location")
            {

                if (string.IsNullOrEmpty(assignControl.AssignSearchText))
                {
                    // bool complete = await vm.GetUserListForAssign(App.LogUser.Id, assignControl.AssignProjectID, string.Empty, string.Empty, "Project", assignControl.AssignSearchText);
                    bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.Project.Id, vm.ProjectLocation.Id, string.Empty, "ProjectLocation", vm.UserSearch);
                    if (complete == true)
                    {
                        vm.IsBusy = false;
                    }
                }
                else
                {
                    bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.Project.Id, vm.ProjectLocation.Id, string.Empty, "ProjectLocation", vm.UserSearch);
                    if (complete == true)
                    {
                        vm.UsersAssignList = new System.Collections.ObjectModel.ObservableCollection<User>(vm.UsersAssignList.Where(c => c.FullName.ToUpper().Contains(assignControl.AssignSearchText.ToUpper())));
                        vm.IsBusy = false;
                    }
                    
                }
              //  bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.Project.Id, vm.ProjectLocation.Id, string.Empty, "ProjectLocation", vm.UserSearch);
                //if (complete == true)
                //{

                //}
            }
            if (childWindowAssign.Tag.ToString() == "Building")
            {

                if (string.IsNullOrEmpty(assignControl.AssignSearchText))
                {
                    // bool complete = await vm.GetUserListForAssign(App.LogUser.Id, assignControl.AssignProjectID, string.Empty, string.Empty, "Project", assignControl.AssignSearchText);
                    bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.Project.Id, string.Empty, vm.ProjectBuilding.Id, "Building", vm.UserSearch);
                    if (complete == true)
                    {
                        vm.IsBusy = false;
                    }
                }
                else
                {
                    bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.Project.Id, string.Empty, vm.ProjectBuilding.Id, "Building", vm.UserSearch);
                    if (complete == true)
                    {
                        vm.UsersAssignList = new System.Collections.ObjectModel.ObservableCollection<User>(vm.UsersAssignList.Where(c => c.FullName.ToUpper().Contains(assignControl.AssignSearchText.ToUpper())));
                        vm.IsBusy = false;
                    }
                  
                }
                //bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.Project.Id, string.Empty, vm.ProjectBuilding.Id, "Building", vm.UserSearch);
                //if (complete == true)
                //{

                //}
            }
            vm.IsBusy = false;
        }
        private async void UCAddEdit_ClickDel(object sender, EventArgs e)
        {
            childDeleteConfirmation.Visibility = Visibility.Visible;
            childDeleteConfirmation.Show();
        }

        private void UCAddEdit_ClickBack(object sender, EventArgs e)
        {
            UCAddEdit.IsEdit = true;
            Project old= Newtonsoft.Json.JsonConvert.DeserializeObject<Project>(vm.ObjectString);
            bool b = false;
            if (old.Name == vm.DataModel.Name&& old.Address == vm.DataModel.Address&& old.Description == vm.DataModel.Description)
            {
                b = false;
            }
            else
            {
                b = true;
            }

            //vm.Compare();
            // bool b = vm.Compare(vm.DataModel,vm.Project); 
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
            else {
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

        public bool IsEdit { get; set; }
        private void BtnDataCancel_Click(object sender, RoutedEventArgs e)
        {
            IsEdit = false;
        }

        private void BtnDataSave_Click(object sender, RoutedEventArgs e)
        {
            IsEdit = false;
        }

        private void BtnDataEdit_Click(object sender, RoutedEventArgs e)
        {
            IsEdit = true;
        }

        private void ProjectPageView_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();

            
            childDeleteConfirmation.Visibility = Visibility.Collapsed;
            childDeleteConfirmation.Close();

            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }

        //private void sss_MouseDown(object sender, MouseButtonEventArgs e)
        // {
        //     MessageBox.Show("3   ->>>>>" + ((Control)sender).Name);
        // }

        void s_PreviewMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            //Button button = (Button)sender;
            //if (sender is Button)
            //{
            //    //button.ClearValue(Button.BackgroundProperty);
            //    button.Background = Brushes.Green;
            //}
           // vm.ProjLocationSelectedItemCommand.Execute();
        }
            private void LboxBuildingLocation_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
          //  if (IsDrop == false)
               // vm.ProjBuildingSelectedItemCommand.Execute();
        }

        private void LboxProjectLocation_Drop(object sender, DragEventArgs e)
        {
            ProjectViewModel vm = DataContext as ProjectViewModel;
            if (sender is ListBoxItem)
            {
                ProjectLocation droppedData = e.Data.GetData(typeof(ProjectLocation)) as ProjectLocation;
                ProjectLocation target = ((ListBoxItem)(sender)).DataContext as ProjectLocation;

                int removedIdx = lboxProjectLocation.Items.IndexOf(droppedData);
                int targetIdx = lboxProjectLocation.Items.IndexOf(target);

                if (removedIdx < targetIdx)
                {
                    vm.ProjectLocations.Insert(targetIdx + 1, droppedData);
                    vm.ProjectLocations.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (vm.ProjectLocations.Count + 1 > remIdx)
                    {
                        vm.ProjectLocations.Insert(targetIdx, droppedData);
                        vm.ProjectLocations.RemoveAt(remIdx);
                    }
                }
            }
        }

        private void LboxProjectLocation_PreviewMouseMove(object sender, MouseEventArgs e)
        {
           object t= e.Source;
            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                // draggedItem.IsSelected = true;
            }
        }

        public bool IsDrop { get; set; }
        private void LboxProjectLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if(IsDrop==false)
            //vm.ProjLocationSelectedItemCommand.Execute();
        }
     

        private T FindVisualParent<T>(DependencyObject child)
           where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            return FindVisualParent<T>(parentObject);
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


        private void lboxProjectLocationDataBinding_Drop(object sender, DragEventArgs e)
        {
            ProjectViewModel vm = DataContext as ProjectViewModel;
            if (sender is ListBoxItem)
            {
                ProjectLocation droppedData = e.Data.GetData(typeof(ProjectLocation)) as ProjectLocation;
                ProjectLocation target = ((ListBoxItem)(sender)).DataContext as ProjectLocation;

                int removedIdx = lboxProjectLocation.Items.IndexOf(droppedData);
                int targetIdx = lboxProjectLocation.Items.IndexOf(target);

                if (removedIdx < targetIdx)
                {
                    vm.ProjectLocations.Insert(targetIdx + 1, droppedData);
                    vm.ProjectLocations.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (vm.ProjectLocations.Count + 1 > remIdx)
                    {
                        vm.ProjectLocations.Insert(targetIdx, droppedData);
                        vm.ProjectLocations.RemoveAt(remIdx);
                    }
                }
            }
        }
        private void lboxBuildingLocation_Drop(object sender, DragEventArgs e)
        {
            ProjectViewModel vm = DataContext as ProjectViewModel;
            if (sender is ListBoxItem)
            {
                ProjectBuilding droppedData = e.Data.GetData(typeof(ProjectBuilding)) as ProjectBuilding;
                ProjectBuilding target = ((ListBoxItem)(sender)).DataContext as ProjectBuilding;

                int removedIdx = lboxBuildingLocation.Items.IndexOf(droppedData);
                int targetIdx = lboxBuildingLocation.Items.IndexOf(target);

                if (removedIdx < targetIdx)
                {
                    vm.ProjectBuildings.Insert(targetIdx + 1, droppedData);
                    vm.ProjectBuildings.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (vm.ProjectBuildings.Count + 1 > remIdx)
                    {
                        vm.ProjectBuildings.Insert(targetIdx, droppedData);
                        vm.ProjectBuildings.RemoveAt(remIdx);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //lboxProjectLocation.AllowDrop = true;
            // lboxProjectLocation.SelectionChanged -= LboxProjectLocation_SelectionChanged;
           
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            IsDrop = true;
            btnSave.Visibility = btnCancel.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Collapsed;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ProjectViewModel vm = DataContext as ProjectViewModel;
            ErrorModel err= await vm.ReorderProjectLocation();
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
            IsDrop = false;
            btnSave.Visibility = btnCancel.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsDrop = false;
            btnSave.Visibility = btnCancel.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
        }

        private void LboxBuildingLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // if (IsDrop == false)
               // vm.ProjBuildingSelectedItemCommand.Execute();
        }

        private void btnBuildingEdit_Click(object sender, RoutedEventArgs e)
        {
            IsDrop = true;
            btnBuildingSave.Visibility = btnBuildingCancel.Visibility = Visibility.Visible;
            btnBuildingEdit.Visibility = Visibility.Collapsed;
        }

        private async void btnBuildingSave_Click(object sender, RoutedEventArgs e)
        {
            ProjectViewModel vm = DataContext as ProjectViewModel;
            ErrorModel err = await vm.ReorderProjectBuilding();
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
            ErrorModel err=await vm.Delete();
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
        private void LocationHandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = ((ListBoxItem)sender).DataContext as ProjectLocation;
            vm.ProjLocationSelectedItemCommand.Execute(obj);
        }
        private void BuildingHandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = ((ListBoxItem)sender).DataContext as ProjectBuilding;
            vm.ProjBuildingSelectedItemCommand.Execute(obj);
        }

        private async void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            vm.IsBusy = true;
            if (childWindowAssign.Tag.ToString() == "Location")
            {
                ErrorModel err = await vm.AssignLocation();
                childWindowAssign.Visibility = Visibility.Collapsed;
                childWindowAssign.Close();
                childWindowFeedback.DataContext = err;
                childWindowFeedback.Visibility = Visibility.Visible;
                childWindowFeedback.Show();
                vm.ReloadLocation();
              //  MessageBox.Show(err.Message, err.Status, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (childWindowAssign.Tag.ToString() == "Building")
            {
                ErrorModel err = await vm.AssignBuilding();
                childWindowAssign.Visibility = Visibility.Collapsed;
                childWindowAssign.Close();
                childWindowFeedback.DataContext = err;
                childWindowFeedback.Visibility = Visibility.Visible;
                childWindowFeedback.Show();
                vm.ReloadBuilding();
                // MessageBox.Show(err.Message, err.Status, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            vm.IsBusy = false;
        }

        private void btnAssignClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowAssign.Visibility = Visibility.Collapsed;
            childWindowAssign.Close();

        }
        private async void btnLocationAgging_click(object sender, RoutedEventArgs e)
        {
            vm.UserSearch = null;
            vm.IsBusy = true;
            ProjectLocation p = ((Button)sender).DataContext as ProjectLocation;
            vm.ProjectLocation = p;
            bool complete = await vm.GetUserListForAssign(App.LogUser.Id, p.ProjectId, p.Id, string.Empty, "ProjectLocation",vm.UserSearch);
            if (complete == true)
            {
                vm.IsBusy = false;
                childWindowAssign.Tag = "Location";
                childWindowAssign.Caption = "Assign project common locatation - " + p.Name + " to user(s)";
                childWindowAssign.FontWeight = FontWeights.Bold;
                childWindowAssign.Visibility = Visibility.Visible;
                childWindowAssign.Show();
            }

            //   ListBoxItem selectedItem = (ListBoxItem)lboxProjectLocation.ItemContainerGenerator.
            //                  ContainerFromItem(((Button)sender).DataContext);
            //selectedItem.IsSelected = true;
            //var obj = ((ListBoxItem)sender).DataContext as ProjectBuilding;
            //vm.ProjBuildingSelectedItemCommand.Execute(obj);
        }
        private async void btnBuildingAgging_click(object sender, RoutedEventArgs e)
        {
            vm.UserSearch = null;
            vm.IsBusy = true;
            ProjectBuilding p = ((Button)sender).DataContext as ProjectBuilding;
            vm.ProjectBuilding = p;
            bool complete = await vm.GetUserListForAssign(App.LogUser.Id, p.ProjectId,string.Empty, p.Id, "Building", vm.UserSearch);
            if (complete == true)
            {
                vm.IsBusy = false;
                childWindowAssign.Tag = "Building";
                childWindowAssign.Caption = "Assign building - " + p.Name + " to user(s)";
                childWindowAssign.FontWeight = FontWeights.Bold;
                childWindowAssign.Visibility = Visibility.Visible;
                childWindowAssign.Show();
            }

            //   ListBoxItem selectedItem = (ListBoxItem)lboxProjectLocation.ItemContainerGenerator.
            //                  ContainerFromItem(((Button)sender).DataContext);
            //selectedItem.IsSelected = true;
            //var obj = ((ListBoxItem)sender).DataContext as ProjectBuilding;
            //vm.ProjBuildingSelectedItemCommand.Execute(obj);
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
