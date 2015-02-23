using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TypographyEffects
{
    public static class ThreeWordTextEffect
    {
        public static void PopulateWordByWord(Graphics drawing, SizeF textSize, string text, float X, float Y, int i, SizeF size, Font stringFont)
        {
            Brush textBrush = new SolidBrush(Color.White);
            drawing.DrawString(text, stringFont, textBrush, new Point((int)(X), (int)Y), StringFormat.GenericTypographic);

            drawing.Save();
            textBrush.Dispose();
        }
    }
}
