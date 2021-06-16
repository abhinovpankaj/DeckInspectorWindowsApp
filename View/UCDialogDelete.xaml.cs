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

namespace UI.Code.View
{
    /// <summary>
    /// Interaction logic for UCDialogDelete.xaml
    /// </summary>
    public partial class UCDialogDelete : UserControl
    {
        public event EventHandler BackImageDeleteClick;
        public UCDialogDelete()
        {
            InitializeComponent();
            btnImageDelete.Click += BtnImageDelete_Click;
        }

        private void BtnImageDelete_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = this.BackImageDeleteClick;

            if (eventHandler != null)
            {
                eventHandler(this, e);
            }

        }
    }
}
