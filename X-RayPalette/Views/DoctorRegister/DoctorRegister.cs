using ImGuiNET;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Helpers;

namespace X_RayPalette.Views.DoctorRegister
{
    public class DoctorRegister : View
    {
        private string _newDoctorName;
        private string _newDoctorSurname;
        private int _newDoctorSex;
        private string _newDoctorPesel;
        private string _newDoctorEmail;
        private string _newDoctorPhone;
        private PhoneAreaCode _newDoctorPhoneAreaCode;
        //auth
        private string _passwordRepeat;
        private string _usernameRegister;
        private string _passwordRegister;

        public DoctorRegister()
        {
            _newDoctorName = "";
            _newDoctorSurname = "";
            _newDoctorSex = 1;
            _newDoctorPesel = "";
            _newDoctorEmail = "";
            _newDoctorPhone = "";
            _newDoctorPhoneAreaCode = InputDataHelper.PhoneAreaCodes.First();

            _passwordRepeat = "";
            _usernameRegister = "";
            _passwordRegister = "";
        }
        public override void Back()
        {
            OnBackEvent();
        }

        public override void Render(bool isAdmin)
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
                Back();
                // TODO: add using
                MySqlDataReader reader = Program.dbService.ExecuteFromSql("Select login from login_info;");
                while (reader.Read())
                {
                    if (reader.GetString(0) == _usernameRegister)
                    {
                        _usernameRegister = "";

                    }
                }
                reader.Close();
                if (_newDoctorName != "" && _newDoctorSurname != "" && _newDoctorPesel != "" && _newDoctorPhone != "" && _usernameRegister != "")
                {
                    if (_usernameRegister != "" && _passwordRegister != "" && _passwordRepeat != "" && _passwordRegister == _passwordRepeat)
                    {

                        string _passwdhashed = BCrypt.Net.BCrypt.EnhancedHashPassword(_passwordRepeat);
                        //to do: add to database DONE 
                        // to do: remained adding frontend validation ( backend validation already exsists)
                        // inserting data to database
                        var res = Program.dbService.ExecuteNonQuery("INSERT INTO `doctors` (first_name, sur_name, sex, PESEL, email, phone) " +
                        "VALUES('" + _newDoctorName + "','" + _newDoctorSurname + "','" + _newDoctorSex + "', '" + _newDoctorPesel + "','" + _newDoctorEmail + "','" + _newDoctorPhone + "');");
                        // inserting login credentials to database
                        var res1 = Program.dbService.ExecuteNonQuery("INSERT INTO `login_info` (login, password) " +
                       "VALUES('" + _usernameRegister + "','" + _passwdhashed + "');");
                    }
                }
               
            }
        }
    }
}
