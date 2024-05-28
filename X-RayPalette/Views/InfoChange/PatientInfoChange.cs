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
        List<byte[][]> allData;

        public PatientInfoChange()
        {
            _tempdataPatientCi = "Choose Patient";
            _search = "";
            allData = new List<byte[][]>();
            MySqlDataReader allReader;
            allReader = Program.dbService.ExecuteFromSql("SELECT * FROM patient");
            while (allReader.Read())
            {
                byte[][] row = new byte[13][];
                for (int i = 0; i < 13; i++)
                {
                    string cellValue = allReader.GetValue(i).ToString();
                    row[i] = new byte[256];
                    byte[] valueBytes = Encoding.UTF8.GetBytes(cellValue);
                    Array.Copy(valueBytes, row[i], Math.Min(valueBytes.Length, row[i].Length));
                }

                allData.Add(row);
            }
            
            allReader.Close();
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

            string GetGender(string search)
            {
                return search.Equals("Male", StringComparison.OrdinalIgnoreCase) ? "1" :
                    search.Equals("Female", StringComparison.OrdinalIgnoreCase) ? "2" : "";
            }

            if (string.IsNullOrEmpty(_search))
            {
                allReader = Program.dbService.ExecuteFromSql("SELECT * FROM patient");
            }
            else
            {
                string gender = GetGender(_search);
                string query =
                    $"SELECT * FROM patient WHERE Pesel LIKE '%{_search}%' OR first_name LIKE '%{_search}%' OR Sur_name LIKE '%{_search}%' OR sex LIKE '{gender}' OR doctors_id LIKE '%{_search}%' OR email LIKE '%{_search}%' OR phone LIKE '%{_search}%' OR city LIKE '%{_search}%' OR street LIKE '%{_search}%' OR country LIKE '%{_search}%';";
                allReader = Program.dbService.ExecuteFromSql(query);
            }

            allReader.Close();

            ImGui.Separator();

            if (ImGui.BeginTable("allTable", 13))
            {
                SetupTableColumns();
                ImGui.TableHeadersRow();

                for (int rowIndex = 0; rowIndex < allData.Count; rowIndex++)
                {
                    byte[][] row = allData[rowIndex];
                    ImGui.TableNextRow();
                    for (int columnIndex = 0; columnIndex < 13; columnIndex++)
                    {
                        ImGui.TableSetColumnIndex(columnIndex);
                        ImGui.Separator();

                        if (columnIndex == 3)
                        {
                            RenderGenderCell(row[columnIndex]);
                        }
                        else
                        {
                            string label = $"##Label{rowIndex}{columnIndex}##";
                            if (ImGui.InputText(label, row[columnIndex], (uint)row[columnIndex].Length))
                            {
                                // Update cell value if it changes
                                byte[] updatedValue =
                                    row[columnIndex].Where(b => b != 0).ToArray(); // Remove trailing zeros
                                Array.Copy(updatedValue, row[columnIndex], updatedValue.Length);
                            }
                        }

                        ImGui.NextColumn();
                    }
                }

                ImGui.EndTable();
            }

            ImGui.Separator();
            new Button("Confirm changes").OnClick(() => SaveChanges(allData)).Render();
        }

        private void SetupTableColumns()
        {
            ImGui.TableSetupColumn("Pesel", ImGuiTableColumnFlags.NoHeaderWidth, 3);
            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.NoHeaderWidth, 3.5f);
            ImGui.TableSetupColumn("Surname", ImGuiTableColumnFlags.NoHeaderWidth, 3.5f);
            ImGui.TableSetupColumn("Sex", ImGuiTableColumnFlags.NoHeaderWidth, 1);
            ImGui.TableSetupColumn("Doctor", ImGuiTableColumnFlags.NoHeaderWidth, 1);
            ImGui.TableSetupColumn("Email", ImGuiTableColumnFlags.NoHeaderWidth, 3.5f);
            ImGui.TableSetupColumn("Phone", ImGuiTableColumnFlags.NoHeaderWidth, 2);
            ImGui.TableSetupColumn("City", ImGuiTableColumnFlags.NoHeaderWidth, 2.5f);
            ImGui.TableSetupColumn("Street", ImGuiTableColumnFlags.NoHeaderWidth, 2.5f);
            ImGui.TableSetupColumn("House number", ImGuiTableColumnFlags.NoHeaderWidth, 2);
            ImGui.TableSetupColumn("Flat number", ImGuiTableColumnFlags.NoHeaderWidth, 2);
            ImGui.TableSetupColumn("Postal code", ImGuiTableColumnFlags.NoHeaderWidth, 2);
            ImGui.TableSetupColumn("Country", ImGuiTableColumnFlags.NoHeaderWidth, 2);
        }

        private void RenderGenderCell(byte[] genderValueBytes)
        {
            string genderValue = Encoding.UTF8.GetString(genderValueBytes).TrimEnd('\0');
            string gender = genderValue == "1" ? "Male" : genderValue == "2" ? "Female" : string.Empty;
            ImGui.Text(gender);
        }

        private void SaveChanges(List<byte[][]> allData)
        {
            foreach (var row in allData)
            {
                string[] rowValues = row.Select(cell => Encoding.UTF8.GetString(cell).TrimEnd('\0')).ToArray();

                // Construct the SQL UPDATE statement using the values from the row
                string updateQuery = $@"
            UPDATE patient 
            SET first_name = '{rowValues[1]}',
                sur_name = '{rowValues[2]}',
                sex = '{rowValues[3]}',
                doctors_id = '{rowValues[4]}',
                email = '{rowValues[5]}',
                phone = '{rowValues[6]}',
                City = '{rowValues[7]}',
                Street = '{rowValues[8]}',
                House_number = '{rowValues[9]}',
                Flat_number = '{rowValues[10]}',
                Post_code = '{rowValues[11]}',
                Country = '{rowValues[12]}'
            WHERE pesel = '{rowValues[0]}';
        ";

                try
                {
                    // Execute the update query
                    Program.dbService.ExecuteNonQuery(updateQuery);
                }
                catch (MySqlException ex)
                {
                    // Log the exception or display an error message
                    Console.WriteLine($"Error updating patient data for PESEL {rowValues[0]}: {ex.Message}");
                }
            }

            // Clear and reload the data from the database after saving changes
            allData.Clear();
            MySqlDataReader allReader = Program.dbService.ExecuteFromSql("SELECT * FROM patient");
            while (allReader.Read())
            {
                byte[][] row = new byte[13][];
                for (int i = 0; i < 13; i++)
                {
                    string cellValue = allReader.GetValue(i).ToString();
                    row[i] = new byte[256];
                    byte[] valueBytes = Encoding.UTF8.GetBytes(cellValue);
                    Array.Copy(valueBytes, row[i], Math.Min(valueBytes.Length, row[i].Length));
                }

                allData.Add(row);
            }
            allReader.Close();
        }
    }
}