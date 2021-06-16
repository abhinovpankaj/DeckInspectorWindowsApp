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
    /// Interaction logic for LogInfo.xaml
    /// </summary>
    public partial class LogInfo : UserControl
    {
        public LogInfo()
        {
            InitializeComponent();
            this.Loaded += LogInfo_Loaded;
          
        }

        private void LogInfo_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Text = App.LogUser.FullName;
        }
    }
}
