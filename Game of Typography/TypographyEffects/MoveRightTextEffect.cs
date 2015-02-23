using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TypographyEffects
{
    public static class MoveRightTextEffect
    {
        public static void MoveRightWithFirstFrameFreezed(Graphics drawing, SizeF textSize, string text, float X, float Y, int i, Font font)
        {
            //Font stringFont = new Font("Arial", Properties.Settings.Default.FontSize);
            Brush textBrush = new SolidBrush(Color.White);

            drawing.DrawString(text, font, textBrush, X, Y);
            drawing.DrawString(text, font, textBrush, new Point((int)(X + i * 10), (int)Y), StringFormat.GenericTypographic);

            drawing.Save();
            textBrush.Dispose();
        }

        public static void MoveRightWithZoom(Graphics drawing, SizeF textSize, string text, float X, float Y, int i, Font stringFont)
        {
            int j = i / 4 / 2;
            float k = (float)j;
            if (i % 4 == 1)
                k += (float)1.0;
            else if (i % 4 == 2)
                k += (float)0.5;
            else if (i % 4 == 3)
                k += (float)1.5;

            Font font = new Font("Arial", stringFont.Size + k);
            Brush textBrush = new SolidBrush(Color.White);

            var characterWidths = GetCharacterWidths(drawing, text, font).ToArray();
            var textLength = characterWidths.Sum();

            X = (textSize.Width - textLength) / 2;
            Y = textSize.Height / 2;

            //drawing.DrawString(text, font, textBrush, X, Y);
            drawing.DrawString(text, font, textBrush, new Point((int)(X), (int)Y), StringFormat.GenericTypographic);

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
