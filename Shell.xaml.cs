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
using System.Windows.Shapes;
using UI.Code.Model;
using UI.Code.ViewModel;

namespace UI.Code
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
       
        public Shell()
        {
            
            InitializeComponent();
            this.Loaded += Shell_Loaded;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
            //txtUsername.Text = App.LogUserName;
            this.DataContext = new ShellViewModel();
            ShellViewModel vm = this.DataContext as ShellViewModel;
          //  container.Margin = new Thickness(0);
          //  gridNavigation.Visibility = Visibility.Collapsed;
            vm.OnLoadedCommand.Execute();
        }
        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            //img_bg.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
          //  img_bg.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           // Tg_Btn.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();

            ErrorModel err = new ErrorModel() {Message= "Are you sure you want to close this application?",Status= "Warning" };
            childWindowMessageBox.DataContext = err;
            childWindowMessageBox.Visibility = Visibility.Visible;
            childWindowMessageBox.Show();
            //MessageBoxResult res= MessageBox.Show("", "Warning", MessageBoxButton.YesNo);
            //if (res == MessageBoxResult.Yes)
            //{
            //    Application.Current.Shutdown();
            //}
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Minimized;
            }
            else if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState==WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void btnBackClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
        }

        private void btnBackOk_Click(object sender, RoutedEventArgs e)
        {
            App.LogUser = null;
            Application.Current.Shutdown();
        }

        //private void MoveCursorMenu(int index)
        //{
        //    TrainsitionigContentSlide.OnApplyTemplate();
        //    GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        //}
    }
}
