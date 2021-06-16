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
    /// Interaction logic for ProjectsPageView.xaml
    /// </summary>
    public partial class ProjectsPageView : UserControl
    {
        public event EventHandler ClickMove;
        ProjectsPageViewModel vm;
        public ProjectsPageView()
        {
            InitializeComponent();
            vm = this.DataContext as ProjectsPageViewModel;
            
            //fileControl.DataContext = new UCFilePageViewModel(null);
            fileControl.ClickClose += FileControl_ClickClose;
           
            //   DataContext = new ProjectsPageViewModel();
            // List<TypeList> projectTypelist = new List<TypeList>();
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual3" });
            //projectTypelist.Add(new TypeList() { ProjectType = "Visual4" });
            //CbProType.ItemsSource = projectTypelist;
            this.Loaded += ProjectsPageView_Loaded;
            assignControl.ClickUserSearch += AssignControl_ClickUserSearch;
            assignControl.ClickUserReset += AssignControl_ClickUserReset;
            //lvDataBinding.SelectionChanged += LvDataBinding_SelectionChanged;
        }

        private void FileControl_ClickClose(object sender, EventArgs e)
        {
            vm.SearchCommand.Execute();
            childWindowTree.Visibility = Visibility.Collapsed;
            childWindowTree.Close();
            //throw new NotImplementedException();
        }

        private async  void AssignControl_ClickUserReset(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(App.AppTempProjectID) == true)
            //{
            //    App.AppTempProjectID = vm.SelectedItem.Id;
            //}
            assignControl.AssignSearchText = string.Empty;
                vm.IsBusy = true;
                // vm.UserSearch = null;
                bool complete = await vm.GetUserListForAssign(App.LogUser.Id, assignControl.AssignProjectID, string.Empty, string.Empty, "Project", null);
                if (complete == true)
                {
                    vm.IsBusy = false;
                }
            //}
            
        }

        private async void AssignControl_ClickUserSearch(object sender, EventArgs e)
        {
            vm.IsBusy = true;
            //App.AppTempProjectID = string.Empty;
            //if (vm.SelectedItem != null)
            //{
            //  vm.IsBusy = true;
            // App.AppTempProjectID = vm.SelectedItem.Id;

            if (string.IsNullOrEmpty(assignControl.AssignSearchText))
                {
                    bool complete = await vm.GetUserListForAssign(App.LogUser.Id, assignControl.AssignProjectID, string.Empty, string.Empty, "Project", assignControl.AssignSearchText);
                    if (complete == true)
                    {
                        vm.IsBusy = false;
                    }
                }
                else
                {
                bool complete = await vm.GetUserListForAssign(App.LogUser.Id, assignControl.AssignProjectID, string.Empty, string.Empty, "Project", assignControl.AssignSearchText);
               
                if (complete == true)
                {
                    vm.UsersAssignList = new System.Collections.ObjectModel.ObservableCollection<User>(vm.UsersAssignList.Where(c => c.FullName.ToUpper().Contains(assignControl.AssignSearchText.ToUpper())));
                    vm.IsBusy = false;
                }
            }
                //bool complete = await vm.GetUserListForAssign(App.LogUser.Id, vm.SelectedItem.Id, string.Empty, string.Empty, "Project", assignControl.AssignSearchText);
                //if (complete == true)
                //{
                //    vm.IsBusy = false;
                //}
            //}

        }

        private void ProjectsPageView_Loaded(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();
            childWindowdownload.Visibility = Visibility.Collapsed;
            childWindowdownload.Close();
            childWindowdownload.Visibility = Visibility.Collapsed;
            childWindowTree.Close();
        }

        private async void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void lvDataBinding_Drop(object sender, DragEventArgs e)
        {
            ProjectsPageViewModel vm = DataContext as ProjectsPageViewModel;
            if (sender is ListBoxItem)
            {
                Project droppedData = e.Data.GetData(typeof(Project)) as Project;
                Project target = ((ListBoxItem)(sender)).DataContext as Project;

                int removedIdx = lvDataBinding.Items.IndexOf(droppedData);
                int targetIdx = lvDataBinding.Items.IndexOf(target);

                if (removedIdx < targetIdx)
                {
                    vm.Projects.Insert(targetIdx + 1, droppedData);
                    vm.Projects.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (vm.Projects.Count + 1 > remIdx)
                    {
                        vm.Projects.Insert(targetIdx, droppedData);
                        vm.Projects.RemoveAt(remIdx);
                    }
                }
            }
        }
        void s_PreviewMouseMoveEvent(object sender, MouseEventArgs e)
        {
            //if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            //{
            //    ListBoxItem draggedItem = sender as ListBoxItem;
            //    DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
            //    draggedItem.IsSelected = true;
            //}
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
        private void LvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var item = (ListBox)sender;
            //var obj = (Project)item.SelectedItem;
            //if (obj != null)
            //{
            //  vm.SelectedItemCommand.Execute();
            //}
        }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = ((ListBoxItem)sender).DataContext as Project;
            vm.SelectedItemCommand.Execute(obj);
        }
        private async void btnAgging_click(object sender, RoutedEventArgs e)
        {
            vm.UserSearch = null;
            vm.IsBusy = true;
            Project p = ((Button)sender).DataContext as Project;
            vm.SelectedItem = p;
            bool complete = await vm.GetUserListForAssign(App.LogUser.Id, p.Id, string.Empty, string.Empty, "Project", vm.UserSearch);
            if (complete == true)
            {
                vm.IsBusy = false;
                assignControl.AssignProjectID = vm.SelectedItem.Id;
                childWindowAssign.Caption = "Assign project - " + p.Name + " to user(s)";
                childWindowAssign.FontWeight = FontWeights.Bold;
                childWindowAssign.Visibility = Visibility.Visible;
                childWindowAssign.Show();
            }

            //   ListBoxItem selectedItem = (ListBoxItem)lboxProjectLocation.ItemContainerGenerator.
            //                  ContainerFromItem(((Button)sender).DataContext);
            //selectedItem.IsSelected = true;
            //var obj = ((ListBoxItem)sender).DataContext as ProjectBuilding;
            //vm.ProjBuildingSelectedItemCommand.Execute(obj);
        }

        private void btnAssignClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowAssign.Visibility = Visibility.Collapsed;
            childWindowAssign.Close();
        }
        private void btnInvasive_Click(object sender, RoutedEventArgs e)
        {
           // vm.UserSearch = null;
            vm.IsBusy = true;
            Project p = ((Button)sender).DataContext as Project;
            vm.InvasiveItemCommand.Execute(p);
            //vm.SelectedItem = p;
            //bool complete = await vm.GetUserListForAssign(App.LogUser.Id, p.Id, string.Empty, string.Empty, "Project", vm.UserSearch);
            //if (complete == true)
            //{
            //    vm.IsBusy = false;
            //    childWindowAssign.Caption = "Assign project - " + p.Name + " to user(s)";
            //    childWindowAssign.FontWeight = FontWeights.Bold;
            //    childWindowAssign.Visibility = Visibility.Visible;
            //    childWindowAssign.Show();
         //  }
            
        }
        private async void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            vm.IsBusy = true;
            ErrorModel err = await vm.Save();
            childWindowAssign.Visibility = Visibility.Collapsed;
            childWindowAssign.Close();
            vm.ReloadLocation();
            vm.IsBusy = false;
            childWindowMessageBox.DataContext = err;
            childWindowMessageBox.Visibility = Visibility.Visible;
            childWindowMessageBox.Show();
            //  MessageBox.Show(err.Message, err.Status,MessageBoxButton.OK,MessageBoxImage.Information);
            if (err.Message == "Success")
            {

                // childWindowFeedback.DataContext = err;


            }
            else
            {
                childWindowAssign.Visibility = Visibility.Collapsed;
                childWindowAssign.Close();
                // MessageBox.Show(err.Message,"Error");
            }
            //childWindowFeedback.Visibility = Visibility.Visible;
            // childWindowFeedback.Show();
        }

        private void btnAssign_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnErrorClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowFeedback.Visibility = Visibility.Collapsed;
            childWindowFeedback.Close();

        }

        private void btnErrorMsgClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowMessageBox.Visibility = Visibility.Collapsed;
            childWindowMessageBox.Close();
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            rbTab1.IsChecked = true;
            //vm.UserSearch = null;
            //vm.IsBusy = true;
            Project p = ((Button)sender).DataContext as Project;
            childWindowdownload.DataContext = p;
            childWindowdownload.Visibility = Visibility.Visible;
            childWindowdownload.Show();
            if(string.IsNullOrEmpty(p.InvasiveProjectID))
            {
                DI_Invasive_Panel.Visibility = WICR_Invasive_Panel.Visibility= Visibility.Collapsed;
            }
            else
            {
                DI_Invasive_Panel.Visibility = WICR_Invasive_Panel.Visibility = Visibility.Visible;

            }
            //VISUAL FOR DI
            btnReport_Visual_Word.Tag=btnReport_Visual.Tag= p.Id;
            //Invasive FOR DI
            btnReport_Invasive.Tag = btnDIInvasiveWord.Tag = p.InvasiveProjectID;
            //Finel FOR DI WORD AND PDF
            btnFinelReport_Deck.Tag = btnFilelReport_Deck_Word.Tag = p.Id;

            //WICR VISUAL
            btnReport_Visual_WICR.Tag = WICR_Visual_Word.Tag = p.Id;

            btnReport_Invasive_wicr.Tag= WICR_Invasive_Word.Tag= p.InvasiveProjectID;

            btnFinelReport_Wicr.Tag = btnWICR_FinelReport_Word.Tag = p.Id;


            //btnFinelReport_Deck.Tag = p.Id;

            //btnReport_Invasive.Tag=WICR_Invasive_Word.Tag = p.InvasiveProjectID;

            //btnFilelReport_Deck_Word.Tag = btnWICR_FinelReport_Word.Tag = p.Id;

            //btnReport_Visual_WICR.Tag = p.Id;
            //btnReport_Invasive_wicr.Tag= btnDIInvasiveWord.Tag = p.InvasiveProjectID;


           
            //btnFinelReport_Wicr.Tag = p.Id;
            //vm.SelectedItem = p;
            //bool complete = await vm.GetUserListForAssign(App.LogUser.Id, p.Id, string.Empty, string.Empty, "Project", vm.UserSearch);
            //if (complete == true)
            //{
            //    vm.IsBusy = false;
            //    childWindowAssign.Caption = "Assign project - " + p.Name + " to user(s)";
            //    childWindowAssign.FontWeight = FontWeights.Bold;
            //    childWindowAssign.Visibility = Visibility.Visible;
            //    childWindowAssign.Show();
            //}
        }
        //Report VISUAL FOR DI PDF
        private void BtnReport_Visual_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordVisual?projectID=" + projectId + "&company=DI&Type=Word");
        }
        //Report VISUAL FOR DI WORD
        private void BtnReport_Visual_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();

            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordVisual?projectID=" + projectId + "&company=DI&Type=pdf");
        }
        //Report Invasive  DI PDF
        private void BtnReport_Invasive_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            //  string url = App.AppUrl + "/api/values/GetInvasivelDI?projectID=" + projectId;
            //  System.Diagnostics.Process.Start(App.AppUrl +"/api/values/GetInvasivelDI?projectID=" + projectId + "&company=DI");
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?projectID=" + projectId + "&company=DI&Type=pdf");
            // System.Diagnostics.Process.Start("http://techcodevity.com/api/values/GetVisualDI?projectID=11D6DFDB-EF89-42E4-A127-7565CCE65DC0");
        }
        //Report Invasive  DI Word
        private void BtnDIInvasiveWord_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?projectID=" + projectId + "&company=DI&Type=Word");
        }
        //FINEL DI PDF
        private void btnFinelReport_Deck_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId + "&company=DI&Type=PDF");
            //string projectId = ((Button)sender).Tag.ToString();
            //string url = App.AppUrl + "/api/values/GetInvasivelDI?projectID=" + projectId;
            // System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId);
        }
        //FINEL DI WORD
        private void BtnFilelReport_Deck_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId + "&company=DI&Type=Word");
        }


        //----------------WICR-----------------------------------------------------

        private void BtnReport_Visual_WICR_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordVisual?projectID=" + projectId + "&company=WICR&Type=pdf");
        }
        private void WICR_Visual_Word_Click(object sender, RoutedEventArgs e)
        {

            string projectId = ((Button)sender).Tag.ToString();
            //  string url = App.AppUrl + "/api/values/GetInvasivelDI?projectID=" + projectId;
            //  System.Diagnostics.Process.Start(App.AppUrl +"/api/values/GetInvasivelDI?projectID=" + projectId + "&company=DI");
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordVisual?projectID=" + projectId + "&company=WICR&Type=Word");
            // S
        }


        private void BtnReport_Invasive_wicr_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?projectID=" + projectId + "&company=WICR&Type=pdf");
        }
        private void WICR_Invasive_Word_Click(object sender, RoutedEventArgs e)
        {

            string projectId = ((Button)sender).Tag.ToString();
           
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?projectID=" + projectId + "&company=WICR&Type=Word");
            
        }

        private void btnFinelReport_Wicr_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();

            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId + "&company=WICR&Type=pdf");
        }
        private void BtnWICR_FinelReport_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?projectID=" + projectId + "&company=WICR&Type=Word");
        }

      
       
        private void BtndownloadClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowdownload.Visibility = Visibility.Collapsed;
            childWindowdownload.Close();
        }

       

       
       
        

       

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;
        }

        private void btnFinelStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnFinelStatusProgress_Click(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;
            p.FinelReport = true;
            vm.IsBusy = true;
            ErrorModel err = await vm.FinelSaveCompleted(p);
            
            vm.ReloadLocation();
            vm.IsBusy = false;
            
            //  MessageBox.Show(err.Message, err.Status,MessageBoxButton.OK,MessageBoxImage.Information);
            if (err.Message == "Success")
            {

                // childWindowFeedback.DataContext = err;


            }
            else
            {
                childWindowMessageBox.DataContext = err;
                childWindowMessageBox.Visibility = Visibility.Visible;
                childWindowMessageBox.Show();
                // childWindowAssign.Visibility = Visibility.Collapsed;
                //  childWindowAssign.Close();
                // MessageBox.Show(err.Message,"Error");
            }

        }

        private async void btnFinelStatuscomplete_Click(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;
            p.FinelReport = false;
            vm.IsBusy = true;
            ErrorModel err = await vm.FinelSaveCompleted(p);

            vm.ReloadLocation();
            vm.IsBusy = false;

            //  MessageBox.Show(err.Message, err.Status,MessageBoxButton.OK,MessageBoxImage.Information);
            if (err.Message == "Success")
            {

                // childWindowFeedback.DataContext = err;


            }
            else
            {
                childWindowMessageBox.DataContext = err;
                childWindowMessageBox.Visibility = Visibility.Visible;
                childWindowMessageBox.Show();
                // childWindowAssign.Visibility = Visibility.Collapsed;
                //  childWindowAssign.Close();
                // MessageBox.Show(err.Message,"Error");
            }

        }

      

       

        private void BtnMove_Click(object sender, RoutedEventArgs e)
        {
            Project p = ((Button)sender).DataContext as Project;
           // vm.FileCommand.Execute(p);
            // UCFilePageViewModel vmFile=fileControl.DataContext as UCFilePageViewModel;
            App.MoveProject = p;
            //vmFile.MoveProject = p;
            //  fileTree.DataContext = new FilePageViewModel(null);
            //Project p = ((Button)sender).DataContext as Project;
            //var eventHandler = this.ClickMove;

            //if (eventHandler != null)
            //{
            //    eventHandler(this, e);
            //}

            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            //  childWindowTree.Margin = new Thickness(0, 10, 0, 0);
            childWindowTree.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            childWindowTree.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
            childWindowTree.Visibility = Visibility.Visible;
            childWindowTree.Visibility = Visibility.Visible;
            childWindowTree.Show();
        }

        private void BtnTreeClose_Click(object sender, RoutedEventArgs e)
        {
            vm.SearchCommand.Execute();
            childWindowTree.Visibility = Visibility.Collapsed;
            childWindowTree.Close();
        }

       

     

        

      

        //private async void chbIsCompleted_Checked(object sender, RoutedEventArgs e)
        //{
        //    if(chbIsCompleted.IsChecked==true)
        //    {
        //        vm.SelectedeportType = "Completed";
        //         vm.SearchCommand.Execute();
        //       //vm.se
        //    }
        //    else
        //    {
        //        vm.SelectedeportType = "All";
        //        vm.SearchCommand.Execute();
        //    }
        //}
    }
}
