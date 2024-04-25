using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_RayPalette.Views.Patient
{
    public class PatientAssignment : View
    {
        private string _tempdataPatientEp;
        private string _tempdataDocEp;
        public PatientAssignment()
        {
            _tempdataPatientEp = "Choose Patient";
            _tempdataDocEp = "Choose Doctor";
        }
        public override void Back()
        {
            _tempdataPatientEp = "Choose Patient";
            _tempdataDocEp = "Choose Doctor";
            OnBackEvent();
        }

        public override void Render(bool isAdmin)
        {
            ImGui.PushItemWidth(150);
            ImGui.Text("Select Patient");
            ImGui.SameLine(245);
            ImGui.Text("Select new Doctor");
            string[] temp1 = { "patient1", "patient2" };
            if (ImGui.BeginCombo("##PatientChange##", _tempdataPatientEp))
            {
                foreach (var patient in temp1)
                {
                    if (ImGui.Selectable(patient))
                    {
                        _tempdataPatientEp = patient;
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.SameLine();
            ImGui.Text("assigne to");
            ImGui.SameLine();
            string[] temp2 = { "dr1", "dr2" };
            if (ImGui.BeginCombo("##DocChange##", _tempdataDocEp))
            {
                foreach (var doc in temp2)
                {
                    if (ImGui.Selectable(doc))
                    {
                        _tempdataDocEp = doc;
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.Separator();
            if (ImGui.Button("Change assigment"))
            {

                //TODO: delete previous patient doc assigment and add to new doc
                Back();
            }
            ImGui.PopItemWidth();
        }
    }
}
