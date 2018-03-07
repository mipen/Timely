using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Timely
{
    public class AppListView : ListView
    {
        public AppListView() : base()
        {
            this.ItemSelected += (s, e) =>
            {
                this.TapCommand?.Execute(e.SelectedItem);
            };
        }

        public static readonly BindableProperty TapCommandProperty = BindableProperty.Create(
            nameof(TapCommand),
            typeof(ICommand),
            typeof(AppListView),
            null,
            BindingMode.OneWay,
            null,
            null,
            null,
            null,
            null
            );

        public ICommand TapCommand
        {
            get
            {
                return (ICommand)GetValue(TapCommandProperty);
            }
            set
            {
                SetValue(TapCommandProperty, value);
            }
        }
    }
}
