using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Timely
{
    public class ImageButton : Image
    {

        public ImageButton() : base()
        {
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Command = new Command(async () =>
            {
                await OnTapped(this, new EventArgs());
            });
            GestureRecognizers.Add(tap);
        }

        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create(
            nameof(TappedCommand),
            typeof(ICommand),
            typeof(ImageButton),
            null,
            BindingMode.OneWay,
            null,
            null,
            null,
            null,
            null);

        public ICommand TappedCommand
        {
            get
            {
                return (ICommand)GetValue(TappedCommandProperty);
            }
            set
            {
                SetValue(TappedCommandProperty, value);
            }
        }

        private async Task OnTapped(object sender, EventArgs e)
        {
            IsEnabled = false;
            await this.ScaleTo(0.9, 50, Easing.Linear);
            await Task.Delay(50);
            await this.ScaleTo(1, 50, Easing.BounceOut);
            TappedCommand?.Execute(this);
            Task.Run(() =>
            {
                Thread.Sleep(500);
            });
        }
    }
}
