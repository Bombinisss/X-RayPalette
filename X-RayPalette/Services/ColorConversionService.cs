using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Helpers;
using X_RayPalette;

namespace X_RayPalette.Services
{
    public class ColorConversionService
    {
        public bool IsProcessing;
        public void Start(string inputPath, int mode, ImageRenderService imageRenderService)
        {
            IsProcessing = false;
            ProccessImage(inputPath, ImagePathHelper.ImagesFolderPath() + "\\output.png", mode, imageRenderService);
        }

        void ProccessImage(string inputPath, string outputPath, int mode, ImageRenderService service)
        {
            using (Bitmap inputBitmap = new Bitmap(inputPath))
            {
                Bitmap outputBitmap = new Bitmap(inputBitmap.Width, inputBitmap.Height);

                for (int y = 0; y < inputBitmap.Height; y++)
                {
                    for (int x = 0; x < inputBitmap.Width; x++)
                    {
                        Color pixelColor = inputBitmap.GetPixel(x, y);
                        int grayscaleValue = (int)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);

                        Color rainbowColor = mode == 0 ? ColorHelper.Pm3DColor(grayscaleValue) : ColorHelper.LongRainbowColor(grayscaleValue);

                        outputBitmap.SetPixel(x, y, rainbowColor);
                    }
                }
                outputBitmap.Save(outputPath, ImageFormat.Png);
                Gui.ImageHandlerOut = service.Create(ImagePathHelper.ImagesFolderPath() + "\\output.png");
                IsProcessing = true;
            }
        }
    }
}