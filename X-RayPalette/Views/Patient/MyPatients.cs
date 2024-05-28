using ImGuiNET;
using MySql.Data.MySqlClient;
using System.Numerics;
using System.Text;
using X_RayPalette.Helpers;

namespace X_RayPalette.Views.InfoChange
{
    public class MyPatients : View
    {
        private string _tempdataPatientCi;
        private string _search;
        private string _tempSearch;
        private string _selectedPesel;
        List<byte[][]> allData;

        public MyPatients()
        {
            _tempdataPatientCi = "Choose Patient";
            _search = "";
            _tempSearch = "";
            _selectedPesel = "";
            allData = new List<byte[][]>();
            MySqlDataReader allReader;
            int id = Globals.LoggedDocID;
            allReader = Program.dbService.ExecuteFromSql($"SELECT * FROM patient WHERE doctors_id LIKE '%{id}%'");
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
            OnBackEvent();
        }

        int _allCount = Convert.ToInt32(Program.dbService.ExecuteScalar("Select count(PESEL) from patient"));

        public override void Render(bool isAdmin)
        {
            ImGui.Text("Search: ");
            ImGui.SameLine(80);
            ImGui.InputText("##Search##", ref _search, 128);

            MySqlDataReader allReader;

            string GetGender(string search)
            {
                return search.Equals("Male", StringComparison.OrdinalIgnoreCase) ? "1" :
                    search.Equals("Female", StringComparison.OrdinalIgnoreCase) ? "2" : "";
            }

            if (_tempSearch != _search)
            {
                if (string.IsNullOrEmpty(_search))
                {
                    int id = Globals.LoggedDocID;
                    allReader = Program.dbService.ExecuteFromSql($"SELECT * FROM patient WHERE doctors_id LIKE '%{id}%'");
                    
                    allData = new List<byte[][]>();
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
                }
                else
                {
                    string gender = GetGender(_search);
                    int id = Globals.LoggedDocID;
                    string query =
                        $"SELECT * FROM patient WHERE Pesel LIKE '%{_search}%' OR first_name LIKE '%{_search}%' OR Sur_name LIKE '%{_search}%' OR sex LIKE '{gender}' OR doctors_id = {id} OR email LIKE '%{_search}%' OR phone LIKE '%{_search}%' OR city LIKE '%{_search}%' OR street LIKE '%{_search}%' OR country LIKE '%{_search}%';";
                    allReader = Program.dbService.ExecuteFromSql(query);
                
                    allData = new List<byte[][]>();
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
                }

                allReader.Close();
                
                _tempSearch = _search;
            }

            ImGui.Separator();

            if (ImGui.BeginTable("allTable", 2, ImGuiTableFlags.Resizable))
            {
                SetupTableColumns();
                ImGui.TableHeadersRow();

                for (int rowIndex = 0; rowIndex < allData.Count; rowIndex++)
                {
                    byte[][] row = allData[rowIndex];

                    // Begin a new row
                    ImGui.TableNextRow();

                    for (int columnIndex = 0; columnIndex < 2; columnIndex++)
                    {
                        ImGui.TableSetColumnIndex(columnIndex);

                        // Display table data
                        string result = Encoding.ASCII.GetString(row[columnIndex + 1]);

                        // Position the selectable area on top of the text
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 10); // Adjust X position if needed
                        ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 5); // Adjust Y position if needed

                        // Unique ID for each selectable item
                        string itemId = $"##Row{rowIndex}Col{columnIndex}##";

                        // Allow selecting rows
                        if (ImGui.Selectable(itemId, _selectedPesel == Encoding.ASCII.GetString(row[columnIndex]), ImGuiSelectableFlags.SpanAllColumns))
                        {
                            _selectedPesel = Encoding.ASCII.GetString(row[columnIndex]);
                            // Perform actions when a row is selected...
                        }

                        // Display text
                        ImGui.SameLine();
                        ImGui.Text(result);
                        ImGui.Separator();
                        
                        ImGui.NextColumn();
                    }
                }

                ImGui.EndTable();
            }
        }

        private void SetupTableColumns()
        {
            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.NoHeaderWidth, 3.5f);
            ImGui.TableSetupColumn("Surname", ImGuiTableColumnFlags.NoHeaderWidth, 3.5f);
        }
    }
}