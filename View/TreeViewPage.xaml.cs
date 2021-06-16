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
    /// Interaction logic for TreeViewPage.xaml
    /// </summary>
    public partial class TreeViewPage : UserControl
    {
        ProjectsPageViewModel vm;
        public TreeViewPage()
        {
            InitializeComponent();
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(TreeViewItem_PreviewMouseRigh));
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewMouseDoubleClickEvent, new RoutedEventHandler(TreeViewItem_PreviewMouseRightButtonDownEvent));
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.SelectedEvent, new RoutedEventHandler(GetSelect));
            //this.Loaded += TreeViewPage_Loaded;
            vm = (ProjectsPageViewModel)this.DataContext;
         //   ucProject.DataContext = vm;
           // ucProject.ClickMove += UcProject_ClickMove;
            //base.OnStartup(e);
        }
        private void GetSelect (object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            if (e.Source is TreeViewItem
        && (e.Source as TreeViewItem).IsSelected)
            {
                Organization org = (Organization)tree.SelectedItem;
                // your code
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
        private void TreeViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            //if(tree.Items.Count==0)
            //{
            //    operation.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    operation.Visibility = Visibility.Collapsed;
            //}
            childWindowAddEdit.Visibility = Visibility.Collapsed;
            childWindowAddEdit.Close();

            childWindowTreeMessageBox.Visibility = Visibility.Collapsed;
            childWindowTreeMessageBox.Close();

            childDeleteConfirmation.Visibility = Visibility.Collapsed;
            childDeleteConfirmation.Close();

            // PopulateTreeView(null, null);
        }

        private async void TreeViewItem_PreviewMouseRightButtonDownEvent(object sender, RoutedEventArgs e)
        {
            await vm.CallTreeLongOperation(string.Empty);
            //(sender as TreeViewItem).IsSelected = true;
            // Organization org = (Organization)tree.SelectedItem;
        }
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            txtDelete.Text = string.Empty;
            Organization org = (Organization)tree.SelectedItem;
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
                    }
                    else
                    {
                        Organization org = (Organization)tree.SelectedItem;
                        org.NodeCode = "New";
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

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int index = 0, indexOfSelectedNode=-1;

            if (tree.Items.Count >= 0)

            {

                var tree = sender as TreeView;

                if (tree.SelectedItem != null)

                {

                    index++;

                    TreeViewItem item = tree.SelectedItem as TreeViewItem;

                    ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(item);

                    while (parent != null && parent.GetType() == typeof(TreeViewItem))

                    {

                        index++;

                        parent = ItemsControl.ItemsControlFromItemContainer(parent);

                    }

                    indexOfSelectedNode = index;

                }
            }
            int u = indexOfSelectedNode;
            //Organization org = (Organization)tree.SelectedItem;
            //var tvi = FindTviFromObjectRecursive(tree, org);
            //if (tvi != null) tvi.IsSelected = true;
            //Organization o=
        }

        private void Op_AddNode_Click(object sender, RoutedEventArgs e)
        {
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
                Organization org = (Organization)tree.SelectedItem;
                await vm.DeleteNode(org);
                childDeleteConfirmation.Visibility = Visibility.Collapsed;
                childDeleteConfirmation.Close();
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

        //private async void btnAddNode_Click(object sender, RoutedEventArgs e)
        //{
        //    childWindowAddEdit.Visibility = Visibility.Visible;
        //    childWindowAddEdit.Show();

        //}
    }


}
