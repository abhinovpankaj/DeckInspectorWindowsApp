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
    public partial class ProjectAddOrEdit : UserControl
    {
        ProjectAddOrEditViewModel vm;
        public ProjectAddOrEdit()
        {
            InitializeComponent();
            DataContext = new ProjectAddOrEditViewModel();
            vm = this.DataContext as ProjectAddOrEditViewModel;
           
            this.Loaded += ProjectAddOrEdit_Loaded;
        }

        private void ProjectAddOrEdit_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ErrorModel err=await vm.Save();
           
                childWindowFeedback.DataContext = err;
                childWindowFeedback.Visibility = Visibility.Visible;
                childWindowFeedback.Show();
           
        }

        private void btnErrorClose_Click(object sender, RoutedEventArgs e)
        {
            //  vm.Go();
            ErrorModel err = (ErrorModel)childWindowFeedback.DataContext;
            if(err.Status== "Success")
            {
                childWindowFeedback.Visibility = Visibility.Collapsed;
                childWindowFeedback.Close();
                vm.Go();
            }
            else
            {
                childWindowFeedback.Visibility = Visibility.Collapsed;
                childWindowFeedback.Close();
            }
          
        }
    }
}
