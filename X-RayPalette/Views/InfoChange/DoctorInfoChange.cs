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
        private string _tempdataDocCi;
        public DoctorInfoChange()
        {
            _tempdataDocCi = "Choose Doctor";
        }
        public override void Back()
        {
            _tempdataDocCi = "Choose Doctor";
            OnBackEvent();
        }

        public override void Render(bool isAdmin)
        {
            ImGui.Text("Choose:");
            ImGui.SameLine();
            string[] temp2 = { "dr1", "dr2" };
            if (ImGui.BeginCombo("##DocInfoChange##", _tempdataDocCi))
            {
                foreach (var doc in temp2)
                {
                    if (ImGui.Selectable(doc))
                    {
                        _tempdataDocCi = doc;
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.Separator();
            ImGui.Text("when DB will work there will be shown chosen doctor's information with change function");
            ImGui.Separator();
            if (ImGui.Button("Confirm changes"))
            {
                //TODO: replacing changed values in the database
                Back();
            }
        }
    }
}
