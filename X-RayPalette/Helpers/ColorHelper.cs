using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_RayPalette.Helpers
{
    public static class ColorHelper
    {
        public static Color LongRainbowColor(int value)
        {
            int r, g, b;
            double f = (double)value / 255;

            if (f <= 0.25)
            {
                r = 0;
                g = 0;
                b = (int)(255 * (0.25 - f) / 0.25);
            }
            else if (f <= 0.5)
            {
                r = 0;
                g = (int)(255 * (f - 0.25) / 0.25);
                b = 255;
            }
            else if (f <= 0.75)
            {
                r = (int)(255 * (f - 0.5) / 0.25);
                g = 255;
                b = (int)(255 * (0.75 - f) / 0.25);
            }
            else
            {
                r = 255;
                g = (int)(255 * (1 - f) / 0.25);
                b = 0;
            }

            return Color.FromArgb(r, g, b);
        }
        public static Color Pm3DColor(int value)
        {
            int r, g, b;
            double f = (double)value / 255;

            if (f < 0.25)
            {
                r = 0;
                g = 0;
                b = (int)(255 * f / 0.25);
            }
            else if (f < 0.5)
            {
                r = 0;
                g = (int)(255 * (f - 0.25) / 0.25);
                b = 255;
            }
            else if (f < 0.75)
            {
                r = (int)(255 * (f - 0.5) / 0.25);
                g = 255;
                b = (int)(255 - 255 * (f - 0.5) / 0.25);
            }
            else
            {
                r = 255;
                g = (int)(255 - 255 * (f - 0.75) / 0.25);
                b = 0;
            }

            return Color.FromArgb(r, g, b);
        }
    }
}
