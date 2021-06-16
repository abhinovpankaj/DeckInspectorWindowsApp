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

        private void WaterPopup_Opened(object sender, EventArgs e)
        {
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                foreach (var item in vm.SelectedWaterproofElements)
                {
                    if (vm.WaterProofingElements.Contains(item.Name))
                        item.IsSelected = true;
                }
            }

        }

        private void WaterPopup_Closed(object sender, EventArgs e)
        {
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                vm.WaterProofingElements.Clear();
                foreach (var item in vm.SelectedWaterproofElements)
                {
                    if (item.IsSelected)
                    {
                        vm.WaterProofingElements.Add(item.Name);

                    }
                }

                vm.CountWaterProofingElements = vm.WaterProofingElements.Count().ToString();
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

        private void ExteriorPopup_Opened(object sender, EventArgs e)
        {
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                foreach (var item in vm.SelectedExteriorElements)
                {
                    if (vm.ExteriorElements.Contains(item.Name))
                        item.IsSelected = true;
                }
            }
        }

        private void ExteriorPopup_Closed(object sender, EventArgs e)
        {
            var vm = this.DataContext as EditVisualReportViewModel;
            if (vm != null)
            {
                vm.ExteriorElements.Clear();
                foreach (var item in vm.SelectedExteriorElements)
                {
                    if (item.IsSelected)
                    {
                        vm.ExteriorElements.Add(item.Name);

                    }
                }

                vm.CountExteriorElements = vm.ExteriorElements.Count().ToString();
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
}
