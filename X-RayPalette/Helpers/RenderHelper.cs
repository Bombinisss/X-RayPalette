using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Components;

namespace X_RayPalette.Helpers
{
    public class RenderHelper
    {
        public static void TextCentered(string header, string b1, string b2, string or, ref bool b1Value, ref bool b2Value)
        {
            var windowWidth = ImGui.GetWindowSize();
            var headerWidth = ImGui.CalcTextSize(header);
            var b1Width = ImGui.CalcTextSize(b1);
            var orWidth = ImGui.CalcTextSize("O");
            var space = ImGui.CalcTextSize(" ");

            ImGui.SetCursorPosX((windowWidth.X - headerWidth.X) * 0.5f);
            ImGui.Text(header);

            ImGui.SetCursorPosX(windowWidth.X * 0.5f - b1Width.X - 2 * orWidth.X - space.X - 3.0f);

           
            if (new Button(b1).Render())
            {
                b1Value = true;
            }
            ImGui.SameLine();
            ImGui.Text(or);
            ImGui.SameLine();
            if (new Button(b2).Render())
            {
                b2Value = true;
            }
        }
    }
}
