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
    public partial class CreateMacro : PhoneApplicationPage
    {
        public CreateMacro()
        {
            InitializeComponent();
            this.acMacroText.TextFilter = FilterText;
            List<string> cities = new List<string>();
            cities.Add("/cast");
            cities.Add("/castsequence");
            cities.Add("/say");
            cities.Add("#show");
            cities.Add("[modifier:alt]");
            cities.Add("#showtooltip");
            cities.Add("/cleartarget");
            cities.Add("/targetenemy");
            cities.Add("/startattack");
            cities.Add("Victory Rush");
            cities.Add("reset=");
            cities.Add("Strangulate");
            cities.Add("Mind Freeze");
            cities.Add("Mind Sear");
            cities.Add("/stopmacro");
            this.acMacroText.ItemsSource = cities;
            this.acMacroText.Populating += new PopulatingEventHandler(acBox_Populating);
            this.acMacroText.TextChanged += new RoutedEventHandler(acBox_TextChanged);
            this.acMacroText.TextInputUpdate += new TextCompositionEventHandler(acMacroText_TextInputUpdate);
        }

        void acMacroText_TextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            return;
        }


        void acBox_Populating(object sender, PopulatingEventArgs e)
        {
            return;
        }

        String OldValue = String.Empty;
        void acBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (OldValue != acMacroText.Text && !acMacroText.Text.Contains("/"))
            {
                OldValue = OldValue + " " + acMacroText.Text;
                acMacroText.Text = OldValue;
            }
            OldValue = acMacroText.Text;
            return;
        }

        public bool FilterText(string search, string value)
        {

            if (search.Split(' ').Last().StartsWith("/") && value.Split(' ').Last().StartsWith("/"))
            {
                return true;
            }

            if (!search.Split(' ').Last().StartsWith("/") && !value.Split(' ').Last().StartsWith("/"))
            {
                return true;
            }

            return false;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}