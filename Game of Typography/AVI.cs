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

        public void CreateVideo(FilePaths fps, double frameRate, TextEffectConfig config, TextEffect textEffect)
        {
            if (File.Exists(fps.VideoPath))
                File.Delete(fps.VideoPath);

            CurvedTextEffectConfig curvedTextConfig;
            BounceTextEffectConfig bounceTextConfig;
            Font stringFont =  new Font("Arial", 16);;
            if(textEffect == TextEffect.CurvedTextEffect)
            {
                curvedTextConfig = (CurvedTextEffectConfig)config;
                stringFont = new Font("Arial", curvedTextConfig.FontSize);
            }
            else if (textEffect == TextEffect.BouncingTextEffect)
            {
                bounceTextConfig = (BounceTextEffectConfig)config;
                stringFont = new Font("Arial", bounceTextConfig.FontSize);
            }
            else
            {
                bounceTextConfig = (BounceTextEffectConfig)config;
                stringFont = new Font("Arial", bounceTextConfig.FontSize);
            }

            DateTime lastVerifiedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

            List<Subtitle> subtitles = new List<Subtitle>();
            Subtitles.SubtitlesUtility.ParseSubtitle(out subtitles, fps.SrtPath);

            int length, bitrate;
            GetAudioFileDetails(out bitrate, out length, fps);

            Bitmap img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

           
            SizeF textSize = drawing.MeasureString("I will follow you wherever you may be           ", stringFont);
            textSize.Height = 400;
            textSize.Width = 800;


            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);
            drawing = Graphics.FromImage(img);
            drawing.Clear(Color.Blue);

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
               // stringFont = new Font("Arial", config.FontSize);
                img.Dispose();

                img = new Bitmap((int)textSize.Width, (int)textSize.Height);
                drawing = Graphics.FromImage(img);
                drawing.Clear(Color.Blue);
                drawing.Save();
                bitmap.Dispose();

                //bitmap.Save(@"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\\Frame" + n + ".bmp");

                int frameCount = (int)((s.EndTime - s.StartTime).TotalMilliseconds * frameRate / 1000);
                // File.AppendAllText(@"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\\FramesCount.txt", n.ToString() + ": " + frameCount.ToString() + "\n");

                int d = frameCount / s.Lyrics.Length;
                if (d == 1)
                    d = 2;

                string[] words = s.Lyrics.Split(' ');
                int numberOfFrmaePerWord = frameCount / words.Length;
                int currentIndexOfWord = 0;
                float X = textSize.Width / 10, Y = textSize.Height / 2;

                for (int i = 0; i < frameCount; i++)
                {
                    img.Dispose();
                    drawing.Dispose();
                    bitmap.Dispose();

                    img = new Bitmap((int)textSize.Width, (int)textSize.Height);
                    drawing = Graphics.FromImage(img);
                    drawing.Clear(Color.Blue);
                    bitmap = (Bitmap)img;

                    if ((n%8 == 1 && textEffect == TextEffect.Random) || textEffect == TextEffect.CurvedTextEffect)
                    {
                        CurveEffect(drawing, s.Lyrics, stringFont, textBrush, textSize, i);
                    }
                    else if ((n % 8 == 2 && textEffect == TextEffect.Random) || textEffect == TextEffect.BouncingTextEffect)
                    {
                        BounceEffect(drawing, i, s.Lyrics, d, textSize, stringFont);
                    }
                    else if ((n % 8 == 3 && textEffect == TextEffect.Random) || textEffect == TextEffect.AlternateLetterUpAndDownEffect)
                    {
                        AlternateLetterUpAndDownEffect(drawing, i, s.Lyrics, textSize, stringFont);
                    }
                    else if ((n % 8 == 4 && textEffect == TextEffect.Random) || textEffect == TextEffect.AngledTextEffect)
                    {
                        var characterWidths = GetCharacterWidths(drawing, s.Lyrics, stringFont).ToArray();
                        var textLength = characterWidths.Sum();

                        X = (textSize.Width - textLength) / 2;
                        Y = textSize.Height / 2;
                        if (i <= frameCount * 6 / 10)
                            AngledTextEffect.DrawAtAnAngleTextOfCenterAtCenterOfScreen(drawing, textSize, s.Lyrics, i, stringFont);
                        else
                            DrawTextAtCenter(drawing, textSize, s.Lyrics, i);
                    }
                    else if ((n % 8 == 5 && textEffect == TextEffect.Random) || textEffect == TextEffect.MoveRightWithFirstFrameFreezedTextEffect)
                    {
                        MoveRightTextEffect.MoveRightWithFirstFrameFreezed(drawing, textSize, s.Lyrics, X, Y, i, stringFont);
                    }
                    else if ((n % 8 == 6 && textEffect == TextEffect.Random) || textEffect == TextEffect.MoveRightWithZoomEffect)
                    {
                        MoveRightTextEffect.MoveRightWithZoom(drawing, textSize, s.Lyrics, X, Y, i, stringFont);
                    }
                    else if ((n % 8 == 7 && textEffect == TextEffect.Random) || textEffect == TextEffect.ThreeWordTextEffect)
                    {
                        int count = i % (32 * d);
                        int counter = numberOfFrmaePerWord;
                        if (currentIndexOfWord < words.Length)
                        {
                            PopulateWordEffect(drawing, numberOfFrmaePerWord, currentIndexOfWord, words, textSize, count, i);
                            while (counter > 0 && i < frameCount)
                            {
                                counter--;
                                i++;
                                aviStream.AddFrame(bitmap);
                            }

                            while (currentIndexOfWord == words.Length-1 && i < frameCount)
                            {
                                i++;
                                //bitmap.Save(@"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\FrameFull" + i + ".bmp");
                                aviStream.AddFrame(bitmap);
                            }
                            i--;
                        }
                        currentIndexOfWord++;
                    }
                    else if ((n % 8 == 0 && textEffect == TextEffect.Random) || textEffect == TextEffect.LetterEffect)
                    {
                        string text = s.Lyrics;
                        while (i < text.Length)
                        {
                            var characterWidths = GetCharacterWidths(drawing, text, stringFont).ToArray();

                            float w;

                            if (i == 0)
                                w = 0;
                            else
                                w = i < text.Length ? characterWidths[i - 1] : characterWidths[text.Length - 1];

                            LetterEffect.PopulateLetterByLetter(drawing, textSize, text, X + w, Y, i, i, stringFont);
                            X += w;
                            bitmap = (Bitmap)img;
                            //bitmap.Save(@"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\Frame" + i + ".bmp");
                            aviStream.AddFrame(bitmap);
                            i++;
                        }
                        while (i < frameCount)
                        {
                            //bitmap.Save(@"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\FrameFull" + i + ".bmp");
                            aviStream.AddFrame(bitmap);
                            i++;
                        }
                    }

                    drawing.Save();

                    if (textEffect != TextEffect.ThreeWordTextEffect && textEffect != TextEffect.LetterEffect)
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
            drawing.Clear(Color.Blue);

            bitmap = SubtitlesUtility.GetEmptyFrame(textSize);

            int emptyFrameCountEnd = (int)((length - (lastVerifiedTime.Hour * 3600 + lastVerifiedTime.Minute * 60 + lastVerifiedTime.Second)) * frameRate);

           // File.AppendAllText(@"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\FramesCount.txt", subtitles.Count.ToString() + ": " + emptyFrameCountEnd.ToString() + "\n");

            while (emptyFrameCountEnd-- != 0)
                aviStream.AddFrame(bitmap);
            bitmap.Dispose();

            aviManager.Close();
            addSound(fps);
        }

        public void CurveEffect(Graphics drawing, string s, Font stringFont, Brush textBrush, SizeF textSize, int i)
        {
            float f = (float)45.0 + (float)i/30;
            Point c = new Point((int)(textSize.Width / 2), 0);// (int)(textSize.Height / 2));
            CurvedTextEffects.DrawCurvedText(drawing, s, c, 250, f/*(float)45*/, stringFont, textBrush);
        }

        public void BounceEffect(Graphics drawing, int i, string s, int d, SizeF textSize, Font stringFont)
        {
            int count = i % (32 * d);

            int strLength = s.Length;
            int startIndex = 0;
            SizeF size = drawing.MeasureString(s, stringFont);
            //float firstCoordinate = 0.0f;
            float firstCoordinate = (textSize.Width - size.Width) / 2;

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
                firstCoordinate = BounceTextEffect.MeasureCharacterRangesRegions(drawing, s.Substring(startIndex, Min(strLength, 32)), firstCoordinate, i, count / 2, count % 2 * 10 + 10, outerLoopCounter == innerLoopCounter && IsJumpChar, textSize.Height);
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

        public void AlternateLetterUpAndDownEffect(Graphics drawing, int i, string s, SizeF textSize, Font stringFont)
        {
            int strLength = s.Length;
            int startIndex = 0;
            SizeF size = drawing.MeasureString(s, stringFont);
            //float firstCoordinate = 0.0f;
            float firstCoordinate = (textSize.Width - size.Width) / 2;

            while (strLength > 0)
            {
                firstCoordinate = AlternateLetterUpAndDownTextEffect.MeasureCharacterRangesRegions(drawing, s.Substring(startIndex, Min(strLength, 32)), firstCoordinate, i, textSize.Height);
                strLength -= 32;
                startIndex += 32;
                drawing.Save();
            }
        }

        private void GetAudioFileDetails(out int bitrate, out int length, FilePaths fps)
        {
            using (var f = File.OpenRead(fps.AudioPath))
            {
                f.Seek(28, SeekOrigin.Begin);
                byte[] val = new byte[4];
                f.Read(val, 0, 4);
                bitrate = (8 * BitConverter.ToInt32(val, 0)) / 1000;
                length = (int)(f.Length / BitConverter.ToInt32(val, 0));
            }
        }

        public void DrawTextAtCenter(Graphics drawing, SizeF textSize, string text, int i)
        {
            float j = (float)i / (float)8.0;
            float k = (float)j;
            if (i % 4 == 1)
                k += (float)0.1;
            else if (i % 4 == 2)
                k += (float)0.2;
            else if (i % 4 == 3)
                k += (float)0.3;

            float X, Y;

            //Font stringFont = new Font("Arial", Properties.Settings.Default.FontSize + k);
            Font stringFont = new Font("Arial", (Math.Abs((float)40.0 - (float)(i + 1)) == 0) ? 1 : Math.Abs((float)40.0 - (float)(i + 1)));
            Brush textBrush = new SolidBrush(Color.White);

            var characterWidths = GetCharacterWidths(drawing, text, stringFont).ToArray();
            var textLength = characterWidths.Sum();

            X = (textSize.Width - textLength) / 2;
            Y = textSize.Height / 2;

            //drawing.DrawString(text, stringFont, textBrush, X, Y);
            drawing.DrawString(text, stringFont, textBrush, new Point((int)(X), (int)Y), StringFormat.GenericTypographic);

            drawing.Save();
            textBrush.Dispose();
        }

        private static IEnumerable<float> GetCharacterWidths(Graphics graphics, string text, Font font)
        {
            // The length of a space. Necessary because a space measured using StringFormat.GenericTypographic has no width.
            // We can't use StringFormat.GenericDefault for the characters themselves, as it adds unwanted spacing.
            var spaceLength = graphics.MeasureString(" ", font, Point.Empty, StringFormat.GenericDefault).Width;

            return text.Select(c => c == ' ' ? spaceLength : graphics.MeasureString(c.ToString(), font, Point.Empty, StringFormat.GenericTypographic).Width);
        }

        private static void PopulateWordEffect(Graphics drawing, int numberOfFrmaePerWord, int currentIndexOfWord, string[] words, SizeF textSize, int count, int i)
        {
            float X, Y;
            Font stringFont = new Font("Times New Roman", (float)70.0);

            stringFont = new Font("Times New Roman", (float)70.0);
            SizeF size = drawing.MeasureString(words[currentIndexOfWord], stringFont);
            X = textSize.Width * 2 / 10;
            Y = (textSize.Height - size.Height) / 2 + textSize.Height/5;
            ThreeWordTextEffect.PopulateWordByWord(drawing, textSize, words[currentIndexOfWord], X, Y, i, size, stringFont);
            drawing.Save();

            //bitmap = (Bitmap)img;
            //bitmap.Save(@"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\FrameHalf" + i + ".bmp");

            if (currentIndexOfWord - 1 >= 0)
            {
                stringFont = new Font("Times New Roman", (float)50.0);
                size = drawing.MeasureString(words[currentIndexOfWord-1], stringFont);

                if(count <  numberOfFrmaePerWord/2)
                    Y = (Y - size.Height/2);
                else
                    Y = (Y - size.Height);

                ThreeWordTextEffect.PopulateWordByWord(drawing, textSize, words[currentIndexOfWord - 1], X, Y, i, size, stringFont);
                drawing.Save();
            }

            if (currentIndexOfWord - 2 >= 0)
            {
                stringFont = new Font("Times New Roman", (float)30.0);
                size = drawing.MeasureString(words[currentIndexOfWord - 2], stringFont);

                if (count < numberOfFrmaePerWord / 2)
                    Y = (Y - size.Height / 2);
                else
                    Y = (Y - size.Height);

                ThreeWordTextEffect.PopulateWordByWord(drawing, textSize, words[currentIndexOfWord - 2], X, Y, i, size, stringFont);
                drawing.Save();
            }

            if (currentIndexOfWord - 3 >= 0)
            {
                stringFont = new Font("Times New Roman", (float)20.0);
                size = drawing.MeasureString(words[currentIndexOfWord - 3], stringFont);

                if (count < numberOfFrmaePerWord / 2)
                    Y = (Y - size.Height / 2);
                else
                    Y = (Y - size.Height);

                ThreeWordTextEffect.PopulateWordByWord(drawing, textSize, words[currentIndexOfWord - 3], X, Y, i, size, stringFont);
                drawing.Save();
            }

            currentIndexOfWord++;
        }
    }
}
