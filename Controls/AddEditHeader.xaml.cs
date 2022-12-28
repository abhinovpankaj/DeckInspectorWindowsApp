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
using UI.Code.ViewModel;

namespace UI.Code.Controls
{
    /// <summary>
    /// Interaction logic for AddEditHeader.xaml
    /// </summary>
    public partial class AddEditHeader : UserControl
    {
        private bool isEdit;
        public event EventHandler ClickSave;
        public event EventHandler ClickBack;
        public event EventHandler ClickDel;
        public event EventHandler ClickInvasive;
        public event EventHandler ClickExport;
        public event EventHandler ClickImageUpload;
        // public bool IsEdit { get => isEdit; set => isEdit = value; }

        private bool isDisplay;
       
        public bool IsDisplay { get => isDisplay; set => isEdit = isDisplay; }
        public AddEditHeader()
        {
            InitializeComponent();
            IsEdit = false;
            IsDisplay = true;
            btnDataEdit.Click += BtnDataEdit_Click;
            btnDataSave.Click += BtnDataSave_Click;
            btnDataCancel.Click += BtnDataCancel_Click;
            btnInvasive.Click += BtnInvasive_Click;
            btnBack.Click += BtnBack_Click;
            btnDelete.Click += BtnDelete_Click;
            btnExport.Click += BtnExport_Click1;
            btnImageEdit.Click += BtnImageEdit_Click;
            if(IsAddress==true)
            {
                txtAddress.Visibility = Visibility.Visible;
            }
            //var vm = this.DataContext;

            //if (vm.GetType() != typeof(ProjectViewModel))
            //{
            //    btnExport.Visibility = Visibility.Hidden;
            //}
            this.Loaded += AddEditHeader_Loaded;
        }

        private void BtnImageEdit_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.ClickImageUpload;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        private void BtnExport_Click1(object sender, RoutedEventArgs e)
        {
            //var vm = this.DataContext as ProjectViewModel;
            // vm.Project
            var eventHandler = this.ClickExport;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        private void BtnInvasive_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.ClickInvasive;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        private void AddEditHeader_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext;

            if (vm.GetType() == typeof(ProjectViewModel) || vm.GetType() == typeof(VisualSingleLevelProjectLocationViewModel))
            {

                btnExport.Visibility = Visibility.Visible;
                btnInvasive.Visibility = Visibility.Visible;
            }
            else
            {
                btnExport.Visibility = Visibility.Hidden;
                btnInvasive.Visibility = Visibility.Hidden;
            }
            
            gDisplay.Visibility = btnDataEdit.Visibility = Visibility.Visible;
            gAddEdit.Visibility = Visibility.Collapsed;
            btnDataSave.Visibility = btnDataCancel.Visibility = Visibility.Collapsed;
            btnImageEdit.Visibility=Visibility.Collapsed;
           
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.ClickDel;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.ClickBack;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        public static readonly DependencyProperty IsEditProperty =
      DependencyProperty.Register(
      "IsEdit", typeof(Boolean),
      typeof(AddEditHeader)
      );
        public bool IsEdit
        {
            get { return (bool)GetValue(IsEditProperty); }
            set { SetValue(IsEditProperty, value); }
        }


        //  public static readonly DependencyProperty IsAddressProperty =
        //DependencyProperty.Register(
        //"IsAddress", typeof(Boolean),
        //typeof(AddEditHeader),
        //new PropertyMetadata(false, new PropertyChangedCallback(HandleAddressValueChanged)));
        public static readonly DependencyProperty IsAddressProperty =
        DependencyProperty.Register("IsAddress", typeof(bool), typeof(AddEditHeader),
        new PropertyMetadata(false, new PropertyChangedCallback(HandleAddressValueChanged)));


        private static void HandleAddressValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AddEditHeader tv = d as AddEditHeader;
            if (tv != null)
            {

                if ((bool)e.NewValue == true)
                    tv.txtAddress.Visibility = Visibility.Visible;
                else
                {
                    tv.txtAddress.Visibility = Visibility.Collapsed;
                }

            }
        }

        public bool IsAddress
        {
            get { return (bool)GetValue(IsAddressProperty); }
            set { SetValue(IsAddressProperty, value); }
        }
        private void BtnDataSave_Click(object sender, RoutedEventArgs e)
        {
            IsEdit = false;
               var eventHandler = this.ClickSave;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }

            btnDataSave.Visibility = btnDataCancel.Visibility = gAddEdit.Visibility = Visibility.Collapsed;
            btnDataEdit.Visibility = gDisplay.Visibility = Visibility.Visible;
            btnImageEdit.Visibility = Visibility.Collapsed;
        }

        private void BtnDataCancel_Click(object sender, RoutedEventArgs e)
        {
            IsDisplay = true;
            IsEdit = false;
            btnDataEdit.Visibility = gDisplay.Visibility = Visibility.Visible;
            btnDataSave.Visibility = gAddEdit.Visibility = Visibility.Collapsed;
            btnDataCancel.Visibility = Visibility.Collapsed;
            btnImageEdit.Visibility = Visibility.Collapsed;
        }



        private void BtnDataEdit_Click(object sender, RoutedEventArgs e)
        {
            IsDisplay = false;
            IsEdit = true;
            btnDataEdit.Visibility = gDisplay.Visibility = Visibility.Collapsed;
            btnDataSave.Visibility = gAddEdit.Visibility = Visibility.Visible;
            btnImageEdit.Visibility = Visibility.Visible;
            btnDataCancel.Visibility = Visibility.Visible;
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {

        }        
    }
}
