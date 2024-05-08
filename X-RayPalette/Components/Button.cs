using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_RayPalette.Components
{
    public class Button : IComponent
    {
        private string _label;
        // to do disabling
        private bool _disabled;
        private int? _width;
        private Action _onClick;
        public Button(string label = "", bool disabled = false)
        {

            _label = label;
            _disabled = disabled;
        }
        public Button OnClick(Action onClick)
        {
            _onClick = onClick;
            return this;
        }
        public Button Width(int width)
        {
            _width = width;
            return this;
        }
        public Button Label(string label)
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
                if (_onClick != null)
                    _onClick();
            }
            if (_width.HasValue)
                ImGui.PopItemWidth();
            return clicked;
        }
    }
}
