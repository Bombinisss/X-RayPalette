using ImGuiNET;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
            ImGui.Text("Search using PESEL: ");
            ImGui.SameLine(110);
            ImGui.InputText("##Search##", ref _search, 128);
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");
            MySqlDataReader allReader = Program.dbService.ExecuteFromSql("Select * from patient");

            List<dynamic> _allList = new List<dynamic>();
        if (ImGui.BeginTable("allTable", 13))
         {
            while (allReader.Read())
            {
                _allList.Add(allReader.GetValue(0) + " " + allReader.GetValue(1) + " " + allReader.GetValue(2) + " " + allReader.GetValue(3) + " " + allReader.GetValue(4) + " " + allReader.GetValue(5) + " " + allReader.GetValue(6) + " " + allReader.GetValue(7) + " " + allReader.GetValue(8) + " " + allReader.GetValue(9) + " " + allReader.GetValue(10) + " " + allReader.GetValue(11) + " " + allReader.GetValue(12));
                    for (int row = 0; row < 1; row++)
                    {
                        ImGui.TableSetupColumn("Pesel");
                        ImGui.TableSetupColumn("Name");
                        ImGui.TableSetupColumn("Surname");
                        ImGui.TableSetupColumn("Sex");
                        ImGui.TableSetupColumn("Doctor");
                        ImGui.TableSetupColumn("Email");
                        ImGui.TableSetupColumn("Phone");
                        ImGui.TableSetupColumn("City");
                        ImGui.TableSetupColumn("Street");
                        ImGui.TableSetupColumn("House number");
                        ImGui.TableSetupColumn("Flat number");
                        ImGui.TableSetupColumn("Postal code");
                        ImGui.TableSetupColumn("Country");
                        ImGui.TableNextRow();

                        for (int column = 0; column < 13; column++)
                        {
                            ImGui.TableSetColumnIndex(column);
                            ImGui.Text(Convert.ToString(allReader.GetValue(column)));
                        }
                    }
                }
                           
            }
            ImGui.EndTable();


            allReader.Close();  

                ImGui.Separator();

            new Button("Confirm changes").OnClick(Back).Render();
        }
    }
}
