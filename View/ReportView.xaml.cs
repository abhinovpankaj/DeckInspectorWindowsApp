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
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl
    {
        public ReportView()
        {
            InitializeComponent();
            DataContext = new ReportPageViewModel();
        }
        private void lboxProjectLocationDataBinding_Drop(object sender, DragEventArgs e)
        {
            ReportPageViewModel vm = DataContext as ReportPageViewModel;
            if (sender is ListBoxItem)
            {
                ProjectLocation droppedData = e.Data.GetData(typeof(ProjectLocation)) as ProjectLocation;
                ProjectLocation target = ((ListBoxItem)(sender)).DataContext as ProjectLocation;

                int removedIdx = lboxProjectLocation.Items.IndexOf(droppedData);
                int targetIdx = lboxProjectLocation.Items.IndexOf(target);

                if (removedIdx < targetIdx)
                {
                    vm.ProjectLocations.Insert(targetIdx + 1, droppedData);
                    vm.ProjectLocations.RemoveAt(removedIdx);
                }
                else
                {
                    int remIdx = removedIdx + 1;
                    if (vm.ProjectLocations.Count + 1 > remIdx)
                    {
                        vm.ProjectLocations.Insert(targetIdx, droppedData);
                        vm.ProjectLocations.RemoveAt(remIdx);
                    }
                }
            }
        }
        void lbox_PreviewMouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                // draggedItem.IsSelected = true;
            }
        }

        private void lboxProjectLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    

}
