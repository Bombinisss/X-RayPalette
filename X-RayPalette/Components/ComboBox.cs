using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_RayPalette.Components
{
    public class ComboBox<T> : IComponent
    {
        private List<T> _items;
        private int? _width = null;
        private string _label;
        private T _selectedItem;
        private Action<T> _onSelect;
        private Action<T,T> _onValueChange;
        public ComboBox(T dataSource, string label = "", List<T> items = null)
        {
            _label = label;
            if (items == null)
                items = new List<T>();

            _selectedItem = dataSource;
            _items = items;
        }
        public ComboBox<T> Label(string label)
        {
            _label = label;
            return this;
        }
        public ComboBox<T> Items(List<T> items)
        {
            _items = items;
            return this;
        }
        public ComboBox<T> Width(int width)
        {
            _width = width;
            return this;
        }
        public ComboBox<T> DataSource(T selectedItem)
        {
            _selectedItem = selectedItem;
            return this;
        }
        public ComboBox<T> OnSelect(Action<T> onSelect)
        {
            _onSelect = onSelect;
            return this;
        }
        public ComboBox<T> AddItems(List<T> items)
        {
            _items.AddRange(items);
            return this;
        }
        public ComboBox<T> OnValueChange(Action<T,T> action)
        {
            _onValueChange = action;
            return this;
        }
        public bool Render()
        {
            if (_width.HasValue)
                ImGui.PushItemWidth(_width.Value);
            if (ImGui.BeginCombo(_label, _selectedItem?.ToString()))
            {
                foreach (var item in _items)
                {
                    var itemLabel = item.ToString();
                    if (ImGui.Selectable(itemLabel))
                    {

                        if (_selectedItem == null || !_selectedItem.Equals(item))
                        {
                            if (_onValueChange != null)
                            {
                                _onValueChange(_selectedItem,item);
                            }
                        }

                        _selectedItem = item;
                        if (_onSelect != null)
                            _onSelect(_selectedItem);
                    }
                }
                ImGui.EndCombo();
            }
            if (_width.HasValue)
                ImGui.PopItemWidth();
            return true;
        }
    }
}
