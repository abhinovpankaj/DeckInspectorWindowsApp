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
    /// Interaction logic for SingleLevelProjectPageView.xaml
    /// </summary>
    public partial class SingleLevelProjectPageView : UserControl
    {
        VisualSingleLevelProjectLocationViewModel vm;

        public SingleLevelProjectPageView()
        {
            InitializeComponent();
            vm = this.DataContext as VisualSingleLevelProjectLocationViewModel;
            
            this.Loaded += VisualProjectLocationView_Loaded;
            UCAddEdit.ClickSave += UCAddEdit_ClickSave;
            UCAddEdit.ClickBack += UCAddEdit_ClickBack;
            UCAddEdit.ClickDel += UCAddEdit_ClickDel;
            UCAddEdit.ClickInvasive += UCAddEdit_ClickInvasive;
            
            UCAddEdit.ClickExport += UCAddEdit_ClickExport;
            vm.OnProjectLoadSuccess += Vm_OnProjectLoadSuccess;
        }

        private void UCAddEdit_ClickExport(object sender, EventArgs e)
        {
            
        }

        private void Vm_OnProjectLoadSuccess(object sender, EventArgs e)
        {
            DataTemplate template = LocationContent.ContentTemplate;
            if (App.IsInvasive)
            {
                if (invControl==null)
                {
                    invControl = (VisualUserControle)template.FindName("vcInvasive", LocationContent);
                    if (invControl!=null)
                    {
                        invControl.BackClick += Vc_BackClick;
                        invControl.btnSave.Click += BtnSave_Click;

                        invControl.ClickSearch += Vc_ClickSearch;
                        invControl.ClickVisualReorder += Vc_ClickVisualReorder;
                    }
                    
                }
                
            }
            else
            {
                if (visControl==null)
                {
                    visControl = (VisualUserControleForVisual)template.FindName("vc", LocationContent);
                    visControl.BackClick += Vc_BackClick;
                    visControl.btnSave.Click += BtnSave_Click;

                    visControl.ClickSearch += Vc_ClickSearch;
                    visControl.ClickVisualReorder += Vc_ClickVisualReorder;
                }
                
            }
        }

        private void UCAddEdit_ClickDel(object sender, EventArgs e)
        {
            childDeleteConfirmation.Visibility = Visibility.Visible;
            childDeleteConfirmation.Close();
        }
        private async void UCAddEdit_ClickInvasive(object sender, EventArgs e)
        {
            ErrorModel err = await vm.CreateInvasive();
            if (err.Status != "Success")
            {
                childWindowFeedback.DataContext = err;
                childWindowFeedback.Visibility = Visibility.Visible;
                childWindowFeedback.Show();
            }
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
            Project old = Newtonsoft.Json.JsonConvert.DeserializeObject<Project>(vm.ObjectString);
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
            if (vm.SelectedItem == null)
            {
                MessageBoxResult res = MessageBox.Show("Please Select Visual Inspection", "Information", MessageBoxButton.OK);
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
        VisualUserControle invControl;
        VisualUserControleForVisual visControl;
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
