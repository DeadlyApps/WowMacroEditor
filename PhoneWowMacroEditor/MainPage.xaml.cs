using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace WowMacroEditorPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void btnFavorites_Click(object sender, EventArgs e)
        {
            pvMain.SelectedItem = piFavMacros;
        }

        private void btnCreateMacro_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CreateMacro.xaml", UriKind.Relative));
        }

        private void lbNewMacros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = (sender as ListBox);
            if (lb == null) return; 
              ItemViewModel selectedResult = (ItemViewModel)lb.SelectedItem;
            if (selectedResult != null)
            {
                IsolatedStorageHelper.SaveObject(selectedResult.MacroTitle.ToString(), selectedResult);
                NavigationService.Navigate(new Uri("/MacroDetails.xaml?id=" + selectedResult.MacroTitle, UriKind.Relative));
            }
        }
    }
}