using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using static Timely.App;

namespace Timely
{
    public class ActivityDatabase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ActivityDatabase()
        {
            Database.CreateTableAsync<Activity>().Wait();
            Database.CreateTableAsync<ActivityPeriod>().Wait();
        }


        public Task<List<Activity>> GetAllItemsAsync()
        {
            return Task.Run(async () =>
            {
                List<Activity> actList = await Database.Table<Activity>().ToListAsync();
                List<ActivityPeriod> actPList = await Database.Table<ActivityPeriod>().ToListAsync();
                if (actList.Count > 0)
                {
                    if (actPList.Count > 0)
                    {
                        foreach (var act in actList)
                        {
                            var apQuery = (from t in actPList
                                           where t.ActivityID == act.ID
                                           select t).ToList();
                            if (apQuery.Count > 0)
                            {
                                act.AddActivityPeriods(apQuery);
                            }
                        }
                    }
                }
                return actList;
            });
        }

        public Task<Activity> FindAsync(int id)
        {
            return Task.Run<Activity>(async () =>
            {
                Activity act = await Database.Table<Activity>().Where(a => a.ID == id).FirstOrDefaultAsync();
                List<ActivityPeriod> aps = await FindAllPeriodsAsync(act.ID);
                if (aps.Count > 0)
                {
                    act.AddActivityPeriods(aps);
                }
                return act;
            });
        }

        private Task<List<ActivityPeriod>> FindAllPeriodsAsync(int actID)
        {
            return Task.Run<List<ActivityPeriod>>(() =>
            {
                return Database.Table<ActivityPeriod>().Where(ap => ap.ActivityID == actID).ToListAsync();
            });
        }

        public Task InsertAsync(Activity act)
        {
            return Task.Run(async () =>
            {
                int id = 0;
                if (act.ID == 0)
                {
                    id = await Database.InsertAsync(act);
                }
                else
                {
                    id = act.ID;
                    await Database.UpdateAsync(act);
                }
                if (act.ActivityPeriods.Count > 0)
                {
                    foreach (var ap in act.ActivityPeriods)
                    {
                        ap.ActivityID = id;
                        await InsertAsync(ap);
                    }
                }
                OnPropertyChanged("Activities");
            });
        }

        public Task InsertAsync(List<Activity> list)
        {
            return Task.Run(async () =>
            {
                foreach (var a in list)
                {
                    await InsertAsync(a);
                }
            });
        }

        public Task InsertAsync(ActivityPeriod ap)
        {
            if (ap.ID == 0)
            {
                return Database.InsertAsync(ap);
            }
            else
            {
                return Database.UpdateAsync(ap);
            }
        }

        public Task DeleteAsync(Activity act)
        {
            return Task.Run(async () =>
            {
                if (act.ActivityPeriods.Count > 0)
                {
                    await DeleteAsync(act.ActivityPeriods);
                }
                await Database.DeleteAsync(act);
                OnPropertyChanged("Activities");
            });
        }

        public Task DeleteAsync(ActivityPeriod ap, bool doUpdate = true)
        {
            return Task.Run(async () =>
            {
                await Database.DeleteAsync(ap);
                if (doUpdate)
                    OnPropertyChanged("Activities");
            });
        }

        public Task DeleteAsync(List<ActivityPeriod> aps)
        {
            return Task.Run(async () =>
            {
                foreach (var ap in aps)
                {
                    await DeleteAsync(ap, false);
                }
            });
        }

        public Task DeleteAllItemsAsync()
        {
            return Task.Run(async () =>
            {
                await Database.DropTableAsync<Activity>();
                await Database.DropTableAsync<ActivityPeriod>();
                await Database.CreateTableAsync<Activity>();
                await Database.CreateTableAsync<ActivityPeriod>();
            });
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
