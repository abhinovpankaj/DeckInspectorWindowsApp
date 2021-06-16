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
    /// Interaction logic for AssigeProjectView.xaml
    /// </summary>
    public partial class AssigeProjectView : UserControl
    {
        AssigeProjectViewModel vm;
        public AssigeProjectView()
        {
            InitializeComponent();
            vm = DataContext as AssigeProjectViewModel;
            //   DataContext = new AssigeProjectViewModel();
            // List<TypeList> projectTypelist = new List<TypeList>();
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual3" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual4" });
            //CbProType.ItemsSource = projectTypelist;
            this.Loaded += AssigeProjectView_Loaded;
        }

        private void AssigeProjectView_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {

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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ErrorModel err = await vm.Save();
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
        }

        private void btnErrorClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }
    }
}
