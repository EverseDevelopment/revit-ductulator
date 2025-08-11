using Autodesk.Revit.DB;
using Ductulator.Common.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ductulator.Common.Views.ViewModels
{
    public class UnitsViewModel : ViewModelBase
    {
        // Item for the ComboBox
        public class UnitOption
        {
            public string UnitName { get; }
            public string Abbreviation { get; }
            public ForgeTypeId RevitUnit { get; }

            public UnitOption(string unitName, string abbreviation, ForgeTypeId revitUnit)
            {
                UnitName = unitName;
                Abbreviation = abbreviation;
                RevitUnit = revitUnit;
            }

            public override string ToString() => UnitName;
        }

        // Common LENGTH units for HVAC ducts
        public ObservableCollection<UnitOption> LengthUnits { get; } =
            new ObservableCollection<UnitOption>
            {
                new UnitOption("Millimeters", "mm", UnitTypeId.Millimeters),
                new UnitOption("Centimeters", "cm", UnitTypeId.Centimeters),
                new UnitOption("Meters", "mts", UnitTypeId.Meters),
                new UnitOption("Inches (decimal)", "in", UnitTypeId.Inches),
                new UnitOption("Feet (decimal)", "ft", UnitTypeId.Feet),
                // Optional:
                // new UnitOption("Feet & Fractional Inches", "ft-in", UnitTypeId.FeetFractionalInches),
            };

        private UnitOption _selectedLengthUnit;
        public UnitOption SelectedLengthUnit
        {
            get => _selectedLengthUnit;
            set
            {
                if (SetProperty(ref _selectedLengthUnit, value))
                {
                    SettingsConfig.SetValue("selectedunit", _selectedLengthUnit.UnitName);
                }
            }
        }

        public UnitsViewModel()
        {
            var saved = SettingsConfig.GetValue("selectedunit") ?? "Inches (decimal)";

            _selectedLengthUnit = LengthUnits
                .FirstOrDefault(u => u.UnitName.Equals(saved, StringComparison.OrdinalIgnoreCase))
                ?? LengthUnits.First(u => u.UnitName == "Inches (decimal)");

            OnPropertyChanged(nameof(SelectedLengthUnit));
        }

        // Convert from Revit internal units (feet) to selected unit
        public double FromInternalLength(double internalFeetValue)
        {
            var unitId = SelectedLengthUnit?.RevitUnit ?? UnitTypeId.Millimeters;
            return UnitUtils.ConvertFromInternalUnits(internalFeetValue, unitId);
        }

        // Convert to Revit internal units (feet) from selected unit
        public double ToInternalLength(double userValue)
        {
            var unitId = SelectedLengthUnit?.RevitUnit ?? UnitTypeId.Millimeters;
            return UnitUtils.ConvertToInternalUnits(userValue, unitId);
        }
    }
}
