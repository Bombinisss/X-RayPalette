using System.Numerics;
using ImGuiNET;
using NativeFileDialogExtendedSharp;
using Veldrid.Sdl2;
using X_RayPalette.Views;
using X_RayPalette.Helpers;
using X_RayPalette.Views.InfoChange;
using X_RayPalette.Views.Patient;
using X_RayPalette.Views.DoctorRegister;
using X_RayPalette.Services;
using X_RayPalette.Components;

namespace X_RayPalette
{
    public partial class Gui
    {
        private ImGuiWindowFlags _flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove |
                                               ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.HorizontalScrollbar |
                                               ImGuiWindowFlags.NoResize;

        private readonly Sdl2Window _windowCopy;

        public bool DevOpen;
        public string Path;
        public bool ImagePathExist;
        public IntPtr ImageHandler;
        public static IntPtr ImageHandlerOut;
        private IntPtr _imageHandlerLoading;
        public bool ConvertButton;

        private int _theme;
        private int _colorMode;
        private bool _isRunning;
        private bool _loggedIn;
        public bool LoggedOut;
        private bool _invalidPass;

        private bool _adminLoggedIn;
        private bool _adminAddNewPatient;
        private bool _adminAddExistingPatient;
        private bool _adminChangePatientInfo;
        private bool _adminChangeDoctorInfo;

        private string _username;
        private string _password;
        private bool _passwordNotShown;

        private readonly View _addPatientView;
        private readonly View _patientAssigment;
        private readonly View _patientAssigmentAlt;

        private readonly View _doctorRegister;

        private readonly View _infoChangeWrapper;
        private readonly View _infoChangeDoctor;
        private readonly View _infoChangePatient;
        private readonly View _myPatients;

        //color conversion services + image render services
        private readonly ColorConversionService _colorConvert;
        private readonly ImageRenderService _imageRender;
        public Vector2 ImgSize;
        private readonly ImageRenderService _imageRenderOut;
        private readonly ImageRenderService _imageRenderLoading;
        public bool PathError;
        private static bool showErrorPopup;

        public Gui(Sdl2Window windowCopy)
        {
            _colorConvert = new ColorConversionService();

            //render instaces for source, out and loading image
            _imageRender = new ImageRenderService();
            _imageRenderOut = new ImageRenderService();
            _imageRenderLoading = new ImageRenderService();
            PathError = false;

            ImagePathExist = false;
            ConvertButton = false;
            DevOpen = false;
            _theme = 0;
            _colorMode = 0;
            _windowCopy = windowCopy;
            _isRunning = true;
            _loggedIn = false;
            LoggedOut = false;
            _invalidPass = false;

            _adminLoggedIn = false;
            _adminAddExistingPatient = false;
            _adminAddNewPatient = false;

            _username = "";
            _password = "";
            _passwordNotShown = true;

            //config for patients view
            _addPatientView = new PatientAdd();
            _addPatientView.OnBack += (sender, args) => { _adminAddNewPatient = false; };

            _patientAssigment = new PatientAssignment();
            _patientAssigment.OnBack += (sender, args) => { _adminAddExistingPatient = false; };

            _patientAssigmentAlt = new PatientAssigmentWrapper();
            _patientAssigmentAlt.OnBack += (sender, args) =>
            {
                _adminAddNewPatient = false;
                _adminAddExistingPatient = false;
                _patientAssigment.Back();
                _addPatientView.Back();
            };

            //config for doctor

            _doctorRegister = new DoctorRegister();
            //doctorRegister.OnBack += (sender, args) => { AdminAddNewPatient = false; };


            //config for change info view
            _infoChangeDoctor = new DoctorInfoChange();
            _infoChangeDoctor.OnBack += (sender, args) => { _adminChangeDoctorInfo = false; };
            _infoChangePatient = new PatientInfoChange();
            _infoChangePatient.OnBack += (sender, args) => { _adminChangePatientInfo = false; };
            _myPatients = new MyPatients();

            _infoChangeWrapper = new WrapperInfoChange();
            _infoChangeWrapper.OnBack += (sender, args) =>
            {
                _adminChangeDoctorInfo = false;
                _adminChangePatientInfo = false;
                _infoChangeDoctor.Back();
                _infoChangePatient.Back();
            };
            showErrorPopup = false;
        }

