using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Timely.ViewModels
{
    public class NewActivityViewModel : INotifyPropertyChanged
    {
        private Activity act;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool acceptButtonEnabled = true;

        public INavigation Navigation { get; set; }
        public string ActivityName
        {
            get
            {
                return act.ActivityName;
            }
            set
            {
                if (act.ActivityName != value)
                {
                    act.ActivityName = value;
                    OnPropertyChanged("ActivityName");
                }
            }
        }
        public string Category
        {
            get
            {
                return act.Category;
            }
            set
            {
                if (act.Category != value)
                {
                    act.Category = value;
                    OnPropertyChanged("Category");
                }
            }
        }
        public Activity Activity
        {
            get
            {
                return act;
            }
        }
        public bool HasValidInput
        {
            get
            {
                return !String.IsNullOrEmpty(act.ActivityName);
            }
        }
        public ICommand BackButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PopModalAsync();
                });
            }
        }
        public ICommand AcceptButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (HasValidInput)
                    {
                        App.ActivityDatabase.InsertAsync(act);
                        Navigation.PopModalAsync();
                    }
                    AcceptButtonEnabled = true;
                });
            }
        }
        public ImageSource AcceptButtonImage
        {
            get
            {
                if (HasValidInput)
                    return ImageSource.FromFile("acceptbtn.png");
                else
                    return ImageSource.FromFile("disabledacceptbtn.png");
            }
        }
        public bool NameWarningVisible
        {
            get
            {
                return !HasValidInput;
            }
        }
        public bool AcceptButtonEnabled
        {
            get
            {
                return acceptButtonEnabled;
            }
            set
            {
                acceptButtonEnabled = value;
                OnPropertyChanged("AcceptButtonEnabled");
            }
        }

        public NewActivityViewModel(INavigation navigation)
        {
            Navigation = navigation;
            act = new Activity();
            act.PropertyChanged += Act_PropertyChanged;
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "HasValidInput")
                {
                    OnPropertyChanged("AcceptButtonImage");
                    OnPropertyChanged("NameWarningVisible");
                }
            };
        }

        private void Act_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propName = e.PropertyName;
            if (propName == "ActivityName")
            {
                OnPropertyChanged("HasValidInput");
                OnPropertyChanged("NameEntryImage");
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
