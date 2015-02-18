using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Subtitles;
using System.Drawing;
using AVILibrary;
using TypographyEffects;

namespace Game_of_Typography
{
    public class AVI
    {
        private void addSound(FilePaths fps)
        {
            string filename = fps.AudioPath;//getfilename("sounds (*.wav)|*.wav");
            if (filename != null)
            {
                //txtreportsound.text = "adding to sound.wav to " + txtavifilename.text + "...\r\n";
                AviManager aviManager = new AviManager(fps.VideoPath, true);
                try
                {
                    int countFrames = aviManager.GetVideoStream().CountFrames;
                    if (countFrames > 0)
                    {
                        aviManager.AddAudioStream(filename, 0);
                    }
                    else
                    {
                        //MessageBox.Show(this, "Frame 0 does not exists. The video stream contains frame from 0 to " + (countFrames - 1) + ".");
                    }
                }
                catch (Exception ex)
                {
                  //  MessageBox.Show(this, "The file does not accept the new wave audio stream.\r\n" + ex.ToString(), "Error");
                }
                aviManager.Close();
            }
        }

        public void CreateVideo(FilePaths fps, double frameRate, TextEffectConfig config)
        {
            if (File.Exists(fps.VideoPath))
                File.Delete(fps.VideoPath);

            CurvedTextEffectConfig curvedTextConfig = (CurvedTextEffectConfig)config;

            DateTime lastVerifiedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

            List<Subtitle> subtitles = new List<Subtitle>();
            Subtitles.SubtitlesUtility.ParseSubtitle(out subtitles, fps.SrtPath);

            Bitmap img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            Font stringFont = new Font("Arial", curvedTextConfig.FontSize);
            SizeF textSize = drawing.MeasureString("I will follow you wherever you may be           ", stringFont);
            textSize.Height = 300;
            textSize.Width = 600;


            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);
            drawing = Graphics.FromImage(img);
            drawing.Clear(Color.Red);

            Brush textBrush = new SolidBrush(Color.White);

            drawing.DrawString("Escucha el ritmo de tu corazon", stringFont, textBrush, 0, 0);
            drawing.Save();
            
            

            AviManager aviManager = new AviManager(fps.VideoPath, false);
            VideoStream aviStream = aviManager.AddVideoStream(true, frameRate, img);
            img.Dispose();

            Bitmap bitmap;
            for (int n = 1; n <= subtitles.Count(); n++)
            {
                Subtitle s = subtitles.Where(i => i.Serial == n).ToList().Single();

                bitmap = SubtitlesUtility.GetEmptyFrame(textSize);

                int emptyFrameCount = (int)((s.StartTime - lastVerifiedTime).TotalMilliseconds * frameRate / 1000);

                //File.AppendAllText(@"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\\FramesCount.txt", n.ToString() + ": " + emptyFrameCount.ToString() + "\n");

                while (emptyFrameCount-- != 0)
                    aviStream.AddFrame(bitmap);

                lastVerifiedTime = s.EndTime;

                img = new Bitmap(1, 1);
                drawing = Graphics.FromImage(img);
                stringFont = new Font("Arial", curvedTextConfig.FontSize);
                img.Dispose();

                img = new Bitmap((int)textSize.Width, (int)textSize.Height);
                drawing = Graphics.FromImage(img);
                drawing.Clear(Color.Red);
                drawing.Save();

                bitmap = (Bitmap)img;
                //bitmap.Save(@"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\\Frame" + n + ".bmp");

                int frameCount = (int)((s.EndTime - s.StartTime).TotalMilliseconds * frameRate / 1000);
               // File.AppendAllText(@"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\\FramesCount.txt", n.ToString() + ": " + frameCount.ToString() + "\n");

                for (int i = 0; i < frameCount; i++)
                {
                    bitmap = new Bitmap(img);

                    //stringFont = new Font("Arial", i+16);
                    drawing = Graphics.FromImage(bitmap);

                    //TODO: make a switch case block which will tell us which effect to use.
                    //For this we will keep a list of constants and then map them with all the effect from the drop down list in UI
                    //It would look something like

                    //switch(constants.effect)
                    //{
                    //  case constants.curveeffect:
                        CurveEffect(drawing, s.Lyrics.Single(), stringFont, textBrush, textSize);
                    //break;
                    //case constants.bounceeffect:
                    //bouneeffect();
                    //break;

                    //}
                    
                    drawing.Save();

                    aviStream.AddFrame(bitmap);
                    bitmap.Dispose();
                }
                bitmap.Dispose();
                bitmap.Dispose();
                img.Dispose();
            }
            aviManager.Close();
            addSound(fps);
        }

        public void CurveEffect(Graphics drawing, string s, Font stringFont, Brush textBrush, SizeF textSize)
        {
            Point c = new Point((int)(textSize.Width / 2), (int)(textSize.Height / 2));
            CurvedTextEffects.DrawCurvedText(drawing, s, c, 120, (float)45, stringFont, textBrush);
        }
    }
}