        public void SubmitUi()
        {
            DevOpen = false;
            ImGui.Begin("MedApp", ref _isRunning, _flags);
            if (!_isRunning) Environment.Exit(0);

            if (ImGui.BeginMenuBar())
            {
                if (_loggedIn || _adminLoggedIn)
                    if (ImGui.BeginMenu("File"))
                    {
                        if (ImGui.MenuItem("Close")) { _isRunning = false; }

                        if (ImGui.BeginMenu("Settings"))
                        {
                            ImGui.Text("Theme:      ");
                            ImGui.SameLine();

                            if (ImGui.RadioButton("Light", ref _theme, 0))
                            {
                                DarkTitleBarClass.UseImmersiveDarkMode(_windowCopy.Handle, false, 0x00FFFFFF);
                                _flags &= ~ImGuiWindowFlags.NoBackground;
                                if (!EnviromentHelpers.IsSupportedOS(10, 22000))
                                {
                                    _windowCopy.Visible = false;
                                    _windowCopy.Visible = true;
                                }
                                SetupImGuiStyle0();
                            }
                            ImGui.SameLine();
                            if (ImGui.RadioButton("Dark", ref _theme, 1))
                            {
                                DarkTitleBarClass.UseImmersiveDarkMode(_windowCopy.Handle, true, 0x00000000);
                                _flags |= ImGuiWindowFlags.NoBackground;
                                if (!EnviromentHelpers.IsSupportedOS(10, 22000))
                                {
                                    _windowCopy.Visible = false;
                                    _windowCopy.Visible = true;
                                }
                                SetupImGuiStyle1();
                            }

                            ImGui.Text("Color Mode: ");
                            ImGui.SameLine();
                            ImGui.RadioButton("PM3D", ref _colorMode, 0);
                            ImGui.SameLine();
                            ImGui.RadioButton("Long Rainbow", ref _colorMode, 1);

                            ImGui.EndMenu();
                        }

                        if (ImGui.MenuItem("Logout"))
                        {
                            _flags &= ~ImGuiWindowFlags.MenuBar;
                            _windowCopy.Height = 150;
                            _windowCopy.Width = 400;
                            LoggedOut = true;
                            return;
                        }

                        ImGui.EndMenu();
                    }
                ImGui.EndMenuBar();
            }

            if (_loggedIn || _adminLoggedIn)
            {
                if (ImGui.BeginTabBar("TabBar", ImGuiTabBarFlags.FittingPolicyResizeDown))
                {
                    if (!_adminLoggedIn)
                    {
                        if (ImGui.BeginTabItem("My Patients"))
                        {
                            _myPatients.Render(true);
                            
                            ImGui.EndTabItem();
                        }
                    }
                    if (ImGui.BeginTabItem("Add Patient"))
                    {
                        if (_adminLoggedIn && !_adminAddExistingPatient && !_adminAddNewPatient)
                        {
                            RenderHelper.TextCentered("Choose option", "Add new Patient", "Change Patient Assigment", "OR", ref _adminAddNewPatient, ref _adminAddExistingPatient);
                        }
                        if (_adminAddExistingPatient || _adminAddNewPatient)
                        {
                            _patientAssigmentAlt.Render(_adminLoggedIn);
                        }
                        if (_adminAddNewPatient || _loggedIn)
                        {
                            _addPatientView.Render(_adminLoggedIn);

                        }
                        if (_adminAddExistingPatient)
                        {
                            _patientAssigment.Render(_adminLoggedIn);
                        }
                        ImGui.EndTabItem();
                    }//END add patient tab
                    if (_adminLoggedIn)
                    {

                        if (ImGui.BeginTabItem("Register Doctor"))
                        {
                            _doctorRegister.Render(_adminLoggedIn);
                            ImGui.EndTabItem();
                        }

                    }
                    if (_adminLoggedIn)
                    {
                        if (ImGui.BeginTabItem("Change information"))
                        {
                            ImGui.PushItemWidth(150);
                            if (!_adminChangeDoctorInfo && !_adminChangePatientInfo)
                            {
                                RenderHelper.TextCentered("Choose user type", "Patients", "Doctors", "OR", ref _adminChangePatientInfo, ref _adminChangeDoctorInfo);
                            }
                            if (_adminChangePatientInfo || _adminChangeDoctorInfo)
                            {
                                _infoChangeWrapper.Render(_adminLoggedIn);
                            }
                            if (_adminChangeDoctorInfo)
                            {
                                _infoChangeDoctor.Render(_adminLoggedIn);
                            }
                            if (_adminChangePatientInfo)
                            {
                                _infoChangePatient.Render(_adminLoggedIn);
                            }
                            ImGui.PopItemWidth();
                            ImGui.EndTabItem();
                        }
                    }
                    if (ImGui.BeginTabItem("Images Conversion"))
                    {
                        DevOpen = true;
                        new ImagePicker("Select Image").OnPickedValid(path =>
                        {
                            //onpickedValid -> always valid path
                            Console.WriteLine(path); //check image path
                            ConvertButton = false;
                            ImagePathExist = true;
                            PathError = false;
                            Path = path;
                            string extension = Path.Substring(Path.Length - 3).ToLower();
                            if (extension != "jpg" && extension != "png" && extension != "bmp")
                            {
                                PathError = true;
                            }
                            else
                            {
                                ImageHandler = _imageRender.Create(Path);
                                ImgSize = new (_imageRender.Width, _imageRender.Height);
                            }
                            
                        }).Render();
                        if(PathError)
                        {
                            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "Invalid file format");
                        }
                        ImGui.SameLine();
                        new Button("Convert Image").OnClick(() =>
                        {
                            if (Path != null && !PathError)
                            {
                                ConvertButton = true;
                                Thread thread = new Thread(() => _colorConvert.Start(Path, _colorMode, _imageRenderOut));
                                thread.Start();
                                ImagePathHelper.ImagesFolderPath();
                                _imageHandlerLoading = _imageRenderLoading.Create(ImagePathHelper.ImagesFolderPath() + "\\loading.jpg");
                            }

                        }).Render();
                        ImGui.SameLine();
                        new Button("Save Image").OnClick(() =>
                        {
                            if (Path != null && !PathError)
                            {
                                NfdDialogResult path = Nfd.FileSave(InputFilterHelper.NfdFilter(), "untitled.png", "%USERPROFILE%\\Pictures");
                                Console.WriteLine(path.Path);
                                try
                                {
                                    File.Copy(ImagePathHelper.ImagesFolderPath() + "\\output.png", path.Path, overwrite: true);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"An error occurred: {ex.Message}");
                                    showErrorPopup = true;
                                }
                            }

                        }).Render();

                        if (ImagePathExist && Path != null && !PathError)
                        {
                            ImGui.Image(this.ImageHandler, ImageResizer.ResizeImage(ImgSize,700,450));

                            if (ConvertButton && _colorConvert.IsProcessing)
                            {
                                ImGui.Image(ImageHandlerOut, ImageResizer.ResizeImage(new Vector2(_imageRenderOut.Width, _imageRenderOut.Height),700,450));
                            }
                            else if (ConvertButton)
                            {
                                ImGui.Image(this._imageHandlerLoading, ImageResizer.ResizeImage(new Vector2(_imageRenderLoading.Width, _imageRenderLoading.Height),100,100));
                            }
                        }
                        
                        if (showErrorPopup)
                        {
                            ImGui.OpenPopup("Error");

                            if (ImGui.BeginPopupModal("Error", ref showErrorPopup, ImGuiWindowFlags.Popup|ImGuiWindowFlags.AlwaysAutoResize|ImGuiWindowFlags.Modal))
                            {
                                ImGui.Text("Try Converting Image!");
                                if (ImGui.Button("OK"))
                                {
                                    ImGui.CloseCurrentPopup();
                                    showErrorPopup = false;
                                }
                                ImGui.EndPopup();
                            }
                        }
                        
                        ImGui.EndTabItem();
                    }
                }
            }
            else
            {
                string _invalidPassMsg = "Invalid username or password.";
                ImGuiInputTextFlags flags = ImGuiInputTextFlags.EnterReturnsTrue;
                if(_passwordNotShown) flags |= ImGuiInputTextFlags.Password;

                ImGui.SetCursorPosX((ImGui.GetWindowSize().X - ImGui.CalcTextSize("Please Log In").X) * 0.5f);
                ImGui.Text("Please Log In");
                ImGui.NewLine();
                
                new TextInput(_username, "##username##").OnInput(v => _username = v).Title("Username: ", 0).Render();
                if (new TextInput(_password, "##password##").InputType(flags)
                    .Title("Password: ", 0).OnInput(v => _password = v).OnInputChanged((v, x) =>
                    {
                        //clear after other password typed
                        _invalidPass = false;
                    }).Render())
                {
                    if (Program.dbService.AuthUser(_username, _password))
                    {
                        if (_username == "Admin")
                        {
                            _adminLoggedIn = true;
                        }
                        else
                        {
                            _loggedIn = true;
                        }
                        _flags |= ImGuiWindowFlags.MenuBar;
                        _windowCopy.Height = 540;
                        _windowCopy.Width = 960;
                    }
                    else
                    {
                        _invalidPass = true;
                        Console.WriteLine("Invalid username or password.");
                    }
                }
                ImGui.SameLine();
                new Button("*").OnClick(() =>
                {
                    _passwordNotShown =! _passwordNotShown;
                }).Render();
                if(ImGui.IsItemHovered()){ImGui.SetTooltip("Show Password");};

                if (_invalidPass)
                {
                    ImGui.NewLine();
                    ImGui.SameLine(90);
                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), _invalidPassMsg);
                }
                new Button("Login").OnClick(() =>
                {
                    if (Program.dbService.AuthUser(_username, _password))
                    {
                        if (_username == "Admin")
                        {
                            _adminLoggedIn = true;
                        }
                        else
                        {
                            _loggedIn = true;
                        }
                        Globals.LoggedDoc = _username;
                        _flags |= ImGuiWindowFlags.MenuBar;
                        _windowCopy.Height = 540;
                        _windowCopy.Width = 960;
                    }
                    else
                    {
                        _invalidPass = true;
                        Console.WriteLine("Invalid username or password.");
                    }
                }).Render();
            }
        }
    }
}