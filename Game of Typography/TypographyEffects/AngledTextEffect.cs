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

            Font font = new Font("Arial", stringFont.Size + k);
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
    }
}
