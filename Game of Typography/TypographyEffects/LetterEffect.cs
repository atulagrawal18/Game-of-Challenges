using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TypographyEffects
{
    public static class LetterEffect
    {
        public static void PopulateLetterByLetter(Graphics drawing, SizeF textSize, string text, float X, float Y, int i, int currentIndex, Font font)
        {
            Font stringFont = new Font(font.FontFamily, font.Size);
            var characterWidths = GetCharacterWidths(drawing, text, stringFont).ToArray();

            SizeF txt_size = drawing.MeasureString(text, stringFont);

            //X = (textSize.Width - txt_size.Width) / 2;
            Y = textSize.Height / 2;

            //Rectangle r = new Rectangle((int)X + i * 4 + 30, (int)Y, (int)txt_size.Width - i * 4 - 30, (int)txt_size.Height);
            //Brush textBrush = new LinearGradientBrush(r, Color.Red, Color.Blue, 0f);
            Brush textBrush = new SolidBrush(Color.Black);

            if (currentIndex == 0)
            {
                drawing.DrawString(text.ElementAt(currentIndex).ToString(), stringFont, textBrush, new Point((int)(X), (int)Y), StringFormat.GenericTypographic);
            }
            else
            {
                if (currentIndex < text.Length)
                {
                    drawing.DrawString(text.ElementAt(currentIndex).ToString(), stringFont, textBrush, new Point((int)(X), (int)Y), StringFormat.GenericTypographic);
                }
            }
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
