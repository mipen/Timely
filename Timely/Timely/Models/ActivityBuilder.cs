using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Timely.ViewModels;

namespace Timely
{
    public static class ActivityBuilder
    {
        private static string[] chars = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", " ", };
        public static DbTestViewModel vm;
        public static async Task<List<Activity>> GetActivitiesAsync(int numOfActivities, DbTestViewModel vm)
        {
            ActivityBuilder.vm = vm;
            return await Task.Run(async () =>
            {
                Random r = new Random();
                List<Activity> finalList = new List<Activity>();
                for (int i = 0; i < numOfActivities; i++)
                {
                    Activity a = await GetActivityAsync();
                    finalList.Add(a);
                }
                return finalList;
            });
        }

        public static async Task<Activity> GetActivityAsync()
        {
            return await Task.Run(() =>
            {
                Random r = new Random();
                Activity a = new Activity();
                int num1 = r.Next(2, 8);
                int num2 = r.Next(2, 8);
                int count = 0;
                StringBuilder sb = new StringBuilder();
                for (count = 0; count < num1; count++)
                {
                    sb.Append(chars[r.Next(chars.Length)]);
                }
                a.ActivityName = sb.ToString();
                sb.Clear();
                for (count = 0; count < num2; count++)
                {
                    sb.Append(chars[r.Next(chars.Length)]);
                }
                a.Category = sb.ToString();
                vm.ActivitiesGenerated++;
                vm.ElapsedTime = vm.Watch.ElapsedMilliseconds;
                return a;
            });
        }
    }
}
