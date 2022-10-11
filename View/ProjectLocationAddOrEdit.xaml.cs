using Microsoft.Win32;
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
using UI.Code.Services;
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

        private async void btnImage_Click(object sender, RoutedEventArgs e)
        {
            //browse select image
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                var localPath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                var response = await DataUploadService.UploadFile(localPath, vm.ProjectLocation.Name, $"api/Project/AddImage");
                if (response.Status != ApiResult.Fail)
                {
                    vm.ProjectLocation.ImageUrl = response.Data.ToString();
                }                
            }
        }

    }
}
