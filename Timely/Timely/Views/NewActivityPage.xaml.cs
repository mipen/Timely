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
    public partial class NewActivityPage : ContentPage
    {
        private Entry entryName;

        public NewActivityPage()
        {
            BindingContext = new NewActivityViewModel(Navigation);
            InitializeComponent();
            InitialiseElements();
        }

        private void InitialiseElements()
        {
            ImageButton imgBackBtn = new ImageButton()
            {
                Source = "backarrow.png",
                Aspect = Aspect.Fill
            };
            imgBackBtn.SetBinding(ImageButton.TappedCommandProperty, "BackButtonCommand");
            Image imgNameBg = new Image()
            {
                Source = "entrybg.png",
                Aspect = Aspect.Fill
            };

            Image imgNameWarning = new Image()
            {
                Source = "entrynotifybg.png",
                Aspect = Aspect.Fill
            };
            imgNameWarning.SetBinding(Image.IsVisibleProperty, "NameWarningVisible");

            Image imgCategoryBg = new Image()
            {
                Source = "entrybg.png",
                Aspect = Aspect.Fill
            };
            ImageButton imgAcceptBtn = new ImageButton()
            {
                Aspect = Aspect.Fill
            };
            imgAcceptBtn.SetBinding(ImageButton.TappedCommandProperty, "AcceptButtonCommand");
            imgAcceptBtn.SetBinding(Image.IsEnabledProperty, "AcceptButtonEnabled", BindingMode.TwoWay);
            imgAcceptBtn.SetBinding(Image.SourceProperty, "AcceptButtonImage");
            Label labelTitle = new Label()
            {
                Text = "Create new activity",
                FontSize = 24,
                FontFamily = "Roboto",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Constants.TextColor
            };
            entryName = new Entry()
            {
                FontFamily = "Roboto",
                HorizontalTextAlignment = TextAlignment.Center,
                Placeholder = "Name"
            };
            entryName.SetBinding(Entry.TextProperty, "ActivityName", BindingMode.TwoWay);

            Entry entryCategory = new Entry()
            {
                FontFamily = "Roboto",
                HorizontalTextAlignment = TextAlignment.Center,
                Placeholder = "Category"
            };
            entryCategory.SetBinding(Entry.TextProperty, "Category", BindingMode.TwoWay);
            //X, Y, Width, Height
            rl.AddView(imgBackBtn, Constraint.RelativeToParent(p =>
            {
                return p.X + 10;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Y + 10;
            }),
            Constraint.Constant(32),
            Constraint.Constant(32)
            );

            rl.AddView(labelTitle, Constraint.RelativeToParent(p =>
            {
                return p.X;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Y + 60;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width;
            }),
            Constraint.Constant(32)
            );

            int entryWidthMargin = 40;
            rl.AddView(imgNameBg, Constraint.RelativeToParent(p =>
            {
                return p.X + 40;
            }),
            Constraint.RelativeToView(labelTitle, (p, s) =>
            {
                return s.Y + s.Height + 124;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width - 80;
            }),
            Constraint.Constant(49)
            );
            rl.AddView(imgNameWarning, Constraint.RelativeToView(imgNameBg, (p, s) =>
            {
                return s.X;
            }),
            Constraint.RelativeToView(imgNameBg, (p, s) =>
            {
                return s.Y;
            }),
            Constraint.RelativeToView(imgNameBg, (p, s) =>
            {
                return s.Width;
            }),
            Constraint.RelativeToView(imgNameBg, (p, s) =>
            {
                return s.Height;
            })
            );
            rl.AddView(entryName, Constraint.RelativeToView(imgNameBg, (p, s) =>
            {
                return s.X + entryWidthMargin / 2;
            }),
            Constraint.RelativeToView(imgNameBg, (p, s) =>
            {
                return s.Y;
            }),
            Constraint.RelativeToView(imgNameBg, (p, s) =>
            {
                return s.Width - entryWidthMargin;
            }),
            Constraint.RelativeToView(imgNameBg, (p, s) =>
            {
                return s.Height;
            })
            );

            rl.AddView(imgCategoryBg, Constraint.RelativeToParent(p =>
            {
                return p.X + 40;
            }),
            Constraint.RelativeToView(imgNameBg, (p, s) =>
            {
                return s.Y + s.Height + 43;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Width - 80;
            }),
            Constraint.Constant(49)
            );
            rl.AddView(entryCategory, Constraint.RelativeToView(imgCategoryBg, (p, s) =>
            {
                return s.X + entryWidthMargin / 2;
            }),
            Constraint.RelativeToView(imgCategoryBg, (p, s) =>
            {
                return s.Y;
            }),
            Constraint.RelativeToView(imgCategoryBg, (p, s) =>
            {
                return s.Width - entryWidthMargin;
            }),
            Constraint.RelativeToView(imgCategoryBg, (p, s) =>
            {
                return s.Height;
            })
            );

            int btnWidth = 103;
            rl.AddView(imgAcceptBtn, Constraint.RelativeToParent(p =>
            {
                return p.Width / 2 - btnWidth / 2;
            }),
            Constraint.RelativeToParent(p =>
            {
                return p.Height - 100;
            }),
            Constraint.Constant(btnWidth),
            Constraint.Constant(41)
            );
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            entryName.Focus();
        }
    }
}