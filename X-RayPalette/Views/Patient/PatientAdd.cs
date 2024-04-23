using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
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

        private string _tempdataDoc_AP;

        private PhoneAreaCode _newPatientPhoneAreaCode;

        public PatientAdd()
        {

            _tempdataDoc_AP = "Choose Doctor";

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

            if (!isAdmin)
            {
                ImGui.Text("Images");
                ImGui.Separator();
                ImGui.Button("Upload Images"); // TODO: add file Popup and upload images
                ImGui.Separator();             // TODO: display uploaded images
            }

            if (isAdmin)
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
                if (isAdmin)
                {
                    Back();
                    // TODO: add patient to database from admin view (to choosen doctor)
                }
                // TODO: add patient to database
            }
            OnRenderEvent();
        }

        public override void Back()
        {
            _tempdataDoc_AP = "Choose Doctor";
            OnBackEvent();
        }

    }
}
