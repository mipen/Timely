using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Timely
{
    public class ActivityCell : ViewCell
    {
        public ActivityCell()
        {
            RelativeLayout rl = new RelativeLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            Label labelName = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 18,
                FontFamily = "Roboto",
                TextColor = Constants.ActivityCellTextColor
            };
            Label labelDate = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 14,
                FontFamily = "Roboto",
                TextColor = Constants.ActivityCellTextColor
            };
            Label labelTime = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 14,
                FontFamily = "Roboto",
                TextColor = Constants.ActivityCellTextColor
            };
            Label labelTimeElapsed = new Label()
            {
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 18,
                FontFamily = "Roboto",
                TextColor = Constants.TotalTimeElapsedColor
            };
            Image imgSep = new Image()
            {
                Source = "mainpageseparator.png",
                Aspect = Aspect.Fill
            };

            //Set bindings
            labelName.SetBinding(Label.TextProperty, "ActivityName");
            labelDate.SetBinding(Label.TextProperty, "LastStartDateDisplay");
            labelTime.SetBinding(Label.TextProperty, "StartTimeDisplay");
            labelTimeElapsed.SetBinding(Label.TextProperty, "TimeElapsedDisplay");

            //Add the views
            //Method goes -> X, Y, Width, Height
            int indent = 10;
            rl.AddView(labelName, Constraint.RelativeToParent(p =>
            {
                return p.X + indent;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Y;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width - 50;
            }),
            Constraint.Constant(24)
            );
            rl.AddView(labelDate, Constraint.RelativeToParent(p =>
            {
                return p.X + indent * 2;
            }),
            Constraint.RelativeToView(labelName, (p, s) =>
            {
                return s.Y + s.Height + 2;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width - 50;
            }),
            Constraint.Constant(20)
            );
            rl.AddView(labelTime, Constraint.RelativeToParent(p =>
            {
                return p.X + indent * 2;
            }),
            Constraint.RelativeToView(labelDate, (p, s) =>
            {
                return s.Y + s.Height + 2;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width - 50;
            }),
            Constraint.Constant(20)
            );
            rl.AddView(labelTimeElapsed, Constraint.RelativeToParent(p =>
            {
                return p.Width / 2;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Y;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width / 2 - indent;
            }),
            Constraint.Constant(24)
            );
            rl.AddView(imgSep, Constraint.RelativeToParent(p =>
            {
                return p.X + indent;
            }),
            Constraint.RelativeToView(labelTime, (p, s) =>
            {
                return s.Y + s.Height + 4;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width - indent * 2;
            }),
            Constraint.Constant(1)
            );

            View = rl;
        }
    }
}
