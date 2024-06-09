using ImGuiNET;
using MySql.Data.MySqlClient;
using NativeFileDialogExtendedSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Veldrid.OpenGLBinding;
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
        List<byte[][]> _selectedPatientImages = new List<byte[][]>();
        List<byte[][]> allData;

        public IntPtr ImageHandler;
        public static IntPtr ImageHandlerOut;
        private IntPtr _imageHandlerLoading;
        private byte[] _selectedImageBytes;
        private readonly ImageRenderService _imageRender;
        private readonly ImageRenderService _imageRenderOut;
        private readonly ImageRenderService _imageRenderLoading;
        private readonly ColorConversionService _colorConvert;
        public Vector2 ImgSize;
        public string Path;

        public int _colorMode = 0;
        private bool _imageConverted = false;
        public MyPatients()
        {
            _colorConvert = new ColorConversionService();

            _imageRender = new ImageRenderService();
            _imageRenderOut = new ImageRenderService();
            _imageRenderLoading = new ImageRenderService();

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
            var windowSize = ImGui.GetContentRegionAvail();

            ImGui.BeginChild("v_list", new Vector2(windowSize.X * 0.4f - 4, 0));
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
                            ImageHandler = IntPtr.Zero;
                            ImageHandlerOut = IntPtr.Zero;
                            _imageConverted = false;

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
                            _selectedPatientImages = FetchImages(_selectedPesel);
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
            ImGui.BeginChild("v_view", new Vector2(windowSize.X * 0.6f - 4, 0));
            if (_selectedPesel != null && _selectedPesel.Length > 0 && _selectedPatient != null)
            {
                ImGui.Text("Selected Patient");
                ImGui.NewLine();
                ImGui.Separator();
                ImGui.Text("Name: " + Encoding.ASCII.GetString(_selectedPatient[1]));
                ImGui.Text("Surname: " + Encoding.ASCII.GetString(_selectedPatient[2]));
                ImGui.NewLine();
                ImGui.Text("Sex: " + (Encoding.ASCII.GetString(_selectedPatient[3]).Trim('\0') == "1" ? "Male" : "Female"));
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
                // images
                new ImagePicker("Select Image").OnPickedValid(path =>
                        {
                            //onpickedValid -> always valid path
                            Console.WriteLine(path); //check image path
                            Path = path;
                            string extension = Path.Substring(Path.Length - 3).ToLower();
                            if (extension != "jpg" && extension != "png" && extension != "bmp")
                            {

                            }
                            else
                            { 

                                ImageHandler = _imageRender.Create(Path);
                                ImgSize = new(_imageRender.Width, _imageRender.Height);
                            }

                        }).Render();
                ImGui.Text("Color Mode: ");
                ImGui.SameLine();
                ImGui.RadioButton("PM3D", ref _colorMode, 0);
                ImGui.SameLine();
                ImGui.RadioButton("Long Rainbow", ref _colorMode, 1);
                new Button("ConvertImage").OnClick(() =>
                        {
                            if (Path != null)
                            {
                                _imageConverted = true;
                                Thread thread = new Thread(() =>
                                {

                                    var imgData= _colorConvert.Start(Path, _colorMode, _imageRenderOut);
                                    var image = imgData.bytes;
                                    ImageHandlerOut = imgData.intprt;
                                    //send to db
                                    string query = $"INSERT INTO images (user_id,name,data ) VALUES ('{_selectedPesel}', '{"image"}','{Convert.ToBase64String(image)}')";
                                    Program.dbService.ExecuteNonQuery(query);
                                    _selectedPatientImages = FetchImages(_selectedPesel);
                                    });
                                thread.Start();
                                ImagePathHelper.ImagesFolderPath();
                                _imageHandlerLoading = _imageRenderLoading.Create(ImagePathHelper.ImagesFolderPath() + "\\loading.jpg");

                            }

                        }).Render();
                if (ImageHandler != IntPtr.Zero)
                {
                    ImGui.Image(ImageHandler, ImgSize);
                }
                else
                {
                    ImGui.Text("No Image");
                }
                ImGui.SameLine();
                if (_imageConverted && _colorConvert.IsProcessing)
                {
                    ImGui.Image(ImageHandlerOut, new Vector2(_imageRenderOut.Width, _imageRenderOut.Height));
                }
                else if (_imageConverted)
                {
                    ImGui.Image(this._imageHandlerLoading, new Vector2(_imageRenderLoading.Width, _imageRenderLoading.Height));
                }
                ImGui.NewLine();
                ImGui.Separator();
                ImGui.Text("Images");

                foreach (var image in _selectedPatientImages)
                {
                    byte[] data = image[2];
                    byte[] idBytes = image[0];
                    //get striogn from byters
                    string idString = Encoding.ASCII.GetString(idBytes).TrimEnd('\0');
                    int id = Convert.ToInt32(idString);
                    string base64 = Encoding.ASCII.GetString(data);
                    byte[] imageBytes = Convert.FromBase64String(base64);
                    
                    new Button("Download").OnClick(() =>
                    {
                        NfdDialogResult saveFileDialog = Nfd.FileSave(InputFilterHelper.NfdFilter(), "image", "C:\\");
                        if (saveFileDialog.Path != null)
                        {
                            try { 
                            var bitmap = new Bitmap(new MemoryStream(imageBytes));
                                byte[] bitmapBytes;
                                using (var memoryStream = new MemoryStream())
                                {
                                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                    bitmapBytes = memoryStream.ToArray();
                                    //save to file
                                    File.WriteAllBytes(saveFileDialog.Path, bitmapBytes);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }

                    }).Render();
                    ImGui.SameLine();
                    new Button("Delete").OnClick(() =>
                    {
                        string query = $"DELETE FROM images WHERE id = {id}";
                        Program.dbService.ExecuteNonQuery(query);
                        _selectedPatientImages = FetchImages(_selectedPesel);
                    }).Render();
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
        private List<byte[][]> FetchImages(string patientPesel)
        {
            MySqlDataReader allReader;
            var images = new List<byte[][]>();
            allReader = Program.dbService.ExecuteFromSql($"SELECT id,user_id,data,name FROM images WHERE user_id = '{patientPesel}'");
            while (allReader.Read())
            {
                byte[][] row = new byte[4][];
                for (int i = 0; i < 4; i++)
                {
                    string cellValue = allReader.GetValue(i).ToString();
                    row[i] = new byte[256];
                    byte[] valueBytes = Encoding.UTF8.GetBytes(cellValue);
                    Array.Copy(valueBytes, row[i], Math.Min(valueBytes.Length, row[i].Length));
                }

                images.Add(row);
            }
            allReader.Close();
            return images;
        }
    }
}