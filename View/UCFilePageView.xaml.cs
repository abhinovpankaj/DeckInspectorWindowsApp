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
    public partial class UCFilePageView : UserControl
    {
        public event EventHandler ClickClose;
        public event EventHandler ClickMove;
        UCFilePageViewModel vm;
        public UCFilePageView()
        {
            InitializeComponent();
            
          //  EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(TreeViewItem_PreviewMouseRigh));
        //    EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewMouseDoubleClickEvent, new RoutedEventHandler(TreeViewItem_PreviewMouseRightButtonDownEvent));
           // EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.SelectedEvent, new RoutedEventHandler(GetSelect));
            vm = this.DataContext as UCFilePageViewModel;
            //   DataContext = new FilePageViewModel();
            // List<TypeList> projectTypelist = new List<TypeList>();
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual3" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual4" });
            //CbProType.ItemsSource = projectTypelist;
            this.Loaded += FilePageView_Loaded;
            
           // tree.PreviewMouseDoubleClick += Tree_PreviewMouseDoubleClick;
            //lvDataBinding.SelectionChanged += LvDataBinding_SelectionChanged;
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
               
             //  gridProjects.Visibility = Visibility.Collapsed;
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

        private  async void FilePageView_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (vm.ControlVisible==false)
            {
                mainGrid2.Margin = new Thickness(0, 0, 0, 0);
            }
         //   await  vm.LongOperation(string.Empty, string.Empty, string.Empty, string.Empty);
            

            childWindowAddEdit.Visibility = Visibility.Collapsed;
            childWindowAddEdit.Close();

            childWindowTreeMessageBox.Visibility = Visibility.Collapsed;
            childWindowTreeMessageBox.Close();

            childDeleteConfirmation.Visibility = Visibility.Collapsed;
            childDeleteConfirmation.Close();
            childMoveConfirmation.Visibility = Visibility.Collapsed;
            childMoveConfirmation.Close();

        }

        private async void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {

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
     

        

       

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;
        }

        private void btnFinelStatus_Click(object sender, RoutedEventArgs e)
        {

        }

       

        private void btnFinelReport_Deck_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            //string url = App.AppUrl + "/api/values/GetInvasivelDI?projectID=" + projectId;
            System.Diagnostics.Process.Start(App.AppUrl + "/api/values/GetFinelDI?projectID=" + projectId);
        }

        private void btnFinelReport_Wicr_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            //string url = App.AppUrl + "/api/values/GetInvasivelDI?projectID=" + projectId;
            System.Diagnostics.Process.Start(App.AppUrl + "/api/values/GetFinelWICR?projectID=" + projectId);
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
     

        async void MyTreeView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var org = tree.SelectedItem as Organization;

            if (org != null)
            {
                //   await vm.CallTreeLongOperation(org.Id);
                e.Handled = true;
            }
            btnMoveOk.Visibility = Visibility.Visible;
            btnMoveClose.Content = "No";
            btnMoveClose.ToolTip = "No";
            Project p = App.MoveProject;
            btnMoveOk.Tag = p.Id;
            //  Organization org1 = (Organization)tree.SelectedItem;
            if (tree.SelectedItem != null)
            {
                txtMove.Text = "Are you sure you want to move \'" + p.Name + "\' in \'" + org.NodeTitle + "\' ?";
                btnMoveOk.Visibility = Visibility.Visible;
                btnMoveClose.Content = "No";
                btnMoveClose.ToolTip = "No";

            }
            else
            {
                txtMove.Text = "Please select item in tree ";
                btnMoveOk.Visibility = Visibility.Collapsed;
                btnMoveClose.Content = "Close";
                btnMoveClose.ToolTip = "Close";
            }

            childMoveConfirmation.Visibility = Visibility.Visible;

            childMoveConfirmation.Show();
            //if (vm.ControlVisible == true)
            //{
            //    var org = tree.SelectedItem as Organization;

            //    if (org != null)
            //    {
            //        await vm.CallTreeLongOperation(org.Id);
            //        e.Handled = true;
            //    }
            //}
            //else
            //{


            //}

            //var clickedItem = TryGetClickedItem(tree, e);
            //if (clickedItem != null)
            //{
            //    Organization org = clickedItem.DataContext as Organization;
            //    await vm.CallTreeLongOperation(org.Id);
            //    e.Handled = true;
            //    return;
            //}
            //e.Handled = true; // to cancel expanded/collapsed toggle

            // clickedItem.IsSelected = true;
            //  clickedItem.IsSelectionActive = true;

            //  clickedItem.Focus();
            //clickedItem.UpdateLayout();

            // return;
            // tree.UpdateLayout();
            //  DoStuff(clickedItem);
        }
        private async void TreeViewItem_PreviewMouseRightButtonDownEvent(object sender, RoutedEventArgs e)
        {
            var clickedItem = TryGetClickedItem(tree, e);
            if (clickedItem == null)
                return;

            e.Handled = true;
            var t=clickedItem;
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
                item.IsSelected = true;
                item.Focus();
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
                txtTitle.Text = "New Folder in " + org.NodeTitle;
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
            ErrorModel err = await vm.Delete_CheakStatus_Node(org);
            if (err.Status == "1")
            {
                ErrorModel error = new ErrorModel();
                error.Status = "Message";
                error.Message = "Make sure no project exists in the folder before deleting it.";
                childWindowTreeMessageBox.DataContext = error;
                childWindowTreeMessageBox.Visibility = Visibility.Visible;
                childWindowTreeMessageBox.Show();
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
                    this.DataContext = new UCFilePageViewModel(null);
                    // await vm.Reset(); ;
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
                        org.NodeCode = "Edit";
                        await vm.AddNewNode(org, txtNodeTitle.Text);
                        btnAddEdit.Tag = string.Empty;
                        //   vm.ResetCommand.Execute();
                        this.DataContext = new UCFilePageViewModel(null);
                        //   await vm.Reset();
                    }
                    else
                    {
                        Organization org = (Organization)tree.SelectedItem;
                        org.NodeCode = "New";
                       ErrorModel cerr=  await vm.AddNewNode(org, txtNodeTitle.Text);
                        if(string.IsNullOrEmpty(cerr.Status)==false)
                        {
                            //vm.ResetCommand.Execute();
                            this.DataContext = new UCFilePageViewModel(null);
                        }
                        btnAddEdit.Tag = string.Empty;
                       
                         
                        //this.UpdateLayout();
                      //  await vm.Reset();
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

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           
           // Organization o = (Organization)e.NewValue;
            
            //int index = 0, indexOfSelectedNode = -1;

            //if (tree.Items.Count >= 0)

            //{

            //    var tree = sender as TreeView;

            //    if (tree.SelectedItem != null)

            //    {

            //        index++;

            //        TreeViewItem item = tree.SelectedItem as TreeViewItem;

            //        ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(item);

            //        while (parent != null && parent.GetType() == typeof(TreeViewItem))

            //        {

            //            index++;

            //            parent = ItemsControl.ItemsControlFromItemContainer(parent);

            //        }

            //        indexOfSelectedNode = index;

            //    }
            //}
            //int u = indexOfSelectedNode;
            //Organization org = (Organization)tree.SelectedItem;
            //var tvi = FindTviFromObjectRecursive(tree, org);
            //if (tvi != null) tvi.IsSelected = true;
            //Organization o=
        }

        private void Op_AddNode_Click(object sender, RoutedEventArgs e)
        {
            var treeViewItem = tree.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
            treeViewItem.IsSelected = false;
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
                this.DataContext = new UCFilePageViewModel(null);
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
                if(err.Status=="Error")
                {
                    btnMoveOk.Tag = "Error";
                }
                else
                {
                    btnMoveOk.Tag = "Success";
                }
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
                if (err.Status == "Error")
                {
                    btnMoveOk.Tag = "Error";
                }
                else
                {
                    btnMoveOk.Tag = "Success";
                }
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
            var eventHandler = this.ClickClose;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
           
        }

        private void BtnMove_Click(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;
            //vm.FileCommand.Execute(p);
            //  fileTree.DataContext = new FilePageViewModel(null);
            //Project p = ((Button)sender).DataContext as Project;
            //var eventHandler = this.ClickMove;

            //if (eventHandler != null)
            //{
            //    eventHandler(this, e);
            //}
           
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

        private void BtnTreeClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
