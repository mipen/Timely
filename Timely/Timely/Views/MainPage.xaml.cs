using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Timely.ViewModels;
using Xamarin.Forms;

namespace Timely
{
    public partial class MainPage : ContentPage
    {
        private bool startup = true;
        #region InitialiseCommandProperty
        public static readonly BindableProperty InitialiseCommandProperty = BindableProperty.Create(
            nameof(InitialiseCommand),
            typeof(ICommand),
            typeof(MainPage),
            null,
            BindingMode.OneWay,
            null,
            null,
            null,
            null,
            null);
        #endregion

        public ICommand InitialiseCommand
        {
            get
            {
                return (ICommand)GetValue(InitialiseCommandProperty);
            }
            set
            {
                SetValue(InitialiseCommandProperty, value);
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(Navigation);
            this.SetBinding(InitialiseCommandProperty, "InitialiseCommand");
            InitialiseElements();
        }

        private void InitialiseElements()
        {
            #region ViewInit
            Image imgBg = new Image()
            {
                Source = "mainpagebg.png",
                Aspect = Aspect.Fill
            };
            Image imgSearchBanner = new Image()
            {
                Source = "mainpagesearchbg.png",
                Aspect = Aspect.Fill
            };
            RelativeLayout rlActivityInProgress = new RelativeLayout();
            ImageButton imgActivityBtn = new ImageButton()
            {
                Source = "pausebtnsmall.png",
                Aspect = Aspect.Fill
            };
            //TODO:: This
            Label labelActivityName = new Label()
            {
                Text = "Activity",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 28,
                FontFamily = "Roboto"
            };
            Label labelActivityCategory = new Label()
            {
                Text = "Category",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 20,
                FontFamily = "Roboto"
            };
            Label labelTimeStarted = new Label()
            {
                Text = "Started",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                FontFamily = "Roboto"
            };
            Label labelTimeElapsed = new Label()
            {
                Text = "Time Elapsed",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 28,
                FontFamily = "Roboto",
                TextColor = Constants.TotalTimeElapsedColor
            };
            AppListView listViewActivites = new AppListView()
            {
                HasUnevenRows = true,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(ActivityCell))
            };
            listViewActivites.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };
            listViewActivites.SetBinding(AppListView.TapCommandProperty, "ListViewItemTapped");
            listViewActivites.SetBinding(ListView.ItemsSourceProperty, "Activities");
            listViewActivites.SetBinding(ListView.IsRefreshingProperty, "LoadingData");

            ImageButton imgAddActivityBtn = new ImageButton()
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (startup)
            {
            InitialiseCommand?.Execute(this);
                startup = false;
            }
        }
    }
}
