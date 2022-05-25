using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    public class InfoCard : INotifyPropertyChanged
    {
        public InfoCard(string info, string pictureUrl)
        {
            PictureUrl = pictureUrl;
            Info = info;
        }
        private string url;

        public string PictureUrl
        {
            get { return url; }
            set { url = value; NotifyPropertyChanged("PictureUrl"); }
        }
        private string info;

        public string Info
        {
            get { return info; }
            set { info = value; NotifyPropertyChanged("Info"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}