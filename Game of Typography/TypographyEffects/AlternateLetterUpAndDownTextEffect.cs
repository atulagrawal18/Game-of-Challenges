using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace TypographyEffects
{
    public static class AlternateLetterUpAndDownTextEffect
    {
        public static float MeasureCharacterRangesRegions(Graphics graphics, string text, float X, int level, float Y, Font font)
        {
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
            using (Font stringFont = new Font(font.FontFamily, font.Size, FontStyle.Bold))
            {

                //
                // Get the max width.. for the complete length
                SizeF size = g.MeasureString(measureString, stringFont);

                //
                // Assume the string is in a stratight line, just to work out the 
                // regions. We will adjust the containing rectangles later.
                RectangleF layoutRect = new RectangleF(X, Y/2, size.Width, size.Height);

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

                    //float[] numbers = new float[] {.5f,1,1.5f,2,2.5f,3,3.5f,4,4.5f,5,5.5f,6,5.5f,5,4.5f,4,3.5f,3,2.5f,2,1.5f,1,.5f,0};

                    float[] numbers = GetNumberArray(.05f, 2);

                    if (level % 2 == 0)
                    {
                        if (indx % 2 == 0)
                        {
                            float y_height = numbers[level % numbers.Length];
                            rect.Offset(0f, y_height);
                            //rect.Offset(0f, (float)5.0f);
                        }
                        else
                        {
                            float y_height = numbers[level % numbers.Length];
                            rect.Offset(0f, -y_height);
                            //rect.Offset(0f, (float)5.0f);
                        }
                    }
                    else
                    {
                        if (indx % 2 == 0)
                        {
                            float y_height = numbers[level % numbers.Length];
                            rect.Offset(0f, -y_height);
                            //rect.Offset(0f, (float)5.0f);
                        }
                        else
                        {
                            float y_height = numbers[level % numbers.Length];
                            rect.Offset(0f, y_height);
                            //rect.Offset(0f, (float)5.0f);
                        }
                    }

                    g.DrawString(measureString.Substring(indx, 1),
                          stringFont, Brushes.White, rect, stringFormat);
                }
            

                g.Save();

                Region regionLast = stringRegions[numChars - 1] as Region;
                RectangleF rectLast = regionLast.GetBounds(g);
                return rectLast.X + rectLast.Width - 10.0f;
            }
        }

        public static float[] GetNumberArray(float gap, float max)
        { 
            float number = gap;
            List<float> numberList = new List<float>();
            while (number <= max)
            {
                numberList.Add(number);
                number += gap;
            }

            while (number > 0)
            {
                number -= gap;
                numberList.Add(number);
            }

            return numberList.ToArray();
        }
    }
}
