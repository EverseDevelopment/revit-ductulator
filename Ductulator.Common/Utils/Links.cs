using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Ductulator.Common.Utils
{
    public class Links
    {
        public static string contactLink = "https://e-verse.com/contact/";
        public static string everseWebsite = "https://e-verse.com";
        public static string ductulatorWebsite = "https://e-verse.com/revit-ductulator-add-in-nancy-duct-resizing/";
        public static string configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "everse");
    }
}
