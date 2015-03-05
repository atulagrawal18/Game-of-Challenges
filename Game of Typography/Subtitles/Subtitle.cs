using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subtitles
{
    public class Subtitle
    {
        public int Serial { get; set; }
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
    }
}
