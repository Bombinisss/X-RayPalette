using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
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
        private static readonly ImageRenderService _devRenderer = new ImageRenderService();

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
                    string path = dragDropEvent.File;
                    string extension = path.Substring(path.Length - 3).ToLower();
                    if (extension == "jpg" || extension == "png" || extension == "bmp")
                    {
                        guiObject.ImagePathExist = true;
                        guiObject.Path = dragDropEvent.File;
                        guiObject.ImageHandler = _devRenderer.Create(guiObject.Path);
                        guiObject.ImgSize = new(_devRenderer.Width, _devRenderer.Height);
                        guiObject.ConvertButton = false;
                    }
                    else
                    {
                        guiObject.PathError = true;
                    }

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

        private const int DwmwaBorderColor = 34;
        private const int DwmwaCaptionColor = 35;

        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled, int darkColor)
        {
            if (!EnviromentHelpers.IsSupportedOS(10,17763)) return false;
            
            var result = DwmSetWindowAttribute(handle, DwmwaBorderColor | DwmwaCaptionColor, ref darkColor,
                sizeof(int)) == 0;

            return result;

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
    

}