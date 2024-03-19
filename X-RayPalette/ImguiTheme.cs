using System.Numerics;
using ImGuiNET;

namespace X_RayPalette
{
    partial class Gui
    {
        private const int ThemeNumber = 1; //starting from 0

        public static void SetupImGuiStyle0()
        {
            // Trash style from ImThemes
            var style = ImGui.GetStyle();

            style.Alpha = 1.0f;
            style.DisabledAlpha = 0.6000000238418579f;
            style.WindowPadding = new Vector2(8.0f, 8.0f);
            style.WindowRounding = 0.0f;
            style.WindowBorderSize = 1.0f;
            style.WindowMinSize = new Vector2(32.0f, 32.0f);
            style.WindowTitleAlign = new Vector2(0.0f, 0.5f);
            style.WindowMenuButtonPosition = ImGuiDir.Left;
            style.ChildRounding = 6.900000095367432f;
            style.ChildBorderSize = 1.0f;
            style.PopupRounding = 6.900000095367432f;
            style.PopupBorderSize = 1.0f;
            style.FramePadding = new Vector2(4.0f, 3.0f);
            style.FrameRounding = 6.900000095367432f;
            style.FrameBorderSize = 0.0f;
            style.ItemSpacing = new Vector2(8.0f, 4.0f);
            style.ItemInnerSpacing = new Vector2(4.0f, 4.0f);
            style.CellPadding = new Vector2(4.0f, 2.0f);
            style.IndentSpacing = 21.0f;
            style.ColumnsMinSpacing = 6.0f;
            style.ScrollbarSize = 14.0f;
            style.ScrollbarRounding = 6.900000095367432f;
            style.GrabMinSize = 10.0f;
            style.GrabRounding = 6.900000095367432f;
            style.TabRounding = 6.900000095367432f;
            style.TabBorderSize = 0.0f;
            style.TabMinWidthForCloseButton = 0.0f;
            style.ColorButtonPosition = ImGuiDir.Right;
            style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
            style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

            style.Colors[(int)ImGuiCol.Text] = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TextDisabled] =
                new Vector4(0.6000000238418579f, 0.6000000238418579f, 0.6000000238418579f, 1.0f);
            style.Colors[(int)ImGuiCol.WindowBg] =
                new Vector4(0.8369098901748657f, 0.8369014859199524f, 0.8369014859199524f, 1.0f);
            style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(1.0f, 1.0f, 1.0f, 0.9800000190734863f);
            style.Colors[(int)ImGuiCol.Border] = new Vector4(0.0f, 0.0f, 0.0f, 0.300000011920929f);
            style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.FrameBg] =
                new Vector4(0.8927038908004761f, 0.8926949501037598f, 0.8926949501037598f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.6407473683357239f, 0.2588235437870026f,
                0.9764705896377563f, 0.4000000059604645f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.6037868857383728f, 0.2588235437870026f,
                0.9764705896377563f, 0.6700000166893005f);
            style.Colors[(int)ImGuiCol.TitleBg] =
                new Vector4(0.95686274766922f, 0.95686274766922f, 0.95686274766922f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgActive] =
                new Vector4(0.8196078538894653f, 0.8196078538894653f, 0.8196078538894653f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(1.0f, 1.0f, 1.0f, 0.5099999904632568f);
            style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(1.0f, 0.9999899864196777f, 0.9999899864196777f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.9764705896377563f, 0.9764705896377563f,
                0.9764705896377563f, 0.5299999713897705f);
            style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.686274528503418f, 0.686274528503418f,
                0.686274528503418f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.4862745106220245f, 0.4862745106220245f,
                0.4862745106220245f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] =
                new Vector4(0.4862745106220245f, 0.4862745106220245f, 0.4862745106220245f, 1.0f);
            style.Colors[(int)ImGuiCol.CheckMark] =
                new Vector4(0.4948495924472809f, 0.2599999904632568f, 0.9800000190734863f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrab] =
                new Vector4(0.4322747886180878f, 0.2400000393390656f, 0.8799999952316284f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrabActive] =
                new Vector4(0.4577679634094238f, 0.2599999904632568f, 0.9800000190734863f, 1.0f);
            style.Colors[(int)ImGuiCol.Button] = new Vector4(0.5113858580589294f, 0.2588235437870026f,
                0.9764705896377563f, 0.4000000059604645f);
            style.Colors[(int)ImGuiCol.ButtonHovered] =
                new Vector4(0.6222667694091797f, 0.2588235437870026f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.ButtonActive] =
                new Vector4(0.452663391828537f, 0.05882354825735092f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.Header] =
                new Vector4(0.4936792254447937f, 0.2489270567893982f, 1.0f, 0.3100000023841858f);
            style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.5113858580589294f, 0.2588235437870026f,
                0.9764705896377563f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.HeaderActive] =
                new Vector4(0.5298660397529602f, 0.2588235437870026f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.3882353007793427f, 0.3882353007793427f,
                0.3882353007793427f, 0.6200000047683716f);
            style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.3875619471073151f, 0.1372549086809158f,
                0.800000011920929f, 0.7799999713897705f);
            style.Colors[(int)ImGuiCol.SeparatorActive] =
                new Vector4(0.4387612342834473f, 0.1372549086809158f, 0.800000011920929f, 1.0f);
            style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.3490196168422699f, 0.3490196168422699f,
                0.3490196168422699f, 0.1700000017881393f);
            style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.5113858580589294f, 0.2588235437870026f,
                0.9764705896377563f, 0.6700000166893005f);
            style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.585306704044342f, 0.2588235437870026f,
                0.9764705896377563f, 0.949999988079071f);
            style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.7927627563476562f, 0.7607843279838562f, 0.8352941274642944f,
                0.9309999942779541f);
            style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.4929056763648987f, 0.2588235437870026f,
                0.9764705896377563f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.TabActive] =
                new Vector4(0.7092316746711731f, 0.5921568870544434f, 0.8823529481887817f, 1.0f);
            style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.9243793487548828f, 0.9176470637321472f,
                0.9333333373069763f, 0.9861999750137329f);
            style.Colors[(int)ImGuiCol.TabUnfocusedActive] =
                new Vector4(0.8241184949874878f, 0.741176426410675f, 0.9137254953384399f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLines] =
                new Vector4(0.3882353007793427f, 0.3882353007793427f, 0.3882353007793427f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLinesHovered] =
                new Vector4(1.0f, 0.4274509847164154f, 0.3490196168422699f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogram] =
                new Vector4(0.0f, 0.606256365776062f, 0.8025751113891602f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.0f, 0.4470588266849518f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TableHeaderBg] =
                new Vector4(0.7764706015586853f, 0.8666666746139526f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderStrong] =
                new Vector4(0.5686274766921997f, 0.5686274766921997f, 0.6392157077789307f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderLight] =
                new Vector4(0.6784313917160034f, 0.6784313917160034f, 0.7372549176216125f, 1.0f);
            style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(0.2980392277240753f, 0.2980392277240753f,
                0.2980392277240753f, 0.09000000357627869f);
            style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.5113858580589294f, 0.2588235437870026f,
                0.9764705896377563f, 0.3499999940395355f);
            style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.41898512840271f, 0.2588235437870026f,
                0.9764705896377563f, 0.949999988079071f);
            style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.5298660397529602f, 0.2588235437870026f,
                0.9764705896377563f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(0.6980392336845398f, 0.6980392336845398f,
                0.6980392336845398f, 0.699999988079071f);
            style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.2000000029802322f, 0.2000000029802322f,
                0.2000000029802322f, 0.2000000029802322f);
            style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.2000000029802322f, 0.2000000029802322f,
                0.2000000029802322f, 0.3499999940395355f);
        }

        public static void SetupImGuiStyle1()
        {
            // Comfy style by Giuseppe from ImThemes
            ImGuiStylePtr style = ImGui.GetStyle();

            style.Alpha = 1.0f;
            style.DisabledAlpha = 0.1000000014901161f;
            style.WindowPadding = new Vector2(8.0f, 8.0f);
            style.WindowRounding = 0.0f;
            style.WindowBorderSize = 0.0f;
            style.WindowMinSize = new Vector2(30.0f, 30.0f);
            style.WindowTitleAlign = new Vector2(0.5f, 0.5f);
            style.WindowMenuButtonPosition = ImGuiDir.Right;
            style.ChildRounding = 5.0f;
            style.ChildBorderSize = 1.0f;
            style.PopupRounding = 10.0f;
            style.PopupBorderSize = 0.0f;
            style.FramePadding = new Vector2(5.0f, 3.5f);
            style.FrameRounding = 5.0f;
            style.FrameBorderSize = 0.0f;
            style.ItemSpacing = new Vector2(5.0f, 4.0f);
            style.ItemInnerSpacing = new Vector2(5.0f, 5.0f);
            style.CellPadding = new Vector2(4.0f, 2.0f);
            style.IndentSpacing = 5.0f;
            style.ColumnsMinSpacing = 5.0f;
            style.ScrollbarSize = 15.0f;
            style.ScrollbarRounding = 9.0f;
            style.GrabMinSize = 15.0f;
            style.GrabRounding = 5.0f;
            style.TabRounding = 5.0f;
            style.TabBorderSize = 0.0f;
            style.TabMinWidthForCloseButton = 0.0f;
            style.ColorButtonPosition = ImGuiDir.Right;
            style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
            style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

            style.Colors[(int)ImGuiCol.Text] = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(1.0f, 1.0f, 1.0f, 0.3605149984359741f);
            style.Colors[(int)ImGuiCol.WindowBg] =
                new Vector4(0.09803921729326248f, 0.09803921729326248f, 0.09803921729326248f, 1.0f);
            style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.PopupBg] =
                new Vector4(0.09803921729326248f, 0.09803921729326248f, 0.09803921729326248f, 1.0f);
            style.Colors[(int)ImGuiCol.Border] = new Vector4(0.4235294163227081f, 0.3803921639919281f,
                0.572549045085907f,
                0.54935622215271f);
            style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.FrameBg] =
                new Vector4(0.1568627506494522f, 0.1568627506494522f, 0.1568627506494522f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.3803921639919281f, 0.4235294163227081f,
                0.572549045085907f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.6196078658103943f, 0.5764706134796143f,
                0.7686274647712708f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.TitleBg] =
                new Vector4(0.09803921729326248f, 0.09803921729326248f, 0.09803921729326248f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgActive] =
                new Vector4(0.09803921729326248f, 0.09803921729326248f, 0.09803921729326248f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgCollapsed] =
                new Vector4(0.2588235437870026f, 0.2588235437870026f, 0.2588235437870026f, 0.0f);
            style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.ScrollbarBg] =
                new Vector4(0.1568627506494522f, 0.1568627506494522f, 0.1568627506494522f, 0.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrab] =
                new Vector4(0.1568627506494522f, 0.1568627506494522f, 0.1568627506494522f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] =
                new Vector4(0.2352941185235977f, 0.2352941185235977f, 0.2352941185235977f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] =
                new Vector4(0.294117659330368f, 0.294117659330368f, 0.294117659330368f, 1.0f);
            style.Colors[(int)ImGuiCol.CheckMark] =
                new Vector4(0.294117659330368f, 0.294117659330368f, 0.294117659330368f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.6196078658103943f, 0.5764706134796143f,
                0.7686274647712708f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.8156862854957581f, 0.772549033164978f,
                0.9647058844566345f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.Button] = new Vector4(0.6196078658103943f, 0.5764706134796143f,
                0.7686274647712708f,
                0.5490196347236633f);
            style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.7372549176216125f, 0.6941176652908325f,
                0.886274516582489f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.41f, 0.41f, 0.41f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.Header] = new Vector4(0.6196078658103943f, 0.5764706134796143f,
                0.7686274647712708f,
                0.5490196347236633f);
            style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.7372549176216125f, 0.6941176652908325f,
                0.886274516582489f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.8156862854957581f, 0.772549033164978f,
                0.9647058844566345f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.6196078658103943f, 0.5764706134796143f,
                0.7686274647712708f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.7372549176216125f, 0.6941176652908325f,
                0.886274516582489f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.8156862854957581f, 0.772549033164978f,
                0.9647058844566345f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.6196078658103943f, 0.5764706134796143f,
                0.7686274647712708f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.7372549176216125f, 0.6941176652908325f,
                0.886274516582489f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.8156862854957581f, 0.772549033164978f,
                0.9647058844566345f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.6196078658103943f, 0.5764706134796143f, 0.7686274647712708f,
                0.5490196347236633f);
            style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.7372549176216125f, 0.6941176652908325f,
                0.886274516582489f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.8156862854957581f, 0.772549033164978f,
                0.9647058844566345f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.0f, 0.4509803950786591f, 1.0f, 0.0f);
            style.Colors[(int)ImGuiCol.TabUnfocusedActive] =
                new Vector4(0.1333333402872086f, 0.2588235437870026f, 0.4235294163227081f, 0.0f);
            style.Colors[(int)ImGuiCol.PlotLines] =
                new Vector4(0.294117659330368f, 0.294117659330368f, 0.294117659330368f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(0.7372549176216125f, 0.6941176652908325f,
                0.886274516582489f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.6196078658103943f, 0.5764706134796143f,
                0.7686274647712708f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.7372549176216125f, 0.6941176652908325f,
                0.886274516582489f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.TableHeaderBg] =
                new Vector4(0.1882352977991104f, 0.1882352977991104f, 0.2000000029802322f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.4235294163227081f, 0.3803921639919281f,
                0.572549045085907f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.4235294163227081f, 0.3803921639919281f,
                0.572549045085907f, 0.2918455004692078f);
            style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.0f, 1.0f, 1.0f, 0.03433477878570557f);
            style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.7372549176216125f, 0.6941176652908325f,
                0.886274516582489f, 0.5490196347236633f);
            style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(1.0f, 1.0f, 0.0f, 0.8999999761581421f);
            style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.0f, 1.0f, 1.0f, 0.699999988079071f);
            style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f,
                0.800000011920929f, 0.2000000029802322f);
            style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f,
                0.800000011920929f, 0.3499999940395355f);
        }
    }
}