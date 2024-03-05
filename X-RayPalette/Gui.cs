using System.Numerics;
using ImGuiNET;

namespace X_RayPalette
{
    public partial class Gui
    {
        private const ImGuiWindowFlags Flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoMove |
                                               ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoBackground;

        private bool _isRunning;
        private bool _loggedIn;
        private string _username;
        private string _password;
        private string _passwordRepeat;

        public Gui()
        {
            _isRunning = true;
            _loggedIn = false;
            _username = "";
            _password = "";
            _passwordRepeat = "";
        }

        public void SubmitUi()
        {
            ImGui.Begin("MedApp", ref _isRunning, Flags);
            if (!_isRunning) Environment.Exit(0);

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
                            _loggedIn = true;
                        }

                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Register"))
                    {
                        ImGui.Text("Username: ");
                        ImGui.SameLine(95);
                        ImGui.InputText("##username##", ref _username, 128);
                        ImGui.Text("Password: ");
                        ImGui.SameLine(95);
                        ImGui.InputText("##passwd##", ref _password, 128, ImGuiInputTextFlags.Password);
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