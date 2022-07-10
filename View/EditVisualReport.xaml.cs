using System;
using System.Collections.Generic;
using System.IO;
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
using UI.Code.ViewModel.VisualForm;

namespace UI.Code.View
{
    /// <summary>
    /// Interaction logic for EditVisualReport.xaml
    /// </summary>
    public partial class EditVisualReport : UserControl
    {
        public EditVisualReport()
        {
            InitializeComponent();

        }

        string StringFromRichTextBox(RichTextBox rtb)
        {
            string rtfText=string.Empty; 
            TextRange tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, DataFormats.Rtf);
                rtfText = Encoding.ASCII.GetString(ms.ToArray());
            }
            return rtfText;
        }

        private void WaterPopup_Opened(object sender, EventArgs e)
        {
            int selectedCount = 0;
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                foreach (var item in vm.SelectedWaterproofElements)
                {
                    if (vm.WaterProofingElements!=null)
                    {
                        if (vm.WaterProofingElements.Contains(item.Name))
                        {
                            item.IsSelected = true;
                            selectedCount++;
                        }
                            
                    }
                }
                if (selectedCount == 4)
                {
                    waterCheck.IsChecked = true;
                }             

            }

        }

        private void WaterPopup_Closed(object sender, EventArgs e)
        {
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                vm.WaterProofingElements?.Clear();
                foreach (var item in vm.SelectedWaterproofElements)
                {
                    if (item.IsSelected)
                    {
                        if (vm.WaterProofingElements == null)
                        {
                            vm.WaterProofingElements = new System.Collections.ObjectModel.ObservableCollection<string>();
                        }
                        vm.WaterProofingElements.Add(item.Name);

                    }
                }

                vm.CountWaterProofingElements = vm.WaterProofingElements?.Count().ToString();
                if (vm.WaterProofingElements!=null)
                {
                    switch (vm.LocationType)
                    {
                        case 0:
                            vm.ProjLocation.WaterProofingElements = string.Join(",", vm.WaterProofingElements);
                            break;
                        case 1:
                            vm.BuildingLocation.WaterProofingElements = string.Join(",", vm.WaterProofingElements);
                            break;
                        case 2:
                            vm.AptLocation.WaterProofingElements = string.Join(",", vm.WaterProofingElements);
                            break;
                        default:
                            break;
                    }
                }
                

            }
        }

        private void ExteriorPopup_Opened(object sender, EventArgs e)
        {
            int selectedCount = 0;
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                foreach (var item in vm.SelectedExteriorElements)
                {
                    if (vm.ExteriorElements!=null)
                    {
                        if (vm.ExteriorElements.Contains(item.Name))
                        {
                            item.IsSelected = true;
                            selectedCount++;
                        }
                            
                    }
                    
                }
                if (selectedCount == 9)
                {
                    exteriorCheck.IsChecked = true;
                }
               
            }
        }

        private void ExteriorPopup_Closed(object sender, EventArgs e)
        {
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                vm.ExteriorElements?.Clear();
                foreach (var item in vm.SelectedExteriorElements)
                {
                    if (item.IsSelected)
                    {
                        if (vm.ExteriorElements==null)
                        {
                            vm.ExteriorElements = new System.Collections.ObjectModel.ObservableCollection<string>();
                        }
                        vm.ExteriorElements.Add(item.Name);

                    }
                }

                vm.CountExteriorElements = vm.ExteriorElements?.Count().ToString();
                if (vm.ExteriorElements!=null)
                {
                    switch (vm.LocationType)
                    {
                        case 0:
                            vm.ProjLocation.ExteriorElements = string.Join(",", vm.ExteriorElements);
                            break;
                        case 1:
                            vm.BuildingLocation.ExteriorElements = string.Join(",", vm.ExteriorElements);
                            break;
                        case 2:
                            vm.AptLocation.ExteriorElements = string.Join(",", vm.ExteriorElements);
                            break;
                        default:
                            break;
                    }
                }
                
            }
        }

        private void txtDes_LostFocus(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                vm.AdditionalConsideration = StringFromRichTextBox(sender as RichTextBox);

            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            txtDes.TextChanged -= txtDes_TextChanged;
            var currentState = (bool)e.NewValue;
            if (currentState )
            {
                var vm = this.DataContext as EditVisualReportViewModel;
                if (vm != null)
                {
                    string rtfText = vm.AdditionalConsideration;
                    TextRange tr = new TextRange(txtDes.Document.ContentStart, txtDes.Document.ContentEnd);
                    if (!string.IsNullOrEmpty(rtfText))
                    {
                        byte[] byteArray = Encoding.ASCII.GetBytes(rtfText);
                        using (MemoryStream ms = new MemoryStream(byteArray))
                        {

                            tr.Load(ms, DataFormats.Rtf);
                            //tr.ApplyPropertyValue(FontSizeProperty, 15d);
                        }
                    }
                    else
                        tr.Text = "";
                    
                }
            }
            txtDes.TextChanged += txtDes_TextChanged;
        }

        private void txtDes_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange tr = new TextRange(txtDes.Document.ContentStart, txtDes.Document.ContentEnd);
            tr.ApplyPropertyValue(FontSizeProperty, 15d);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                vm.SelectedExteriorElements.ToList().ForEach(x => x.IsSelected = (bool)checkbox.IsChecked);
            }
                
        }

        private void CheckBox_Checked1(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                vm.SelectedWaterproofElements.ToList().ForEach(x => x.IsSelected = (bool)checkbox.IsChecked);
            }

        }
    }
}
