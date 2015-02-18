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

            int length, bitrate;
            GetAudioFileDetails(out bitrate, out length);

            Bitmap img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            Font stringFont = new Font("Arial", curvedTextConfig.FontSize);
            SizeF textSize = drawing.MeasureString("I will follow you wherever you may be           ", stringFont);
            textSize.Height = 400;
            textSize.Width = 800;


            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);
            drawing = Graphics.FromImage(img);
            drawing.Clear(Color.Red);

            Brush textBrush = new SolidBrush(Color.White);

            drawing.DrawString("", stringFont, textBrush, 0, 0);
            drawing.Save();
            
            

            AviManager aviManager = new AviManager(fps.VideoPath, false);
            VideoStream aviStream = aviManager.AddVideoStream(true, frameRate, img);
            img.Dispose();

            Bitmap bitmap = (Bitmap)img;
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
                bitmap.Dispose();

                //bitmap.Save(@"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\\Frame" + n + ".bmp");

                int frameCount = (int)((s.EndTime - s.StartTime).TotalMilliseconds * frameRate / 1000);
                // File.AppendAllText(@"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\\FramesCount.txt", n.ToString() + ": " + frameCount.ToString() + "\n");

                int d = frameCount / s.Lyrics.Single().Length;
                if (d == 1)
                    d = 2;

                for (int i = 0; i < frameCount; i++)
                {
                    img.Dispose();
                    drawing.Dispose();
                    bitmap.Dispose();

                    img = new Bitmap((int)textSize.Width, (int)textSize.Height);
                    drawing = Graphics.FromImage(img);
                    drawing.Clear(Color.Red);
                    bitmap = (Bitmap)img;

                    //TODO: make a switch case block which will tell us which effect to use.
                    //For this we will keep a list of constants and then map them with all the effect from the drop down list in UI
                    //It would look something like

                    //switch(constants.effect)
                    //{
                    //  case constants.curveeffect:
                        //CurveEffect(drawing, s.Lyrics.Single(), stringFont, textBrush, textSize);
                    //break;
                    //case constants.bounceeffect:
                        

                    BounceEffect(drawing, i, s.Lyrics.Single(), d);
                    //break;

                    //}
                    
                    drawing.Save();

                    aviStream.AddFrame(bitmap);
                    bitmap.Dispose();
                    drawing.Dispose();
                }
                bitmap.Dispose();
                bitmap.Dispose();
                img.Dispose();
            }

            img.Dispose();
            drawing.Dispose();
            bitmap.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);
            drawing = Graphics.FromImage(img);
            drawing.Clear(Color.Red);

            bitmap = SubtitlesUtility.GetEmptyFrame(textSize);

            int emptyFrameCountEnd = (int)((length - (lastVerifiedTime.Hour * 3600 + lastVerifiedTime.Minute * 60 + lastVerifiedTime.Second)) * frameRate);

            File.AppendAllText(@"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\FramesCount.txt", subtitles.Count.ToString() + ": " + emptyFrameCountEnd.ToString() + "\n");

            while (emptyFrameCountEnd-- != 0)
                aviStream.AddFrame(bitmap);
            bitmap.Dispose();

            aviManager.Close();
            addSound(fps);
        }

        public void CurveEffect(Graphics drawing, string s, Font stringFont, Brush textBrush, SizeF textSize)
        {
            Point c = new Point((int)(textSize.Width / 2), (int)(textSize.Height / 2));
            CurvedTextEffects.DrawCurvedText(drawing, s, c, 120, (float)45, stringFont, textBrush);
        }

        public void BounceEffect(Graphics drawing, int i, string s, int d)
        {
            int count = i % (32 * d);

            int strLength = s.Length;
            int startIndex = 0;
            float firstCoordinate = 0.0f;

            int outerLoopCounter = 0;
            int innerLoopCounter = 0;
            bool IsJumpChar = true;

            if (i >= 32 * d)
            {
                if (strLength <= 32)
                    IsJumpChar = false;
                else
                    innerLoopCounter = -1;
            }
            else
            {
                innerLoopCounter = 0;
            }

            while (strLength > 0)
            {
                firstCoordinate = BounceTextEffect.MeasureCharacterRangesRegions(drawing, s.Substring(startIndex, Min(strLength, 32)), firstCoordinate, i, count / 2, count % 2 * 10 + 10, outerLoopCounter == innerLoopCounter && IsJumpChar);
                strLength -= 32;
                startIndex += 32;
                drawing.Save();
                if (strLength > 0)
                    innerLoopCounter++;
            }
        }

        private int Min(int a, int b)
        {
            if (a < b)
                return a;
            else
                return b;
        }

        private void GetAudioFileDetails(out int bitrate, out int length)
        {
            using (var f = File.OpenRead(txtAudio.Text))
            {
                f.Seek(28, SeekOrigin.Begin);
                byte[] val = new byte[4];
                f.Read(val, 0, 4);
                bitrate = (8 * BitConverter.ToInt32(val, 0)) / 1000;
                length = (int)(f.Length / BitConverter.ToInt32(val, 0));
            }
        }
    }
}
