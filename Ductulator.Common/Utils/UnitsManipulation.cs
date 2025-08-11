using Autodesk.Revit.DB;
using System;
using static Ductulator.Common.Views.ViewModels.UnitsViewModel;

namespace Ductulator.Common.Utils
{
    public static class UnitsManipulation
    {
        public static UnitOption selectedUnit()
        {
            string savedUnit = SettingsConfig.GetValue("selectedunit");
            string key = (savedUnit ?? string.Empty).Trim().ToLowerInvariant();

            switch (key)
            {
                case "millimeters":
                    return new UnitOption("Millimeters", "mm", UnitTypeId.Millimeters);

                case "centimeters":
                    return new UnitOption("Centimeters", "cm", UnitTypeId.Centimeters);

                case "meters":
                    return new UnitOption("Meters", "mts", UnitTypeId.Meters);

                case "feet (decimal)":
                    return new UnitOption("Feet (decimal)", "ft", UnitTypeId.Feet);

                case "inches (decimal)":
                default:
                    return new UnitOption("Inches (decimal)", "in", UnitTypeId.Inches);
            }
        }
    }
}
