﻿using ImGuiNET;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using X_RayPalette.Components;
using X_RayPalette.Helpers;
using System.Globalization;

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
        private bool _invalidData;
        private bool _registeredMessage;
        
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
            _invalidData = false;
            _registeredMessage = false;
            _newPatientPhoneAreaCode = InputDataHelper.PhoneAreaCodes.First();
        }
        public override void Render(bool isAdmin)
        {

            // testing textInput components
            new TextInput(_newPatientName,"##name##").IsRequired().Width(208).Title("Name:",110).OnInput((v)=> _newPatientName=v).Render();
            new TextInput(_newPatientSurname, "##surname##").IsRequired().Width(208).Title("Surname:", 110).OnInput((v) => _newPatientSurname = v).Render();

            ImGui.Text("Sex: ");
            ImGui.SameLine(110);

            ImGui.RadioButton("Men", ref _newPatientSex, 1);
            ImGui.SameLine(200);
            ImGui.RadioButton("Woman", ref _newPatientSex, 2);

            new TextInput(_newPatientPesel, "##pesel##").MaxLength(11).IsRequired().Width(208).InputType(ImGuiInputTextFlags.CharsDecimal).Title("PESEL:", 110).OnInput((v) => _newPatientPesel = v).Render();
            new TextInput(_newPatientEmail, "##emailadress##").Width(208).Title("E-mail:", 110).OnInput((v) => _newPatientEmail = v).Render();

            ImGui.Text("Phone: ");
            ImGui.SameLine(110);
            ImGui.PopItemWidth();
            new ComboBox<PhoneAreaCode>(_newPatientPhoneAreaCode, "##phoneArea", InputDataHelper.PhoneAreaCodes).Width(50).OnSelect((x) => _newPatientPhoneAreaCode = x).Render();
            ImGui.SameLine(0);
            new TextInput(_newPatientPhone, "##phone##").Width(150).IsRequired().MaxLength(15).InputType(ImGuiInputTextFlags.CharsDecimal).OnInput((v) => _newPatientPhone = v).Render();
            ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "\u002A - required field");

            ImGui.PushItemWidth(208);
            ImGui.Text("Address");
            ImGui.Separator();
            new TextInput(_newPatientCity, "##city##").Width(208).Title("City:", 110).OnInput((v) => _newPatientCity = v).Render();
            new TextInput(_newPatientStreet, "##street##").Width(208).Title("Street:", 110).OnInput((v) => _newPatientStreet = v).Render();
            new TextInput(_newPatientHouseNumber, "##house##").Width(208).Title("House number:", 110).OnInput((v) => _newPatientHouseNumber = v).Render();
            new TextInput(_newPatientFlatNumber, "##flat##").Width(208).Title("Flat number:", 110).OnInput((v) => _newPatientFlatNumber = v).Render();
            new TextInput(_newPatientPostCode, "##post##").Width(208).Title("Post code:", 110).OnInput((v) => _newPatientPostCode = v).Render();
            new TextInput(_newPatientCountry, "##country##").Width(208).Title("Country:", 110).OnInput((v) => _newPatientCountry = v).Render();
            ImGui.Separator();
            ImGui.PopItemWidth();

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
                }).Width(310).Render();
            }
            ImGui.NewLine();
            new Button("Add Patient").OnClick(() =>
            {
                if (isAdmin)
                {
                    System.Console.WriteLine(_tempdataDocAp);

                    var isPeselInDB2 = Program.dbService.IsPESELInDB(_newPatientPesel);
                    if (isPeselInDB2 || !IsValidPesel(_newPatientPesel))
                    {
                        _newPatientPesel = "";
                        _invalidData = true;
                        Thread thread = new Thread(new ThreadStart(WaitAndSetFlag));
                        thread.Start();
                    }

                    if (_newPatientName != "" && _newPatientSurname != "" && _newPatientPesel != "" && _newPatientPhone != "" && IsValidPesel(_newPatientPesel))
                    {
                        //to do: add to database DONE 
                        // to do: remained adding frontend validation ( backend validation already exsists) 
                        string[] InfSelectedDoc = _tempdataDocAp.Split(' ');
                        int LoggedId = Convert.ToInt32(InfSelectedDoc[0]);
                        var res = Program.dbService.ExecuteNonQuery("INSERT INTO `patient` (first_name, sur_name, sex, doctors_id, PESEL, email, phone, City, Street, House_number, Flat_number, Post_code, Country) " +
                        "VALUES('" + _newPatientName + "','" + _newPatientSurname + "','" + _newPatientSex + "', '" + LoggedId + "', '" + _newPatientPesel + "','" + _newPatientEmail + "','" + _newPatientPhone + "','" + _newPatientCity + "','" + _newPatientStreet + "','" + _newPatientHouseNumber + "','" + _newPatientFlatNumber + "','" + _newPatientPostCode + "','" + _newPatientCountry + "');");
                        _registeredMessage = true;
                        Thread thread = new Thread(new ThreadStart(WaitAndSetFlag));
                        thread.Start();
                    }

                }
                var isPeselInDB = Program.dbService.IsPESELInDB(_newPatientPesel);
                if (isPeselInDB || !IsValidPesel(_newPatientPesel))
                {
                    _newPatientPesel = "";
                    _invalidData = true;
                    Thread thread = new Thread(new ThreadStart(WaitAndSetFlag));
                    thread.Start();
                }
                
                if (_newPatientName != "" && _newPatientSurname != "" && _newPatientPesel != "" && _newPatientPhone != "")
                {
                    //to do: add to database DONE 
                    // to do: remained adding frontend validation ( backend validation already exsists) 
                    int LoggedId = Program.dbService.docNametoId(Globals.LoggedDoc);
                    var res = Program.dbService.ExecuteNonQuery("INSERT INTO `patient` (first_name, sur_name, sex, doctors_id, PESEL, email, phone, City, Street, House_number, Flat_number, Post_code, Country) " +
                    "VALUES('" + _newPatientName + "','" + _newPatientSurname + "','" + _newPatientSex + "', '" + LoggedId + "', '" + _newPatientPesel + "','" + _newPatientEmail + "','" + _newPatientPhone + "','" + _newPatientCity + "','" + _newPatientStreet + "','" + _newPatientHouseNumber + "','" + _newPatientFlatNumber + "','" + _newPatientPostCode + "','" + _newPatientCountry + "');");
                    _registeredMessage = true;
                    Thread thread = new Thread(new ThreadStart(WaitAndSetFlag));
                    thread.Start();
                }
            }).Render();
            
            if (_invalidData && !_registeredMessage)
            {
                ImGui.SameLine();
                ImGui.TextColored(new Vector4(0.8f, 0.20f, 0.20f, 0.90f), "Invalid Data");
            }
            
            if (_registeredMessage)
            {
                ImGui.SameLine();
                ImGui.TextColored(new Vector4(0.12f, 0.8f, 0.20f, 0.90f), "Registered");
            }

            OnRenderEvent();
        }

        public override void Back()
        {
            _tempdataDocAp = "Choose Doctor";
            OnBackEvent();
        }
        
        private void WaitAndSetFlag()
        {
            Thread.Sleep(5000);
            
            _registeredMessage = false;
            _invalidData = false;
        }

        private bool IsValidPesel(string pesel)
        {
            if (pesel.Length != 11 || !pesel.All(char.IsDigit))
            {
                return false;
            }
            
            return true;
        }

    }
}
