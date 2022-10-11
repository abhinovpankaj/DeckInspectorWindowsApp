using Microsoft.Win32;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (vm.Factor==2)
            {
                rb1.IsChecked = true;
            }
            if (vm.Factor == 3)
            {
                rb2.IsChecked = true;

            }
            if (vm.Factor == 4)
            {
                rb3.IsChecked = true;
            }
            if (vm.Factor == 5)
            {
                rb4.IsChecked = true;
            }
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
            childWindowTree.Visibility = Visibility.Collapsed;
            childWindowTree.Close();

            childWindowupload.Visibility = Visibility.Collapsed;
            childWindowupload.Close();
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
                WICR_OnlyInvasive_Panel.Visibility = WICR_Invasive_Panel.Visibility = Visibility.Collapsed;
            }
            else
            {
                DI_Invasive_Panel.Visibility = WICR_Invasive_Panel.Visibility = Visibility.Visible;
                WICR_OnlyInvasive_Panel.Visibility = WICR_Invasive_Panel.Visibility = Visibility.Visible;

            }
            //VISUAL FOR DI
            btnReport_Visual_Word.Tag=btnReport_Visual.Tag= p.Id;
            //Invasive FOR DI
            btnReport_Invasive.Tag = btnDIInvasiveWord.Tag = p.InvasiveProjectID;
            btnReport_InvasiveOnly.Tag = btnDIInvasiveOnlyWord.Tag = p.InvasiveProjectID;
            //Finel FOR DI WORD AND PDF
            btnFinelReport_Deck.Tag = btnFilelReport_Deck_Word.Tag = p.Id;

            //WICR VISUAL
            btnReport_Visual_WICR.Tag = WICR_Visual_Word.Tag = p.Id;

            btnReport_Invasive_wicr.Tag= WICR_Invasive_Word.Tag= p.InvasiveProjectID;
            btnReport_InvasiveOnly_wicr.Tag = WICR_InvasiveOnly_Word.Tag = p.InvasiveProjectID;
            
            btnFinelReport_Wicr.Tag = btnWICR_FinelReport_Word.Tag = p.Id;

        }
       
        //Report VISUAL FOR DI PDF
        private void BtnReport_Visual_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();

            Task.Run(()=>vm.WordVisual(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId));

            ShowHideUI();
           // System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordVisual?quality="+ vm.ImageQuality+"&height="+vm.Factor+"&width="+vm.ImageWidth +"&projectID=" + projectId + "&company=DI&Type=Word");
        }
        //Report VISUAL FOR DI WORD
        private void BtnReport_Visual_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(()=>vm.WordVisual(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId,"DI","pdf"));

            ShowHideUI();

            //System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordVisual?quality=" + vm.ImageQuality + 
            //"&height=" + vm.Factor + "&width=" + vm.ImageWidth + "&projectID=" + projectId + "&company=DI&Type=pdf");
        }
        //Report Invasive  DI PDF
        private void BtnReport_Invasive_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(() => vm.WordInvasive(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId, "DI", "pdf"));

            ShowHideUI();
            //System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?quality=" + vm.ImageQuality + "&height=" + vm.Factor + "&width=" + vm.ImageWidth + "&projectID=" + projectId + "&company=DI&Type=pdf");

        }

        private void informUser(Task obj)
        {
            childWindowMessageBox.DataContext = new ErrorModel { Message = "Reported Created Successfully in your Downloads folder.", Status = "Deck Inspectors Report" };
            childWindowMessageBox.Visibility = Visibility.Visible;
            childWindowMessageBox.Show();
        }

      
        private void ShowHideUI()
        {           
            childWindowdownload.Visibility = Visibility.Collapsed;
            childWindowdownload.Close();
            childWindowMessageBox.DataContext = new ErrorModel { Message = "Report creation will start in background, continue your work while we create it for you.Report will be saved in Downloads folder.",
                Status = "Deck Inspectors Report" };
            childWindowMessageBox.Visibility = Visibility.Visible;
            childWindowMessageBox.Show();
        }

        //Report Invasive  DI Word
        private void BtnDIInvasiveWord_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(() => vm.WordInvasive(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId, "DI", "Word"));

            ShowHideUI();
            //System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?quality=" + vm.ImageQuality + "&height=" + 
            //    vm.Factor + "&width=" + vm.ImageWidth + "&projectID=" + projectId + "&company=DI&Type=Word");

        }
        //FINEL DI PDF
        private void btnFinelReport_Deck_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?quality=" + vm.ImageQuality + "&height=" + vm.Factor + "&width=" + vm.ImageWidth + "&projectID=" + projectId + "&company=DI&Type=PDF");
            
        }
        //FINEL DI WORD
        private void BtnFilelReport_Deck_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?quality=" + vm.ImageQuality + "&height=" + vm.Factor + "&width=" + vm.ImageWidth + "&projectID=" + projectId + "&company=DI&Type=Word");
        }


        //----------------WICR-----------------------------------------------------

        private void BtnReport_Visual_WICR_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(()=>vm.WordVisual(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId,"WICR","pdf"));

            ShowHideUI();
           // System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordVisual?quality=" + vm.ImageQuality + "&height=" + vm.Factor + "&width=" + vm.ImageWidth + "&projectID=" +
           // projectId + "&company=WICR&Type=pdf");
        }
        private void WICR_Visual_Word_Click(object sender, RoutedEventArgs e)
        {

            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(()=>vm.WordVisual(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId,"WICR","Word"));

            ShowHideUI();
            //System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordVisual?quality=" + vm.ImageQuality + "&height=" + vm.Factor + "&width=" +
            //vm.ImageWidth + "&projectID=" + projectId + "&company=WICR&Type=Word");
            
        }


        private void BtnReport_Invasive_wicr_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(() => vm.WordInvasive(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId, "WICR", "pdf"));

            ShowHideUI();
            //System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?quality=" + vm.ImageQuality + "&height=" + vm.Factor +
            //"&width=" + vm.ImageWidth + "&projectID=" + projectId + "&company=WICR&Type=pdf");
        }
        private void WICR_Invasive_Word_Click(object sender, RoutedEventArgs e)
        {

            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(() => vm.WordInvasive(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId, "WICR", "Word"));

            ShowHideUI();
            //System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/wordInvasive?quality=" + vm.ImageQuality + "&height=" + vm.Factor + "&width=" +
            //vm.ImageWidth + "&projectID=" + projectId + "&company=WICR&Type=Word");

        }

        private void btnFinelReport_Wicr_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();

            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?quality=" + vm.ImageQuality + "&height=" + vm.Factor + "&width=" + vm.ImageWidth + "&projectID=" + projectId + "&company=WICR&Type=pdf");
        }
        private void BtnWICR_FinelReport_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            System.Diagnostics.Process.Start(App.AppUrl + "/api/SF/WordFinel?quality=" + vm.ImageQuality + "&height=" + vm.Factor + "&width=" + vm.ImageWidth + "&projectID=" + projectId + "&company=WICR&Type=Word");
        }

      
       
        private void BtndownloadClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowdownload.Visibility = Visibility.Collapsed;
            childWindowdownload.Close();

            //Properties.Settings.Default.ImageHeight = vm.ImageHeight;
            Properties.Settings.Default.ImageQuality = vm.ImageQuality;
            //Properties.Settings.Default.ImageWidth = vm.ImageWidth;
            Properties.Settings.Default.Factor = vm.Factor;
            Properties.Settings.Default.Save();

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

        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;
            int intIndex = Convert.ToInt32(radioButton.Content.ToString());
            vm.Factor = intIndex;
            //MessageBox.Show(intIndex.ToString(CultureInfo.InvariantCulture));
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var myVM = this.DataContext as ProjectsPageViewModel;
            Project p = ((Button)sender).DataContext as Project;
            ErrorModel err = new ErrorModel();
            p.IsDelete = false;
            try
            {
                Response result = await myVM.projectService.DeletePermanentlyItemAsync(p.Id);
                if (result.Status == ApiResult.Success)
                {

                    err.Status = "Project deleted successfully.";
                    err.Message = result.Message;
                }
                else
                {
                    err.Status = "Error";
                    err.Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;
            }
            childWindowMessageBox.DataContext = err;
            childWindowMessageBox.Visibility = Visibility.Visible;
            childWindowMessageBox.Show();
            myVM.ReloadLocation(true);
        }

        

        private async void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            var myVM = this.DataContext as ProjectsPageViewModel;
            Project p = ((Button)sender).DataContext as Project;
            ErrorModel err = new ErrorModel();
            p.IsDelete = false;
            try
            {
                Response result = await myVM.projectService.RestoreItemAsync(p);
                if (result.Status == ApiResult.Success)
                {
                    
                    err.Status = "Project restored successfully.";
                    err.Message = result.Message;
                }
                else
                {
                    err.Status = "Error"; 
                    err.Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                err.Status = "Error";
                err.Message = ex.Message;
            }
            childWindowMessageBox.DataContext = err;
            childWindowMessageBox.Visibility = Visibility.Visible;
            childWindowMessageBox.Show();
            myVM.ReloadLocation(true);
           
        }

        private void BtnReport_InvasiveOnly_wicr_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(() => vm.WordInvasive(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId, "WICR", "pdf",true));

            ShowHideUI();
        }

        private void WICR_InvasiveOnly_Word_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(() => vm.WordInvasive(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId, "WICR", "Word",true));

            ShowHideUI();
        }

        private void BtnReport_InvasiveOnly_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(() => vm.WordInvasive(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId, "DI", "pdf", true));

            ShowHideUI();
        }

        private void BtnDIInvasiveOnlyWord_Click(object sender, RoutedEventArgs e)
        {
            string projectId = ((Button)sender).Tag.ToString();
            Task.Run(() => vm.WordInvasive(vm.ImageQuality, vm.Factor, vm.ImageWidth, projectId, "DI", "Word",true));

            ShowHideUI();
        }
        Project selectedProject;
        private async void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            selectedProject = ((Button)sender).DataContext as Project;
            vm.SelectedItem = selectedProject;
            await vm.GetAllDocumentsForProject(selectedProject);
            childWindowupload.DataContext = selectedProject;
            childWindowupload.Visibility = Visibility.Visible;
            childWindowupload.Show();
        }

        private void btnuploadClose_Click(object sender, RoutedEventArgs e)
        {
            childWindowupload.Visibility = Visibility.Collapsed;
            childWindowupload.Close();
        }

        private void btnUploadDocuments_Click(object sender, RoutedEventArgs e)
        {
            List<string> docs = new List<string>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Project files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    docs.Add(System.IO.Path.GetFullPath(filename));

                vm.UploadDocumentsForProject(docs, selectedProject.Id);                
            }
        }
    }
}

