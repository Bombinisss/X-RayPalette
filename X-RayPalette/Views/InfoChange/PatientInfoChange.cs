using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_RayPalette.Views.InfoChange
{
    public class PatientInfoChange : View
    {
        private string _tempdataPatientCi;
        public PatientInfoChange()
        {
            _tempdataPatientCi = "Choose Patient";
        }
        public override void Back()
        {
            _tempdataPatientCi = "Choose Patient";
            OnBackEvent();
        }
        public override void Render(bool isAdmin)
        {
            ImGui.Text("Choose:");
            ImGui.SameLine();
            string[] temp2 = { "patient1", "patient2" };
            if (ImGui.BeginCombo("##PatientInfoChange##", _tempdataPatientCi))
            {
                foreach (var patient in temp2)
                {
                    if (ImGui.Selectable(patient))
                    {
                        _tempdataPatientCi = patient;
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.Separator();
            ImGui.Text("when DB will work there will be shown chosen patient's information with change function");
            ImGui.Separator();
            if (ImGui.Button("Confirm changes"))
            {
                //TODO: replacing changed values in the database
                Back();
            }
        }
    }
}
