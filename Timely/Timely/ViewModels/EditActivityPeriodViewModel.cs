using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Timely.ViewModels
{
    public class EditActivityPeriodViewModel : INotifyPropertyChanged
    {
        public static readonly string AcceptMessageString = "EditActivityPeriodAcceptButton";
        public static readonly string DeleteMessageString = "EditActivityPeriodDeleteButton";

        public event PropertyChangedEventHandler PropertyChanged;
        private bool acceptButtonEnabled = true;
        private bool deleteButtonEnabled = true;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private TimeSpan originalStartTime;
        private TimeSpan originalEndTime;
        private DateTime startDate;
        private DateTime endDate;
        private DateTime originalStartDate;
        private DateTime originalEndDate;

        public INavigation Navigation { get; set; }
        public ActivityPeriod AP { get; set; }
        public ImageSource AcceptButtonImage
        {
            get
            {
                if (HasValidInput)
                {
                    return ImageSource.FromFile("acceptbtn.png");
                }
                else
                {
                    return ImageSource.FromFile("disabledacceptbtn.png");
                }
            }
        }
        public ICommand AcceptButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (HasValidInput)
                    {
                        MessagingCenter.Send<EditActivityPeriodViewModel, ConfirmationData>(this,
                            AcceptMessageString,
                            new ConfirmationData()
                            {
                                Title = "Update Activity Period",
                                Message = "Are you sure you wish to make these changes? This action cannot be undone."
                            });
                    }
                    Task.Run(() =>
                    {
                        Thread.Sleep(200);
                        AcceptButtonEnabled = true;
                    });
                });
            }
        }
        public ICommand DeleteButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessagingCenter.Send<EditActivityPeriodViewModel, ConfirmationData>(this, DeleteMessageString, new ConfirmationData()
                    {
                        Title = "Delete Activity Period",
                        Message = "Are you sure you wish to delete this Activity Period? This action cannot be undone."
                    });
                    Task.Run(() =>
                    {
                        Thread.Sleep(200);
                        DeleteButtonEnabled = true;
                    });
                });
            }
        }
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
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    OnPropertyChanged("StartDate");
                    OnPropertyChanged("MinimumEndDate");
                    OnPropertyChanged("HasValidInput");
                }
            }
        }
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged("EndTime");
                    OnPropertyChanged("HasValidInput");
                }
            }
        }
        public DateTime MaximumStartDate
        {
            get
            {
                return DateTime.Now;
            }
        }
        public DateTime MinimumEndDate
        {
            get
            {
                return StartDate;
            }
        }
        public TimeSpan StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged("HasValidInput");
                    OnPropertyChanged("StartTime");
                    OnPropertyChanged("AcceptButtonImage");
                }
            }
        }
        public TimeSpan EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged("HasValidInput");
                    OnPropertyChanged("EndTime");
                    OnPropertyChanged("AcceptButtonImage");
                }
            }
        }
        public bool HasValidInput
        {
            get
            {
                return (StartTime.CompareTo(EndTime) == -1) && ((StartDate.Date != originalStartDate.Date) || (EndDate.Date != originalEndDate.Date) || (StartTime != originalStartTime) || (EndTime != originalEndTime));
            }
        }
        public bool AcceptButtonEnabled
        {
            get
            {
                return acceptButtonEnabled;
            }
            set
            {
                acceptButtonEnabled = value;
                OnPropertyChanged("AcceptButtonEnabled");
            }
        }
        public bool DeleteButtonEnabled
        {
            get
            {
                return deleteButtonEnabled;
            }
            set
            {
                deleteButtonEnabled = value;
                OnPropertyChanged("DeleteButtonEnabled");
            }
        }
        public bool EndPickersVisible
        {
            get
            {
                return !AP.Active;
            }
        }

        public EditActivityPeriodViewModel(INavigation navigation, ActivityPeriod ap)
        {
            Navigation = navigation;
            AP = ap;
            startDate = AP.StartTime;
            endDate = AP.EndTime;
            startTime = new TimeSpan(AP.StartTime.Hour, AP.StartTime.Minute, AP.StartTime.Second);
            endTime = new TimeSpan(AP.EndTime.Hour, AP.EndTime.Minute, AP.EndTime.Second);
            originalStartTime = startTime;
            originalEndTime = endTime;
            originalStartDate = AP.StartTime;
            originalEndDate = AP.EndTime;
            this.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "HasValidInput")
                    OnPropertyChanged("AcceptButtonImage");
            };
        }

        public async Task AcceptChanges()
        {
            AP.StartTime = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);
            if (!AP.Active)
            {
                AP.EndTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);
            }
            await App.ActivityDatabase.InsertAsync(AP);
            AP.Activity.OnPropertyChanged("ActivityPeriods");
            AP.Activity.OnPropertyChanged("LastCompletedPeriod");
            AP.Activity.OnPropertyChanged("StartTimeDisplay");
            AP.Activity.OnPropertyChanged("EndTimeDisplay");
            AP.Activity.OnPropertyChanged("LastStartDateDisplay");
            AP.Activity.OnPropertyChanged("LastEndDateDisplay");
            AP.Activity.OnPropertyChanged("TimeElapsedDisplay");
        }

        public async Task DeleteActivityPeriod()
        {
            AP.Activity.DeleteActivityPeriod(AP);
            await App.ActivityDatabase.DeleteAsync(AP);
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
