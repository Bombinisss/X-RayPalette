﻿using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using NativeFileDialogExtendedSharp;
using Veldrid.ImageSharp;
using System.Drawing;
using System.Drawing.Imaging;
using X_RayPalette.Helpers;
using X_RayPalette.Services;

namespace X_RayPalette
{
    public static class Program
    {
        private static Sdl2Window _window;
        // TO DO: set config in separate file
        public static readonly DatabaseService dbService = new DatabaseService("81.171.31.232", "pracowaniartg", "RTG_ordynator", "RTG_ordynator1", 3306, true); // creating connection to database;
        public static GraphicsDevice Gd;
        private static CommandList _cl;
        public static ImGuiRenderer Renderer;
        private static readonly Vector3 ClearColor = new(0.0f, 0.0f, 0.0f);
        private static readonly KeyUpdater KeyUpdater = new();

        private static void Main(string[] args)
        {
            // Create window, GraphicsDevice, and all resources necessary for the demo.
            VeldridStartup.CreateWindowAndGraphicsDevice(
                new WindowCreateInfo(50, 50, 400, 150, WindowState.Normal, "Med App"),
                new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
                out _window,
                out Gd);
            _window.Resized += () =>
            {
                Gd.MainSwapchain.Resize((uint)_window.Width, (uint)_window.Height);
                Renderer.WindowResized(_window.Width, _window.Height);
            };
            _cl = Gd.ResourceFactory.CreateCommandList();
            Renderer = new ImGuiRenderer(Gd, Gd.MainSwapchain.Framebuffer.OutputDescription, _window.Width,
                _window.Height);

            var stopwatch = Stopwatch.StartNew();

            // Main application loop

            var guiObject = new Gui(_window);
            _window.DragDrop += (dragDropEvent) =>
            {
                if (guiObject.DevOpen)
                {
                    Console.WriteLine(dragDropEvent.File); //printing path to dropped file
                    guiObject.ImagePathExist = true;
                    guiObject.Path = dragDropEvent.File;
                    guiObject.ImageHandler = ImageIntPtr.CreateImgPtr(guiObject.Path);
                    guiObject.ConvertButton = false;
                }

            };
            ImGui.StyleColorsDark();
            Gui.SetupImGuiStyle0();
            DarkTitleBarClass.UseImmersiveDarkMode(_window.Handle, false, 0x00FFFFFF);

            while (_window.Exists)
            {
                var deltaTime = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency;
                stopwatch.Restart();
                var snapshot = _window.PumpEvents();
                if (!_window.Exists) break;
                Renderer.Update(deltaTime,
                    snapshot); // Feed the input events to our ImGui controller, which passes them through to ImGui.
                KeyUpdater.UpdateImGuiInput(snapshot);

                // ImGui window position and size
                var viewport = ImGui.GetMainViewport();
                ImGui.SetNextWindowPos(viewport.WorkPos);
                ImGui.SetNextWindowSize(viewport.WorkSize);
                guiObject.SubmitUi();

                _cl.Begin();
                _cl.SetFramebuffer(Gd.MainSwapchain.Framebuffer);
                _cl.ClearColorTarget(0, new RgbaFloat(ClearColor.X, ClearColor.Y, ClearColor.Z, 1f));
                Renderer.Render(Gd, _cl);
                _cl.End();
                Gd.SubmitCommands(_cl);
                Gd.SwapBuffers(Gd.MainSwapchain);
                if (guiObject.LoggedOut)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    guiObject = new Gui(_window);
                }
            }

