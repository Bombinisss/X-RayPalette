using ImGuiNET;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Text;
using System.Formats.Asn1;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Components;

namespace X_RayPalette.Views.InfoChange
{
    public class PatientInfoChange : View
    {

        private string _tempdataPatientCi;
        private string _search;
        public PatientInfoChange()
        {
            _tempdataPatientCi = "Choose Patient";
            _search = "";
        }
        public override void Back()
        {
            _tempdataPatientCi = "Choose Patient";
            OnBackEvent();
        }
        int _allCount = Convert.ToInt32(Program.dbService.ExecuteScalar("Select count(PESEL) from patient"));

        public override void Render(bool isAdmin)
        {
            ImGui.Text("Search: ");
            ImGui.SameLine(80);
            ImGui.InputText("##Search##", ref _search, 128);
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");
            MySqlDataReader allReader;

         string whatGender(string _search)
            {
                if (_search == "Male" || _search == "male")
                    return "1";

                if (_search =="Female" || _search =="female")
                    return "2";
                else return "";
            }

            if (_search == "")
            {
                allReader = Program.dbService.ExecuteFromSql("Select * from patient");
            }
            else
            {
                allReader = Program.dbService.ExecuteFromSql("Select * from patient where Pesel like '%" + _search + "%' or first_name like '%" + _search + "%' or Sur_name like '%" + _search + "%' or sex like '"+ whatGender(_search) +"' or doctors_id like '%" + _search + "%' or email like '%" + _search + "%' or phone like '%" + _search + "%' or city like '%" + _search + "%' or street like '%" + _search + "%' or country like '%" + _search + "%';");
            }

            List<dynamic> _allList = new List<dynamic>();
            ImGui.Separator();
            if (ImGui.BeginTable("allTable", 13))

            
            ImGui.TableSetupColumn("Pesel", ImGuiTableColumnFlags.NoHeaderWidth,3);
            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.NoHeaderWidth,3);
            ImGui.TableSetupColumn("Surname", ImGuiTableColumnFlags.NoHeaderWidth, 3);
            ImGui.TableSetupColumn("Sex", ImGuiTableColumnFlags.NoHeaderWidth, 1);
            ImGui.TableSetupColumn("Doctor", ImGuiTableColumnFlags.NoHeaderWidth, 1);
            ImGui.TableSetupColumn("Email", ImGuiTableColumnFlags.NoHeaderWidth, 4);
            ImGui.TableSetupColumn("Phone", ImGuiTableColumnFlags.NoHeaderWidth, 2);
            ImGui.TableSetupColumn("City", ImGuiTableColumnFlags.NoHeaderWidth,2);
            ImGui.TableSetupColumn("Street", ImGuiTableColumnFlags.NoHeaderWidth,2);
            ImGui.TableSetupColumn("House number", ImGuiTableColumnFlags.NoHeaderWidth,1);
            ImGui.TableSetupColumn("Flat number", ImGuiTableColumnFlags.NoHeaderWidth,1);
            ImGui.TableSetupColumn("Postal code", ImGuiTableColumnFlags.NoHeaderWidth,1);
            ImGui.TableSetupColumn("Country", ImGuiTableColumnFlags.NoHeaderWidth,1);
            ImGui.TableHeadersRow();
            {
            while (allReader.Read())
            {
                _allList.Add(allReader.GetValue(0) + " " + allReader.GetValue(1) + " " + allReader.GetValue(2) + " " + allReader.GetValue(3) + " " + allReader.GetValue(4) + " " + allReader.GetValue(5) + " " + allReader.GetValue(6) + " " + allReader.GetValue(7) + " " + allReader.GetValue(8) + " " + allReader.GetValue(9) + " " + allReader.GetValue(10) + " " + allReader.GetValue(11) + " " + allReader.GetValue(12));
                    for (int row = 0; row < 1; row++)
                    {
                        ImGui.TableNextRow();
                        ImGui.Spacing();
                        for (int column = 0; column < 13; column++)
                        {
                            ImGui.TableSetColumnIndex(column);
                            
                            ImGui.Separator();
                            if (column == 3 && Convert.ToString(allReader.GetValue(3)) == "1")
                            {
                                ImGui.Text("Male");
                            }
                            else if (column == 3 && Convert.ToString(allReader.GetValue(3)) == "2")
                            {
                                ImGui.Text("Female");
                            }
                            else
                            ImGui.Text(Convert.ToString(allReader.GetValue(column)));
                            ImGui.NextColumn();
                        }
                    }
            }        
         }   
            allReader.Close();  
            ImGui.EndTable();

            ImGui.Separator();
            new Button("Confirm changes").OnClick(Back).Render();
        }
    }
}
