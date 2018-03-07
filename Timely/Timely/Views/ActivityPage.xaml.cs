using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Timely.ViewModels;

namespace Timely
{
    public partial class ActivityPage : ContentPage
    {
        private Image imgBg;
        private ImageButton imgActivityBtn;
        private Label labelActivityName;
        private Label labelActivityCategory;
        private Label labelStartedTime;
        private Label labelElapsedTime;
        private Label labelHistory;
        private ListView listViewHistory;

        public ActivityPage(Activity act)
        {
            BindingContext = new ActivityViewModel(act, Navigation);
            InitializeComponent();
            InitialiseViews();
        }

        public void imgActivityBtn_Tapped(object sender, EventArgs e)
        {
            //    if (act.CurrentActivityPeriod != null)
            //    {
            //        //Change the button image to start
            //        imageActivityTimerButton.Source = startImage;
            //        ActivityPeriod actP = act.CurrentActivityPeriod;
            //        //End the current activity period
            //        actP.EndTime = DateTime.Now;
            //        //Update the elapsed time label
            //        labelActivityElapsedTime.Text = act.ElapsedTimeDisplay;
            //        //Update the database with activity
            //        App.ActivityDatabase.SaveItem(act);
            //        //Update database with activity period
            //        App.ActivityPeriodDatabase.SaveItem(actP);
            //        //Update list view
            //        activityPeriods.Insert(0, actP);
            //    }
            //    else
            //    {
            //        ActivityPeriod actP = new ActivityPeriod(DateTime.Now);
            //        //Add a new activity period
            //        act.ActivityPeriods.Add(actP);
            //        //change the button image to endImage
            //        imageActivityTimerButton.Source = endImage;
            //        //Update the database with activity
            //        App.ActivityDatabase.SaveItem(act);
            //        //Update the database with activity period
            //        App.ActivityPeriodDatabase.SaveItem(actP);
            //    }
        }

        private void InitialiseViews()
        {
            #region ViewInitialisation
            ImageButton imgBackBtn = new ImageButton()
            {
                Source = "backarrow.png",
                Aspect = Aspect.Fill
            };
            imgBackBtn.SetBinding(ImageButton.TappedCommandProperty, "BackButtonCommand");
            imgBackBtn.SetBinding(ImageButton.IsEnabledProperty, "BackButtonEnabled", BindingMode.TwoWay);

            ImageButton imgEditActivityBtn = new ImageButton()
            {
                Source = "editbtn.png",
                Aspect = Aspect.AspectFill
            };
            imgEditActivityBtn.SetBinding(ImageButton.TappedCommandProperty, "EditActivityButtonCommand");
            imgEditActivityBtn.SetBinding(ImageButton.IsEnabledProperty, "EditActivityButtonEnabled", BindingMode.TwoWay);

            imgBg = new Image()
            {
                Source = "activitypagebg.png",
                VerticalOptions = LayoutOptions.Start,
                Aspect = Aspect.Fill
            };

            imgActivityBtn = new ImageButton()
            {
                Aspect = Aspect.Fill
            };
            imgActivityBtn.SetBinding(Image.SourceProperty, "ActivityBtnImageSource");
            imgActivityBtn.SetBinding(ImageButton.TappedCommandProperty, "ActivityButtonCommand");
            imgActivityBtn.SetBinding(ImageButton.IsEnabledProperty, "ActivityButtonEnabled", BindingMode.TwoWay);

            labelActivityName = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 28,
                FontFamily = "Roboto"
            };
            labelActivityName.SetBinding(Label.TextProperty, "ActivityName", BindingMode.OneWay);

            labelActivityCategory = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 20,
                FontFamily = "Roboto"
            };
            labelActivityCategory.SetBinding(Label.TextProperty, "Category", BindingMode.OneWay);

            labelStartedTime = new Label()
            {
                Text = "Started time",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                FontFamily = "Roboto",
            };
            labelStartedTime.SetBinding(Label.IsVisibleProperty, "StartTimeVisible");
            labelStartedTime.SetBinding(Label.TextProperty, "StartTimeDisplay");

