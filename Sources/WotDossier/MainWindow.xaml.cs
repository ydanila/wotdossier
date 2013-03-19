﻿using System.Windows;
using System.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay;
using WotDossier.Applications.View;
using WotDossier.Applications.ViewModel;

namespace WotDossier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IShellView
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsViewModel model = new SettingsViewModel(new Settings());
            model.Show();
        }

        public ChartPlotter Chart
        {
            get
            {
                return CommonTab.Chart;
            }
        }
    }
}