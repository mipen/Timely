using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Timely
{
    public class ActivityPeriodCell : ViewCell
    {
        public ActivityPeriodCell()
        {
            RelativeLayout relLayout = new RelativeLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            Label labelDate = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                FontFamily = "Roboto",
                TextColor = Constants.HistoryTextColor
            };
            Label labelStartTime = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                FontFamily = "Roboto",
                TextColor = Constants.HistoryTextColor
            };
            Label labelEndTime = new Label()
            {
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                FontFamily = "Roboto",
                TextColor = Constants.HistoryTextColor
            };
            Label labelTimeElapsed = new Label()
            {
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                FontFamily = "Roboto",
                TextColor = Constants.HistoryTextColor
            };
            Image imgSeparator = new Image()
            {
                Source = "historyseparator.png"
            };

            //Set bindings
            labelDate.SetBinding(Label.TextProperty, "DateDisplay");
            labelEndTime.SetBinding(Label.TextProperty, "EndTimeDisplay");
            labelEndTime.SetBinding(Label.IsVisibleProperty, "EndTimeVisible");
            labelStartTime.SetBinding(Label.TextProperty, "StartTimeDisplay");
            labelTimeElapsed.SetBinding(Label.TextProperty, "TimeElapsedDisplay");
            labelTimeElapsed.SetBinding(Label.TextColorProperty, "TimeElapsedColor");
            //TODO:: Start a timer to update elapsed time display

            //Method goes -> X, Y, Width, Height
            //Add the controls
            int indent = 25;
            relLayout.Children.Add(labelDate, Constraint.RelativeToParent(p =>
            {
                return p.X + indent;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Y;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width / 2 - indent;
            }),
            Constraint.Constant(20)
            );

            relLayout.Children.Add(labelStartTime, Constraint.RelativeToView(labelDate, (p, s) =>
            {
                return s.X;
            }),
            Constraint.RelativeToView(labelDate, (p, s) =>
            {
                return s.Height + 5;
            }),
            Constraint.RelativeToView(labelDate, (p, s) =>
            {
                return s.Width;
            }),
            Constraint.Constant(20)
            );

            relLayout.Children.Add(labelTimeElapsed, Constraint.RelativeToParent(p =>
            {
                return p.Width - indent - 100;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Y;
            }),
            Constraint.Constant(100),
            Constraint.Constant(20)
            );

            relLayout.Children.Add(labelEndTime, Constraint.RelativeToView(labelTimeElapsed, (p, s) =>
            {
                return s.X;
            }),
            Constraint.RelativeToView(labelStartTime, (p, s) =>
            {
                return s.Height + 5;
            }),
            Constraint.RelativeToView(labelTimeElapsed, (p, s) =>
            {
                return s.Width;
            }),
            Constraint.Constant(20)
            );

            relLayout.Children.Add(imgSeparator, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToView(labelStartTime, (p, s) =>
            {
                return s.Y + s.Height + 2;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.Constant(1));

            //TODO:: Implement this delete action long press
            //Add delete option
            //MenuItem deleteAction = new MenuItem() { Text = "Delete", IsDestructive = true };
            //deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            //deleteAction.Clicked += async (sender, e) =>
            // {
            //     var mi = ((MenuItem)sender);
            //     //Debug.WriteLine("Delete action pressed: " + mi.CommandParameter);
            //     await ((ContentPage)(mi.Parent.Parent.Parent.Parent.Parent)).DisplayAlert("Long press", "Delete action pressed: " + mi.CommandParameter, "ok");
            // };

            View = relLayout;
        }
    }
}
