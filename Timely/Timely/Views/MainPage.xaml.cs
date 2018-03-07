using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timely.ViewModels;
using Xamarin.Forms;

namespace Timely
{
    public partial class MainPage : ContentPage
    {
        private Image imgBg;
        private Image imgSearchBanner;
        private ImageButton imgActivityBtn;
        private ImageButton imgAddActivityBtn;
        private RelativeLayout rlActivityInProgress;
        private Label labelActivityName;
        private Label labelActivityCategory;
        private Label labelTimeStarted;
        private Label labelTimeElapsed;
        private AppListView listViewActivites;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(Navigation);
            InitialiseElements();
        }

        private void InitialiseElements()
        {
            #region ViewInit
            imgBg = new Image()
            {
                Source = "mainpagebg.png",
                Aspect = Aspect.Fill
            };
            imgSearchBanner = new Image()
            {
                Source = "mainpagesearchbg.png",
                Aspect = Aspect.Fill
            };
            rlActivityInProgress = new RelativeLayout();
            imgActivityBtn = new ImageButton()
            {
                Source = "pausebtnsmall.png",
                Aspect = Aspect.Fill
            };
            //TODO:: This
            labelActivityName = new Label()
            {
                Text = "Activity",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 28,
                FontFamily = "Roboto"
            };
            labelActivityCategory = new Label()
            {
                Text = "Category",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 20,
                FontFamily = "Roboto"
            };
            labelTimeStarted = new Label()
            {
                Text = "Started",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                FontFamily = "Roboto"
            };
            labelTimeElapsed = new Label()
            {
                Text = "Time Elapsed",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 28,
                FontFamily = "Roboto",
                TextColor = Constants.TotalElapsedTimeColor
            };
            listViewActivites = new AppListView()
            {
                HasUnevenRows = true,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(ActivityCell))
            };
            listViewActivites.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };
            listViewActivites.SetBinding(ListView.SelectedItemProperty, "SelectedActivity", BindingMode.OneWayToSource);
            listViewActivites.SetBinding(AppListView.TapCommandProperty, "ListViewItemTapped");
            listViewActivites.SetBinding(ListView.ItemsSourceProperty, "Activities");
            listViewActivites.SetBinding(ListView.IsRefreshingProperty, "LoadingData");

            imgAddActivityBtn = new ImageButton()
            {
                Source = "addactivitybtn.png",
                Aspect = Aspect.Fill
            };
            imgAddActivityBtn.SetBinding(ImageButton.TappedCommandProperty, "AddActivityTapCommand");
            imgAddActivityBtn.SetBinding(Image.IsEnabledProperty, "AddActivityButtonEnabled", BindingMode.TwoWay);
            #endregion
            //Method goes -> X, Y, Width, Height
            #region BGImage
            rlPageContainer.Children.Add(imgBg, Constraint.RelativeToParent(p =>
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
            Constraint.Constant(290)
            );
            #endregion
            #region SearchBanner
            rlPageContainer.Children.Add(imgSearchBanner, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToView(imgBg, (p, s) =>
            {
                return s.Height;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.Constant(30)
            );
            #endregion
            #region ActivityInPropgressLayout
            rlPageContainer.Children.Add(rlActivityInProgress, Constraint.RelativeToParent(p =>
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
            Constraint.RelativeToView(imgBg, (p, s) =>
            {
                return s.Height;
            })
            );
            #endregion
            #region ActivityNameLabel
            rlActivityInProgress.AddView(labelActivityName, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Y + 20;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.Constant(36)
            );
            #endregion
            #region CategoryLabel
            rlActivityInProgress.AddView(labelActivityCategory, Constraint.RelativeToParent(p =>
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
            Constraint.Constant(28)
            );
            #endregion
            #region StartedTimeLabel
            rlActivityInProgress.AddView(labelTimeStarted, Constraint.RelativeToParent(p =>
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
            Constraint.Constant(22)
            );
            #endregion
            #region ElapsedTimeLabel
            rlActivityInProgress.AddView(labelTimeElapsed, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToView(labelTimeStarted, (p, s) =>
            {
                return s.Y + s.Height + 12;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.Constant(36)
            );
            #endregion
            #region ActivityPauseButton
            double size = 88;
            rlActivityInProgress.AddView(imgActivityBtn, Constraint.RelativeToParent(p =>
            {
                return p.Width / 2 - size / 2;
            }),
            Constraint.RelativeToView(labelTimeElapsed, (p, s) =>
             {
                 return (s.Y + s.Height);
             }),
            Constraint.Constant(size),
            Constraint.Constant(size)
            );
            #endregion
            #region ActivityListView
            rlPageContainer.AddView(listViewActivites, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToView(imgSearchBanner, (p, s) =>
            {
                return s.Y + s.Height;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.RelativeToView(imgSearchBanner, (p, s) =>
            {
                return p.Height - (s.Y + s.Height);
            })
            );
            #endregion
            #region AddActivityButton
            int btnSize = 64;
            rlPageContainer.AddView(imgAddActivityBtn, Constraint.RelativeToParent(p =>
            {
                return p.Width - btnSize - 20;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Height - btnSize - 20;
            }),
            Constraint.Constant(btnSize),
            Constraint.Constant(btnSize)
            );
            #endregion
        }

        //private async Task AddActivityBtn_Tapped()
        //{
        //    await imgAddActivityBtn.ScaleTo(0.9, 50, Easing.Linear);
        //    await Task.Delay(100);
        //    await imgAddActivityBtn.ScaleTo(1, 50, Easing.BounceOut);
        //    await Navigation.PushModalAsync(new NewActivityPage());
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
