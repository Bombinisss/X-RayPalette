using ImGuiNET;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Components;
using X_RayPalette.Helpers;

namespace X_RayPalette.Views.Patient
{
    public class PatientAdd : View
    {
        private string _newPatientName;
        private string _newPatientSurname;
        private int _newPatientSex;
        private string _newPatientPesel;
        private string _newPatientEmail;
        private string _newPatientPhone;

        private string _newPatientCity;
        private string _newPatientStreet;
        private string _newPatientHouseNumber;
        private string _newPatientFlatNumber;
        private string _newPatientPostCode;
        private string _newPatientCountry;

        private string _tempdataDocAp;

        private PhoneAreaCode _newPatientPhoneAreaCode;

        public PatientAdd()
        {

            _tempdataDocAp = "Choose Doctor";

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
        public override void Render(bool isAdmin)
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
            new ComboBox<PhoneAreaCode>(_newPatientPhoneAreaCode, "##phoneArea", InputDataHelper.PhoneAreaCodes).Width(50).OnSelect((x) => _newPatientPhoneAreaCode = x).Render();
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

            if (!isAdmin)
            {
                ImGui.Text("Images");
                ImGui.Separator();
                // TODO: add file Popup and upload images
                new Button("Upload Images").Render();
                ImGui.Separator();             // TODO: display uploaded images
            }

            if (isAdmin)
            {
                List<string> _nameArray = new List<string>();
                MySqlDataReader reader = Program.dbService.ExecuteFromSql("Select doctors_id, first_name, sur_name from doctors");
                while (reader.Read())
                {
                    _nameArray.Add(reader.GetInt32(0) + " " + reader.GetString(1) + " " + reader.GetString(2));
                }
                _nameArray.ToArray();
                reader.Close();
                new ComboBox<string>(_tempdataDocAp, "##Doctorchoose##", _nameArray).OnSelect((string val) =>
                {
                    _tempdataDocAp = val;
                }).Width(350).Render();
            }
            ImGui.NewLine();
            new Button("Add Patient").OnClick(() =>
            {
                if (isAdmin)
                {
                    System.Console.WriteLine(_tempdataDocAp);

                    var isPeselInDB2 = Program.dbService.IsPESELInDB(_newPatientPesel);
                    if (isPeselInDB2)
                    {
                        _newPatientPesel = "";
                    }

                    if (_newPatientName != "" && _newPatientSurname != "" && _newPatientPesel != "" && _newPatientPhone != "")
                    {
                        //to do: add to database DONE 
                        // to do: remained adding frontend validation ( backend validation already exsists) 
                        string[] InfSelectedDoc = _tempdataDocAp.Split(' ');
                        int LoggedId = Convert.ToInt32(InfSelectedDoc[0]);
                        var res = Program.dbService.ExecuteNonQuery("INSERT INTO `patient` (first_name, sur_name, sex, doctors_id, PESEL, email, phone, City, Street, House_number, Flat_number, Post_code, Country) " +
                        "VALUES('" + _newPatientName + "','" + _newPatientSurname + "','" + _newPatientSex + "', '" + LoggedId + "', '" + _newPatientPesel + "','" + _newPatientEmail + "','" + _newPatientPhone + "','" + _newPatientCity + "','" + _newPatientStreet + "','" + _newPatientHouseNumber + "','" + _newPatientFlatNumber + "','" + _newPatientPostCode + "','" + _newPatientCountry + "');");

                    }

                }
                var isPeselInDB = Program.dbService.IsPESELInDB(_newPatientPesel);
                if (isPeselInDB)
                {
                    _newPatientPesel = "";
                }

                if (_newPatientName != "" && _newPatientSurname != "" && _newPatientPesel != "" && _newPatientPhone != "")
                {
                    //to do: add to database DONE 
                    // to do: remained adding frontend validation ( backend validation already exsists) 
                    int LoggedId = Program.dbService.docNametoId(Globals.LoggedDoc);
                    var res = Program.dbService.ExecuteNonQuery("INSERT INTO `patient` (first_name, sur_name, sex, doctors_id, PESEL, email, phone, City, Street, House_number, Flat_number, Post_code, Country) " +
                    "VALUES('" + _newPatientName + "','" + _newPatientSurname + "','" + _newPatientSex + "', '" + LoggedId + "', '" + _newPatientPesel + "','" + _newPatientEmail + "','" + _newPatientPhone + "','" + _newPatientCity + "','" + _newPatientStreet + "','" + _newPatientHouseNumber + "','" + _newPatientFlatNumber + "','" + _newPatientPostCode + "','" + _newPatientCountry + "');");

                }
            }).Render();

            OnRenderEvent();
        }

        public override void Back()
        {
            _tempdataDocAp = "Choose Doctor";
            OnBackEvent();
        }

    }
}
