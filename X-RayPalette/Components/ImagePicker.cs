using ImGuiNET;
using NativeFileDialogExtendedSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Helpers;

namespace X_RayPalette.Components
{

    public class ImagePicker : IComponent
    {
        private string _label;
        // to do disabling
        private bool _disabled;
        private string _path;
        private int? _width;
        private Action<string> _onPicked;
        private Action<string> _onPickedvalid;
        public ImagePicker(string label = "", bool disabled = false)
        {

            _label = label;
            _disabled = disabled;
        }
        public ImagePicker OnPicked(Action<string> onPicked)
        {
            _onPicked = onPicked;
            return this;
        }
        public ImagePicker OnPickedValid(Action<string> onPickedvalid)
        {
            _onPickedvalid = onPickedvalid;
            return this;
        }
        public ImagePicker Width(int width)
        {
            _width = width;
            return this;
        }
        public ImagePicker Label(string label)
        {
            _label = label;
            return this;
        }
        public bool Render()
        {
            if (_width.HasValue)
                ImGui.PushItemWidth(_width.Value);
            var clicked = ImGui.Button(_label);
            if (clicked)
            {
                NfdDialogResult path = Nfd.FileOpen(InputFilterHelper.NfdFilter(), "C:\\"); //path - selected image path
                                                                                            // Console.WriteLine(path.Path); //check image path
                _onPicked(path.Path);
                if (path.Path != null)
                {
                    _onPickedvalid(path.Path);
                }
            }
            if (_width.HasValue)
                ImGui.PopItemWidth();
            return clicked;
        }
    }
}
