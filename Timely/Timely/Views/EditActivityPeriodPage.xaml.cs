using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timely.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timely
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditActivityPeriodPage : ContentPage
    {
        public EditActivityPeriodPage(ActivityPeriod ap)
        {
            BindingContext = new EditActivityPeriodViewModel(Navigation, ap);
            InitializeComponent();
            InitialiseElements();
        }

        private void InitialiseElements()
        {
            Label labelTitle = new Label()
            {
                Text = "Edit",
                FontSize = 24,
                FontFamily = "Roboto",
                TextColor = Constants.TextColor,
                HorizontalTextAlignment = TextAlignment.Center
            };

            Label labelStarted = new Label()
            {
                Text = "Started",
                FontSize = 24,
                FontFamily = "Roboto",
                TextColor = Constants.TextColor,
                HorizontalTextAlignment = TextAlignment.Start
            };

            Label labelEnded = new Label()
            {
                Text = "Ended",
                FontSize = 24,
                FontFamily = "Roboto",
                TextColor = Constants.TextColor,
                HorizontalTextAlignment = TextAlignment.Start
            };
            labelEnded.SetBinding(Label.IsVisibleProperty, "EndPickersVisible");

            ImageButton btnBackArrow = new ImageButton()
            {
                Source = "backarrow.png",
                Aspect = Aspect.Fill
            };
            btnBackArrow.SetBinding(ImageButton.TappedCommandProperty, "BackButtonCommand");

            ImageButton btnAccept = new ImageButton()
            {
                Aspect = Aspect.Fill
            };
            btnAccept.SetBinding(Image.SourceProperty, "AcceptButtonImage");
            btnAccept.SetBinding(ImageButton.TappedCommandProperty, "AcceptButtonCommand");
            btnAccept.SetBinding(ImageButton.IsEnabledProperty, "AcceptButtonEnabled", BindingMode.TwoWay);

            ImageButton btnDelete = new ImageButton()
            {
                Source = "deletebtn.png",
                Aspect = Aspect.Fill
            };
            btnDelete.SetBinding(ImageButton.TappedCommandProperty, "DeleteButtonCommand");
            btnDelete.SetBinding(ImageButton.IsEnabledProperty, "DeleteButtonEnabled", BindingMode.TwoWay);

            DatePicker dpStartDate = new DatePicker()
            {
                Format = Constants.DateFormat,
                TextColor = Constants.TextColor
            };
            dpStartDate.SetBinding(DatePicker.DateProperty, "StartDate");

            TimePicker tpStartTime = new TimePicker()
            {
                Format = Constants.TimeSecondsFormat,
                TextColor = Constants.TextColor
            };
            tpStartTime.SetBinding(TimePicker.TimeProperty, "StartTime");

            DatePicker dpEndDate = new DatePicker()
            {
                Format = Constants.DateFormat,
                TextColor = Constants.TextColor
            };
            dpEndDate.SetBinding(DatePicker.DateProperty, "EndDate");
            dpEndDate.SetBinding(DatePicker.IsVisibleProperty, "EndPickersVisible");

            TimePicker tpEndTime = new TimePicker()
            {
                Format = Constants.TimeSecondsFormat,
                TextColor = Constants.TextColor
            };
            tpEndTime.SetBinding(TimePicker.TimeProperty, "EndTime");
            tpEndTime.SetBinding(TimePicker.IsVisibleProperty, "EndPickersVisible");

            rl.AddView(btnBackArrow, Constraint.RelativeToParent((p) =>
            {
                return p.X + 10;
            }),
            Constraint.RelativeToParent((p) =>
            {
                return p.Y + 10;
            }),
            Constraint.Constant(32),
            Constraint.Constant(32)
            );

            rl.AddView(btnDelete, Constraint.RelativeToParent((p) =>
            {
                return p.Width - 10 - 40;
            }),
            Constraint.RelativeToParent((p) =>
            {
                return p.Y + 10;
            }),
            Constraint.Constant(40),
            Constraint.Constant(40)
            );

            rl.AddView(labelTitle, Constraint.RelativeToParent((p) =>
            {
                return p.X;
            }),
            Constraint.RelativeToParent((p) =>
            {
                return p.Y + 55;
            }),
            Constraint.RelativeToParent((p) =>
            {
                return p.Width;
            }),
            Constraint.Constant(32)
            );

            int indent = 42;
            rl.AddView(labelStarted, Constraint.RelativeToParent((p) =>
            {
                return p.X + indent;
            }),
            Constraint.RelativeToView(labelTitle, (p, s) =>
            {
                return s.Y + s.Height + 120;
            }),
            Constraint.Constant(150),
            Constraint.Constant(32)
            );

            int buffer = 10;
            int margin = 10;
            int height = 40;
            rl.AddView(dpStartDate, Constraint.RelativeToView(labelStarted, (p, s) =>
            {
                return s.X;
            }),
            Constraint.RelativeToView(labelStarted, (p, s) =>
            {
                return s.Y + s.Height + 10;
            }),
            Constraint.RelativeToView(labelStarted, (p, s) =>
            {
                return ((p.Width - indent - margin) / 2) - buffer / 2;
            }),
            Constraint.Constant(height)
            );

            rl.AddView(tpStartTime, Constraint.RelativeToView(dpStartDate, (p, s) =>
            {
                return s.X + s.Width + (buffer / 2);
            }),
            Constraint.RelativeToView(dpStartDate, (p, s) =>
            {
                return s.Y;
            }),
            Constraint.RelativeToView(dpStartDate, (p, s) =>
            {
                return s.Width;
            }),
            Constraint.Constant(height)
            );

            rl.AddView(labelEnded, Constraint.RelativeToParent((p) =>
            {
                return p.X + indent;
            }),
            Constraint.RelativeToView(labelStarted, (p, s) =>
            {
                return s.Y + s.Height + 75;
            }),
            Constraint.Constant(150),
            Constraint.Constant(32)
            );

            rl.AddView(dpEndDate, Constraint.RelativeToView(labelEnded, (p, s) =>
            {
                return s.X;
            }),
            Constraint.RelativeToView(labelEnded, (p, s) =>
            {
                return s.Y + s.Height + 10;
            }),
            Constraint.RelativeToView(labelEnded, (p, s) =>
            {
                return ((p.Width - indent - margin) / 2) - buffer / 2;
            }),
            Constraint.Constant(height)
            );

            rl.AddView(tpEndTime, Constraint.RelativeToView(dpEndDate, (p, s) =>
            {
                return s.X + s.Width + (buffer / 2);
            }),
            Constraint.RelativeToView(dpEndDate, (p, s) =>
            {
                return s.Y;
            }),
            Constraint.RelativeToView(dpEndDate, (p, s) =>
            {
                return s.Width;
            }),
            Constraint.Constant(height)
            );

            int acceptBtnWidth = 103;
            int acceptBtnHeight = 41;
            rl.AddView(btnAccept, Constraint.RelativeToParent((p) =>
            {
                return p.Width / 2 - acceptBtnWidth / 2;
            }),
            Constraint.RelativeToParent((p) =>
            {
                return p.Height - acceptBtnHeight - 70;
            }),
            Constraint.Constant(acceptBtnWidth),
            Constraint.Constant(acceptBtnHeight)
            );
        }
    }
}