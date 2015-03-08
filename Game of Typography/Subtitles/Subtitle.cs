using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Subtitles
{
    public class Subtitle : INotifyPropertyChanged
    {
        public int Serial { get; set; }

        private string _textEffect;
        public string TextEffect
        {
            get { return _textEffect; }
            set { _textEffect = value; NotifyPropertyChanged("TextEffect"); }
        }

        private float _fontSize;
        public float FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; NotifyPropertyChanged("FontSize"); }
        }

        private string _fontFamily;
        public string FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; NotifyPropertyChanged("FontFamily"); }
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string> LyricsList { get; set; }
        public string Lyrics 
        { 
            get 
            { 
                return LyricsList.Aggregate((i, j) => i + " " + j);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
