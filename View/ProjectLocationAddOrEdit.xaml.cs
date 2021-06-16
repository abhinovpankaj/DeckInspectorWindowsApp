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
    /// Interaction logic for ProjectDetailPageView.xaml
    /// </summary>
    public partial class ProjectLocationAddOrEdit : UserControl
    {
        ProjectLocationAddOrEditViewModel vm;
        public ProjectLocationAddOrEdit()
        {
            InitializeComponent();
            vm = this.DataContext as ProjectLocationAddOrEditViewModel;
            this.Loaded += ProjectLocationAddOrEdit_Loaded;
        }

        private void ProjectLocationAddOrEdit_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            vm.IsBusy = true;
            ErrorModel err = await vm.Save();
            vm.IsBusy = false;
           
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
