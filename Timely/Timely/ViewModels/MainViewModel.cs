using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Timely.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private object selectedActivity;
        private ObservableCollection<Activity> activities = new ObservableCollection<Activity>();
        private bool loadingData = true;
        private bool addActivityButtonEnabled = true;

        public INavigation Navigation { get; set; }
        public ObservableCollection<Activity> Activities
        {
            get
            {
                return activities;
            }
            set
            {
                activities = value;
                OnPropertyChanged("Activities");
            }
        }
        public ICommand AddActivityTapCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigation.PushModalAsync(new NewActivityPage());
                    Thread.Sleep(200);
                    AddActivityButtonEnabled = true;
                });
            }
        }
        public ICommand ListViewItemTapped
        {
            get
            {
                return new Command(() =>
                {
                    if (SelectedActivity != null)
                    {
                        Activity act = SelectedActivity as Activity;
                        if (act != null)
                        {
                            Navigation.PushModalAsync(new ActivityPage(act));
                        }
                    }
                });
            }
        }
        public ICommand InitialiseCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ReloadDatabase();
                });
            }
        }
        public object SelectedActivity
        {
            get
            {
                return selectedActivity;
            }
            set
            {
                selectedActivity = value;
            }
        }
        public bool LoadingData
        {
            get
            {
                return loadingData;
            }
            set
            {
                if (loadingData != value)
                {
                    loadingData = value;
                    OnPropertyChanged("LoadingData");
                }
            }
        }
        public bool AddActivityButtonEnabled
        {
            get
            {
                return addActivityButtonEnabled;
            }
            set
            {
                addActivityButtonEnabled = value;
                OnPropertyChanged("AddActivityButtonEnabled");
            }
        }

        public MainViewModel(INavigation navigation)
        {
            Navigation = navigation;
            App.ActivityDatabase.PropertyChanged += Activities_PropertyChanged;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Activities_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Activities")
            {
                Task.Run(ReloadDatabase);
            }
        }

        public async Task ReloadDatabase()
        {
            LoadingData = true;
            List<Activity> acts = await App.ActivityDatabase.GetAllItemsAsync();
            Activities = new ObservableCollection<Activity>(acts);
            LoadingData = false;
        }
    }
}
