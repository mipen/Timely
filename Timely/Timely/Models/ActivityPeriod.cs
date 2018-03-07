using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Timely
{
    public class ActivityPeriod : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime startTime = DateTime.MinValue;
        private DateTime endTime = DateTime.MinValue;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int ActivityID { get; set; }
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                OnPropertyChanged("Active");
            }
        }
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                OnPropertyChanged("Active");
            }
        }
        [Ignore]
        public TimeSpan TimeElapsed
        {
            get
            {
                if (StartTime != DateTime.MinValue)
                {
                    if (EndTime != DateTime.MinValue)
                        return EndTime.Subtract(StartTime);
                    else
                        return DateTime.Now.Subtract(StartTime);
                }
                else
                    return new TimeSpan(0);
            }
        }
        [Ignore]
        public Color TimeElapsedColor
        {
            get
            {
                if (Active)
                    return Constants.InProgressTimeElapsedColor;
                else
                    return Constants.HistoryTextColor;
            }
        }
        [Ignore]
        public bool EndTimeVisible
        {
            get
            {
                return !Active;
            }
        }
        [Ignore]
        public string DateDisplay
        {
            get
            {
                return StartTime.ToString(Constants.DateFormat);
            }
        }
        [Ignore]
        public string StartTimeDisplay
        {
            get
            {
                return StartTime.ToString(Constants.TimeFormat);
            }
        }
        [Ignore]
        public string EndTimeDisplay
        {
            get
            {
                return EndTime.ToString(Constants.TimeFormat);
            }
        }
        [Ignore]
        public string TimeElapsedDisplay
        {
            get
            {
                if (TimeElapsed.TotalHours > 1)
                    return TimeElapsed.TotalHours.ToString("0.#") + "h";
                else if (TimeElapsed.TotalMinutes > 1)
                    return TimeElapsed.TotalMinutes.ToString("0") + "m";
                else
                    return TimeElapsed.TotalSeconds.ToString("0") + "s";
            }
        }
        [Ignore]
        public bool Active
        {
            get
            {
                return EndTime == DateTime.MinValue && StartTime != DateTime.MinValue;
            }
        }

        public ActivityPeriod()
        {

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
