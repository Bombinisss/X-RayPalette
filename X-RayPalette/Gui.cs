using System.Drawing;
using System.Numerics;
using ImGuiNET;
using Veldrid.Sdl2;
using X_RayPalette.Helpers;
using NativeFileDialogExtendedSharp;

namespace X_RayPalette
{
    public partial class Gui
    {
        private const ImGuiWindowFlags Flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove |
                                               ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoBackground | 
                                               ImGuiWindowFlags.HorizontalScrollbar | ImGuiWindowFlags.NoResize;

        private Sdl2Window _windowCopy;
        public bool _opendev;
        private bool _isRunning;
        private bool _loggedIn;
        private string _username;
        private string _password;
        private string _passwordRepeat;
        private string _usernameRegister;
        private string _passwordRegister;

        private string _newPatientName;
        private string _newPatientSurname;
        private int _newPatientSex;
        private string _newPatientPESEL;
        private string _newPatientEmail;
        private string _newPatientPhone;

        private string _newPatientCity;
        private string _newPatientStreet;
        private string _newPatientHouseNumber;
        private string _newPatientFlatNumber;
        private string _newPatientPostCode;
        private string _newPatientCountry;
        private PhoneAreaCode _newPatientPhoneAreaCode;
        public Gui(Sdl2Window windowCopy)
        {
            _opendev = false;
            _windowCopy = windowCopy;
            _isRunning = true;
            _loggedIn = false;
            _username = "";
            _password = "";
            _passwordRepeat = "";
            _usernameRegister = "";
            _passwordRegister = "";

            _newPatientName = "";
            _newPatientSurname = "";
            _newPatientSex = 1;
            _newPatientPESEL = "";
            _newPatientEmail = "";
            _newPatientPhone = "";
            _newPatientCity = "";
            _newPatientStreet = "";
            _newPatientHouseNumber = "";
            _newPatientFlatNumber = "";
            _newPatientPostCode = "";    
            _newPatientCountry = "";
            _newPatientPhoneAreaCode = InputDataHelper.PhoneAreaCodes.Find(x => x.AreaCode == "48");
        }

