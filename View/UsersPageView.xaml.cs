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
    public partial class UsersPageView : UserControl
    {
        public UsersPageView()
        {
            InitializeComponent();
            //   DataContext = new ProjectsPageViewModel();
            // List<TypeList> projectTypelist = new List<TypeList>();
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual3" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual4" });
            //CbProType.ItemsSource = projectTypelist;
            this.Loaded += ProjectsPageView_Loaded;
        }

        private void ProjectsPageView_Loaded(object sender, RoutedEventArgs e)
        {
            //childWindowFeedback.Visibility = Visibility.Collapsed;
           // childWindowFeedback.Close();
        }

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {

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
            //childWindowFeedback.Visibility = Visibility.Visible;
            //childWindowFeedback.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //childWindowFeedback.Visibility = Visibility.Collapsed;
            //childWindowFeedback.Close();
        }
    }
}
