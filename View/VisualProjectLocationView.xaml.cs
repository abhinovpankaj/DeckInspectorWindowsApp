using Microsoft.Win32;
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
using UI.Code.Services;
using UI.Code.ViewModel;

namespace UI.Code.View
{
    /// <summary>
    /// Interaction logic for ProjectsPageView.xaml
    /// </summary>
    public partial class VisualProjectLocationView : UserControl
    {
        VisualProjectLocationViewModel vm;
        public VisualProjectLocationView()
        {
            InitializeComponent();
            vm = this.DataContext as VisualProjectLocationViewModel;
            //   DataContext = new ProjectsPageViewModel();
            // List<TypeList> projectTypelist = new List<TypeList>();
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual3" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual4" });
            //CbProType.ItemsSource = projectTypelist;
            this.Loaded += VisualProjectLocationView_Loaded;
            UCAddEdit.ClickSave += UCAddEdit_ClickSave;
            UCAddEdit.ClickBack += UCAddEdit_ClickBack;
            UCAddEdit.ClickDel += UCAddEdit_ClickDel;
            //lvDataBinding.SelectionChanged += LvDataBinding_SelectionChanged;
            vc.BackClick += Vc_BackClick;
            vc.btnSave.Click += BtnSave_Click;
            //vc.btnSaveInvasive.Click += BtnSaveInvasive_Click;
            vc.ClickSearch += Vc_ClickSearch;
            vc.ClickVisualReorder += Vc_ClickVisualReorder;
            UCAddEdit.ClickImageUpload += UCAddEdit_ClickImageUpload;
        }
        private async void UCAddEdit_ClickImageUpload(object sender, EventArgs e)
        {
            //browse select image
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                var localPath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                var response = await DataUploadService.UploadFile(localPath, vm.Project.Name, $"api/Project/AddImage");
                if (response.Status != ApiResult.Fail)
                {
                    vm.DataModel.ImageUrl = response.Data.ToString();
                }

            }
        }

        private void UCAddEdit_ClickDel(object sender, EventArgs e)
        {
            childDeleteConfirmation.Visibility = Visibility.Visible;
            childDeleteConfirmation.Close();
        }

        private async void BtnSaveInvasive_Click(object sender, RoutedEventArgs e)
        {
            vm.IsBusy = true;
            ErrorModel err = await vm.Reorder();
            vm.IsBusy = false;
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
        }

        private void UCAddEdit_ClickBack(object sender, EventArgs e)
        {
            UCAddEdit.IsEdit = true;
            ProjectLocation old = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectLocation>(vm.ObjectString);
            bool b = false;
            if (old.Name == vm.DataModel.Name && old.Description == vm.DataModel.Description)
            {
                b = false;
            }
            else
            {
                b = true;
            }
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
            else
            {
                vm.BackCommand.Execute();
            }
        }

        private async void Vc_ClickVisualReorder(object sender, EventArgs e)
        {
            vm.IsBusy = true;
            ErrorModel err = await vm.ReorderVisualLocation();
            vm.IsBusy = false;
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();

        }
        private async void UCAddEdit_ClickSave(object sender, EventArgs e)
        {

            ErrorModel err = await vm.Save();
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
        }
        private void Vc_ClickSearch(object sender, EventArgs e)
        {
            if(vm.SelectedItem==null)
            {
              MessageBoxResult res=  MessageBox.Show("Please Select Visual Inspection","Information",MessageBoxButton.OK);
                return;
            }
           // vm.GetImagesCommand.Execute();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            vm.IsBusy = true;
            ErrorModel err = await vm.Reorder();
            vm.IsBusy = false;
            childWindowFeedback.DataContext = err;
            childWindowFeedback.Visibility = Visibility.Visible;
            childWindowFeedback.Show();
           
        }

        private void Vc_BackClick(object sender, EventArgs e)
        {
            vm.BackCommand.Execute();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }
        private void btnDeleteClose_Click(object sender, RoutedEventArgs e)
        {
            childDeleteConfirmation.Visibility = Visibility.Collapsed;
            childDeleteConfirmation.Close();
        }
        private async void btnDeleteOk_Click(object sender, RoutedEventArgs e)
        {
            await vm.DeleteMain();
            //childWindowFeedback.DataContext = err;
            //childWindowFeedback.Visibility = Visibility.Visible;
            //childWindowFeedback.Show();
         //   childDeleteConfirmation.Visibility = Visibility.Collapsed;
           // childDeleteConfirmation.Close();
        }
        private void btnErrorClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
        }
        private void VisualProjectLocationView_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
           
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
