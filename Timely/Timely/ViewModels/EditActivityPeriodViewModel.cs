using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace Timely.ViewModels
{
    public class EditActivityPeriodViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool acceptButtonEnabled = true;
        private bool deleteButtonEnabled = true;

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
                return new Command(async () =>
                {
                    //TODO:: This
                    Thread.Sleep(200);
                    AcceptButtonEnabled = true;
                });
            }
        }
        public ICommand DeleteButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    //TODO:: This
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
                return AP.StartTime;
            }
        }
        public TimeSpan StartTime
        {
            get
            {
                return new TimeSpan(AP.StartTime.Hour, AP.StartTime.Minute, AP.StartTime.Second);
            }
        }
        public DateTime EndDate
        {
            get
            {
                return AP.EndTime;
            }
        }
        public TimeSpan EndTime
        {
            get
            {
                return new TimeSpan(AP.EndTime.Hour, AP.EndTime.Minute, AP.EndTime.Second);
            }
        }
        public bool HasValidInput { get; set; }//TODO:: This
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
                return deleteButtonEnabled = true;
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
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
