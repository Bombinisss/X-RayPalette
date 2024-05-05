using ImGuiNET;
using MySql.Data.MySqlClient;
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


            List<string> _nameArrayPat = new List<string>();
            MySqlDataReader reader = Program.dbService.ExecuteFromSql("Select pesel, first_name, sur_name from patient");
            while (reader.Read())
            {
                _nameArrayPat.Add(reader.GetInt32(0) + " " + reader.GetString(1) + " " + reader.GetString(2));
            }
            _nameArrayPat.ToArray();
            reader.Close();
            if (ImGui.BeginCombo("##PatientChange##", _tempdataPatientEp))
            {
                foreach (var patient in _nameArrayPat)
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
            List<string> _nameArrayDoc = new List<string>();
            MySqlDataReader readerDoc = Program.dbService.ExecuteFromSql("Select doctors_id, first_name, sur_name from doctors");
            while (readerDoc.Read())
            {
                _nameArrayDoc.Add(readerDoc.GetInt32(0) + " " + readerDoc.GetString(1) + " " + readerDoc.GetString(2));
            }
            _nameArrayDoc.ToArray();
            readerDoc.Close();
            if (ImGui.BeginCombo("##DocChange##", _tempdataDocEp))
            {
                foreach (var doc in _nameArrayDoc)
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
                string[] InfSelectedDoc = _tempdataDocEp.Split(' ');
                int SelectedDocId = Convert.ToInt32(InfSelectedDoc[0]);

                string[] InfSelectedPat = _tempdataPatientEp.Split(' ');
                string SelectedPatPesel = InfSelectedPat[0];
                var res = Program.dbService.ExecuteNonQuery("Update patient Set doctors_id = '" + SelectedDocId + "' where pesel='" + SelectedPatPesel + "'");
                Back();
            }
            ImGui.PopItemWidth();
        }
    }
}
