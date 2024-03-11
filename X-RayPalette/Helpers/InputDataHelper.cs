using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace X_RayPalette.Helpers
{
    public static class InputDataHelper
    {
        public static readonly List<PhoneAreaCode> PhoneAreaCodes = new List<PhoneAreaCode>()
        {
            new PhoneAreaCode("48","Poland"),
            new PhoneAreaCode("49","Germany"),
            new PhoneAreaCode("44","United Kingdom"),
            new PhoneAreaCode("1","United States"),
            new PhoneAreaCode("33","France"),
            new PhoneAreaCode("34","Spain"),
            new PhoneAreaCode("39","Italy"),
            new PhoneAreaCode("81","Japan"),
            new PhoneAreaCode("82","South Korea"),
            new PhoneAreaCode("86","China"),
            new PhoneAreaCode("7","Russia"),
            new PhoneAreaCode("61","Australia"),
            new PhoneAreaCode("64","New Zealand"),
            new PhoneAreaCode("31","Netherlands"),
            new PhoneAreaCode("32","Belgium"),
            new PhoneAreaCode("351","Portugal"),
            new PhoneAreaCode("352","Luxembourg"),
            new PhoneAreaCode("353","Ireland"),
            new PhoneAreaCode("354","Iceland"),
            new PhoneAreaCode("355","Albania"),
            new PhoneAreaCode("356","Malta"),
            new PhoneAreaCode("357","Cyprus"),
            new PhoneAreaCode("358","Finland"),
            new PhoneAreaCode("359","Bulgaria"),
            new PhoneAreaCode("370","Lithuania"),
            new PhoneAreaCode("371","Latvia"),
            new PhoneAreaCode("372","Estonia"),
            new PhoneAreaCode("373","Moldova"),
            new PhoneAreaCode("374","Armenia"),
            new PhoneAreaCode("375","Belarus"),
            new PhoneAreaCode("376","Andorra"),
            new PhoneAreaCode("377","Monaco"),
            new PhoneAreaCode("378","San Marino"),
            new PhoneAreaCode("380","Ukraine"),
            new PhoneAreaCode("381","Serbia"),
            new PhoneAreaCode("382","Montenegro"),
            new PhoneAreaCode("385","Croatia"),
            new PhoneAreaCode("386","Slovenia"),
            new PhoneAreaCode("387","Bosnia and Herzegovina"),
            new PhoneAreaCode("389","Macedonia"),
            new PhoneAreaCode("420","Czech Republic"),
            new PhoneAreaCode("421","Slovakia"),
            new PhoneAreaCode("423","Liechtenstein"),
            new PhoneAreaCode("500","Falkland Islands"),
            new PhoneAreaCode("501","Belize"),
            new PhoneAreaCode("502","Guatemala")
        };
    }
    public class PhoneAreaCode
    {
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public PhoneAreaCode(string areaCode,string areaName)
        {
            AreaCode = areaCode;
            AreaName = areaName;
        }
    }
}
