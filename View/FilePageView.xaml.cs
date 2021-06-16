using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for FilePageView.xaml
    /// </summary>
    public partial class FilePageView : UserControl
    {
        public event EventHandler ClickMove;
        FilePageViewModel vm;
        
        public FilePageView()
        {
            InitializeComponent();
            fileControl.ClickClose += FileControl_ClickClose;
            // EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(TreeViewItem_PreviewMouseRigh));
        //   EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewMouseDoubleClickEvent, new RoutedEventHandler(TreeViewItem_PreviewMouseRightButtonDownEvent));
            // EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.SelectedEvent, new RoutedEventHandler(GetSelect));
            vm = this.DataContext as FilePageViewModel;
            //   DataContext = new FilePageViewModel();
            // List<TypeList> projectTypelist = new List<TypeList>();
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual3" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual4" });
            //CbProType.ItemsSource = projectTypelist;
            this.Loaded += FilePageView_Loaded;
            assignControl.ClickUserSearch += AssignControl_ClickUserSearch;
            assignControl.ClickUserReset += AssignControl_ClickUserReset;
            // tree.PreviewMouseDoubleClick += Tree_PreviewMouseDoubleClick;
            //lvDataBinding.SelectionChanged += LvDataBinding_SelectionChanged;
        }
        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.IsSelected = true;
                item.Focus();
                //e.Handled = true;
            }
        }
        private void FileControl_ClickClose(object sender, EventArgs e)
        {
            if (tree.SelectedItem != null)
            {
                Organization org = (Organization)tree.SelectedItem;
                vm.SearchCommand.Execute(org.Id);
            }
            childWindowTree.Visibility = Visibility.Collapsed;
            childWindowTree.Close();
        }

        private void Tree_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tvi = (TreeViewItem)sender;
        }

        private bool showlist;
        public bool ShowList
        {
            get { return showlist; }
            set
            {

                showlist = value;
                if (showlist == false)
                    gridProjects.Visibility = Visibility.Collapsed;
            }
        }
        private async void AssignControl_ClickUserReset(object sender, EventArgs e)
        {
            vm.IsBusy = true;
            vm.UserSearch = null;
            bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.SelectedItem.Id, string.Empty, string.Empty, "Project", null);
            if (complete == true)
            {
                vm.IsBusy = false;
            }
        }

        private async void AssignControl_ClickUserSearch(object sender, EventArgs e)
        {
            if (vm.SelectedItem != null)
            {
                vm.IsBusy = true;

                bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.SelectedItem.Id, string.Empty, string.Empty, "Project", vm.UserSearch);
                if (complete == true)
                {
                    vm.IsBusy = false;
                }
            }

        }

        private async void FilePageView_Loaded(object sender, RoutedEventArgs e)
        {
            if (vm.ControlVisible == false)
            {
                mainGrid.Margin = new Thickness(0, 0, 0, 0);
            }
            //   await  vm.LongOperation(string.Empty, string.Empty, string.Empty, string.Empty);
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
            childWindowdownload.Visibility = Visibility.Collapsed;
            childWindowdownload.Close();

            childWindowAddEdit.Visibility = Visibility.Collapsed;
            childWindowAddEdit.Close();

            childWindowTreeMessageBox.Visibility = Visibility.Collapsed;
            childWindowTreeMessageBox.Close();

            childDeleteConfirmation.Visibility = Visibility.Collapsed;
            childDeleteConfirmation.Close();
            childMoveConfirmation.Visibility = Visibility.Collapsed;
            childMoveConfirmation.Close();

            childWindowTree.Visibility = Visibility.Collapsed;
            childWindowTree.Close();
            vm.TreeTitle = new List<string>();
        }

        private async void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void lvDataBinding_Drop(object sender, DragEventArgs e)
        {
            FilePageViewModel vm = DataContext as FilePageViewModel;
            if (sender is ListBoxItem)
            {
                Project droppedData = e.Data.GetData(typeof(Project)) as Project;
                Project target = ((ListBoxItem)(sender)).DataContext as Project;

                int removedIdx = lvDataBinding.Items.IndexOf(droppedData);
                int targetIdx = lvDataBinding.Items.IndexOf(target);

                if (removedIdx < targetIdx)
                {
                    vm.Projects.Insert(targetIdx + 1, droppedData);
                    vm.Projects.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (vm.Projects.Count + 1 > remIdx)
                    {
                        vm.Projects.Insert(targetIdx, droppedData);
                        vm.Projects.RemoveAt(remIdx);
                    }
                }
            }
        }
        void s_PreviewMouseMoveEvent(object sender, MouseEventArgs e)
        {
            //if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            //{
            //    ListBoxItem draggedItem = sender as ListBoxItem;
            //    DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
            //    draggedItem.IsSelected = true;
            //}
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }
        private void LvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var item = (ListBox)sender;
            //var obj = (Project)item.SelectedItem;
            //if (obj != null)
            //{
            //  vm.SelectedItemCommand.Execute();
            //}
        }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = ((ListBoxItem)sender).DataContext as Project;
            vm.SelectedItemCommand.Execute(obj);
        }
        private async void btnAgging_click(object sender, RoutedEventArgs e)
        {
            vm.UserSearch = null;
            vm.IsBusy = true;
            Project p = ((Button)sender).DataContext as Project;
            vm.SelectedItem = p;
            bool complete = await vm.GetUserListForAssign(App.LogUser.Id, p.Id, string.Empty, string.Empty, "Project", vm.UserSearch);
            if (complete == true)
            {
                vm.IsBusy = false;
                childWindowAssign.Caption = "Assign project - " + p.Name + " to user(s)";
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

        private void btnAssignClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowAssign.Visibility = Visibility.Collapsed;
            childWindowAssign.Close();
        }
        private void btnInvasive_Click(object sender, RoutedEventArgs e)
        {
            // vm.UserSearch = null;
            vm.IsBusy = true;
            Project p = ((Button)sender).DataContext as Project;
            vm.InvasiveItemCommand.Execute(p);
            //vm.SelectedItem = p;
            //bool complete = await vm.GetUserListForAssign(App.LogUser.Id, p.Id, string.Empty, string.Empty, "Project", vm.UserSearch);
            //if (complete == true)
            //{
            //    vm.IsBusy = false;
            //    childWindowAssign.Caption = "Assign project - " + p.Name + " to user(s)";
            //    childWindowAssign.FontWeight = FontWeights.Bold;
            //    childWindowAssign.Visibility = Visibility.Visible;
            //    childWindowAssign.Show();
            //  }

        }
        private async void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            vm.IsBusy = true;
            ErrorModel err = await vm.Save();
            childWindowAssign.Visibility = Visibility.Collapsed;
            childWindowAssign.Close();
            vm.ReloadLocation();
            vm.IsBusy = false;
            childWindowMessageBox.DataContext = err;
            childWindowMessageBox.Visibility = Visibility.Visible;
            childWindowMessageBox.Show();
            //  MessageBox.Show(err.Message, err.Status,MessageBoxButton.OK,MessageBoxImage.Information);
            if (err.Message == "Success")
            {

                // childWindowFeedback.DataContext = err;


            }
            else
            {
                childWindowAssign.Visibility = Visibility.Collapsed;
                childWindowAssign.Close();
                // MessageBox.Show(err.Message,"Error");
            }
            //childWindowFeedback.Visibility = Visibility.Visible;
            // childWindowFeedback.Show();
        }

        private void btnAssign_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnErrorClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();

        }

        private void btnErrorMsgClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
        }

       
        private void BtndownloadClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowdownload.Visibility = Visibility.Collapsed;
            childWindowdownload.Close();
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            rbTab1.IsChecked = true;
            //vm.UserSearch = null;
            //vm.IsBusy = true;
            Project p = ((Button)sender).DataContext as Project;
            childWindowdownload.DataContext = p;
            childWindowdownload.Visibility = Visibility.Visible;
            childWindowdownload.Show();
            if (string.IsNullOrEmpty(p.InvasiveProjectID))
            {
                DI_Invasive_Panel.Visibility = WICR_Invasive_Panel.Visibility = Visibility.Collapsed;
            }
            else
            {
                DI_Invasive_Panel.Visibility = WICR_Invasive_Panel.Visibility = Visibility.Visible;

            }
            //VISUAL FOR DI
            btnReport_Visual_Word.Tag = btnReport_Visual.Tag = p.Id;
            //Invasive FOR DI
            btnReport_Invasive.Tag = btnDIInvasiveWord.Tag = p.InvasiveProjectID;
            //Finel FOR DI WORD AND PDF
            btnFinelReport_Deck.Tag = btnFilelReport_Deck_Word.Tag = p.Id;

            //WICR VISUAL
            btnReport_Visual_WICR.Tag = WICR_Visual_Word.Tag = p.Id;

            btnReport_Invasive_wicr.Tag = WICR_Invasive_Word.Tag = p.InvasiveProjectID;

            btnFinelReport_Wicr.Tag = btnWICR_FinelReport_Word.Tag = p.Id;


            //btnFinelReport_Deck.Tag = p.Id;

            //btnReport_Invasive.Tag=WICR_Invasive_Word.Tag = p.InvasiveProjectID;

            //btnFilelReport_Deck_Word.Tag = btnWICR_FinelReport_Word.Tag = p.Id;

            //btnReport_Visual_WICR.Tag = p.Id;
            //btnReport_Invasive_wicr.Tag= btnDIInvasiveWord.Tag = p.InvasiveProjectID;



            //btnFinelReport_Wicr.Tag = p.Id;
            //vm.SelectedItem = p;
            //bool complete = await vm.GetUserListForAssign(App.LogUser.Id, p.Id, string.Empty, string.Empty, "Project", vm.UserSearch);
            //if (complete == true)
            //{
            //    vm.IsBusy = false;
            //    childWindowAssign.Caption = "Assign project - " + p.Name + " to user(s)";
            //    childWindowAssign.FontWeight = FontWeights.Bold;
            //    childWindowAssign.Visibility = Visibility.Visible;
            //    childWindowAssign.Show();
            //}
        }
        //Report VISUAL FOR DI PDF
        private void BtnReport_Visual_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordVisual?projectID=" + projectId + "&company=DI&Type=Word");
        }
        //Report VISUAL FOR DI WORD
        private void BtnReport_Visual_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();

            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordVisual?projectID=" + projectId + "&company=DI&Type=pdf");
        }
        //Report Invasive  DI PDF
        private void BtnReport_Invasive_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            //  string url = App.AppUrl + "/api/values/GetInvasivelDI?projectID=" + projectId;
            //  System.Diagnostics.Process.Start(App.AppUrl +"/api/values/GetInvasivelDI?projectID=" + projectId + "&company=DI");
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?projectID=" + projectId + "&company=DI&Type=pdf");
            // System.Diagnostics.Process.Start("http://techcodevity.com/api/values/GetVisualDI?projectID=11D6DFDB-EF89-42E4-A127-7565CCE65DC0");
        }
        //Report Invasive  DI Word
        private void BtnDIInvasiveWord_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?projectID=" + projectId + "&company=DI&Type=Word");
        }
        //FINEL DI PDF
        private void btnFinelReport_Deck_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId + "&company=DI&Type=PDF");
            //string projectId = ((Button)sender).Tag.ToString();
            //string url = App.AppUrl + "/api/values/GetInvasivelDI?projectID=" + projectId;
            // System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId);
        }
        //FINEL DI WORD
        private void BtnFilelReport_Deck_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId + "&company=DI&Type=Word");
        }


        //----------------WICR-----------------------------------------------------

        private void BtnReport_Visual_WICR_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordVisual?projectID=" + projectId + "&company=WICR&Type=pdf");
        }
        private void WICR_Visual_Word_Click(object sender, RoutedEventArgs e)
        {

            string projectId = ((Button)sender).Tag.ToString();
            //  string url = App.AppUrl + "/api/values/GetInvasivelDI?projectID=" + projectId;
            //  System.Diagnostics.Process.Start(App.AppUrl +"/api/values/GetInvasivelDI?projectID=" + projectId + "&company=DI");
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordVisual?projectID=" + projectId + "&company=WICR&Type=Word");
            // S
        }


        private void BtnReport_Invasive_wicr_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?projectID=" + projectId + "&company=WICR&Type=pdf");
        }
        private void WICR_Invasive_Word_Click(object sender, RoutedEventArgs e)
        {

            string projectId = ((Button)sender).Tag.ToString();

            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?projectID=" + projectId + "&company=WICR&Type=Word");

        }

        private void btnFinelReport_Wicr_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();

            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId + "&company=WICR&Type=pdf");
        }
        private void BtnWICR_FinelReport_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId + "&company=WICR&Type=Word");
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;
        }

        private void btnFinelStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnFinelStatusProgress_Click(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;
            p.FinelReport = true;
            vm.IsBusy = true;
            ErrorModel err = await vm.FinelSaveCompleted(p);

            vm.ReloadLocation();
            vm.IsBusy = false;

            //  MessageBox.Show(err.Message, err.Status,MessageBoxButton.OK,MessageBoxImage.Information);
            if (err.Message == "Success")
            {

                // childWindowFeedback.DataContext = err;


            }
            else
            {
                childWindowMessageBox.DataContext = err;
                childWindowMessageBox.Visibility = Visibility.Visible;
                childWindowMessageBox.Show();
                // childWindowAssign.Visibility = Visibility.Collapsed;
                //  childWindowAssign.Close();
                // MessageBox.Show(err.Message,"Error");
            }

        }

        private async void btnFinelStatuscomplete_Click(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;
            p.FinelReport = false;
            vm.IsBusy = true;
            ErrorModel err = await vm.FinelSaveCompleted(p);

            vm.ReloadLocation();
            vm.IsBusy = false;

            //  MessageBox.Show(err.Message, err.Status,MessageBoxButton.OK,MessageBoxImage.Information);
            if (err.Message == "Success")
            {

                // childWindowFeedback.DataContext = err;


            }
            else
            {
                childWindowMessageBox.DataContext = err;
                childWindowMessageBox.Visibility = Visibility.Visible;
                childWindowMessageBox.Show();
                // childWindowAssign.Visibility = Visibility.Collapsed;
                //  childWindowAssign.Close();
                // MessageBox.Show(err.Message,"Error");
            }

        }

        

       



        //private async void chbIsCompleted_Checked(object sender, RoutedEventArgs e)
        //{
        //    if(chbIsCompleted.IsChecked==true)
        //    {
        //        vm.SelectedeportType = "Completed";
        //         vm.SearchCommand.Execute();
        //       //vm.se
        //    }
        //    else
        //    {
        //        vm.SelectedeportType = "All";
        //        vm.SearchCommand.Execute();
        //    }
        //}
        private void GetSelect(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            if (e.Source is TreeViewItem
        && (e.Source as TreeViewItem).IsSelected)
            {
                Organization org = (Organization)tree.SelectedItem;
                // your code
                txtFileName.Text = org.NodeTitle;
                e.Handled = true;
            }
            //    TreeViewItem item = sender as TreeViewItem;
            //Organization org = (Organization)tree.SelectedItem;
            //Perform actions when a TreeViewItem
            //controls is selected
        }
        //private void UcProject_ClickMove(object sender, EventArgs e)
        //{
        //    Project p = ((Button)sender).DataContext as Project;
        //}

        //https://stackoverflow.com/questions/5047576/wpf-treeview-how-to-style-selected-items-with-rounded-corners-like-in-explorer
        //private void TreeViewPage_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //if(tree.Items.Count==0)
        //    //{
        //    //    operation.Visibility = Visibility.Visible;
        //    //}
        //    //else
        //    //{
        //    //    operation.Visibility = Visibility.Collapsed;
        //    //}


        //    // PopulateTreeView(null, null);
        //}
        //private IEnumerable<TreeViewItem> Enumerate(ItemsControl itemsControl, ItemCollection items)
        //{
        //    foreach (XmlElement element in items)
        //    {
        //        TreeViewItem item = itemsControl.ItemContainerGenerator.ContainerFromItem(element) as TreeViewItem;
        //        if (item != null) //When second call with <a>.Items, item is null
        //        {
        //            item.IsExpanded = true;
        //            item.UpdateLayout();

        //            yield return item;
        //        }
        //        //Enumerate is called again with <a>.Items 
        //        //Exception in second call, because item is null
        //        foreach (TreeViewItem i in Enumerate(item, item.Items))
        //        {
        //            yield return i;
        //        }
        //    }
        //}
        TreeViewItem TryGetClickedItem(TreeView treeView, RoutedEventArgs e)
        {
            var hit = e.OriginalSource as DependencyObject;
            while (hit != null && !(hit is TreeViewItem))
                hit = VisualTreeHelper.GetParent(hit);

            return hit as TreeViewItem;
        }
        string title = string.Empty;
        async void MyTreeView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var org = tree.SelectedItem as Organization;

            if (org != null)
            {
                string t = tree.SelectedValuePath;
                //  txtFileName.Text = string.Empty;
                //  vm.TreeTitle = string.Empty;
               // Organization org = (Organization)e.NewValue;
                await vm.CallTreeLongOperation(org.Id);
                vm.TreeTitle.Add(org.NodeTitle);
                txtFileName.Text = GetPath(org);
                //   await vm.CallTreeLongOperation(org.Id);
                e.Handled = true;
            }

            //TreeViewItem clickedItem = TryGetClickedItem(tree, e);

            //if (clickedItem == null || clickedItem != sender)
            //    return;

            //txtFileName.Text = string.Join(",", vm.TreeTitle);
            //Organization org = clickedItem.DataContext as Organization;



            //title = string.Empty;

        }
        private async void TreeViewItem_PreviewMouseRightButtonDownEvent(object sender, RoutedEventArgs e)
        {
            var clickedItem = TryGetClickedItem(tree, e);
            if (clickedItem == null)
                return;

            e.Handled = true;
            var t = clickedItem;
            //if (vm.ControlVisible == true)
            //{
            //    if (sender is TreeViewItem)
            //    {
            //        if (!((TreeViewItem)sender).IsSelected)
            //            return;
            //    }


            //    var baseobj = e.OriginalSource as FrameworkElement;
            //    var selectedItem = tree.SelectedItem.Equals(baseobj); ;
            //    // TreeViewItem tv = e.OriginalSource as TreeViewItem;
            //    Organization org = baseobj.DataContext as Organization;
            //    if (org != null)
            //    {


            //        var list = tree.ItemsSource as ObservableCollection<Organization>;


            //        txtFileName.Text = org.NodeTitle;
            //        //     int index = list.IndexOf(org);

            //        if (baseobj is TreeViewItem)
            //        {
            //            TreeViewItem item = baseobj as TreeViewItem;
            //            item.IsSelected = true;
            //            item.Focus();
            //        }

            //        // TreeViewItem item = (TreeViewItem)(tree.ItemContainerGenerator.ContainerFromItem(org));
            //      //  await vm.CallTreeLongOperation(org.Id);
            //        // ((TreeViewItem)tree.Items[index]).IsSelected = true;
            //        //TreeViewItem item = sender as TreeViewItem;
            //        //if (item != null)
            //        //{
            //        //    item.IsSelected = true;
            //        //    item.Focus();
            //        //    // e.Handled = true;
            //        //}
            //        e.Handled = true;
            //        return;
            //    }
            //}
            //else
            //{
            //    var baseobj = e.OriginalSource as FrameworkElement;
            //    // TreeViewItem tv = e.OriginalSource as TreeViewItem;
            //    Organization org1 = baseobj.DataContext as Organization;
            //    if (org1 != null)
            //    {
            //        btnMoveOk.Visibility = Visibility.Visible;
            //        btnMoveClose.Content = "No";
            //        btnMoveClose.ToolTip = "No";
            //        Project p = vm.MoveProject;
            //        btnMoveOk.Tag = p.Id;
            //        //  Organization org1 = (Organization)tree.SelectedItem;
            //        if (tree.SelectedItem != null)
            //        {
            //            txtMove.Text = "Are you sure you want to move \'" + p.Name + "\' in \'" + org1.NodeTitle + "\' ?";
            //            btnMoveOk.Visibility = Visibility.Visible;
            //            btnMoveClose.Content = "No";
            //            btnMoveClose.ToolTip = "No";

            //        }
            //        else
            //        {
            //            txtMove.Text = "Please select item in tree ";
            //            btnMoveOk.Visibility = Visibility.Collapsed;
            //            btnMoveClose.Content = "Close";
            //            btnMoveClose.ToolTip = "Close";
            //        }

            //        childMoveConfirmation.Visibility = Visibility.Visible;

            //        childMoveConfirmation.Show();

            //        e.Handled = false;
            //        return;
            //    }

            //}
            //    string value = tree.SelectedValue as string;
            //    TreeViewItem item = e.Source as TreeViewItem;
            //    if (e.Source is TreeViewItem
            //&& (e.Source as TreeViewItem).IsSelected)
            //    {
            //        TreeViewItem titem = sender as TreeViewItem;
            //        Organization org = (Organization)tree.SelectedItem;
            //        // your code
            //        if (org != null)
            //        {
            //            await vm.CallTreeLongOperation(org.Id);
            //             txtFileName.Text = org.NodeTitle;
            //        }
            //        e.Handled = true;

            //        //if(org!=null)
            //        //await vm.CallTreeLongOperation(org.Id);
            //    }
            //else
            //{

            //}

            //(sender as TreeViewItem).IsSelected = true;
            // Organization org = (Organization)tree.SelectedItem;
        }
        //private async void TreeViewItem_PreviewMouseRightButtonDownEvent(object sender, RoutedEventArgs e)
        //{

        //    if (vm.ControlVisible == true)
        //    {
        //        if (sender is TreeViewItem)
        //        {
        //            if (!((TreeViewItem)sender).IsSelected)
        //                return;
        //        }
        //        var baseobj = e.OriginalSource as FrameworkElement;
        //        // TreeViewItem tv = e.OriginalSource as TreeViewItem;
        //            Organization org = baseobj.DataContext as Organization;
        //        if (org != null)
        //        {

        //            var selectedItem = tree.SelectedItem; ;

        //            var list = tree.ItemsSource as ObservableCollection<Organization>;


        //            txtFileName.Text = org.NodeTitle;
        //            //     int index = list.IndexOf(org);

        //            if(baseobj is TreeViewItem)
        //            {
        //                TreeViewItem item = baseobj as TreeViewItem;
        //                item.IsSelected = true;
        //                item.Focus();
        //            }

        //           // TreeViewItem item = (TreeViewItem)(tree.ItemContainerGenerator.ContainerFromItem(org));
        //            await vm.CallTreeLongOperation(org.Id);
        //            // ((TreeViewItem)tree.Items[index]).IsSelected = true;
        //            //TreeViewItem item = sender as TreeViewItem;
        //            //if (item != null)
        //            //{
        //            //    item.IsSelected = true;
        //            //    item.Focus();
        //            //    // e.Handled = true;
        //            //}
        //            e.Handled = true;
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        var baseobj = e.OriginalSource as FrameworkElement;
        //        // TreeViewItem tv = e.OriginalSource as TreeViewItem;
        //        Organization org1 = baseobj.DataContext as Organization;
        //        if (org1 != null)
        //        {
        //            btnMoveOk.Visibility = Visibility.Visible;
        //            btnMoveClose.Content = "No";
        //            btnMoveClose.ToolTip = "No";
        //            Project p = vm.MoveProject;
        //            btnMoveOk.Tag = p.Id;
        //          //  Organization org1 = (Organization)tree.SelectedItem;
        //            if (tree.SelectedItem != null)
        //            {
        //                txtMove.Text = "Are you sure you want to move \'" + p.Name + "\' in \'" + org1.NodeTitle + "\' ?";
        //                btnMoveOk.Visibility = Visibility.Visible;
        //                btnMoveClose.Content = "No";
        //                btnMoveClose.ToolTip = "No";

        //            }
        //            else
        //            {
        //                txtMove.Text = "Please select item in tree ";
        //                btnMoveOk.Visibility = Visibility.Collapsed;
        //                btnMoveClose.Content = "Close";
        //                btnMoveClose.ToolTip = "Close";
        //            }

        //            childMoveConfirmation.Visibility = Visibility.Visible;

        //            childMoveConfirmation.Show();

        //            e.Handled = false;
        //            return;
        //        }

        //    }
        //    //    string value = tree.SelectedValue as string;
        //    //    TreeViewItem item = e.Source as TreeViewItem;
        //    //    if (e.Source is TreeViewItem
        //    //&& (e.Source as TreeViewItem).IsSelected)
        //    //    {
        //    //        TreeViewItem titem = sender as TreeViewItem;
        //    //        Organization org = (Organization)tree.SelectedItem;
        //    //        // your code
        //    //        if (org != null)
        //    //        {
        //    //            await vm.CallTreeLongOperation(org.Id);
        //    //             txtFileName.Text = org.NodeTitle;
        //    //        }
        //    //        e.Handled = true;

        //    //        //if(org!=null)
        //    //        //await vm.CallTreeLongOperation(org.Id);
        //    //    }
        //    //else
        //    //{

        //    //}

        //    //(sender as TreeViewItem).IsSelected = true;
        //    // Organization org = (Organization)tree.SelectedItem;
        //}
        private void TreeViewItem_PreviewMouseRigh(object sender, RoutedEventArgs e)
        {

            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                //item.IsSelected = true;
                //item.Focus();
                // e.Handled = true;
            }
            //await vm.CallTreeLongOperation(string.Empty);
            //(sender as TreeViewItem).IsSelected = true;
            // Organization org = (Organization)tree.SelectedItem;
        }
        public static TreeViewItem FindTviFromObjectRecursive(ItemsControl ic, object o)
        {
            //Search for the object model in first level children (recursively)
            TreeViewItem tvi = ic.ItemContainerGenerator.ContainerFromItem(o) as TreeViewItem;
            if (tvi != null) return tvi;
            //Loop through user object models
            foreach (object i in ic.Items)
            {
                //Get the TreeViewItem associated with the iterated object model
                TreeViewItem tvi2 = ic.ItemContainerGenerator.ContainerFromItem(i) as TreeViewItem;
                tvi = FindTviFromObjectRecursive(tvi2, o);
                if (tvi != null) return tvi;
            }
            return null;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (tree.SelectedItem != null)
            {
                Organization org = (Organization)tree.SelectedItem;
                //if (org.IsLock == false)
                //{
                //    ErrorModel error = new ErrorModel();
                //    error.Status = "Messaeg";
                //    error.Message = "You can not add folder in level 3.";
                //    childWindowMessageBox.DataContext = error;
                //    childWindowMessageBox.Visibility = Visibility.Visible;
                //    childWindowMessageBox.Show();
                //    return;
                //}
                txtTitle.Text = "New Folder in : " + org.NodeTitle;
            }
            txtNodeTitle.Text = "";

            btnAddEdit.Tag = string.Empty;
            //object t = tree.SelectedItem;
            childWindowAddEdit.Visibility = Visibility.Visible;
            childWindowAddEdit.Show();
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (tree.SelectedItem != null)
            {
                Organization org = (Organization)tree.SelectedItem;
                org.NodeCode = "Edit";
                txtNodeTitle.Text = org.NodeTitle;

                //  org.NodeTitle = txtNodeTitle.Text;
                //  txtNodeTitle.Text = txtError.Text = string.Empty;
                childWindowAddEdit.Visibility = Visibility.Visible;
                childWindowAddEdit.Show();
                btnAddEdit.Tag = "Edit";
            }

        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Organization org = (Organization)tree.SelectedItem;
            ErrorModel err= await vm.Delete_CheakStatus_Node(org);
            if(err.Status=="1")
            {
                ErrorModel error = new ErrorModel();
                error.Status = "Message";
                error.Message = "Make sure no project exists in the folder before deleting it.";
                childWindowMessageBox.DataContext = error;
                childWindowMessageBox.Visibility = Visibility.Visible;
                childWindowMessageBox.Show();
                return;
            }
            txtDelete.Text = string.Empty;
           
            txtDelete.Text = org.NodeTitle + " Are you sure you want to delete?";
            childDeleteConfirmation.Visibility = Visibility.Visible;
            childDeleteConfirmation.Show();
            //childWindowAddEdit.Visibility = Visibility.Collapsed;
            //childWindowAddEdit.Close();
        }

        //Dialog Close
        private void btnAddEditClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowAddEdit.Visibility = Visibility.Collapsed;
            childWindowAddEdit.Close();

        }


        //Dialog Add save Data

        public string GetLevel(Organization tn)
        {
            String path = tn.NodeTitle;




            if (tn.Parent_ID != null)
            {
                Organization node = vm.mainTree.Where(c => c.Id == tn.Parent_ID).SingleOrDefault();
                if (node != null)
                    path = GetPath(node) + "," + path;
                //  If we have a parent, get his path and stick it in front of our text.
                //  If tn.Parent was null, tn is the root and we'll just return tn.Text.

            }

            return path;
        }
        private async void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {

            //if (tree.Items.Count == 0)
            //{
            //    await vm.AddNewNode(null, txtNodeTitle.Text);
            //}
            //else
            //{

           
            if (!string.IsNullOrEmpty(txtNodeTitle.Text))
            {
                if ((string)Op_AddNode.Tag == "root")
                {
                    
                    Organization org = new Organization();
                    org.NodeCode = "New";
                    org.Parent_ID = null;
                    ErrorModel err = await vm.AddNewNode(org, txtNodeTitle.Text);
                    btnAddEdit.Tag = string.Empty;
                    Op_AddNode.Tag = string.Empty;
                    txtError.Text = txtNodeTitle.Text = string.Empty;
                    childWindowAddEdit.Visibility = Visibility.Collapsed;
                    childWindowAddEdit.Close();
                    // operation.Visibility = Visibility.Collapsed;
                    return;
                }
                //else
                //{
                //    ErrorModel cerr = new ErrorModel();
                //    cerr.Status = "Warning";
                //    cerr.Message = "Please select Item";
                //    childWindowTreeMessageBox.DataContext = cerr;
                //    childWindowTreeMessageBox.Visibility = Visibility.Visible;
                //    childWindowTreeMessageBox.Show();
                //}
                if (tree.SelectedItem != null)
                {
                    if ((string)btnAddEdit.Tag == "Edit")
                    {
                        Organization org = (Organization)tree.SelectedItem;
                     //   string level= GetLevel(org);
                       // level.ToArray()
                      
                        org.NodeCode = "Edit";
                        await vm.AddNewNode(org, txtNodeTitle.Text);
                        btnAddEdit.Tag = string.Empty;
                    }
                    else
                    {
                        Organization org = (Organization)tree.SelectedItem;
                        //if(org.IsLock==false)
                        //{
                        //    ErrorModel error = new ErrorModel();
                        //    error.Status = "Messaeg";
                        //    error.Message = "You can not add folder in level 3.";
                        //    childWindowMessageBox.DataContext = error;
                        //    childWindowMessageBox.Visibility = Visibility.Visible;
                        //    childWindowMessageBox.Show();
                        //    return;
                        //}
                      
                        org.NodeCode = "New";
                       // string level = GetLevel(org);
                        //List<string> list = level.Split(',').ToList();
                        //org.level = list.Count();
                        await vm.AddNewNode(org, txtNodeTitle.Text);
                        btnAddEdit.Tag = string.Empty;
                        //  org.NodeTitle = txtNodeTitle.Text;

                    }
                    txtNodeTitle.Text = txtError.Text = string.Empty;
                    // vm.LoadTree();
                }
                else
                {
                    //if (tree.Items.Count == 0)
                    //{
                    //    Organization org = new Organization();
                    //    org.NodeCode = "New";
                    //    ErrorModel err= await vm.AddNewNode(org, txtNodeTitle.Text);
                    //    btnAddEdit.Tag = string.Empty;
                    //    operation.Visibility = Visibility.Collapsed;
                    //}
                    //else
                    //{
                    //    ErrorModel cerr = new ErrorModel();
                    //    cerr.Status = "Warning";
                    //    cerr.Message = "Please select Item";
                    //    childWindowTreeMessageBox.DataContext = cerr;
                    //    childWindowTreeMessageBox.Visibility = Visibility.Visible;
                    //    childWindowTreeMessageBox.Show();
                    //}
                    // await vm.AddNewNode(null, txtNodeTitle.Text);
                }
                txtError.Text = txtNodeTitle.Text = string.Empty;
                childWindowAddEdit.Visibility = Visibility.Collapsed;
                childWindowAddEdit.Close();

            }
            else
            {
                txtError.Text = "Please Enter Name";
            }


            // tree.SelectedItem = null;
            // }
            //foreach (var item in tree.Items)
            //{
            //    var tvi = item as TreeViewItem;
            //    if (tvi != null)
            //        tvi.ExpandSubtree();
            //}
        }
        public string GetPath(Organization tn)
        {
            String path = tn.NodeTitle;




            if (tn.Parent_ID != null)
            {
                Organization node = vm.mainTree.Where(c => c.Id == tn.Parent_ID).SingleOrDefault();
                if (node != null)
                    path = GetPath(node) + "\\" + path;
                //  If we have a parent, get his path and stick it in front of our text.
                //  If tn.Parent was null, tn is the root and we'll just return tn.Text.

            }

            return path;
        }
        
        public static Stack<TreeViewItem> GetNodeParent(UIElement element)
        {
            Stack<TreeViewItem> tempNodePath = new Stack<TreeViewItem>();
            // Walk up the element tree to the nearest tree view item. 
            TreeViewItem container = element as TreeViewItem;

            while ((element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
                if (container != null)
                    tempNodePath.Push(container);
            }

            return tempNodePath;
        }
        private async void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
          
            //string t = tree.SelectedValuePath;
          
            //Organization org = (Organization)e.NewValue;
            //await vm.CallTreeLongOperation(org.Id);
            //vm.TreeTitle.Add(org.NodeTitle);
            //txtFileName.Text = GetPath(org);
          
        }

        private void Op_AddNode_Click(object sender, RoutedEventArgs e)
        {
            var treeViewItem = tree.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = false;
            }
          //  txtFileName.Text = string.Empty;
            //vm.ResetCommand.Execute();
            Op_AddNode.Tag = "root";
            btnAddEdit.Tag = string.Empty;
            //object t = tree.SelectedItem;
            childWindowAddEdit.Visibility = Visibility.Visible;
            childWindowAddEdit.Show();
        }

        private void op_btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void op_btnDelete_Click(object sender, RoutedEventArgs e)
        {


        }

        private void btnTreeErrorMsgClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowTreeMessageBox.Visibility = Visibility.Collapsed;
            childWindowTreeMessageBox.Close();

        }

        private void ProjectsPageView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void btnDeleteOk_Click(object sender, RoutedEventArgs e)
        {

            if (tree.SelectedItem != null)
            {
                vm.IsBusy = true;
                Organization org = (Organization)tree.SelectedItem;
                await vm.DeleteNode(org);
                childDeleteConfirmation.Visibility = Visibility.Collapsed;
                childDeleteConfirmation.Close();
                vm.IsBusy = false;
            }
            else
            {
                ErrorModel cerr = new ErrorModel();
                cerr.Status = "Warning";
                cerr.Message = "Please select Item";
                childWindowTreeMessageBox.DataContext = cerr;
                childWindowTreeMessageBox.Visibility = Visibility.Visible;
                childWindowTreeMessageBox.Show();
            }
        }

        private void btnDeleteClose_Click(object sender, RoutedEventArgs e)
        {
            //TreeViewItem item = tree.SelectedItem as TreeViewItem;
            //if (item != null)
            //{

            //    item.IsSelected = false;
            //}
            childDeleteConfirmation.Visibility = Visibility.Collapsed;
            childDeleteConfirmation.Close();
        }

        private async void BtnMoveOk_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ControlVisible == false)
            {
                vm.IsBusy = true;
                Organization org = (Organization)tree.SelectedItem;
                OrganizationDetail detail = new OrganizationDetail();
                // string ProjectID = btnMoveOk.Tag as string;

                detail.ProjectID = App.MoveProject.Id;
                detail.TreeID = org.Id;

                ErrorModel err = await vm.Move(detail);
                txtMove.Text = err.Message;
                btnMoveOk.Visibility = Visibility.Collapsed;
                btnMoveClose.Content = "Close";
                btnMoveClose.ToolTip = "Close";
                vm.IsBusy = false;
            }
            else
            {
                vm.IsBusy = true;
                Organization org = (Organization)tree.SelectedItem;
                OrganizationDetail detail = new OrganizationDetail();
                string ProjectID = btnMoveOk.Tag as string;

                detail.ProjectID = ProjectID;
                detail.TreeID = org.Id;

                ErrorModel err = await vm.Move(detail);
                txtMove.Text = err.Message;
                btnMoveOk.Visibility = Visibility.Collapsed;
                btnMoveClose.Content = "Close";
                btnMoveClose.ToolTip = "Close";
                vm.IsBusy = false;
            }
        }

        private void BtnMoveClose_Click(object sender, RoutedEventArgs e)
        {


            childMoveConfirmation.Visibility = Visibility.Collapsed;

            childMoveConfirmation.Close();
        }

        private void BtnMove_Click(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;

            App.MoveProject = p;
            // UCFilePageViewModel vmFile = fileControl.DataContext as UCFilePageViewModel;
            //vmFile.MoveProject = p;
            // vm.FileCommand.Execute(p);
            //  fileTree.DataContext = new FilePageViewModel(null);
            //Project p = ((Button)sender).DataContext as Project;
            //var eventHandler = this.ClickMove;

            //if (eventHandler != null)
            //{
            //    eventHandler(this, e);
            //}
            fileControl.DataContext = new UCFilePageViewModel(null);
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
          //  childWindowTree.Margin = new Thickness(0, 10, 0, 0);
            childWindowTree.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            childWindowTree.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
            childWindowTree.Visibility = Visibility.Visible;
            childWindowTree.Show();
        }

        //private void BtnMove_Click(object sender, RoutedEventArgs e)
        //{
        //    btnMoveOk.Visibility = Visibility.Visible;
        //    btnMoveClose.Content = "No";
        //    btnMoveClose.ToolTip = "No";
        //    Project p = ((Button)sender).DataContext as Project;
        //    btnMoveOk.Tag = p.Id;
        //    Organization org = (Organization)tree.SelectedItem;
        //    if (tree.SelectedItem != null)
        //    {
        //        txtMove.Text = "Are you sure you want to move \'" + p.Name + "\' in \'" + org.NodeTitle + "\' ?";
        //        btnMoveOk.Visibility = Visibility.Visible;
        //        btnMoveClose.Content = "No";
        //        btnMoveClose.ToolTip = "No";

        //    }
        //    else
        //    {
        //        txtMove.Text = "Please select item in tree ";
        //        btnMoveOk.Visibility = Visibility.Collapsed;
        //        btnMoveClose.Content = "Close";
        //        btnMoveClose.ToolTip = "Close";
        //    }

        //    childMoveConfirmation.Visibility = Visibility.Visible;

        //    childMoveConfirmation.Show();

        //}

        private void BtnSearchProject_Click(object sender, RoutedEventArgs e)
        {
            if (tree.SelectedItem != null)
            {
                Organization org = (Organization)tree.SelectedItem;
                vm.SearchCommand.Execute(org.Id);
                //Organization org = (Organization)tree.SelectedItem;
                //org.NodeCode = "Edit";
                //txtNodeTitle.Text = org.NodeTitle;

                ////  org.NodeTitle = txtNodeTitle.Text;
                ////  txtNodeTitle.Text = txtError.Text = string.Empty;
                //childWindowAddEdit.Visibility = Visibility.Visible;
                //childWindowAddEdit.Show();
                //btnAddEdit.Tag = "Edit";
            }
            else
            {
                vm.SearchCommand.Execute(string.Empty);
            }

        }

        private async void BtnTreeClose_Click(object sender, RoutedEventArgs e)
        {
            Organization org = (Organization)tree.SelectedItem;
            //vm.SearchCommand.Execute(org.Id);
            //Organization org = clickedItem.DataContext as Organization;
            // txtFileName.Text = org.NodeTitle;
            await vm.CallTreeLongOperation(org.Id);
          //  await vm.Reset();
            childWindowTree.Visibility = Visibility.Collapsed;
            childWindowTree.Close();
            vm.MoveProject = null;
        }

        private void Tree_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.OriginalSource as TreeViewItem;
            Stack<TreeViewItem> path = GetNodeParent(item as UIElement);
        }
    }

}
