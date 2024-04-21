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
        private string _tempdataPatient_EP;
        private string _tempdataDoc_EP;
        public PatientAssignment()
        {
            _tempdataPatient_EP = "Choose Patient";
            _tempdataDoc_EP = "Choose Doctor";
        }
        public override void Back()
        {
            _tempdataPatient_EP = "Choose Patient";
            _tempdataDoc_EP = "Choose Doctor";
            OnBackEvent();
        }

        public override void Render(bool isAdmin)
        {
            ImGui.PushItemWidth(150);
            ImGui.Text("Select Patient");
            ImGui.SameLine(245);
            ImGui.Text("Select new Doctor");
            string[] temp1 = { "patient1", "patient2" };
            if (ImGui.BeginCombo("##PatientChange##", _tempdataPatient_EP))
            {
                foreach (var patient in temp1)
                {
                    if (ImGui.Selectable(patient))
                    {
                        _tempdataPatient_EP = patient;
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.SameLine();
            ImGui.Text("assigne to");
            ImGui.SameLine();
            string[] temp2 = { "dr1", "dr2" };
            if (ImGui.BeginCombo("##DocChange##", _tempdataDoc_EP))
            {
                foreach (var doc in temp2)
                {
                    if (ImGui.Selectable(doc))
                    {
                        _tempdataDoc_EP = doc;
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
