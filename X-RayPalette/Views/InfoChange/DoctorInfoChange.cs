using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_RayPalette.Views.InfoChange
{
    internal class DoctorInfoChange : View
    {
        private string _tempdataDoc_CI;
        public DoctorInfoChange()
        {
            _tempdataDoc_CI = "Choose Doctor";
        }
        public override void Back()
        {
            _tempdataDoc_CI = "Choose Doctor";
            OnBackEvent();
        }

        public override void Render(bool isAdmin)
        {
            ImGui.Text("Choose:");
            ImGui.SameLine();
            string[] temp2 = { "dr1", "dr2" };
            if (ImGui.BeginCombo("##DocInfoChange##", _tempdataDoc_CI))
            {
                foreach (var doc in temp2)
                {
                    if (ImGui.Selectable(doc))
                    {
                        _tempdataDoc_CI = doc;
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.Separator();
            ImGui.Text("when DB will work there will be shown chosen doctor's informations with change function");
            ImGui.Separator();
            if (ImGui.Button("Confirm changes"))
            {
                //TODO: replacing changed values ​​in the database
                Back();
            }
        }
    }
}
