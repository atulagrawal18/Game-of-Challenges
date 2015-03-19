using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TypographyEffects
{
    public static class AngledTextEffect
    {
        public static void DrawAtAnAngleTextOfCenterAtCenterOfScreen(Graphics drawing, SizeF textSize, string text, int i, Font stringFont)
        {
            int j = i / 4 / 2;
            float k = (float)j;
            if (i % 4 == 1)
                k += (float)1.0;
            else if (i % 4 == 2)
                k += (float)0.5;
            else if (i % 4 == 3)
                k += (float)1.5;

            Font font = new Font(stringFont.FontFamily, stringFont.Size + k);
            //Font font = new Font("Arial", k+(float)0.25);
            Brush textBrush = new SolidBrush(Color.White);

            //var characterWidths = GetCharacterWidths(drawing, text, font).ToArray();
            //var textLength = characterWidths.Sum();

            SizeF size = drawing.MeasureString(text, font);
            //drawing.TranslateTransform(textSize.Width / 2 + size.Width / 2, textSize.Height / 2 + size.Height / 2);
            //drawing.TranslateTransform(400 + size.Width / 2, 200 + size.Height / 2);
            //drawing.TranslateTransform(size.Width / 2, size.Height / 2);
            drawing.TranslateTransform(textSize.Width / 2, textSize.Height / 2);
            drawing.RotateTransform(i * 10);
            PointF drawPoint = new PointF(-size.Width / 2, -size.Height / 2);
            drawing.DrawString(text, font, textBrush, drawPoint);//, StringFormat.GenericTypographic);
            drawing.Save();
            textBrush.Dispose();
        }

        public static void DrawTextAtCenter(Graphics drawing, SizeF textSize, string text, Font font, float size)
        {
            float X, Y;

            //Font stringFont = new Font("Arial", Properties.Settings.Default.FontSize + k);
            //Font stringFont = new Font(font.FontFamily, (Math.Abs((float)40.0 - (float)(i + 1)) == 0) ? 1 : Math.Abs((float)40.0 - (float)(i + 1)));
            Font stringFont = new Font(font.FontFamily, size);
            Brush textBrush = new SolidBrush(Color.White);

            var characterWidths = GetCharacterWidths(drawing, text, stringFont).ToArray();
            var textLength = characterWidths.Sum();

            X = (textSize.Width - textLength) / 2;
            Y = textSize.Height / 2;

            //drawing.DrawString(text, stringFont, textBrush, X, Y);
            drawing.DrawString(text, stringFont, textBrush, new PointF(X, Y), StringFormat.GenericTypographic);

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
    }
}
