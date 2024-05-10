using ImGuiNET;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Components;
using X_RayPalette.Helpers;


namespace X_RayPalette.Views.InfoChange
{

    internal class DoctorInfoChange : View
    {
        private string _tempdataDocCi;

        private string _updateDoctorName;
        private string _updateDoctorSurname;
        private int _updateDoctorSex;
        private string _updateDoctorPesel;
        private string _updateDoctorEmail;
        private string _updateDoctorPhone;
        private PhoneAreaCode _updateDoctorPhoneAreaCode;
        //auth
        private string _updatePasswordRepeat;
        private string _updateUsernameRegister;
        private string _updatePasswordRegister;

        public DoctorInfoChange()
        {
            _tempdataDocCi = "Choose Doctor";
            _updateDoctorName = "";
            _updateDoctorSurname = "";
            _updateDoctorSex = 1;
            _updateDoctorPesel = "";
            _updateDoctorEmail = "";
            _updateDoctorPhone = "";
            _updateDoctorPhoneAreaCode = InputDataHelper.PhoneAreaCodes.First();
            _updatePasswordRepeat = "";
            _updateUsernameRegister = "";
            _updatePasswordRegister = "";
        }
        public override void Back()
        {
            _tempdataDocCi = "";
            OnBackEvent();
        }

        public override void Render(bool isAdmin)
        {
            ImGui.Text("Choose:");
            ImGui.SameLine();
            List<string> _nameArray = new List<string>();
            MySqlDataReader reader = Program.dbService.ExecuteFromSql("Select doctors_id, first_name, sur_name from doctors");
            while (reader.Read())
            {
                _nameArray.Add(reader.GetInt32(0) + " " + reader.GetString(1) + " " + reader.GetString(2));
            }
            _nameArray.ToArray();
            reader.Close();
            string old,n;
            new ComboBox<string>(_tempdataDocCi, "##DocInfoChange##", _nameArray).OnSelect((string _selectedDocString) => _tempdataDocCi = _selectedDocString)
            .OnValueChange((old, n)=>
            {
                string[] _selectedDocIdName = n.Split(' ');
                try
                {
                    string _selectedDocId = _selectedDocIdName[0];
                    List<dynamic> _MainReaderList = new List<dynamic>();
                    MySqlDataReader MainReader = Program.dbService.ExecuteFromSql("Select first_name, sur_name, sex, PESEL, email, phone from doctors where doctors_id = '"+ _selectedDocId + "';");
                    while(MainReader.Read())
                    {
                        _MainReaderList.Add(MainReader.GetString(0));
                        _MainReaderList.Add(MainReader.GetString(1));
                        _MainReaderList.Add(MainReader.GetInt32(2));
                        _MainReaderList.Add(MainReader.GetString(3));
                        _MainReaderList.Add(MainReader.GetString(4));
                        _MainReaderList.Add(MainReader.GetInt32(5));
                    }
                    MainReader.Close();
                    dynamic[] data = _MainReaderList.ToArray();
                    _updateDoctorName = data[0];
                    _updateDoctorSurname = data[1];
                    _updateDoctorSex = Convert.ToInt32(data[2]);
                    _updateDoctorPesel = data[3];
                    _updateDoctorEmail = data[4];
                    _updateDoctorPhone = Convert.ToString(data[5]);
                    _updatePasswordRepeat = "";
                    _updateUsernameRegister = "";
                    _updatePasswordRegister = "";

              
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                    _tempdataDocCi = "Choose Doctor";
                    _updateDoctorName = "";
                    _updateDoctorSurname = "";
                    _updateDoctorSex = 1;
                    _updateDoctorPesel = "";
                    _updateDoctorEmail = "";
                    _updateDoctorPhone = "";
                    _updateDoctorPhoneAreaCode = InputDataHelper.PhoneAreaCodes.First();
                    _updatePasswordRepeat = "";
                    _updateUsernameRegister = "";
                    _updatePasswordRegister = "";

                }
         
            }).Render();



                ImGui.Separator();

                    ImGui.Text("Name: ");
                    ImGui.SameLine(110);
                    ImGui.InputText("##Doctorname##", ref _updateDoctorName, 128);
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                    ImGui.Text("Surname: ");
                    ImGui.SameLine(110);
                    ImGui.InputText("##Doctorsurname##", ref _updateDoctorSurname, 128);
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                    ImGui.Text("Sex: ");
                    ImGui.SameLine(110);

                    ImGui.RadioButton("Men", ref _updateDoctorSex, 1);
                    ImGui.SameLine(200);
                    ImGui.RadioButton("Woman", ref _updateDoctorSex, 2);

                    ImGui.Text("PESEL: ");
                    ImGui.SameLine(110);
                    ImGui.InputText("##Doctorpesel##", ref _updateDoctorPesel, 11);
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                    ImGui.Text("E-mail: ");
                    ImGui.SameLine(110);
                    ImGui.InputText("##Doctoremailadress##", ref _updateDoctorEmail, 128);

                    ImGui.Text("Phone: ");
                    ImGui.SameLine(110);
                    new ComboBox<PhoneAreaCode>(_updateDoctorPhoneAreaCode, "##DoctorphoneArea", InputDataHelper.PhoneAreaCodes).Width(50).OnSelect((x) => _updateDoctorPhoneAreaCode = x).Render();
                    ImGui.SameLine(0);
                    ImGui.PushItemWidth(150);
                    ImGui.InputText("##Doctorphone##", ref _updateDoctorPhone, 15, ImGuiInputTextFlags.CharsDecimal);
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A - required field");

                    ImGui.Separator();
                    ImGui.Text("Username: ");
                    ImGui.SameLine(110);
                    ImGui.InputText("##usernameReg##", ref _updateUsernameRegister, 128);
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");
                    ImGui.Text("Password: ");
                    ImGui.SameLine(110);
                    ImGui.InputText("##passwdReg##", ref _updatePasswordRegister, 128, ImGuiInputTextFlags.Password);
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");
                    ImGui.Text("Confirm Pwd: ");
                    ImGui.SameLine(110);
                    ImGui.InputText("##passwd2##", ref _updatePasswordRepeat, 128, ImGuiInputTextFlags.Password);
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A");

                    ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A - required field");
                    ImGui.Separator();
                

                new Button("Confirm changes").OnClick(Back).Render();


        }
    }
}
