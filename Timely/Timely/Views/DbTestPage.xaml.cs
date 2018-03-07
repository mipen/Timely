using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timely
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DbTestPage : ContentPage
    {
        public DbTestPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.DbTestViewModel(Navigation);
            entryActivityName.SetBinding(Entry.TextProperty, "ActivityName", BindingMode.TwoWay);
            entryCategory.SetBinding(Entry.TextProperty, "Category", BindingMode.TwoWay);
            btnSave.SetBinding(Button.CommandProperty, "SaveButtonCommand");
            btnSave.SetBinding(Button.IsEnabledProperty, "HasValidInput");
            listView.SetBinding(ListView.ItemsSourceProperty, "Activities");
            listView.SetBinding(ListView.IsRefreshingProperty, "LoadingData");
            listView.ItemTemplate = new DataTemplate(typeof(ActivityCell));
            labelPeopleGenerated.SetBinding(Label.TextProperty, "ActivitiesGeneratedString");
            labelElapsedTime.SetBinding(Label.TextProperty, "ElapsedTimeString");
            btnPause.SetBinding(Button.CommandProperty, "PauseButtonCommand");
        }
    }
}