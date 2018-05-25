using System;
using System.IO;
using System.IO.Ports;
using System.Collections;
using System.Windows.Forms; // for Application.StartupPath

namespace Termie
{
    /// <summary>
    /// Persistent settings
    /// </summary>
    public class Settings
    {
        /// <summary> Port settings. </summary>
        public class Port
        {
            public static string PortName = "COM1";
            public static int BaudRate = 9600;
            public static int DataBits = 8;
            public static System.IO.Ports.Parity Parity = System.IO.Ports.Parity.None;
            public static System.IO.Ports.StopBits StopBits = System.IO.Ports.StopBits.One;
            public static System.IO.Ports.Handshake Handshake = System.IO.Ports.Handshake.None;
        }

        /// <summary> Option settings. </summary>
        public class Option
        {
            public enum AppendType
            {
                AppendNothing,
                AppendCR,
                AppendLF,
                AppendCRLF
            }

            public static AppendType AppendToSend = AppendType.AppendCR;
            public static bool HexOutput = false;
            public static bool MonoFont = true;
            public static bool LocalEcho = true;
            public static bool StayOnTop = false;
			public static bool FilterUseCase = false;
			public static string LogFileName = "";
		}

        public class personalized
        {
            public static string s_com_name_A;
            public static string s_com_name_B;
            public static string s_com_detail_A;
            public static string s_com_detail_A_2;
            public static string s_com_detail_B;
            public static string s_com_detail_B_2;
            public static string s_app_name;
            
        }
        public static string s_printer_name;
        public static void Read_personalized()
        {
            IniFile ini = new IniFile(Application.StartupPath + "\\personalized.ini");
            personalized.s_com_name_A = ini.ReadValue("COMA", "com_name", personalized.s_com_name_A);
            personalized.s_com_detail_A = ini.ReadValue("COMA", "com_detail", personalized.s_com_detail_A);
            personalized.s_com_detail_A_2 = ini.ReadValue("COMA", "com_detail2", personalized.s_com_detail_A_2);
            personalized.s_com_name_B = ini.ReadValue("COMB", "com_name", personalized.s_com_name_B);
            personalized.s_com_detail_B = ini.ReadValue("COMB", "com_detail", personalized.s_com_detail_B);
            personalized.s_com_detail_B_2 = ini.ReadValue("COMB", "com_detail2", personalized.s_com_detail_B_2);
            personalized.s_app_name = ini.ReadValue("APPNAME", "app_name", personalized.s_app_name);
            
        }

        public static void Write_personalized()
        {
            IniFile ini = new IniFile(Application.StartupPath + "\\personalized.ini");
            ini.WriteValue("COMA", "com_name", personalized.s_com_name_A);
            ini.WriteValue("COMA", "com_detail", personalized.s_com_detail_A);
            ini.WriteValue("COMA", "com_detail2", personalized.s_com_detail_A_2);
            ini.WriteValue("COMB", "com_name", personalized.s_com_name_B);
            ini.WriteValue("COMB", "com_detail", personalized.s_com_detail_B);
            ini.WriteValue("COMB", "com_detail2", personalized.s_com_detail_B_2);
            ini.WriteValue("APPNAME", "app_name", personalized.s_app_name);

        }
        /// <summary>
        ///   Read the settings from disk. </summary>
        public static void Read()
        {
            IniFile ini = new IniFile(Application.StartupPath + "\\Settings.ini");
            Port.PortName = ini.ReadValue("Port", "PortName", Port.PortName);
            Port.BaudRate = ini.ReadValue("Port", "BaudRate", Port.BaudRate);
            Port.DataBits = ini.ReadValue("Port", "DataBits", Port.DataBits);
            Port.Parity = (Parity)Enum.Parse(typeof(Parity), ini.ReadValue("Port", "Parity", Port.Parity.ToString()));
            Port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), ini.ReadValue("Port", "StopBits", Port.StopBits.ToString()));
            Port.Handshake = (Handshake)Enum.Parse(typeof(Handshake), ini.ReadValue("Port", "Handshake", Port.Handshake.ToString()));

            Option.AppendToSend = (Option.AppendType)Enum.Parse(typeof(Option.AppendType), ini.ReadValue("Option", "AppendToSend", Option.AppendToSend.ToString()));
            Option.HexOutput = bool.Parse(ini.ReadValue("Option", "HexOutput", Option.HexOutput.ToString()));
            Option.MonoFont = bool.Parse(ini.ReadValue("Option", "MonoFont", Option.MonoFont.ToString()));
            Option.LocalEcho = bool.Parse(ini.ReadValue("Option", "LocalEcho", Option.LocalEcho.ToString()));
			Option.StayOnTop = bool.Parse(ini.ReadValue("Option", "StayOnTop", Option.StayOnTop.ToString()));
			Option.FilterUseCase = bool.Parse(ini.ReadValue("Option", "FilterUseCase", Option.FilterUseCase.ToString()));

			s_printer_name =ini.ReadValue("Printer", "PrinterName", s_printer_name);
		}

        /// <summary>
        ///   Write the settings to disk. </summary>
        public static void Write()
        {
            IniFile ini = new IniFile(Application.StartupPath + "\\Settings.ini");
            ini.WriteValue("Port", "PortName", Port.PortName);
            ini.WriteValue("Port", "BaudRate", Port.BaudRate);
            ini.WriteValue("Port", "DataBits", Port.DataBits);
            ini.WriteValue("Port", "Parity", Port.Parity.ToString());
            ini.WriteValue("Port", "StopBits", Port.StopBits.ToString());
            ini.WriteValue("Port", "Handshake", Port.Handshake.ToString());

            ini.WriteValue("Option", "AppendToSend", Option.AppendToSend.ToString());
            ini.WriteValue("Option", "HexOutput", Option.HexOutput.ToString());
            ini.WriteValue("Option", "MonoFont", Option.MonoFont.ToString());
            ini.WriteValue("Option", "LocalEcho", Option.LocalEcho.ToString());
			ini.WriteValue("Option", "StayOnTop", Option.StayOnTop.ToString());
			ini.WriteValue("Option", "FilterUseCase", Option.FilterUseCase.ToString());

			ini.WriteValue("Printer", "PrinterName", s_printer_name);
		}
	}
}