            labelElapsedTime = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 28,
                FontFamily = "Roboto",
                TextColor = Constants.TotalElapsedTimeColor
            };
            labelElapsedTime.SetBinding(Label.TextProperty, "ElapsedTime", BindingMode.OneWay);
            labelElapsedTime.SetBinding(Label.TextColorProperty, "ElapsedTimeColor");

            labelHistory = new Label()
            {
                Text = "History",
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                FontFamily = "Roboto",
                TextColor = Constants.HistoryTextColor
            };

            listViewHistory = new ListView()
            {
                HasUnevenRows = true,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(ActivityPeriodCell))
            };
            listViewHistory.SetBinding(ListView.ItemsSourceProperty, "History");
            listViewHistory.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };
            #endregion
            //Methods goes -> X, Y, Width, Height
            //Add bg image
            #region BGImg
            relLayout.Children.Add(imgBg, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Y;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Height / 2;
            }));
            #endregion
            //Add the history label
            #region HistoryLabel
            relLayout.Children.Add(labelHistory, Constraint.RelativeToParent((p) =>
            {
                //X coord
                return p.X + 12;
            }),
            Constraint.RelativeToView(imgBg, (p, s) =>
            {
                //Y coord
                return s.Height + 4;
            }),
            Constraint.Constant(60),
            Constraint.Constant(21));
            #endregion
            //Add the labels
            #region ActivityLabel
            relLayout.Children.Add(labelActivityName, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Y + 24;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.Constant(36));
            #endregion
            #region ActivityCategoryLabel
            relLayout.Children.Add(labelActivityCategory, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToView(labelActivityName, (p, s) =>
            {
                return s.Y + s.Height + 24;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.Constant(26));
            #endregion
            #region StartedTimeLabel
            relLayout.Children.Add(labelStartedTime, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToView(labelActivityCategory, (p, s) =>
            {
                return s.Y + s.Height + 24;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.Constant(21));
            #endregion
            #region ElapsedTimeLabel
            relLayout.Children.Add(labelElapsedTime, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToView(labelStartedTime, (p, s) =>
            {
                return s.Y + s.Height + 12;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.Constant(36));
            #endregion
            #region HistoryListView
            relLayout.Children.Add(listViewHistory, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToView(imgBg, (p, s) =>
            {
                return s.Height + 30;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.RelativeToView(imgBg, (p, s) =>
            {
                return p.Height - s.Height - 30;
            }));
            #endregion
            //Add activity button
            #region ActivityButton
            int size = 128;
            relLayout.Children.Add(imgActivityBtn, Constraint.RelativeToView(imgBg, (p, s) =>
            {
                //X coord
                return p.Width / 2 - size / 2;
            }),
            Constraint.RelativeToView(imgBg, (p, s) =>
            {
                //Y coord
                return s.Height - 80;
            }),
            Constraint.Constant(size),
            Constraint.Constant(size));
            #endregion
            //Add the back button
            #region BackButton
            relLayout.Children.Add(imgBackBtn, Constraint.RelativeToView(imgBg, (p, s) =>
            {
                //X coord
                return p.X + 10;
            }),
            Constraint.RelativeToParent(p =>
            {
                //Y coord
                return p.Y + 10;
            }),
            Constraint.Constant(32),
            Constraint.Constant(32));
            #endregion
            //Add the edit button
            #region EditButton
            int editBtnSize = 64;
            relLayout.Children.Add(imgEditActivityBtn, Constraint.RelativeToParent(p =>
            {
                //X coord
                return p.Width - editBtnSize - 10;
            }),
            Constraint.RelativeToParent(p =>
            {
                //Y coord
                return p.Y + 5;
            }),
            Constraint.Constant(editBtnSize),
            Constraint.Constant(editBtnSize));
            #endregion
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}