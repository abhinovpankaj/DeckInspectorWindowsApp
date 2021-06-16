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
using UI.Code.ViewModel;

namespace UI.Code.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginPageView : UserControl
    {
        public LoginPageView()
        {
            InitializeComponent();
            DataContext = new LoginPageViewModel();
            this.Loaded += LoginPageView_Loaded;
        }

        private void LoginPageView_Loaded(object sender, RoutedEventArgs e)
        {
            LoginPageViewModel vm = DataContext as LoginPageViewModel;
            txtPassword.Password = vm.Password;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginPageViewModel vm = DataContext as LoginPageViewModel;
            vm.Password = txtPassword.Password;
        }
    }
}
