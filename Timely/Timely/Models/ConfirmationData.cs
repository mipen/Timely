using System;
using System.Collections.Generic;
using System.Text;

namespace Timely
{
    public class ConfirmationData
    {
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public string Accept { get; set; } = "Yes";
        public string Cancel { get; set; } = "No";
    }
}
