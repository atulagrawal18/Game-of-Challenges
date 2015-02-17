using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Game_of_Typography
{
    public class FilePaths:INotifyPropertyChanged
    {
         private string audioPath;
        private string videoPath;
        private string srtPath;
        private string bmpPath;
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string AudioPath { get { return audioPath; } set { audioPath = value; NotifyPropertyChanged("AudioPath"); } }
        public string VideoPath { get { return videoPath; } set { videoPath = value; NotifyPropertyChanged("VideoPath"); } }
        public string SrtPath { get { return srtPath; } set { srtPath = value; NotifyPropertyChanged("SrtPath"); } }
        public string BmpPath { get { return bmpPath; } set { bmpPath = value; NotifyPropertyChanged("BmpPath"); } }
    }
}
