using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ZstdSharp.Unsafe;

namespace X_RayPalette.Components
{
    public class TextInput : IComponent
    {
        private string _label;
        private uint? _maxLength;
        private bool _isRequired;
        private ImGuiInputTextFlags _inputType;
        private string _dataSource;
        private int? _width;
        private Action<string> _onInput;
        private Action<string, string> _onInputChanged;
        private int? _titleWidth;
        private string _title;
        public TextInput(ref string dataSource, string label)
        {
            _dataSource = dataSource;
            _label = label;
        }
        public TextInput(string dataSource, string label)
        {
            _dataSource = dataSource;
            _label = label;
        }
        public TextInput Width(int width)
        {
            _width = width;
            return this;
        }
        public TextInput Label(string label)
        {
            _label = label;
            return this;
        }
        public TextInput MaxLength(uint maxLength)
        {
            _maxLength = maxLength;
            return this;
        }
        public TextInput IsRequired(bool isRequired=true)
        {
            _isRequired = isRequired;
            return this;
        }
        public TextInput InputType(ImGuiInputTextFlags inputType)
        {
            _inputType = inputType;
            return this;
        }
        public TextInput DataSource(string dataSource)
        {
            _dataSource = dataSource;
            return this;
        }
        public TextInput DataSource(ref string dataSource)
        {
            _dataSource = dataSource;
            return this;
        }
        public TextInput OnInput(Action<string> onInput)
        {
            _onInput = onInput;
            return this;
        }
        
        public TextInput OnInputChanged(Action<string, string> onInputChanged)
        {
            _onInputChanged = onInputChanged;
            return this;
        }
        public TextInput Title(string title, int titleWidth)
        {
            _titleWidth = titleWidth;
            _title = title;
            return this;
        }
        public bool Render()
        {
            if (_width.HasValue)
                ImGui.PushItemWidth(_width.Value);

            if (_title != null && _titleWidth.HasValue)
            {
                ImGui.Text(_title);
                ImGui.SameLine(_titleWidth.Value);
            }
            var oldValue = _dataSource;
            var com = ImGui.InputText(_label,ref _dataSource, _maxLength.HasValue ? _maxLength.Value : 256, _inputType);


            if (_isRequired)
            {
                ImGui.SameLine();
                ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");
            }
            if (_onInput != null)
                _onInput(_dataSource);
            if (oldValue != _dataSource)
            {
                if (_onInputChanged != null)
                    _onInputChanged(_dataSource,oldValue);
            }
            if (_width.HasValue)
                ImGui.PopItemWidth();
            return com;
        }
    }
}
