using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Timely
{
    public class Activity : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        private List<ActivityPeriod> activityPeriods = new List<ActivityPeriod>();
        private string activityName;
        private string category;

        public event PropertyChangedEventHandler PropertyChanged;
        [Ignore]
        public List<ActivityPeriod> ActivityPeriods
        {
            get
            {
                return activityPeriods;
            }
            set
            {
                activityPeriods = value;
            }
        }
        [Ignore]
        public List<ActivityPeriod> ActivityPeriodsSorted
        {
            get
            {
                return activityPeriods.OrderByDescending(x => x.StartTime).ToList();
            }
        }
        /// <summary>
        /// Returns the current active activity period. If there is no active period, returns null
        /// </summary>
        [Ignore]
        public ActivityPeriod CurrentActiveActivityPeriod
        {
            get
            {
                if (ActivityPeriods.Count > 0)
                {
                    var apList = (from t in ActivityPeriods
                                  where t.Active == true
                                  select t).ToList();
                    if (apList.Count > 0)
                        return apList.First();
                    else
                        return null;
                }
                else
                    return null;
            }
        }
        [Ignore]
        public ActivityPeriod LastCompletedPeriod
        {
            get
            {
                if (ActivityPeriods.Count > 0)
                {
                    var q = (from t in ActivityPeriodsSorted
                            where t.Active == false
                            select t);
                    return q.Count() > 0 ? q.First() : null;
                }
                else
                    return null;
            }
        }
        [Ignore]
        public TimeSpan TimeElapsed
        {
            get
            {
                TimeSpan totalTime = new TimeSpan(0);
                if (ActivityPeriods.Count > 0)
                {
                    foreach (ActivityPeriod ap in ActivityPeriods)
                    {
                        if (!ap.Active)
                            totalTime += ap.TimeElapsed;
                    }
                }
                return totalTime;
            }
        }
        [Ignore]
        public bool Active
        {
            get
            {
                return CurrentActiveActivityPeriod != null;
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
                if (value != activityName)
                {
                    activityName = value;
                    OnPropertyChanged("ActivityName");
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
                category = value;
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
        /// <summary>
        /// Gets the latest activity period start time (HH:mm)
        /// </summary>
        [Ignore]
        public string StartTimeDisplay
        {
            get
            {
                if (LastCompletedPeriod != null)
                    return LastCompletedPeriod.StartTime.ToString(Constants.TimeFormat);
                else
                    return "";
            }
        }
        /// <summary>
        /// Get the latest activity period end time (HH:mm)
        /// </summary>
        [Ignore]
        public string EndTimeDisplay
        {
            get
            {
                if (LastCompletedPeriod != null)
                    return LastCompletedPeriod.EndTime.ToString(Constants.TimeFormat);
                else
                    return "";
            }
        }
        [Ignore]
        public string LastStartDateDisplay
        {
            get
            {
                if (LastCompletedPeriod != null)
                    return LastCompletedPeriod.StartTime.ToString(Constants.DateFormat);
                else
                    return "";
            }
        }
        [Ignore]
        public string LastEndDateDisplay
        {
            get
            {
                if (LastCompletedPeriod != null && LastCompletedPeriod.Active == false)
                    return LastCompletedPeriod.EndTime.ToString(Constants.DateFormat);
                else
                    return "";
            }
        }

        public Activity()
        {

        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} ID: {1}", ActivityName, ID));
            sb.AppendLine("Category: " + Category);
            sb.AppendLine("Start Time: " + StartTimeDisplay);
            sb.AppendLine("End Time: " + EndTimeDisplay);
            sb.AppendLine("Time Elapsed: " + TimeElapsedDisplay);
            return sb.ToString();
        }
    }
}
