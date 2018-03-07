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
        private Activity act;
        public event PropertyChangedEventHandler PropertyChanged;
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
                    if (Act.Active)
                    {
                        //Start button pressed
                        ActivityPeriod ap = Act.CurrentActiveActivityPeriod;
                        ap = new ActivityPeriod();
                        ap.StartTime = DateTime.Now;
                        Act.ActivityPeriods.Add(ap);
                        OnPropertyChanged("History");
                        OnPropertyChanged("StartTimeDisplay");
                        OnPropertyChanged("StartTimeVisible");
                        OnPropertyChanged("ActivityBtnImageSource");
                        OnPropertyChanged("TimeElapsedColor");
                        OnPropertyChanged("ElapsedTime");
                        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                        {
                            OnPropertyChanged("ElapsedTime");
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
                        OnPropertyChanged("ElapsedTime");
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
        public ImageSource ActivityBtnImageSource
        {
            get
            {
                if (Act.Active)
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
        public string ElapsedTime
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
            if (Act.Active)
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    OnPropertyChanged("ElapsedTime");
                    return Act.Active;
                });
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
