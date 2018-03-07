using System.IO;
using Timely.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace Timely.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string fileName)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, fileName);
        }
    }
}