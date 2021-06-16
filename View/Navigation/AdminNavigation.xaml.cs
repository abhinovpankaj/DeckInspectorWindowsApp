using CommonServiceLocator;
using Prism.Regions;
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

namespace UI.Code.View.Navigation
{
    /// <summary>
    /// Interaction logic for AdminNavigation.xaml
    /// </summary>
    public partial class AdminNavigation : UserControl
    {
        public AdminNavigation()
        {
            InitializeComponent();
        }
        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_contacts.Visibility = Visibility.Collapsed;
                tt_messages.Visibility = Visibility.Collapsed;
                //tt_maps.Visibility = Visibility.Collapsed;
              //  tt_settings.Visibility = Visibility.Collapsed;
                tt_signout.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_contacts.Visibility = Visibility.Visible;
                tt_messages.Visibility = Visibility.Visible;
               // tt_maps.Visibility = Visibility.Visible;
               // tt_settings.Visibility = Visibility.Visible;
                tt_signout.Visibility = Visibility.Visible;
            }
        }
        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
           // img_bg.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
             // img_bg.Opacity = 0.3;
        }

        

       
        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            //Close();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.DataContext = new ShellViewModel();
            //ShellViewModel vm = this.DataContext as ShellViewModel;
            ////if (vm == null)
            ////    return;
            //int index = ListViewMenu.SelectedIndex;
            //MoveCursorMenu(index);

            //switch (index)
            //{
            //    case 0:
            //        container.Margin = new Thickness(0);
            //        gridNavigation.Visibility = Visibility.Collapsed;
            //        vm.OnLoadedCommand.Execute();


            //        //GridPrincipal.Children.Clear();
            //        // GridPrincipal.Children.Add(new View.Project.ProjectView());
            //        break;
            //    case 1:
            //        vm.ProjectsCommand.Execute();
            //        // GridPrincipal.Children.Clear();
            //        // GridPrincipal.Children.Add(new View.User.UserView());
            //        break;
            //    default:
            //        break;
            //}
        }


        IRegionManager RegionManger
        {
            get
            {
                return (IRegionManager)Prism.Ioc.ContainerLocator.Container.Resolve(typeof(IRegionManager));
            }
        }
        private void logout_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.LogUserName = string.Empty;
            App.Role = string.Empty;
            RegionManger.RequestNavigate("MainRegion", "Login");
            RegionManger.RequestNavigate("NavigationRegion", "DummyView");
        }

        private void btnUsers_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RegionManger.RequestNavigate("MainRegion", "Users");
        }

        private void Home_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RegionManger.RequestNavigate("MainRegion", "Users");
        }
    }
}
