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
    public partial class UserAddOrEdit : UserControl
    {
        UserAddOrEditViewModel vm;
        public UserAddOrEdit()
        {
            InitializeComponent();
            vm = this.DataContext as UserAddOrEditViewModel;
            // DataContext = new ProjectAddOrEditViewModel();
            this.Loaded += UserAddOrEdit_Loaded;
        }

        private void UserAddOrEdit_Loaded(object sender, RoutedEventArgs e)
        {
            cbAdmin.IsChecked = cbMobile.IsChecked = cbdesktop.IsChecked = false;
            if (!string.IsNullOrEmpty(vm.User.Id))
            {
                if (vm.User.RoleId == 1)
                {

                    cbAdmin.IsChecked = true;
                    cbdesktop.IsChecked = cbMobile.IsChecked = true;
                    cbdesktop.IsEnabled = cbMobile.IsEnabled = false;


                }
                else if(vm.User.RoleId == 2)
                {

                    cbdesktop.IsChecked  = true;
                }
                else if (vm.User.RoleId == 3)
                {
                    cbdesktop.IsChecked = cbMobile.IsChecked = true;
                }
                else if (vm.User.RoleId == 4)
                {
                    cbMobile.IsChecked = true;
                }

                if(vm.User.IsDelete==false)
                {
                    vm.SelectedType = "Active";
                }
                else
                {
                    vm.SelectedType = "Dectivate";
                }

            }
            passBox_text.Password = txtPass.Text;
        }

        private void btnShowPass_MouseUp(object sender, MouseButtonEventArgs e)
        {


        }

        private void btnShowPass_MouseDown(object sender, MouseButtonEventArgs e)
        {


            txtPass.Visibility = Visibility.Visible;
            txtPass.Focus();
        }

        private void cbShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            if (cbShowPassword.IsChecked == true)
            {
                passBox_text.Visibility = Visibility.Collapsed;
                txtPass.Visibility = Visibility.Visible;
                txtPass.Focus();
            }
            if (cbShowPassword.IsChecked == false)
            {

            }
        }

        private void passBox_text_PasswordChanged(object sender, RoutedEventArgs e)
        {
            vm.User.Pwd = txtPass.Text = passBox_text.Password;
        }

        private void cbShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            passBox_text.Visibility = Visibility.Visible;
            txtPass.Visibility = Visibility.Collapsed;
            passBox_text.Focus();
        }

        private void cbAdmin_Checked(object sender, RoutedEventArgs e)
        {

            if (cbAdmin.IsChecked == true)
            {
                cbdesktop.IsChecked = cbMobile.IsChecked = true;
                cbdesktop.IsEnabled = cbMobile.IsEnabled = false;
            }
        }

        private void cbAdmin_Unchecked(object sender, RoutedEventArgs e)
        {
            cbdesktop.IsChecked = cbMobile.IsChecked = false;
            cbdesktop.IsEnabled = cbMobile.IsEnabled = true;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {

            string ErrMsg = string.Empty;
            if (string.IsNullOrEmpty(vm.User.FirstName))
            {
                ErrMsg += "\nFirst Name required.";
            }
            if (string.IsNullOrEmpty(vm.User.LastName))
            {
                ErrMsg += "\nLast Name required.";
            }
            if (string.IsNullOrEmpty(vm.User.UserName))
            {
                ErrMsg += "\nUsername required.";
            }
            if (string.IsNullOrEmpty(vm.User.Pwd))
            {
                ErrMsg += "\nPassword required.";
            }

            if (cbdesktop.IsChecked == false && cbMobile.IsChecked == false && cbAdmin.IsChecked == false)
            {
                ErrMsg += "\nPlease Select Role.";
            }
            if (CbActiveType.SelectedItem == null)
            {
                ErrMsg += "\nPlease Select Active/Inactive.";
            }
            if (string.IsNullOrEmpty(ErrMsg))
            {
                if (vm.SelectedType == "Active")
                {
                    vm.User.IsDelete = false;
                }
                else
                {
                    vm.User.IsDelete = true;
                }
                int RoleID = -1;

                if (cbAdmin.IsChecked == true)
                {
                    RoleID = 1;

                }
                else if (cbdesktop.IsChecked == true && cbMobile.IsChecked == true && cbAdmin.IsChecked == false)
                {
                    RoleID = 3;
                }
                else if (cbdesktop.IsChecked == true && cbMobile.IsChecked == false && cbAdmin.IsChecked == false)
                {
                    RoleID = 2;
                }
                else if (cbMobile.IsChecked == true && cbdesktop.IsChecked == false && cbAdmin.IsChecked == false)
                {
                    RoleID = 4;
                }
                vm.User.RoleId = RoleID;
               
                ErrorModel err = await vm.Save();
                childWindowFeedback.DataContext = err;
                childWindowFeedback.Visibility = Visibility.Visible;
                childWindowFeedback.Show();
                //MessageBoxResult res = MessageBox.Show(err.Message, err.Status, MessageBoxButton.OK);
                //if(res==MessageBoxResult.OK)
                //{
                //    vm.BackCommand.Execute();
                //}
                //  err.Status = "Validation";
                // err.Message = ErrMsg;
                //childWindowFeedback.DataContext = err;
                //childWindowFeedback.Visibility = Visibility.Visible;
                //childWindowFeedback.Show();
            }
            else
            {
                ErrorModel err = new ErrorModel();
                err.Status = "Validation";
                err.Message = ErrMsg;
                childWindowFeedback.DataContext = err;
                childWindowFeedback.Visibility = Visibility.Visible;
                childWindowFeedback.Show();
            }
        }

        private void btnErrorClose_Click(object sender, RoutedEventArgs e)
        {
           
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
            vm.BackCommand.Execute();
        }
    }
}