        public void SubmitUi()
        {
            _opendev = false;
            ImGui.Begin("MedApp", ref _isRunning, Flags);
            if (!_isRunning) Environment.Exit(0);

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Close", "Ctrl+W")) { _isRunning= false; }
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
            
            if (_loggedIn)
            {
                if (ImGui.BeginTabBar("TabBar", ImGuiTabBarFlags.FittingPolicyResizeDown))
                {
                    if (ImGui.BeginTabItem("My Patients"))
                    {
                        ImGui.BeginTable("Patients", 1);
                        ImGui.EndTable();

                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Add Patient"))
                    { 
                        ImGui.Text("Name: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##name##", ref _newPatientName, 128);
                        ImGui.SameLine();
                        ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                        ImGui.Text("Surname: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##surname##", ref _newPatientSurname, 128);
                        ImGui.SameLine();
                        ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                        ImGui.Text("Sex: ");
                        ImGui.SameLine(110);

                        ImGui.RadioButton("Men", ref _newPatientSex, 1);
                        ImGui.SameLine(200);
                        ImGui.RadioButton("Woman", ref _newPatientSex, 2);

                        ImGui.Text("PESEL: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##pesel##", ref _newPatientPESEL, 11);
                        ImGui.SameLine();
                        ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                        ImGui.Text("E-mail: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##emailadress##", ref _newPatientEmail, 128);

                        ImGui.Text("Phone: ");
                        ImGui.SameLine(110);
                        ImGui.PushItemWidth(150);
                        ImGui.InputText("##pesel##", ref _newPatientPESEL, 11);
                        ImGui.PopItemWidth();
                        ImGui.Text("Phone: ");
                        ImGui.SameLine(110);
                        ImGui.PushItemWidth(50);
                        if (ImGui.BeginCombo("##phoneArea", _newPatientPhoneAreaCode == null ? "Phone area" : ("+" + _newPatientPhoneAreaCode.AreaCode)))
                        {
                            foreach (var areaCode in InputDataHelper.PhoneAreaCodes)
                            {
                                if (ImGui.Selectable("+" + areaCode.AreaCode + " " + areaCode.AreaName))
                                {
                                    _newPatientPhoneAreaCode = areaCode;
                                }
                            }
                            ImGui.EndCombo();
                        }
                        ImGui.PopItemWidth();
                        ImGui.SameLine(0);
                        ImGui.PushItemWidth(150);
                        ImGui.InputText("##phone##", ref _newPatientPhone, 15, ImGuiInputTextFlags.CharsDecimal);

                        ImGui.SameLine();
                        ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                        ImGui.TextColored(new Vector4(0.8f,0.20f,0.20f,0.90f),"\u002A - required field");
                        
                        ImGui.Text("Address");
                        ImGui.Separator();
                        ImGui.Text("City: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##city##", ref _newPatientCity, 128);
                        ImGui.Text("Street: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##street##", ref _newPatientStreet, 128);
                        ImGui.Text("House number: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##house##", ref _newPatientHouseNumber, 5);
                        ImGui.Text("Flat number: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##flat##", ref _newPatientFlatNumber, 5);
                        ImGui.Text("Post code: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##post##", ref _newPatientPostCode, 6);
                        ImGui.Text("Country: ");
                        ImGui.SameLine(110);
                        ImGui.InputText("##country##", ref _newPatientCountry, 128);
                        ImGui.Separator();

                        ImGui.Text("Images");
                        ImGui.Separator();
                        ImGui.Button("Upload Images"); // TODO: add file Popup and upload images
                        // TODO: display uploaded images
                        ImGui.Separator();

                        ImGui.NewLine();
                        ImGui.Button("Add Patient"); // TODO: add patient to database
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Dev")) //obviously will be moved in the future
                    {
                        _opendev = true;
                        //to do: select or drop image here and convert to long rainbow
                        if (ImGui.Button("Select Image"))
                        { 
                            
                            NfdDialogResult path = Nfd.FileOpen(Filters.CreateNewNfdFilter(), "C:\\"); //path - selected image path
                            Console.WriteLine(path.Path); //check image path

                        }
                        
                        ImGui.EndTabItem();
                    }

                    if (_windowCopy.Width > 289)
                    {
                        ImGui.SameLine(_windowCopy.Width-60);
                        if (ImGui.Button("logout"))
                        {
                            _loggedIn = false;
                        }
                    }
                    else
                    {
                        if (ImGui.TabItemButton("logout"))
                        {
                            _loggedIn = false;
                        }
                    }
                    
                }
            }
            else
            {
                if (ImGui.BeginTabBar("TabBar", ImGuiTabBarFlags.FittingPolicyResizeDown))
                {
                    if (ImGui.BeginTabItem("Login"))
                    {
                        ImGui.Text("Username: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##username##", ref _username, 128);
                        ImGui.Text("Password: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##passwd##", ref _password, 128, ImGuiInputTextFlags.Password);
                        if (ImGui.Button("Login"))
                        {
                            //to do: check login
                            _windowCopy.Height = 540;
                            _windowCopy.Width = 960;
                            _loggedIn = true;
                        }

                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Register"))
                    {
                        ImGui.Text("Username: ");
                        ImGui.SameLine(95);
                        ImGui.InputText("##usernameReg##", ref _usernameRegister, 128);
                        ImGui.Text("Password: ");
                        ImGui.SameLine(95);
                        ImGui.InputText("##passwdReg##", ref _passwordRegister, 128, ImGuiInputTextFlags.Password);
                        ImGui.Text("Confirm Pwd: ");
                        ImGui.SameLine(95);
                        ImGui.InputText("##passwd2##", ref _passwordRepeat, 128, ImGuiInputTextFlags.Password);

                        // Register account
                        if (ImGui.Button("Register"))
                        {
                            //to do: add to database
                        }

                        ImGui.EndTabItem();
                    }
                }
            }
        }
    }
}