            // Clean up Veldrid resources
            Gd.WaitForIdle();
            Renderer.Dispose();
            _cl.Dispose();
            Gd.Dispose();
        }
    }

    internal static class DarkTitleBarClass
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DwmwaUseImmersiveDarkModeBefore20H1 = 19;
        private const int DwmwaUseImmersiveDarkMode = 20;
        private const int DwmwaBorderColor = 34;
        private const int DwmwaCaptionColor = 35;

        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled, int darkColor)
        {
            if (!IsWindows10OrGreater(17763)) return false;
            var result = false;
            var attribute = DwmwaUseImmersiveDarkModeBefore20H1;
            if (IsWindows10OrGreater(19045))
            {
                attribute = DwmwaUseImmersiveDarkMode;
            }
            if (IsWindows10OrGreater(22000))
            {
                enabled = true;
            }

            var useImmersiveDarkMode = enabled ? 1 : 0;
            result = DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;

            result = DwmSetWindowAttribute(handle, DwmwaBorderColor | DwmwaCaptionColor, ref darkColor,
                sizeof(int)) == 0;

            return result;

        }

        public static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }
    }

    public class KeyUpdater
    {
        public void UpdateImGuiInput(InputSnapshot snapshot) // Feed missing input events to our ImGui controller.
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.AddMousePosEvent(snapshot.MousePosition.X, snapshot.MousePosition.Y);
            io.AddMouseButtonEvent(0, snapshot.IsMouseDown(MouseButton.Left));
            io.AddMouseButtonEvent(1, snapshot.IsMouseDown(MouseButton.Right));
            io.AddMouseButtonEvent(2, snapshot.IsMouseDown(MouseButton.Middle));
            io.AddMouseButtonEvent(3, snapshot.IsMouseDown(MouseButton.Button1));
            io.AddMouseButtonEvent(4, snapshot.IsMouseDown(MouseButton.Button2));
            io.AddMouseWheelEvent(0f, snapshot.WheelDelta);

            foreach (var keyEvent in snapshot.KeyEvents)
            {
                if (TryMapKey(keyEvent.Key, out var imguiKey))
                {
                    io.AddKeyEvent(imguiKey, keyEvent.Down);
                }
            }
        }

        private bool TryMapKey(Key key, out ImGuiKey result)
        {
            ImGuiKey KeyToImGuiKeyShortcut(Key keyToConvert, Key startKey1, ImGuiKey startKey2)
            {
                int changeFromStart1 = (int)keyToConvert - (int)startKey1;
                return startKey2 + changeFromStart1;
            }

            result = key switch
            {
                >= Key.F1 and <= Key.F24 => KeyToImGuiKeyShortcut(key, Key.F1, ImGuiKey.F1),
                >= Key.Keypad0 and <= Key.Keypad9 => KeyToImGuiKeyShortcut(key, Key.Keypad0, ImGuiKey.Keypad0),
                >= Key.A and <= Key.Z => KeyToImGuiKeyShortcut(key, Key.A, ImGuiKey.A),
                >= Key.Number0 and <= Key.Number9 => KeyToImGuiKeyShortcut(key, Key.Number0, ImGuiKey._0),
                Key.ShiftLeft or Key.ShiftRight => ImGuiKey.ModShift,
                Key.ControlLeft or Key.ControlRight => ImGuiKey.ModCtrl,
                Key.AltLeft or Key.AltRight => ImGuiKey.ModAlt,
                Key.WinLeft or Key.WinRight => ImGuiKey.ModSuper,
                Key.Menu => ImGuiKey.Menu,
                Key.Up => ImGuiKey.UpArrow,
                Key.Down => ImGuiKey.DownArrow,
                Key.Left => ImGuiKey.LeftArrow,
                Key.Right => ImGuiKey.RightArrow,
                Key.Enter => ImGuiKey.Enter,
                Key.Escape => ImGuiKey.Escape,
                Key.Space => ImGuiKey.Space,
                Key.Tab => ImGuiKey.Tab,
                Key.BackSpace => ImGuiKey.Backspace,
                Key.Insert => ImGuiKey.Insert,
                Key.Delete => ImGuiKey.Delete,
                Key.PageUp => ImGuiKey.PageUp,
                Key.PageDown => ImGuiKey.PageDown,
                Key.Home => ImGuiKey.Home,
                Key.End => ImGuiKey.End,
                Key.CapsLock => ImGuiKey.CapsLock,
                Key.ScrollLock => ImGuiKey.ScrollLock,
                Key.PrintScreen => ImGuiKey.PrintScreen,
                Key.Pause => ImGuiKey.Pause,
                Key.NumLock => ImGuiKey.NumLock,
                Key.KeypadDivide => ImGuiKey.KeypadDivide,
                Key.KeypadMultiply => ImGuiKey.KeypadMultiply,
                Key.KeypadSubtract => ImGuiKey.KeypadSubtract,
                Key.KeypadAdd => ImGuiKey.KeypadAdd,
                Key.KeypadDecimal => ImGuiKey.KeypadDecimal,
                Key.KeypadEnter => ImGuiKey.KeypadEnter,
                Key.Tilde => ImGuiKey.GraveAccent,
                Key.Minus => ImGuiKey.Minus,
                Key.Plus => ImGuiKey.Equal,
                Key.BracketLeft => ImGuiKey.LeftBracket,
                Key.BracketRight => ImGuiKey.RightBracket,
                Key.Semicolon => ImGuiKey.Semicolon,
                Key.Quote => ImGuiKey.Apostrophe,
                Key.Comma => ImGuiKey.Comma,
                Key.Period => ImGuiKey.Period,
                Key.Slash => ImGuiKey.Slash,
                Key.BackSlash or Key.NonUSBackSlash => ImGuiKey.Backslash,
                _ => ImGuiKey.None
            };

            return result != ImGuiKey.None;
        }

    }
    public class ImageIntPtr
    {
        public static float Width;
        public static float Height;
        public static float WidthOut;
        public static float HeightOut;
        public static float WidthLoading;
        public static float HeightLoading;
        public static Texture Dimg;
        public static Texture DimgOut;
        public static Texture DimgLoading;
        static IntPtr _imgPtr;
        static IntPtr _imgPtrOut;
        static IntPtr _imgPtrLoading;

        public static IntPtr CreateImgPtr(string path)
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
        public static IntPtr CreateImgPtrOut(string path)
        {
            if (DimgOut != null)
            {
                Program.Renderer.RemoveImGuiBinding(DimgOut);
                DimgOut.Dispose();
                DimgOut = null;
                _imgPtrOut = IntPtr.Zero;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            var img = new ImageSharpTexture(path);
            DimgOut = img.CreateDeviceTexture(Program.Gd, Program.Gd.ResourceFactory);
            img = null;
            WidthOut = DimgOut.Width;
            HeightOut = DimgOut.Height;
            _imgPtrOut = Program.Renderer.GetOrCreateImGuiBinding(Program.Gd.ResourceFactory, DimgOut); //saves file - returns the intPtr need for Imgui.Image()
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //dimgOut.Dispose();
            return _imgPtrOut;
        }
        public static IntPtr CreateImgPtrLoading(string path)
        {
            if (DimgLoading != null)
            {
                Program.Renderer.RemoveImGuiBinding(DimgLoading);
                DimgLoading.Dispose();
                DimgLoading = null;
                _imgPtrLoading = IntPtr.Zero;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            var img = new ImageSharpTexture(path);
            DimgLoading = img.CreateDeviceTexture(Program.Gd, Program.Gd.ResourceFactory);
            img = null;
            WidthLoading = DimgLoading.Width;
            HeightLoading = DimgLoading.Height;
            _imgPtrLoading = Program.Renderer.GetOrCreateImGuiBinding(Program.Gd.ResourceFactory, DimgLoading); //saves file - returns the intPtr need for Imgui.Image()
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DimgLoading.Dispose();
            return _imgPtrLoading;
        }
    }
    internal static class Filters
    {
        public static IEnumerable<NfdFilter> CreateNewNfdFilter()
        {
            var filter = new NfdFilter
            {
                Description = "Images",
                Specification = "png,jpg,bmp"
            };
            yield return filter;
        }
    }

    public static class ColorChanger
    {
        public static bool WorkerEnd;
        public static void Worker(string inputPath, int mode)
        {
            WorkerEnd = false;
            ConvertToLongRainbow(inputPath, ImagePathHelper.ImagesFolderPath() + "\\output.png", mode);
        }

        static void ConvertToLongRainbow(string inputPath, string outputPath, int mode)
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
                        Color rainbowColor = Color.FromArgb(0, 0, 0);
                        if (mode == 0) { rainbowColor = Pm3DColor(grayscaleValue); }
                        else { rainbowColor = LongRainbowColor(grayscaleValue); }
                        outputBitmap.SetPixel(x, y, rainbowColor);
                    }
                }
                outputBitmap.Save(outputPath, ImageFormat.Png);
                Gui.ImageHandlerOut = ImageIntPtr.CreateImgPtrOut(ImagePathHelper.ImagesFolderPath() + "\\output.png");
                WorkerEnd = true;
            }
        }

        static Color LongRainbowColor(int value)
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

        static Color Pm3DColor(int value)
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