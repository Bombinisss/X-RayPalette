using ImGuiNET;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using NativeFileDialogExtendedSharp;
using Org.BouncyCastle.Utilities;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text;
using Vulkan;
using X_RayPalette.Components;
using X_RayPalette.Helpers;
using X_RayPalette.Services;

namespace X_RayPalette.Views.InfoChange
{
    public class MyPatients : View
    {
        private string _tempdataPatientCi;
        private string _search;
        private string _tempSearch;
        private string _selectedPesel;
        byte[][] _selectedPatient = new byte[13][];
        List<string[]> _selectedPatientImages = new List<string[]>();
        List<byte[][]> allData;

        public Vector2 ImgSize;
        public string ImgPath;
        public IntPtr ImageHandler;
        private readonly ImageRenderService _imageRender;
        public bool ConvertButton;
        public bool ImageImgPathExist;
        public bool ImgPathError;

        public MyPatients()
        {
            _tempdataPatientCi = "Choose Patient";
            _search = "";
            _tempSearch = " ";
            _selectedPesel = "";
            allData = new List<byte[][]>();
            _imageRender = new ImageRenderService();
        }

        public override void Back()
        {
            OnBackEvent();
        }

        int _allCount = Convert.ToInt32(Program.dbService.ExecuteScalar("Select count(PESEL) from patient"));

        public override void Render(bool isAdmin)
        {
            var windowSize= ImGui.GetContentRegionAvail();

            ImGui.BeginChild("v_list", new Vector2(windowSize.X * 0.4f -4, 0));
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
                            _selectedPesel = Encoding.ASCII.GetString(row[columnIndex]).TrimEnd('\0');

                            string query = $"SELECT * FROM patient WHERE Pesel = '{_selectedPesel.ToString()}'";
                            allReader = Program.dbService.ExecuteFromSql(query);

                           
                            while (allReader.Read())
                            {
                               
                                for (int i = 0; i < 13; i++)
                                {
                                    string cellValue = allReader.GetValue(i).ToString();
                                    _selectedPatient[i] = new byte[256];
                                    byte[] valueBytes = Encoding.UTF8.GetBytes(cellValue);
                                    Array.Copy(valueBytes, _selectedPatient[i], Math.Min(valueBytes.Length, _selectedPatient[i].Length));
                                }
                            }
                            allReader.Close();
                            // Perform actions when a row is selected...
                            FetchImages(_selectedPesel);
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
            
            ImGui.EndChild();
            ImGui.SameLine();
            // view fto selected patient
            ImGui.BeginChild("v_view", new Vector2(windowSize.X*0.6f -4, 0));
            if (_selectedPesel != null&& _selectedPesel.Length> 0  && _selectedPatient !=null)
            {
                ImGui.Text("Selected Patient");
                ImGui.NewLine();
                ImGui.Separator();
                ImGui.Text("Name: " + Encoding.ASCII.GetString(_selectedPatient[1]));
                ImGui.Text("Surname: " + Encoding.ASCII.GetString(_selectedPatient[2]));
                ImGui.NewLine();
                ImGui.Text("Sex: " +( Encoding.ASCII.GetString(_selectedPatient[3]).Trim('\0')=="1" ? "Male" : "Female"));
                ImGui.Text("Pesel: " + Encoding.ASCII.GetString(_selectedPatient[0]));
                ImGui.Separator();

                ImGui.NewLine();
                ImGui.Text("Contact");
                ImGui.Separator();
                ImGui.Text("Email: " + Encoding.ASCII.GetString(_selectedPatient[5]));
                ImGui.Text("Phone: " + Encoding.ASCII.GetString(_selectedPatient[6]));
                ImGui.Separator();
                ImGui.NewLine();
                ImGui.Text("Address");
                ImGui.Separator();
                ImGui.Text("City: " + Encoding.ASCII.GetString(_selectedPatient[7]));
                ImGui.Text("Street: " + Encoding.ASCII.GetString(_selectedPatient[8]));
                ImGui.Text("House number: " + Encoding.ASCII.GetString(_selectedPatient[9]));
                ImGui.Text("Flat number: " + Encoding.ASCII.GetString(_selectedPatient[10]));
                ImGui.Text("Postal code: " + Encoding.ASCII.GetString(_selectedPatient[11]));
                ImGui.Text("Country: " + Encoding.ASCII.GetString(_selectedPatient[12]));
                ImGui.Separator();
                ImGui.Text("Images");
                new ImagePicker("Add Image").OnPickedValid(ImgPath =>
                {
                    //onpickedValid -> always valid ImgPath
                    Console.WriteLine(ImgPath); //check image ImgPath
                    ConvertButton = false;
                    ImageImgPathExist = true;
                    ImgPathError = false;
                    ImgPath = ImgPath;
                    string extension = ImgPath.Substring(ImgPath.Length - 3).ToLower();
                    //get content from ImgPath as base64

                    if (extension != "jpg" && extension != "png" && extension != "bmp")
                    {
                        ImgPathError = true;
                    }
                    else
                    {
                        byte[] imgBytes = File.ReadAllBytes(ImgPath);
                        string base64 = Convert.ToBase64String(imgBytes);
                        string fileName = Path.GetFileName(ImgPath);

                        AddImage(_selectedPesel, base64, fileName);
                        FetchImages(_selectedPesel);
                    }

                }).Render();
                ImGui.Separator();
                for (int i=0;i<_selectedPatientImages.Count; i++)
                {
                    ImGui.PushID(i);
                    var img = _selectedPatientImages[i];
                    var id = img[0];
                    var name = img[3];
                    var content = img[2];
                    var bytes = Convert.FromBase64String(content);
                    var imagePrt =img[4];
                   
                    //convert to bitmap
                    Bitmap bitmap = new Bitmap(new MemoryStream(bytes));
                    var with = bitmap.Width;
                    var height = bitmap.Height;
                    ImGui.Image((IntPtr.Parse(imagePrt)), ImageResizer.ResizeImageToHeight(new Vector2(with,height),50));
                    ImGui.SameLine();   

                    ImGui.Text(name);
                    ImGui.SameLine();
                    new Button("Download").OnClick(() =>
                    {
                        NfdDialogResult saveFileDialog= Nfd.FileSave(InputFilterHelper.NfdFilter(), name, "C:\\");
                        if (saveFileDialog.Path != null)
                        {
                            File.WriteAllBytes(saveFileDialog.Path, Convert.FromBase64String(content));
                        }

                    }).Render();
                    ImGui.SameLine();
                    new Button("Delete").OnClick(() =>
                    {
                        DeleteImage(_selectedPesel, int.Parse(id));
                        FetchImages(_selectedPesel);
                    }).Render();
                    ImGui.NewLine();
                    ImGui.PopID();
                }
            }
            else
            {
                ImGui.Text("Choose Patient");
                
            }
            ImGui.EndChild();
        }

