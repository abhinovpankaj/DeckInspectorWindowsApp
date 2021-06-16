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

namespace UI.Code.Controls
{
    /// <summary>
    /// Interaction logic for AssginControl.xaml
    /// </summary>
    public partial class AssginControl : UserControl
    {
        public event EventHandler ClickUserSearch;
        public event EventHandler ClickUserReset;
        public AssginControl()
        {
            InitializeComponent();
            //btnAssignCLose.Click += BtnAssignCLose_Click;
            //btnAssignCLose.Click += BtnAssignCLose_Click1;
        }
        public string AssignSearchText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("AssignSearchText", typeof(string), typeof(AssginControl), new PropertyMetadata(null));


        public string AssignProjectID
        {
            get { return (string)GetValue(AssignProjectProperty); }
            set { SetValue(AssignProjectProperty, value); }
        }
        public static readonly DependencyProperty AssignProjectProperty =
            DependencyProperty.Register("AssignProjectID", typeof(string), typeof(AssginControl), new PropertyMetadata(null));

        private void BtnAssignCLose_Click1(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnAssignCLose_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnAssignCLose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAssignSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReset1_Click(object sender, RoutedEventArgs e)
        {
            CommentTextBoxAssigne.Text = string.Empty;
          
            var eventHandler = this.ClickUserReset;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        private void btnSearch1_Click(object sender, RoutedEventArgs e)
        {
            AssignSearchText = CommentTextBoxAssigne.Text;
            var eventHandler = this.ClickUserSearch;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }

        }
    }
}
