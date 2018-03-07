using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Timely
{
    public static class Constants
    {
        #region Colours
        public static readonly Color TotalTimeElapsedColor = Color.FromHex("#05E249");
        public static readonly Color InProgressTimeElapsedColor = Color.FromHex("#F57F56");
        public static readonly Color TextColor = Color.FromHex("#DBDBDB");
        public static readonly Color HistoryTextColor = Color.FromHex("#9D9D9D");
        public static readonly Color ActivityCellTextColor = Color.FromHex("#404040");
        #endregion
        #region FormatStrings
        public static readonly string DateFormat = @"dd\/ MM\/ yy";
        public static readonly string TimeFormat = "hh:mm tt";
        public static readonly string TimeSecondsFormat = "hh:mm:ss tt";
        #endregion
    }
}
