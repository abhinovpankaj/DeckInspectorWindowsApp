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
        TreetPageViewModel vm;
        public TreeViewPage()
        {
            InitializeComponent();
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewMouseRightButtonDownEvent, new RoutedEventHandler(TreeViewItem_PreviewMouseRightButtonDownEvent));
            this.Loaded += TreeViewPage_Loaded;
            vm = (TreetPageViewModel)this.DataContext;
            //base.OnStartup(e);
        }
        //https://stackoverflow.com/questions/5047576/wpf-treeview-how-to-style-selected-items-with-rounded-corners-like-in-explorer
        private void TreeViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowAddEdit.Visibility = Visibility.Collapsed;
            childWindowAddEdit.Close();
            
            // PopulateTreeView(null, null);
        }
        
        private void TreeViewItem_PreviewMouseRightButtonDownEvent(object sender, RoutedEventArgs e)
        {
            (sender as TreeViewItem).IsSelected = true;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //object t = tree.SelectedItem;
           
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddEditClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowAddEdit.Visibility = Visibility.Collapsed;
            childWindowAddEdit.Close();
        }
        
        private async void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            //if (tree.Items.Count == 0)
            //{
            //    await vm.AddNewNode(null, txtNodeTitle.Text);
            //}
            //else
            //{
                if (tree.SelectedItem != null)
                {
                    Organization org = (Organization)tree.SelectedItem;
                    await vm.AddNewNode(org.Id, txtNodeTitle.Text);
                    //  org.NodeTitle = txtNodeTitle.Text;

                }
            else
            {
                await vm.AddNewNode(null, txtNodeTitle.Text);
            }
       // }
            //foreach (var item in tree.Items)
            //{
            //    var tvi = item as TreeViewItem;
            //    if (tvi != null)
            //        tvi.ExpandSubtree();
            //}
        }

        private async void btnAddNode_Click(object sender, RoutedEventArgs e)
        {
            childWindowAddEdit.Visibility = Visibility.Visible;
            childWindowAddEdit.Show();
           
        }
    }

    
}
