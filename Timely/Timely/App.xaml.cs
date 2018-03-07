using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Timely
{
    public partial class App : Application
    {
        private static ActivityDatabase activityDatabase;
        public static readonly string DBPath = "newdb.db3";
        private static SQLiteAsyncConnection database;
        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (database == null)
                    database = new SQLiteAsyncConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath(DBPath));
                return database;
            }
        }
        public static ActivityDatabase ActivityDatabase
        {
            get
            {
                if (activityDatabase == null)
                    activityDatabase = new ActivityDatabase();

                return activityDatabase;
            }
        }

        public App()
        {
            //File.Delete(DependencyService.Get<IFileHelper>().GetLocalFilePath(DBPath));
            //File.Delete(DependencyService.Get<IFileHelper>().GetLocalFilePath("TimelyDatabase.db3"));
            InitializeComponent();
            InitDatabase();
            MainPage = new MainPage();
        }

        private void InitDatabase()
        {
            database = new SQLiteAsyncConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath(DBPath), false);
            activityDatabase = new ActivityDatabase();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
