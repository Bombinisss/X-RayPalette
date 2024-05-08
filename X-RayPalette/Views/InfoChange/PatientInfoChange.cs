using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Components;

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
            new ComboBox<string>(_tempdataPatientCi,"##PatientInfoChange##", temp2.ToList()).OnSelect((string val) =>
            {
                _tempdataPatientCi = val;
            }).Render();

            ImGui.Separator();
            ImGui.Text("when DB will work there will be shown chosen patient's information with change function");
            ImGui.Separator();

            new Button("Confirm changes").OnClick(Back).Render();
        }
    }
}
