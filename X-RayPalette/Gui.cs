using System.Numerics;
using ImGuiNET;
using Veldrid.Sdl2;
using X_RayPalette.Helpers;
using NativeFileDialogExtendedSharp;
using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace X_RayPalette
{
    public partial class Gui
    {
        private ImGuiWindowFlags _flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove |
                                               ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.HorizontalScrollbar |
                                               ImGuiWindowFlags.NoResize;

        private readonly Sdl2Window _windowCopy;

        public Connector _connector = new Connector(); // creating connection to database;

        public bool DevOpen;
        public string Path;
        public bool ImagePathExist;
        public IntPtr ImageHandler;

        private int _theme;
        private bool _isRunning;
        private bool _loggedIn;
        public bool _loggedout;

        private bool _AdminLoggedIn;
        private bool AdminAddNewPatient;
        private bool AdminAddExistingPatient;
        private bool AdminChangePatientInfo;
        private bool AdminChangeDoctorInfo;

        private string _username;
        private string _password;
        private string _passwordRepeat;
        private string _usernameRegister;
        private string _passwordRegister;

        private string _newPatientName;
        private string _newPatientSurname;
        private int _newPatientSex;
        private string _newPatientPesel;
        private string _newPatientEmail;
        private string _newPatientPhone;

        private string _newDoctorName;
        private string _newDoctorSurname;
        private int _newDoctorSex;
        private string _newDoctorPesel;
        private string _newDoctorEmail;
        private string _newDoctorPhone;
        private PhoneAreaCode _newDoctorPhoneAreaCode;
        private string _tempdataDoc_AP;
        private string _tempdataDoc_EP;
        private string _tempdataDoc_CI;
        private string _tempdataPatient_CI;
        private string _tempdataPatient_EP;

        private string _newPatientCity;
        private string _newPatientStreet;
        private string _newPatientHouseNumber;
        private string _newPatientFlatNumber;
        private string _newPatientPostCode;
        private string _newPatientCountry;
        private PhoneAreaCode _newPatientPhoneAreaCode;
        public Gui(Sdl2Window windowCopy)
        {
            ImagePathExist = false;
            DevOpen = false;
            _theme = 0;
            _windowCopy = windowCopy;
            _isRunning = true;
            _loggedIn = false;
            _loggedout = false;

            _AdminLoggedIn = false;
            AdminAddExistingPatient = false;
            AdminAddNewPatient = false;

            _username = "";
            _password = "";
            _passwordRepeat = "";
            _usernameRegister = "";
            _passwordRegister = "";

            _newDoctorName = "";
            _newDoctorSurname = "";
            _newDoctorSex = 1;
            _newDoctorPesel = "";
            _newDoctorEmail = "";
            _newDoctorPhone = "";
            _newDoctorPhoneAreaCode = InputDataHelper.PhoneAreaCodes.First();

            _tempdataDoc_AP = "Choose Doctor";
            _tempdataDoc_CI = "Choose Doctor";
            _tempdataDoc_EP = "Choose Doctor";
            _tempdataPatient_CI = "Choose Patient";
            _tempdataPatient_EP = "Choose Patient";

            _newPatientName = "";
            _newPatientSurname = "";
            _newPatientSex = 1;
            _newPatientPesel = "";
            _newPatientEmail = "";
            _newPatientPhone = "";

            _newPatientCity = "";
            _newPatientStreet = "";
            _newPatientHouseNumber = "";
            _newPatientFlatNumber = "";
            _newPatientPostCode = "";    
            _newPatientCountry = "";
            _newPatientPhoneAreaCode = InputDataHelper.PhoneAreaCodes.First();
        }

        public void SubmitUi()
        {
            DevOpen = false;
            ImGui.Begin("MedApp", ref _isRunning, _flags);
            if (!_isRunning) Environment.Exit(0);

            if (ImGui.BeginMenuBar())
            {
                if(_loggedIn || _AdminLoggedIn)
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Close")) { _isRunning= false; }

                    if (ImGui.BeginMenu("Settings"))
                    {
                        ImGui.Text("Theme: ");
                        ImGui.SameLine();

                        if (ImGui.RadioButton("Light", ref _theme, 0))
                        {
                            DarkTitleBarClass.UseImmersiveDarkMode(_windowCopy.Handle, false, 0x00FFFFFF);
                            _flags &= ~ImGuiWindowFlags.NoBackground;
                            if (!DarkTitleBarClass.IsWindows10OrGreater(22000))
                            {
                                _windowCopy.Visible = false;
                                _windowCopy.Visible = true;
                            }
                            SetupImGuiStyle0();
                        }
                        ImGui.SameLine();
                        if (ImGui.RadioButton("Dark", ref _theme, 1))
                        {
                            DarkTitleBarClass.UseImmersiveDarkMode(_windowCopy.Handle, true,0x00000000);
                            _flags |= ImGuiWindowFlags.NoBackground;
                            if (!DarkTitleBarClass.IsWindows10OrGreater(22000))
                            {
                                _windowCopy.Visible = false;
                                _windowCopy.Visible = true;
                            }
                            SetupImGuiStyle1();
                        }

                        ImGui.EndMenu();
                    }

                    if (ImGui.MenuItem("Logout"))
                    {
                        _flags &= ~ImGuiWindowFlags.MenuBar;
                        _windowCopy.Height = 150;
                        _windowCopy.Width = 400;
                        _loggedout = true;
                        return;
                    }

                    ImGui.EndMenu();
                }
            ImGui.EndMenuBar();
            }
            
            if (_loggedIn || _AdminLoggedIn)
            {
                if (ImGui.BeginTabBar("TabBar", ImGuiTabBarFlags.FittingPolicyResizeDown))
                {
                    if (!_AdminLoggedIn)
                    {
                        if (ImGui.BeginTabItem("My Patients"))
                        {
                            ImGui.BeginTable("Patients", 1);
                            ImGui.EndTable();

                            ImGui.EndTabItem();
                        }
                    }                   
                    if (ImGui.BeginTabItem("Add Patient"))
                    {
                        if (_AdminLoggedIn && !AdminAddExistingPatient && !AdminAddNewPatient)
                        {                           
                            TextCentered("Choose option", "Add new Patient", "Change Patient Assigment", "OR",ref AdminAddNewPatient,ref AdminAddExistingPatient);                           
                        }
                        if (AdminAddExistingPatient || AdminAddNewPatient)
                        {
                            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.0f, 0.749f, 1.0f, 0.20f));
                            if (ImGui.Button("< back"))
                            {
                                AdminAddNewPatient = false;
                                AdminAddExistingPatient = false;
                                _tempdataDoc_EP = "Choose Doctor";
                                _tempdataPatient_EP = "Choose Patient";
                                _tempdataDoc_AP = "Choose Doctor";
                            }
                            ImGui.PopStyleColor();
                            ImGui.Separator();                          
                        }
                        if (AdminAddNewPatient || _loggedIn)
                        {
                                                                           
                            ImGui.PushItemWidth(208);
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
                            ImGui.InputText("##pesel##", ref _newPatientPesel, 11);
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                            ImGui.Text("E-mail: ");
                            ImGui.SameLine(110);
                            ImGui.InputText("##emailadress##", ref _newPatientEmail, 128);

                            ImGui.Text("Phone: ");
                            ImGui.SameLine(110);
                            ImGui.PopItemWidth();
                            ImGui.PushItemWidth(50);
                            if (ImGui.BeginCombo("##phoneArea", "+" + _newPatientPhoneAreaCode.AreaCode))
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
                            ImGui.PopItemWidth();

                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A - required field");

                            ImGui.PushItemWidth(208);
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
                            ImGui.PopItemWidth();

                            if (!_AdminLoggedIn)
                            {
                                ImGui.Text("Images");
                                ImGui.Separator();
                                ImGui.Button("Upload Images"); // TODO: add file Popup and upload images
                                ImGui.Separator();             // TODO: display uploaded images
                            }

                            if (_AdminLoggedIn)
                            {
                                ImGui.PushItemWidth(150);
                                string[] temp = { "dr1", "dr2" };
                                if (ImGui.BeginCombo("##Doctorchoose##", _tempdataDoc_AP))
                                {

                                    foreach (var doc in temp)
                                    {
                                        if (ImGui.Selectable(doc))
                                        {
                                             _tempdataDoc_AP = doc;
                                        }
                                    }
                                    ImGui.EndCombo();
                                }
                                ImGui.PopItemWidth();
                            }
                            ImGui.NewLine();
                            if (ImGui.Button("Add Patient"))
                            {
                                if (_AdminLoggedIn)
                                {
                                    AdminAddNewPatient = false;
                                    _tempdataDoc_AP = "Choose Doctor";
                                    // TODO: add patient to database from admin view (to choosen doctor)
                                }
                                // TODO: add patient to database
                            }                          
                        }
                        if (AdminAddExistingPatient)
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
                                AdminAddExistingPatient = false;
                                _tempdataPatient_EP = "Choose Patient";
                                _tempdataDoc_EP = "Choose Doctor";
                            }
                            ImGui.PopItemWidth();
                        }
                        ImGui.EndTabItem();
                    }//END add patient tab
                    if (_AdminLoggedIn)
                    {        
                        
                        if (ImGui.BeginTabItem("Register Doctor"))
                        {
                            ImGui.Text("Name: ");
                            ImGui.SameLine(110);
                            ImGui.InputText("##Doctorname##", ref _newDoctorName, 128);
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                            ImGui.Text("Surname: ");
                            ImGui.SameLine(110);
                            ImGui.InputText("##Doctorsurname##", ref _newDoctorSurname, 128);
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                            ImGui.Text("Sex: ");
                            ImGui.SameLine(110);

                            ImGui.RadioButton("Men", ref _newDoctorSex, 1);
                            ImGui.SameLine(200);
                            ImGui.RadioButton("Woman", ref _newDoctorSex, 2);

                            ImGui.Text("PESEL: ");
                            ImGui.SameLine(110);
                            ImGui.InputText("##Doctorpesel##", ref _newDoctorPesel, 11);
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                            ImGui.Text("E-mail: ");
                            ImGui.SameLine(110);
                            ImGui.InputText("##Doctoremailadress##", ref _newDoctorEmail, 128);

                            ImGui.Text("Phone: ");
                            ImGui.SameLine(110);
                            ImGui.PushItemWidth(50);
                            if (ImGui.BeginCombo("##DoctorphoneArea", "+" + _newDoctorPhoneAreaCode.AreaCode))
                            {
                                foreach (var areaCode in InputDataHelper.PhoneAreaCodes)
                                {
                                    if (ImGui.Selectable("+" + areaCode.AreaCode + " " + areaCode.AreaName))
                                    {
                                        _newDoctorPhoneAreaCode = areaCode;
                                    }
                                }
                                ImGui.EndCombo();
                            }
                            ImGui.PopItemWidth();
                            ImGui.SameLine(0);
                            ImGui.PushItemWidth(150);
                            ImGui.InputText("##Doctorphone##", ref _newDoctorPhone, 15, ImGuiInputTextFlags.CharsDecimal);
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A - required field");

                            ImGui.Separator();
                            ImGui.Text("Username: ");
                            ImGui.SameLine(110);
                            ImGui.InputText("##usernameReg##", ref _usernameRegister, 128);
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");
                            ImGui.Text("Password: ");
                            ImGui.SameLine(110);
                            ImGui.InputText("##passwdReg##", ref _passwordRegister, 128, ImGuiInputTextFlags.Password);
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");
                            ImGui.Text("Confirm Pwd: ");
                            ImGui.SameLine(110);
                            ImGui.InputText("##passwd2##", ref _passwordRepeat, 128, ImGuiInputTextFlags.Password);
                            ImGui.SameLine();
                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A - required field");

                            // Register account
                            if (ImGui.Button("Register"))
                            {
                                //to do: add to database
                            }
                            ImGui.EndTabItem();
                        }
                        
                    }
                    if (_AdminLoggedIn)
                    {
                        if (ImGui.BeginTabItem("Change information"))
                        {
                            ImGui.PushItemWidth(150);
                            if (!AdminChangeDoctorInfo && !AdminChangePatientInfo)
                            {
                                TextCentered("Choose user type", "Patients", "Doctors", "OR", ref AdminChangePatientInfo, ref AdminChangeDoctorInfo);
                            }
                            if (AdminChangePatientInfo || AdminChangeDoctorInfo)
                            {
                                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.0f, 0.749f, 1.0f, 0.20f));
                                if (ImGui.Button("< back"))
                                {
                                    AdminChangeDoctorInfo = false;
                                    AdminChangePatientInfo = false;
                                    _tempdataDoc_CI = "Choose Doctor";
                                    _tempdataPatient_CI = "Choose Patient";
                                }
                                ImGui.PopStyleColor();
                            }
                            if (AdminChangeDoctorInfo)
                            {
                                ImGui.Text("Choose:");
                                ImGui.SameLine();
                                string[] temp2 = { "dr1", "dr2" };
                                if (ImGui.BeginCombo("##DocInfoChange##", _tempdataDoc_CI))
                                {
                                    foreach (var doc in temp2)
                                    {
                                        if (ImGui.Selectable(doc))
                                        {
                                            _tempdataDoc_CI = doc;
                                        }
                                    }
                                    ImGui.EndCombo();
                                }
                                ImGui.Separator();
                                ImGui.Text("when DB will work there will be shown chosen doctor's informations with change function");
                                ImGui.Separator();
                                if (ImGui.Button("Confirm changes"))
                                {
                                    //TODO: replacing changed values ​​in the database
                                    _tempdataDoc_CI = "Choose Doctor";
                                    AdminChangeDoctorInfo = false;
                                }
                            }
                            if (AdminChangePatientInfo)
                            {
                                ImGui.Text("Choose:");
                                ImGui.SameLine();
                                string[] temp2 = { "patient1", "patient2" };
                                if (ImGui.BeginCombo("##PatientInfoChange##", _tempdataPatient_CI))
                                {
                                    foreach (var patient in temp2)
                                    {
                                        if (ImGui.Selectable(patient))
                                        {
                                            _tempdataPatient_CI = patient;
                                        }
                                    }
                                    ImGui.EndCombo();
                                }
                                ImGui.Separator();
                                ImGui.Text("when DB will work there will be shown chosen patient's informations with change function");
                                ImGui.Separator();
                                if (ImGui.Button("Confirm changes"))
                                {
                                    //TODO: replacing changed values ​​in the database
                                    _tempdataPatient_CI = "Choose Patient";
                                    AdminChangePatientInfo = false;
                                }
                            }
                            ImGui.PopItemWidth();
                            ImGui.EndTabItem();
                        }
                    }
                    if (ImGui.BeginTabItem("Dev")) //obviously will be moved in the future
                    {              
                        DevOpen = true;
                        //to do: select or drop image here and convert to long rainbow
                        if (ImGui.Button("Select Image"))
                        {
                            NfdDialogResult path = Nfd.FileOpen(Filters.CreateNewNfdFilter(), "C:\\"); //path - selected image path
                            Console.WriteLine(path.Path); //check image path
                            if (path.Path != null)
                            {
                                Path = path.Path;
                                Thread thread = new Thread(() => ColorChanger.Worker(Path));
                                thread.Start();
                            }
                            
                            ImagePathExist = true;
                            ImageHandler = ImageIntPtr.CreateImgPtr(Path);

                        }
                        if (ImagePathExist && Path != null)
                        {
                            ImGui.Image(this.ImageHandler, new Vector2(ImageIntPtr.width, ImageIntPtr.height));
                        }
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
                        string pass;
                        ImGui.Text("Username: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##username##", ref _username, 128);
                        ImGui.Text("Password: ");
                        ImGui.SameLine(0);
                        ImGui.InputText("##passwd##", ref _password, 128, ImGuiInputTextFlags.Password);
                        if (ImGui.Button("Login")) // Admin login: Admin password: Admin12
                        {
                            _connector.cmd.CommandText = "SELECT password FROM login_info WHERE login LIKE '" + _username + "' LIMIT 1";
                            pass = (string)_connector.cmd.ExecuteScalar();
                            if (pass != null && BCrypt.Net.BCrypt.EnhancedVerify(_password, pass))
                            {
                                if (_username == "Admin")
                                {
                                    _AdminLoggedIn = true;
                                }
                                else
                                {
                                    _loggedIn = true;
                                }
                            }
                            else // to do: inform user about invalid inputs
                            {
                                Console.WriteLine("Invalid username or password."); 
                            }
                            
                            _flags |= ImGuiWindowFlags.MenuBar;
                            _windowCopy.Height = 540;
                            _windowCopy.Width = 960;                           
                        }
                        ImGui.EndTabItem();
                    }
                }
            }
        }
        static void TextCentered(string header, string b1, string b2, string or, ref bool b1value, ref bool b2value)
        {
            var windowWidth = ImGui.GetWindowSize();
            var headerWidth = ImGui.CalcTextSize(header);
            var b1Width = ImGui.CalcTextSize(b1);
            var orWidth = ImGui.CalcTextSize("O");
            var space = ImGui.CalcTextSize(" ");

            ImGui.SetCursorPosX((windowWidth.X - headerWidth.X) * 0.5f);
            ImGui.Text(header);

            ImGui.SetCursorPosX(windowWidth.X * 0.5f - b1Width.X - 2 * orWidth.X - space.X - 3.0f);

            if (ImGui.Button(b1))
            {
                b1value = true;
            }
            ImGui.SameLine();
            ImGui.Text(or);
            ImGui.SameLine();
            if (ImGui.Button(b2))
            {
                b2value = true;
            }
        }
    }
}