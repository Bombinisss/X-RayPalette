using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Components;

namespace X_RayPalette.Views.Patient
{
    internal class PatientAssigmentWrapper : View
    {
        public override void Back()
        {
            OnBackEvent();
        }

        public override void Render(bool isAdmin)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.0f, 0.749f, 1.0f, 0.20f));
            new Button("< back").OnClick(Back).Render();
            ImGui.PopStyleColor();
            ImGui.Separator();
        }
    }
}
