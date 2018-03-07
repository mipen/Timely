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

        //TODO:: Clean this class up and rename TimeElapsed\ElapsedTime to something continuous
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
        public ActivityPeriod LastCompletedPeriod
        {
            get
            {
                if (ActivityPeriods.Count > 0)
                {
                    return (from t in ActivityPeriodsSorted
                            where t.Active == false
                            select t).First();
                }
                else
                    return null;
            }
        }
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
        public string ElapsedTimeDisplay
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
        public string StartTimeDisplay
        {
            get
            {
                if (LastCompletedPeriod != null)
                    return LastCompletedPeriod.StartTime.ToString("HH:mm");
                else
                    return "";
            }
        }
        /// <summary>
        /// Get the latest activity period end time (HH:mm)
        /// </summary>
        public string EndTimeDisplay
        {
            get
            {
                if (LastCompletedPeriod != null)
                    return LastCompletedPeriod.EndTime.ToString("HH:mm");
                else
                    return "";
            }
        }
        public string LastStartDateDisplay
        {
            get
            {
                if (LastCompletedPeriod != null)
                    return LastCompletedPeriod.StartTime.ToString(@"dd\/ MM\/ yy");
                else
                    return "";
            }
        }
        public string LastEndDateDisplay
        {
            get
            {
                if (LastCompletedPeriod != null && LastCompletedPeriod.Active == false)
                    return LastCompletedPeriod.EndTime.ToString(@"dd\/ MM\/ yy");
                else
                    return "";
            }
        }
        /// <summary>
        /// Returns the current active activity period. If there is no active period, returns null
        /// </summary>
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

        public Activity()
        {

        }

        public Activity(string activityName)
        {
            ActivityName = activityName;
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
            sb.AppendLine("Time Elapsed: " + ElapsedTimeDisplay);
            return sb.ToString();
        }
    }
}
