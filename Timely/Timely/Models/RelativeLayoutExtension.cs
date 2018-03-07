using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Timely
{
    public static class RelativeLayoutExtension
    {
        public static void AddView(this RelativeLayout rl, View view, Constraint xConstraint, Constraint yConstraint, Constraint widthConstraint, Constraint heightConstraint)
        {
            rl.Children.Add(view, xConstraint, yConstraint, widthConstraint, heightConstraint);
        }
    }
}
