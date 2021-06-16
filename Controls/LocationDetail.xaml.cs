﻿using System;
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
    /// Interaction logic for LocationDetail.xaml
    /// </summary>
    public partial class LocationDetail : UserControl
    {
        public LocationDetail()
        {
            InitializeComponent();
            DataContext = new LocationDetailPageViewModel();
        }
    }
}
