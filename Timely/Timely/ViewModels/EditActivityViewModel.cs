using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Timely.ViewModels
{
    public class EditActivityViewModel : INotifyPropertyChanged
    {
        private string activityName;
        private string category;
        public event PropertyChangedEventHandler PropertyChanged;
        private event PropertyChangedEventHandler ForeignPropertyChanged;
        private bool acceptButtonEnabled = true;
        private bool clearHistoryButtonEnabled = true;
        private bool deleteButtonEnabled = true;

        public INavigation Navigation { get; set; }
        public Activity Act { get; set; }
        public ICommand ClearHistoryCommand
        {
            get
            {
                return new Command(() =>
                {
                    //TODO:: Implement this
                    OnPropertyChanged("ClearHistoryButtonEnabled");
                });
            }
        }
        public ICommand AcceptButtonCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (HasValidInput)
                    {
                        Act.ActivityName = ActivityName;
                        Act.Category = Category;
                        await Navigation.PopModalAsync();
                        ForeignPropertyChanged(this, new PropertyChangedEventArgs("ActivityName"));
                        ForeignPropertyChanged(this, new PropertyChangedEventArgs("Category"));
                        if (HistoryCleared)
                            ForeignPropertyChanged(this, new PropertyChangedEventArgs("History"));
                        await App.ActivityDatabase.InsertAsync(Act);
                    }
                    OnPropertyChanged("AcceptButtonEnabled");
                });
            }
        }
        public ICommand DeleteButtonCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //TODO:: Show confirmation box before deleting
                    //TODO:: Implement deleting items better
                    await App.ActivityDatabase.DeleteAsync(Act);
                    Navigation.PopModalAsync();
                    await Navigation.PopModalAsync();
                    OnPropertyChanged("DeleteButtonEnabled");
                });
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
        public string ActivityName
        {
            get
            {
                return activityName;
            }
            set
            {
                if (activityName != value)
                {
                    activityName = value;
                    OnPropertyChanged("ActivityName");
                    OnPropertyChanged("HasValidInput");
                }
            }
        }
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                if (category != value)
                {
                    category = value;
                    OnPropertyChanged("Category");
                }
            }
        }
        public bool HasValidInput
        {
            get
            {
                return !string.IsNullOrEmpty(ActivityName);
            }
        }
        private bool HistoryCleared { get; set; } = false;
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
        private bool AcceptButtonEnabled
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
        private bool ClearHistoryButtonEnabled
        {
            get
            {
                return clearHistoryButtonEnabled;
            }
            set
            {
                clearHistoryButtonEnabled = value;
                OnPropertyChanged("ClearHistoryButtonEnabled");
            }
        }
        private bool DeleteButtonEnabled
        {
            get
            {
                return deleteButtonEnabled;
            }
            set
            {
                deleteButtonEnabled = value;
                OnPropertyChanged("DeleteButtonEnabled");
            }
        }

        public EditActivityViewModel(INavigation navigation, Activity act, PropertyChangedEventHandler propChanged)
        {
            Navigation = navigation;
            Act = act;
            ActivityName = Act.ActivityName;
            Category = Act.Category;
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "HasValidInput")
                    OnPropertyChanged("AcceptButtonImage");
            };
            ForeignPropertyChanged = propChanged;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
