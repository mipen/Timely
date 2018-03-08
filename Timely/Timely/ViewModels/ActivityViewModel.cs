using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Timely.ViewModels
{
    public class ActivityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Activity act;
        private bool activityButtonEnabled = true;
        private bool editActivityButtonEnabled = true;
        private bool backButtonEnabled = true;

        public Activity Act
        {
            get
            {
                return act;
            }
            set
            {
                act = value;
            }
        }
        public INavigation Navigation { get; set; }
        public ICommand BackButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PopModalAsync();
                });
            }
        }
        public ICommand ActivityButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (!Act.Active)
                    {
                        //Start button pressed
                        ActivityPeriod ap = new ActivityPeriod();
                        ap.StartTime = DateTime.Now;
                        Act.AddActivityPeriod(ap);
                        OnPropertyChanged("History");
                        OnPropertyChanged("StartTimeDisplay");
                        OnPropertyChanged("StartTimeVisible");
                        OnPropertyChanged("ActivityBtnImageSource");
                        OnPropertyChanged("TimeElapsedColor");
                        OnPropertyChanged("TimeElapsed");
                        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                        {
                            OnPropertyChanged("TimeElapsed");
                            if (Act.Active)
                            {
                                Act.CurrentActiveActivityPeriod.OnPropertyChanged("TimeElapsedDisplay");
                                Act.OnPropertyChanged("TimeElapsedDisplay");
                            }
                            return Act.Active;
                        });
                        App.ActivityDatabase.InsertAsync(Act);
                        ActivityButtonEnabled = true;
                    }
                    else
                    {
                        //Stop button pressed
                        ActivityPeriod ap = Act.CurrentActiveActivityPeriod;
                        ap.EndTime = DateTime.Now;
                        OnPropertyChanged("History");
                        OnPropertyChanged("StartTimeDisplay");
                        OnPropertyChanged("StartTimeVisible");
                        OnPropertyChanged("ActivityBtnImageSource");
                        OnPropertyChanged("TimeElapsedColor");
                        OnPropertyChanged("TimeElapsed");
                        App.ActivityDatabase.InsertAsync(Act);
                        ActivityButtonEnabled = true;
                    }
                });
            }
        }
        public ICommand EditActivityButtonCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigation.PushModalAsync(new EditActivityPage(Act, PropertyChanged));
                    Thread.Sleep(200);
                    EditActivityButtonEnabled = true;
                });
            }
        }
        public ICommand HistoryItemTappedCommand
        {
            get
            {
                return new Command((object o) =>
                {
                    if (o != null)
                    {
                        ActivityPeriod ap = o as ActivityPeriod;
                        if (ap != null)
                        {
                            Navigation.PushModalAsync(new EditActivityPeriodPage(ap));
                        }
                    }
                });
            }
        }
        public ImageSource ActivityBtnImageSource
        {
            get
            {
                if (!Act.Active)
                    return ImageSource.FromFile("startbtn.png");
                else
                    return ImageSource.FromFile("pausebtn.png");
            }
        }
        public ObservableCollection<ActivityPeriod> History
        {
            get
            {
                return new ObservableCollection<ActivityPeriod>(Act.ActivityPeriodsSorted);
            }
        }
        public Color TimeElapsedColor
        {
            get
            {
                if (StartTimeVisible)
                    return Constants.InProgressTimeElapsedColor;
                else
                    return Constants.TotalTimeElapsedColor;
            }
        }
        public string ActivityName
        {
            get
            {
                return Act.ActivityName;
            }
            set
            {
                if (Act.ActivityName != value)
                {
                    Act.ActivityName = value;
                    OnPropertyChanged("ActivityName");
                }
            }
        }
        public string Category
        {
            get
            {
                return Act.Category;
            }
            set
            {
                if (Act.Category != value)
                {
                    Act.Category = value;
                    OnPropertyChanged("Category");
                }
            }
        }
        public string StartTimeDisplay
        {
            get
            {
                if (Act.Active)
                    return $"Started {Act.CurrentActiveActivityPeriod.StartTimeDisplay}";
                else
                    return "";
            }
        }
        public string TimeElapsed
        {
            get
            {
                if (StartTimeVisible)
                    return Act.CurrentActiveActivityPeriod.TimeElapsedDisplay;
                else
                    return Act.TimeElapsedDisplay;
            }
        }
        public bool StartTimeVisible
        {
            get
            {
                return Act.Active;
            }
        }
        public bool ActivityButtonEnabled
        {
            get
            {
                return activityButtonEnabled;
            }
            set
            {
                activityButtonEnabled = value;
                OnPropertyChanged("ActivityButtonEnabled");
            }
        }
        public bool EditActivityButtonEnabled
        {
            get
            {
                return editActivityButtonEnabled;
            }
            set
            {
                editActivityButtonEnabled = value;
                OnPropertyChanged("EditActivityButtonEnabled");
            }
        }
        public bool BackButtonEnabled
        {
            get
            {
                return backButtonEnabled;
            }
            set
            {
                backButtonEnabled = value;
                OnPropertyChanged("BackButtonEnabled");
            }
        }


        public ActivityViewModel(Activity act, INavigation navigation)
        {
            Navigation = navigation;
            Act = act;
            Act.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "ActivityPeriodsSorted")
                    OnPropertyChanged("History");
                if (e.PropertyName == "TimeElapsedDisplay")
                    OnPropertyChanged("TimeElapsed");
            };
            if (Act.Active)
            {
                OnPropertyChanged("ActivityBtnImageSource");
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    OnPropertyChanged("TimeElapsed");
                    if (Act.Active)
                        Act.CurrentActiveActivityPeriod.OnPropertyChanged("TimeElapsedDisplay");
                    return Act.Active;
                });
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
