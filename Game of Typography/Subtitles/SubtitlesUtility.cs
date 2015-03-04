using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace Subtitles
{
    public static class SubtitlesUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subtitles"></param>
        /// <param name="filePath"></param>
        public static void ParseSubtitle(out List<Subtitle> subtitles, string filePath)
        {
            subtitles = new List<Subtitle>();

            string[] lines = System.IO.File.ReadAllLines(filePath);

            lines = lines.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
            lines = lines.Where(x => !string.IsNullOrWhiteSpace(x.Trim())).ToArray();

            for (int i = 0, counter = 0; i < lines.Count(); )
            {
                lines[i].Trim();
                if (lines[i] == (counter + 1).ToString())
                {
                    Subtitle subtitle = new Subtitle();
                    subtitle.Lyrics = new List<string>();
                    subtitle.Serial = counter + 1;
                    DateTime startTime, endTime;
                    ParseTime(out startTime, out endTime, lines[++i]);
                    subtitle.StartTime = startTime;
                    subtitle.EndTime = endTime;
                    counter++;
                    i++;
                    while (i < lines.Count() && lines[i] != (counter + 1).ToString())
                    {
                        subtitle.Lyrics.Add(lines[i].Trim());
                        i++;
                    }
                    subtitles.Add(subtitle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSize"></param>
        /// <returns></returns>
        public static Bitmap GetEmptyFrame(SizeF textSize)
        {
            Bitmap bitmap;
            Bitmap img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            Font stringFont = new Font("Arial", 16);
            //textSize = drawing.MeasureString(lyrics[n - 1], stringFont);

            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);
            drawing = Graphics.FromImage(img);
            drawing.Clear(Color.White);

            Brush textBrush = new SolidBrush(Color.White);

            drawing.DrawString(string.Empty, stringFont, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            bitmap = (Bitmap)img;
            return bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="line"></param>
        private static void ParseTime(out DateTime startTime, out DateTime endTime, string line)
        {
            line = line.Trim();
            startTime = DateTime.Now;
            endTime = DateTime.Now;

            string pattern = "HH:mm:ss,fff";

            if (DateTime.TryParseExact(line.Substring(0, 12), pattern, CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out startTime))
            {
                // dt is the parsed value
            }

            if (DateTime.TryParseExact(line.Substring(line.Length - 12, 12), pattern, CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out endTime))
            {
                // dt is the parsed value
            }
        }
    }
}
