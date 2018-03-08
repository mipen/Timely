using System;
using System.Collections.Generic;
using System.Text;

namespace Timely
{
    public static class DatabaseTester
    {
        public async static void TestDatabaseContents()
        {
            List<Activity> activities = await App.ActivityDatabase.GetAllItemsAsync();
            bool flag = true;
            while (flag)
                flag = false;
        }
    }
}
