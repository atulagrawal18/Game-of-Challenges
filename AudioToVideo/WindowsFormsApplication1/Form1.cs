using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AviFile;
using System.IO;
using System.Globalization;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtVideo.Text = @"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\sample.avi";
            txtAudio.Text = @"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\Enrique_Iglesias-_Rhythm_Divine.wav";
            txtSubtitle.Text = @"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\subtitle.srt";
        }

        private void addSound()
        {
            string filename = txtAudio.Text;//getfilename("sounds (*.wav)|*.wav");
            if (filename != null)
            {
                //txtreportsound.text = "adding to sound.wav to " + txtavifilename.text + "...\r\n";
                AviManager aviManager = new AviManager(txtVideo.Text, true);
                try
                {
                    int countFrames = aviManager.GetVideoStream().CountFrames;
                    if (countFrames > 0)
                    {
                        aviManager.AddAudioStream(filename, 0);
                    }
                    else
                    {
                        MessageBox.Show(this, "Frame 0 does not exists. The video stream contains frame from 0 to " + (countFrames - 1) + ".");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "The file does not accept the new wave audio stream.\r\n" + ex.ToString(), "Error");
                }
                aviManager.Close();
            }
        }

        private void btnOpenVideo_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtVideo.Text = openFileDialog1.FileName;
            }
        }

        private void btnCreateVideo_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtVideo.Text))
                File.Delete(txtVideo.Text);
            //double fps = 25;
            List<Subtitle> subtitles = new List<Subtitle>();
            bool flag = false;
            DateTime lastVerifiedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

            parseSubtitle(out subtitles, txtSubtitle.Text);

            int length, bitrate;
            GetAudioFileDetails(out bitrate, out length);

            Bitmap img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            Font stringFont = new Font("Arial", Properties.Settings.Default.FontSize);
            SizeF textSize = drawing.MeasureString("I will follow you wherever you may be           ", stringFont);
            textSize.Height = 400;
            textSize.Width = 800;

            float X = textSize.Width/10, Y = textSize.Height/2;

            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);
            drawing = Graphics.FromImage(img);
            drawing.Clear(Color.Red);

            Brush textBrush = new SolidBrush(Color.White);

            drawing.DrawString("", stringFont, textBrush, 0, 0);
            AviManager aviManager = new AviManager(txtVideo.Text, false);
            VideoStream aviStream = aviManager.AddVideoStream(true, Properties.Settings.Default.fps, img);

            //img.Save(@"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\FirstFrame.bmp");

            Bitmap bitmap = (Bitmap)img;
            Bitmap emptyFrame = (Bitmap)img;
            for (int n = 1; n <= subtitles.Count(); n++)
            {
                Subtitle s = subtitles.Where(i => i.Serial == n).ToList().Single();

                //ResetImage(img, drawing, bitmap, textSize);

                img.Dispose();
                drawing.Dispose();
                bitmap.Dispose();

                img = new Bitmap((int)textSize.Width, (int)textSize.Height);
                drawing = Graphics.FromImage(img);
                drawing.Clear(Color.Red);

                GetFrame(drawing, textSize, string.Empty, 0, 0);
                emptyFrame = (Bitmap)img;

                int emptyFrameCount = (int)((s.StartTime - lastVerifiedTime).TotalMilliseconds * Properties.Settings.Default.fps / 1000);

                File.AppendAllText(@"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\FramesCount.txt",n.ToString()+": "+emptyFrameCount.ToString()+"\n");

                while(emptyFrameCount-- != 0)
                    aviStream.AddFrame(emptyFrame, false);

                lastVerifiedTime = s.EndTime;

                int frameCount = (int)((s.EndTime - s.StartTime).TotalMilliseconds * Properties.Settings.Default.fps / 1000);
                File.AppendAllText(@"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\FramesCount.txt", n.ToString() + ": " + frameCount.ToString()+"\n");
                
                int d = frameCount / s.Lyrics.Single().Length;
                if (d == 1)
                    d = 2;

                int outerLoopCounter = 0;
                int innerLoopCounter = 0;
                bool IsJumpChar = true;

                string[] words = s.Lyrics.Single().ToString().Split(' ');

                int numberOfFrmaePerWord = frameCount/words.Length;
                int currentIndexOfWord = 0;

                for (int i = 0; i < frameCount; i++)//, outerLoopCounter++, innerLoopCounter++)
                {
                    //ResetImage(img, drawing, bitmap, textSize);
                    int count = i % (32*d);
                    img.Dispose();
                    drawing.Dispose();
                    bitmap.Dispose();

                    img = new Bitmap((int)textSize.Width, (int)textSize.Height);
                    drawing = Graphics.FromImage(img);
                    drawing.Clear(Color.Red);

                    //SizeF size = drawing.MeasureString(s.Lyrics.Single(), stringFont);

                    //RectangleF rec = new RectangleF(new PointF((textSize.Width - size.Width) / 2, (textSize.Height - size.Height) / 2), size);
                    //RectangleWithText r = new RectangleWithText(rec, s.Lyrics.Single());
                    //r.Draw(drawing);

                    DrawFlippedText(drawing, stringFont, textBrush, X, Y, s.Lyrics.Single(), bool flip_x, bool flip_y)

                    bitmap = (Bitmap)img;

                    aviStream.AddFrame(bitmap);
                    drawing.Dispose();
                }
                //bitmap.Save(@"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\Frame" + n + ".bmp");
                bitmap.Dispose();
                img.Dispose();
            }

            //ResetImage(img, drawing, bitmap, textSize);
            img.Dispose();
            drawing.Dispose();
            bitmap.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);
            drawing = Graphics.FromImage(img);
            drawing.Clear(Color.Red);

            GetFrame(drawing, textSize, string.Empty, 0, 0);
            bitmap = (Bitmap)img;

            int emptyFrameCountEnd = (int)((length - (lastVerifiedTime.Hour * 3600 + lastVerifiedTime.Minute * 60 + lastVerifiedTime.Second)) * Properties.Settings.Default.fps);

            File.AppendAllText(@"D:\challenge\branches\atul_bounce_effect\AudioToVideo\testdata\FramesCount.txt", subtitles.Count.ToString() + ": " + emptyFrameCountEnd.ToString() + "\n");

            while (emptyFrameCountEnd-- != 0)
                aviStream.AddFrame(bitmap, false);
            emptyFrame.Dispose();
            bitmap.Dispose();
            aviManager.Close();
            addSound();
            MessageBox.Show("Video is complete !!");
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

        private void btnAudioFile_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtAudio.Text = openFileDialog1.FileName;
            }
        }

        private void parseSubtitle(out List<Subtitle> subtitles, string filePath)
        {
            subtitles = new List<Subtitle>();

            string[] lines = System.IO.File.ReadAllLines(filePath);

            lines = lines.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
            lines = lines.Where(x => !string.IsNullOrWhiteSpace(x.Trim())).ToArray();

            for(int i=0,counter=0;i<lines.Count();)
            {
                lines[i].Trim();
                if (lines[i] == (counter+1).ToString())
                {
                    Subtitle subtitle = new Subtitle();
                    subtitle.Lyrics = new List<string>();
                    subtitle.Serial = counter + 1;
                    DateTime startTime,endTime;
                    parseTime(out startTime, out endTime, lines[++i]);
                    subtitle.StartTime = startTime;
                    subtitle.EndTime = endTime;
                    counter++;
                    i++;
                    while (i<lines.Count() && lines[i] != (counter + 1).ToString())
                    {
                        subtitle.Lyrics.Add(lines[i].Trim());
                        i++;
                    }
                    subtitles.Add(subtitle);
                }
            }
        }

        private void btnSubtitleFile_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtSubtitle.Text = openFileDialog1.FileName;
            }
        }

        private void parseTime(out DateTime startTime, out DateTime endTime, string line)
        {
            line = line.Trim();
            startTime = DateTime.Now;
            endTime = DateTime.Now;

            string pattern = "HH:mm:ss,fff";

            if (DateTime.TryParseExact(line.Substring(0,12), pattern, CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out startTime))
            {
                // dt is the parsed value
            }

            if (DateTime.TryParseExact(line.Substring(line.Length-12, 12), pattern, CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out endTime))
            {
                // dt is the parsed value
            }
        }

        private void GetFrame(Graphics drawing, SizeF textSize, string text, float X, float Y, int letter = 0)
        {
            Font stringFont = new Font("Arial", Properties.Settings.Default.FontSize);
            Brush textBrush = new SolidBrush(Color.White);
            drawing.DrawString(text, stringFont, textBrush, X, Y);

            //if (!string.IsNullOrEmpty(text) && text.Length >= letter + 1)
            //{
            //    drawing.DrawString(text.Substring(letter, 1), stringFont, textBrush, X + letter * Properties.Settings.Default.FontSize, Y - 20);
            //    SolidBrush blackBrush = new SolidBrush(Color.Red);
            //    drawing.FillRectangle(blackBrush, X + letter * Properties.Settings.Default.FontSize, Y, Properties.Settings.Default.FontSize, Properties.Settings.Default.FontSize);
            //}

            //for (int i = 0; i < text.Length; i++)
            //{
            //    if(i != letter)
            //    drawing.DrawString(text[i].ToString(), stringFont, textBrush, new Point((int)(X + i * Properties.Settings.Default.FontSize), (int)Y), StringFormat.GenericTypographic);
            //}

            drawing.Save();
            textBrush.Dispose();
        }

        public void MoveRightWithFirstFrameFreezed(Graphics drawing, SizeF textSize, string text, float X, float Y,  int i)
        {
            Font stringFont = new Font("Arial", Properties.Settings.Default.FontSize);
            Brush textBrush = new SolidBrush(Color.White);

            drawing.DrawString(text, stringFont, textBrush, X, Y);
            drawing.DrawString(text, stringFont, textBrush, new Point((int)(X + i * 10), (int)Y), StringFormat.GenericTypographic);

            drawing.Save();
            textBrush.Dispose();
        }

        public void MoveRightWithZoom(Graphics drawing, SizeF textSize, string text, float X, float Y, int i)
        {
            int j = i/4/2;
            float k = (float)j;
            if(i%4 == 1)
                k+= (float)1.0;
            else if(i%4 == 2)
                k+= (float)0.5;
            else if(i%4 == 3)
                k+= (float)1.5;

            Font stringFont = new Font("Arial", Properties.Settings.Default.FontSize + k);
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

        public void DrawAtAnAngleTextOfCenterAtCenterOfScreen(Graphics drawing, SizeF textSize, string text, int i)
        {
            int j = i / 4 / 2;
            float k = (float)j;
            if (i % 4 == 1)
                k += (float)1.0;
            else if (i % 4 == 2)
                k += (float)0.5;
            else if (i % 4 == 3)
                k += (float)1.5;

            Font stringFont = new Font("Arial", Properties.Settings.Default.FontSize + k );
            //Font stringFont = new Font("Arial", k+(float)0.25);
            Brush textBrush = new SolidBrush(Color.White);

            //var characterWidths = GetCharacterWidths(drawing, text, stringFont).ToArray();
            //var textLength = characterWidths.Sum();

            SizeF size = drawing.MeasureString(text, stringFont);
            //drawing.TranslateTransform(textSize.Width / 2 + size.Width / 2, textSize.Height / 2 + size.Height / 2);
            //drawing.TranslateTransform(400 + size.Width / 2, 200 + size.Height / 2);
            //drawing.TranslateTransform(size.Width / 2, size.Height / 2);
            drawing.TranslateTransform(textSize.Width / 2, textSize.Height / 2);
            drawing.RotateTransform(i*10);
            PointF drawPoint = new PointF(-size.Width / 2, -size.Height / 2);
            drawing.DrawString(text, stringFont, textBrush, drawPoint);//, StringFormat.GenericTypographic);
            drawing.Save();
            textBrush.Dispose();
        }

        public void DrawTextAtCenter(Graphics drawing, SizeF textSize, string text, float X, float Y, int i)
        {
            float j = (float)i / (float)8.0;
            float k = (float)j;
            if (i % 4 == 1)
                k += (float)0.1;
            else if (i % 4 == 2)
                k += (float)0.2;
            else if (i % 4 == 3)
                k += (float)0.3;

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

        public void PopulateWordByWord(Graphics drawing, SizeF textSize, string text, float X, float Y, int i, SizeF size, Font stringFont)
        {
            //Font stringFont = new Font("Arial", (float)100.0);
            Brush textBrush = new SolidBrush(Color.White);

            //var characterWidths = GetCharacterWidths(drawing, text, stringFont).ToArray();
            //var textLength = characterWidths.Sum();

            //SizeF size = drawing.MeasureString(text, stringFont);

            //X = (textSize.Width - size.Width) / 2;
            //Y = (textSize.Height - size.Height) / 2;

            //drawing.DrawString(text, stringFont, textBrush, X, Y);
            drawing.DrawString(text, stringFont, textBrush, new Point((int)(X), (int)Y), StringFormat.GenericTypographic);

            drawing.Save();
            textBrush.Dispose();
        }

        private void GraphicPathCurve(Graphics g)
        {
            string text = "Some text to wrap";

            //[[ Calculate coefficients A thru H from the control points ]]

            GraphicsPath textPath = new GraphicsPath();

            // the baseline should start at 0,0, so the next line is not quite correct
            //path.AddString(text, someFont, someStyle, someFontSize, new Point(0,0));

            RectangleF textBounds = textPath.GetBounds();
 
            for (int i =0; i < textPath.PathPoints.Length; i++)
            {
                PointF pt = textPath.PathPoints[i];
                float textX = pt.X;
                float textY = pt.Y;
    
                // normalize the x coordinate into the parameterized value
                // with a domain between 0 and 1.
                float t =  textX / textBounds.Width;  
       
                // calculate spline point for parameter t
                //float Sx = A * Math.Pow(t, 3) + B * Math.Pow(t, 2) + C * t + D;
                //float Sy = E * Math.Pow(t, 3) + F * Math.Pow(t, 2) + G * t + H;

                float Sx = (float) Math.Pow(t, 3) + (float) Math.Pow(t, 2) + t;
                float Sy = (float) Math.Pow(t, 3) + (float) Math.Pow(t, 2) + t;
        
                // calculate the tangent vector for the point        
                //float Tx = 3At2 + 2Bt + C;
                //float Ty = 3Et2 + 2Ft + G;

                float Tx = 3 * (float) Math.Pow(t, 2) + 2 * t;
                float Ty = 3 * (float) Math.Pow(t, 3) + 2 * t;
                // rotate 90 or 270 degrees to make it a perpendicular
                float Px =   Ty;
                float Py = - Tx;
    
                // normalize the perpendicular into a unit vector
                float magnitude = (float) Math.Sqrt(Px * Px + Py * Py);
                Px = Px / magnitude;
                Py = Py / magnitude;

                // assume that input text point y coord is the "height" or 
                // distance from the spline.  Multiply the perpendicular vector 
                // with y. it becomes the new magnitude of the vector.
                Px *= textY;
                Py *= textY;
    
                // translate the spline point using the resultant vector
                float finalX = Px + Sx;
                float finalY = Py + Sy;
    
                // I wish it were this easy, actually need 
                // to create a new path.
                textPath.PathPoints[i] = new PointF(finalX, finalY);
            }

            // draw the transformed text path		
            g.DrawPath(Pens.Black, textPath);

            // draw the Bézier for reference
            //g.DrawBezier(Pens.Black, P0,  P1,  P2,  P3);
        }

        private static IEnumerable<float> GetCharacterWidths(Graphics graphics, string text, Font font)
        {
            // The length of a space. Necessary because a space measured using StringFormat.GenericTypographic has no width.
            // We can't use StringFormat.GenericDefault for the characters themselves, as it adds unwanted spacing.
            var spaceLength = graphics.MeasureString(" ", font, Point.Empty, StringFormat.GenericDefault).Width;

            return text.Select(c => c == ' ' ? spaceLength : graphics.MeasureString(c.ToString(), font, Point.Empty, StringFormat.GenericTypographic).Width);
        }

        public float MeasureCharacterRangesRegions(Graphics graphics, string text, float X, int level, int indexToBeJumped, int heightToBeJumped, bool flag)
        {
            //static int currentIndex = 0;
            Graphics g = graphics;
            string measureString = text;
            // Set up string.
            if (text.Length >= 32)
            {
                measureString = measureString.Substring(0, 32);
            }

            int numChars = measureString.Length;

            //
            // Set up the characted ranger array.
            CharacterRange[] characterRanges = new CharacterRange[numChars];
            for (int i = 0; i < numChars; i++)
                characterRanges[i] = new CharacterRange(i, 1);

            //
            // Set up the string format
            StringFormat stringFormat = new StringFormat();
            stringFormat.FormatFlags = StringFormatFlags.NoClip;   // Make sure the characters are not clipped
            stringFormat.SetMeasurableCharacterRanges(characterRanges);

            //
            // Set up the array to accept the regions.
            Region[] stringRegions = new Region[numChars];

            //
            // The font to use.. 'using' will dispose of it for us
            using (Font stringFont = new Font("Times New Roman", 36.0F))
            {

                //
                // Get the max width.. for the complete length
                SizeF size = g.MeasureString(measureString, stringFont);

                //
                // Assume the string is in a stratight line, just to work out the 
                // regions. We will adjust the containing rectangles later.
                RectangleF layoutRect = new RectangleF(X, 100.0f, size.Width, size.Height);

                //
                // Caluclate the regions for each character in the string.
                stringRegions = g.MeasureCharacterRanges(
                    measureString,
                    stringFont,
                    layoutRect,
                    stringFormat);

                //
                // Some random offsets, uncomment the DrawRectagle if you want to see the bounding box.
                Random rand = new Random();
                for (int indx = 0; indx < numChars; indx++)
                {
                    Region region = stringRegions[indx] as Region;
                    RectangleF rect = region.GetBounds(g);

                    if (level % 2 == 0)
                    {
                        if (indx % 2 == 0)
                        {
                            rect.Offset(0f, (float)5.0f);
                        }
                        else
                        {
                            rect.Offset(0f, -(float)5.0f);
                        }
                    }
                    else
                    {
                        if (indx % 2 == 0)
                        {
                            rect.Offset(0f, -(float)5.0f);
                        }
                        else
                        {
                            rect.Offset(0f, (float)5.0f);
                        }
                    }
                    //if (flag)
                    //{
                    //    if (indexToBeJumped < numChars && indexToBeJumped == indx)
                    //    {
                    //        //rect.Offset(0f, (float)-20.0f);
                    //        rect.Offset(0f, -heightToBeJumped);
                    //    }
                    //}

                    g.DrawString(measureString.Substring(indx, 1),
                          stringFont, Brushes.White, rect, stringFormat);
                }

                g.Save();

                Region regionLast = stringRegions[numChars-1] as Region;
                RectangleF rectLast = regionLast.GetBounds(g);
                return rectLast.X+rectLast.Width-10.0f;
            }
        }

        private int Min(int a, int b)
        {
            if (a < b)
                return a;
            else
                return b;
        }

        public void DrawFlippedText(Graphics gr, Font font, Brush brush, int x, int y, string txt, bool flip_x, bool flip_y)
        {
            //Save the current graphics state.
            GraphicsState state = gr.Save();

            //Set up the transformation.
            gr.ResetTransform();
            gr.ScaleTransform(1.0f, -1.0f);

            //Figure out where to draw.
            SizeF txt_size = gr.MeasureString(txt, font);
            if (flip_x) x = -x - (int) txt_size.Width;
            if (flip_y) y = -y - (int) txt_size.Height;

            //Draw.
            gr.DrawString(txt, font, brush, x, y);

            //Restore the original graphics state.
            gr.Restore(state);
        }

        //private void ResetImage(Bitmap img, Graphics drawing, Bitmap bitmap, SizeF textSize)
        //{
        //    img.Dispose();
        //    drawing.Dispose();
        //    //bitmap.Dispose();

        //    Bitmap img1 = new Bitmap((int)textSize.Width, (int)textSize.Height);
        //    Graphics drawing1 = Graphics.FromImage(img1);
        //    drawing1.Clear(Color.Red);
        //    img = img1;
        //    drawing = drawing1;
        //}

    }
}
