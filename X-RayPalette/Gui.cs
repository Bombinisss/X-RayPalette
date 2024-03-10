using System.Numerics;
using ImGuiNET;
using Veldrid.Sdl2;

namespace X_RayPalette
{
    public partial class Gui
    {
        private const ImGuiWindowFlags Flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove |
                                               ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoBackground | 
                                               ImGuiWindowFlags.HorizontalScrollbar | ImGuiWindowFlags.NoResize;

        private Sdl2Window _windowCopy;
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
        private string _newPatientPhone;

        private string _newPatientCity;
        private string _newPatientStreet;
        private string _newPatientHouseNumber;
        private string _newPatientFlatNumber;
        private string _newPatientPostCode;
        private string _newPatientCountry;
        public Gui(Sdl2Window windowCopy)
        {
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
            _newPatientPhone = "";
            _newPatientCity = "";
            _newPatientStreet = "";
            _newPatientHouseNumber = "";
            _newPatientFlatNumber = "";
            _newPatientPostCode = "";    
            _newPatientCountry = "";
        }

        public void SubmitUi()
        {
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
                        ImGui.SameLine(0);
                        ImGui.InputText("##name##", ref _newPatientName, 128);
                        ImGui.Text("Surname: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##surname##", ref _newPatientSurname, 128);
                        ImGui.Text("Sex: ");
                        ImGui.SameLine(0);

                        ImGui.RadioButton("Men", ref _newPatientSex, 1);
                        ImGui.SameLine(0);
                        ImGui.RadioButton("Woman", ref _newPatientSex, 2);

                        ImGui.Text("PESEL: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##pesel##", ref _newPatientPESEL, 11);
                        ImGui.Text("Phone: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##phone##", ref _newPatientPhone, 9);

                        ImGui.Text("Address");
                        ImGui.Separator();
                        ImGui.Text("City: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##city##", ref _newPatientCity, 128);
                        ImGui.Text("Street: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##street##", ref _newPatientStreet, 128);
                        ImGui.Text("House number: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##house##", ref _newPatientHouseNumber, 5);
                        ImGui.Text("Flat number: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##flat##", ref _newPatientFlatNumber, 5);
                        ImGui.Text("Post code: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##post##", ref _newPatientPostCode, 6);
                        ImGui.Text("Country: ");
                        ImGui.SameLine(0);
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
                        //to do: select or drop image here and convert to long rainbow
                        
                        ImGui.EndTabItem();
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