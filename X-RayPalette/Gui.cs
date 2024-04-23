using System.Numerics;
using ImGuiNET;
using Veldrid.Sdl2;
using NativeFileDialogExtendedSharp;
using X_RayPalette.Views;
using X_RayPalette.Helpers;
using X_RayPalette.Views.InfoChange;
using X_RayPalette.Views.Patient;
using X_RayPalette.Views.DoctorRegister;

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

        private View addpationtView;
        private View patientAssigment;
        private View patientAssigmentAlt;

        private View doctorRegister;

        private View infoChangeWrapper;
        private View infoChangeDoctor;
        private View infoChangePatient;

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

            //config for patients view
             addpationtView = new PatientAdd();
             addpationtView.OnBack += (sender, args) => { AdminAddNewPatient = false;  };

             patientAssigment = new PatientAssignment();
             patientAssigment.OnBack += (sender, args) => { AdminAddExistingPatient = false; };

             patientAssigmentAlt = new PatientAssigmentWrapper();
             patientAssigmentAlt.OnBack += (sender, args) => {
                AdminAddNewPatient = false;
                AdminAddExistingPatient = false;
                patientAssigment.Back();
                addpationtView.Back();
             };

            //config for doctor

            doctorRegister = new DoctorRegister();
            //doctorRegister.OnBack += (sender, args) => { AdminAddNewPatient = false; };


            //config for change info view
            infoChangeDoctor = new DoctorInfoChange();
            infoChangeDoctor.OnBack += (sender, args) => { AdminChangeDoctorInfo = false; };
            infoChangePatient = new PatientInfoChange();
            infoChangePatient.OnBack += (sender, args) => { AdminChangePatientInfo = false; };

            infoChangeWrapper = new WrapperInfoChange();
            infoChangeWrapper.OnBack += (sender, args) => { 
                AdminChangeDoctorInfo = false; 
                AdminChangePatientInfo = false;
                infoChangeDoctor.Back();
                infoChangePatient.Back();
            };

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
                            RenderHelper.TextCentered("Choose option", "Add new Patient", "Change Patient Assigment", "OR",ref AdminAddNewPatient,ref AdminAddExistingPatient);                           
                        }
                        if (AdminAddExistingPatient || AdminAddNewPatient)
                        {
                            patientAssigmentAlt.Render(_AdminLoggedIn);             
                        }
                        if (AdminAddNewPatient || _loggedIn)
                        {
                            addpationtView.Render(_AdminLoggedIn);                                           
                                                  
                        }
                        if (AdminAddExistingPatient)
                        {
                            patientAssigment.Render(_AdminLoggedIn);
                        }
                        ImGui.EndTabItem();
                    }//END add patient tab
                    if (_AdminLoggedIn)
                    {        
                        
                        if (ImGui.BeginTabItem("Register Doctor"))
                        {
                            doctorRegister.Render(_AdminLoggedIn);
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
                                RenderHelper.TextCentered("Choose user type", "Patients", "Doctors", "OR", ref AdminChangePatientInfo, ref AdminChangeDoctorInfo);
                            }
                            if (AdminChangePatientInfo || AdminChangeDoctorInfo)
                            {
                               infoChangeWrapper.Render(_AdminLoggedIn);
                            }
                            if (AdminChangeDoctorInfo)
                            {
                                infoChangeDoctor.Render(_AdminLoggedIn);
                            }
                            if (AdminChangePatientInfo)
                            {
                                infoChangePatient.Render(_AdminLoggedIn);
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
    }
}