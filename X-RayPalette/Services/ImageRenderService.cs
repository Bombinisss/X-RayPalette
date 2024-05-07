using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veldrid.ImageSharp;
using Veldrid;
using X_RayPalette;

namespace X_RayPalette.Services
{
    public class ImageRenderService
    {
        public float Width;
        public float Height;
        public Texture Dimg;
        IntPtr _imgPtr;

        public IntPtr Create(string path)
        {
            if (Dimg != null)
            {
                Program.Renderer.RemoveImGuiBinding(Dimg);
                Dimg.Dispose();
                Dimg = null;
                _imgPtr = IntPtr.Zero;
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
            var img = new ImageSharpTexture(path);
            Dimg = img.CreateDeviceTexture(Program.Gd, Program.Gd.ResourceFactory);
            img = null;
            Width = Dimg.Width;
            Height = Dimg.Height;
            _imgPtr = Program.Renderer.GetOrCreateImGuiBinding(Program.Gd.ResourceFactory, Dimg); //saves file - returns the intPtr need for Imgui.Image()
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Dimg.Dispose();
            return _imgPtr;
        }
    }
}