using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace WowMacroEditorPhone
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { MacroTitle = "Full One button Pet Treatment", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user1.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Victory Rush Nearest Enemy", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user2.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Omni-Interrupt", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user4.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Warrior Attack", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user3.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Howling blast with auto attack", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user3.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Word of Glory Macro", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user1.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Bear Aoe Rotation", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user2.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Shaman Burst DPS", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user4.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Charge, Intercept, Intervene 1 button fury", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user3.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Holy Shock + Divine Favor", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user1.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Mind Sear", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user3.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Panic Heal", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user3.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Mutilate + Attack", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user4.png" });
            this.Items.Add(new ItemViewModel() { MacroTitle = "Spam Sap", MacroDescription = "This macro will revive your pet if it's dead or you hold down shift, call it if it's dismissed, mend it if it's alive, feed it if you press alt at the same time and dismiss it if you right click the button.", ImageLocation = "/Images/ProfileImages/user4.png" });

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}