        private void SetupTableColumns()
        {
            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.NoHeaderWidth, 3.5f);
            ImGui.TableSetupColumn("Surname", ImGuiTableColumnFlags.NoHeaderWidth, 3.5f);
        }
        private void FetchImages(string _patientPesel)
        {
            _selectedPatientImages.Clear();
            var query = $"SELECT * FROM images WHERE user_id = '{_patientPesel}'";
            var reader = Program.dbService.ExecuteFromSql(query);
            while (reader.Read())
            {
                string[] row = new string[5];
                for (int i = 0; i < 5; i++)
                {
                    if (i == 2)
                    {
                        row[i] = Encoding.ASCII.GetString(reader.GetValue(i) as byte[]);
                        continue;
                    }else
                    if(i==4)
                    {
                        var content = row[2];
                        var bytes = Convert.FromBase64String(content);
                        var imgRender = _imageRender.Create(bytes);
                        row[i] = imgRender.ToString();
                    }
                    else
                    {
                        string cellValue = reader.GetValue(i).ToString();
                        row[i] = cellValue;
                    }
                }

                _selectedPatientImages.Add(row);
            }
            reader.Close();
        }
        private bool AddImage(string _patientPesel, string base64Img,string imgName)
        {
            var query = $"INSERT INTO images (user_id, data,name) VALUES (@p0,@p1,@p2)";
            var result = Program.dbService.ExecuteFromSql(query, _patientPesel, base64Img,imgName);
            result.Close();
            return true;
        }
        private bool DeleteImage(string _patientPesel, int id)
        {
            var query = $"DELETE FROM images WHERE user_id = @p0 AND id = @p1";
            var result = Program.dbService.ExecuteFromSql(query, _patientPesel, id);
            result.Close();
            return true;
        }
    }
}