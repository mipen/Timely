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
    public class DbTestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Activity> activities = new ObservableCollection<Activity>();
        private Task ReloadDatabaseTask;
        private Task BuildActivityListTask;
        private string activityName = "";
        private string category = "";
        private bool loadingData = true;
        private int activitiesGenerated = 0;
        private long elapsedTime = 0;
        private Stopwatch watch;

        public INavigation Navigation { get; set; }
        public ICommand SaveButtonCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Activity a = new Activity();
                    a.ActivityName = ActivityName;
                    a.Category = Category;

                    ActivityName = "";
                    Category = "";

                    ReloadDatabaseTask = Task.Run(async () =>
                    {
                        await App.ActivityDatabase.InsertAsync(a);
                        ReloadDatabase();
                    });

                });
            }
        }
        public ICommand PauseButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    DoNothing();
                });
            }
        }
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
        public string ActivityName
        {
            get
            {
                return activityName;
            }
            set
            {
                if (activityName != value)
                {
                    activityName = value;
                    OnPropertyChanged("ActivityName");
                    OnPropertyChanged("HasValidInput");
                }
            }
        }
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                if (category != value)
                {
                    category = value;
                    OnPropertyChanged("Category");
                    OnPropertyChanged("HasValidInput");
                }
            }
        }
        public bool HasValidInput
        {
            get
            {
                return !string.IsNullOrEmpty(ActivityName);
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
        public int ActivitiesGenerated
        {
            get
            {
                return activitiesGenerated;
            }
            set
            {
                if (activitiesGenerated != value)
                {
                    activitiesGenerated = value;
                    OnPropertyChanged("ActivitiesGeneratedString");
                }
            }
        }
        public string ActivitiesGeneratedString
        {
            get
            {
                return $"Activities generated: {ActivitiesGenerated.ToString()}";
            }
        }
        public long ElapsedTime
        {
            get
            {
                return elapsedTime;
            }
            set
            {
                if (elapsedTime != value)
                {
                    elapsedTime = value;
                    OnPropertyChanged("ElapsedTimeString");
                }
            }
        }
        public string ElapsedTimeString
        {
            get
            {
                return "Elapsed time(ms): " + ((double)(ElapsedTime / 1000d)).ToString("0.###");
            }
        }
        public Stopwatch Watch
        {
            get
            {
                return watch;
            }
        }

        public DbTestViewModel(INavigation navigation)
        {
            Navigation = navigation;
            //Task.Run(App.PersonDatabase.DeleteAllAsync);
            //ReloadDatabaseTask = Task.Run(async () =>
            //{
            //    Thread.Sleep(5000);
            //    await ReloadDatabase();
            //});
            BuildActivityListTask = Task.Run(async () =>
            {
                Thread.Sleep(5000);
                watch = Stopwatch.StartNew();
                List<Activity> list = await ActivityBuilder.GetActivitiesAsync(500, this);
                await App.ActivityDatabase.InsertAllAsync(list);
                await ReloadDatabase();
                watch.Stop();
                ElapsedTime = watch.ElapsedMilliseconds;
            });
        }

        public async Task ReloadDatabase()
        {
            LoadingData = true;
            List<Activity> loadList = await App.ActivityDatabase.GetAllItemsAsync();
            //Thread.Sleep(10000);
            Activities = new ObservableCollection<Activity>(loadList);
            LoadingData = false;
        }

        public void DoNothing()
        {
            int i = 1 + 2 + 3;
            string s = i.ToString();
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
