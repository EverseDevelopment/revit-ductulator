using Autodesk.Revit.DB;
using Ductulator.Common.Utils;
using Ductulator.Core;
using Ductulator.Model;
using Ductulator.Views_Cs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using static Ductulator.Common.Views.ViewModels.UnitsViewModel;

namespace Ductulator.Common.Views.ViewModels
{
    public class MainFormViewModel : ViewModelBase
    {
        private const double MinInches = 0.01;
        private double _lastValidA, _lastValidB;

        private UnitOption lastSelectedUnit;

        public enum DuctShapeEnum
        {
            Round,
            Rectangular
        }

        private bool isUpdatingSize = false;
        private static string currentDuctSizeAsString;
        private static string currentDuctTypeAsString;
        private static string projectVersion;
        private static double desiredDiameterDuct;

        private static UnitOption selectedUnit;

        private static double desiredALengthDuct;
        private static double desiredBLengthDuct;

        private static Element currentElm;

        private Dictionary<string, ElementId> ductTypes;
        private ElementId selectedDuctTypeId;
        private DuctShapeEnum currentductShape;

        private System.Windows.Visibility visibilityRound;
        private System.Windows.Visibility visibilitySquare;

        public MainFormViewModel(Element elm)
        {
            Document doc = App.RevitCollectorService.GetDocument();
            currentElm = elm;

            currentDuctSizeAsString = GetCalculatedSize.ElmCalSize(elm);
            CurrentDuctTypeAsString = CurrentDuctType.ElmType(elm);

            DuctTypes = ModelDuctTypes.elmnt(doc);

            if (DuctTypes.ContainsKey(CurrentDuctTypeAsString))
                SelectedDuctTypeId = DuctTypes[CurrentDuctTypeAsString];

            string shape = CurrentDuctShape.elmShape(elm);
            InitialCalculation(shape);
            UpdateVisibility();

            lastSelectedUnit = new UnitOption("Feet (decimal)", "ft", UnitTypeId.Feet);
            SelectedUnit = UnitsManipulation.selectedUnit();
            ProjectVersion = "1.0.0.0";
        }

        public string CurrentDuctSizeAsString
        {
            get => currentDuctSizeAsString;
            set
            {
                currentDuctSizeAsString = value;
                OnPropertyChanged();
            }
        }

        public string CurrentDuctTypeAsString
        {
            get => currentDuctTypeAsString;
            set
            {
                currentDuctTypeAsString = value;
                OnPropertyChanged();
            }
        }

        public string ProjectVersion
        {
            get => projectVersion;
            set
            {
                projectVersion = value;
                OnPropertyChanged();
            }
        }

        
        public UnitOption SelectedUnit
        {
            get => selectedUnit;
            set
            {
                selectedUnit = value;
                OnPropertyChanged();
                double valueA = UnitUtils.Convert(DesiredALengthDuct, lastSelectedUnit.RevitUnit, selectedUnit.RevitUnit);
                SetDesiredALengthWithoutUpdatingB(valueA);
                double valueB = UnitUtils.Convert(DesiredBLengthDuct, lastSelectedUnit.RevitUnit, selectedUnit.RevitUnit);
                SetDesiredBLengthWithoutUpdatingA(valueB);
                DesiredDiameterDuct = UnitUtils.Convert(DesiredDiameterDuct, lastSelectedUnit.RevitUnit, selectedUnit.RevitUnit);
                lastSelectedUnit = value;
            }
        }

        public Dictionary<string, ElementId> DuctTypes
        {
            get => ductTypes;
            set
            {
                ductTypes = value;
                OnPropertyChanged();
            }
        }

        public ElementId SelectedDuctTypeId
        {
            get => selectedDuctTypeId;
            set
            {
                selectedDuctTypeId = value;
                OnPropertyChanged();
                UpdateShape(value);
            }
        }

        public DuctShapeEnum CurrentductShape
        {
            get => currentductShape;
            set
            {
                if (value != currentductShape)
                {
                    currentductShape = value;
                    OnPropertyChanged();
                    UpdateVisibility();
                    UpdateShapeCalculation();
                }
            }
        }

        public System.Windows.Visibility VisibilityRound
        {
            get => visibilityRound;
            private set
            {
                visibilityRound = value;
                OnPropertyChanged();
            }
        }

        public System.Windows.Visibility VisibilitySquare
        {
            get => visibilitySquare;
            private set
            {
                visibilitySquare = value;
                OnPropertyChanged();
            }
        }

        public double DesiredDiameterDuct
        {
            get => desiredDiameterDuct;
            set
            {
                desiredDiameterDuct = value;
                OnPropertyChanged();
            }
        }

        public double DesiredALengthDuct
        {
            get => desiredALengthDuct;
            set
            {
                if (value < MinInches || double.IsNaN(value) || double.IsInfinity(value))
                {
                    return;
                }

                desiredALengthDuct = value;
                _lastValidA = value;
                OnPropertyChanged();

                if (!isUpdatingSize)
                {
                    isUpdatingSize = true;
                    double newB = ResizeRectangular.Ductulate(0.1, DesiredDiameterDuct, desiredALengthDuct);

                    if (newB >= MinInches)
                    {
                        desiredBLengthDuct = newB;
                        _lastValidB = desiredBLengthDuct;
                        OnPropertyChanged(nameof(DesiredBLengthDuct));
                    }
                    isUpdatingSize = false;
                }
            }
        }

        public double DesiredBLengthDuct
        {
            get => desiredBLengthDuct;
            set
            {
                if (value < MinInches || double.IsNaN(value) || double.IsInfinity(value))
                {
                    return;
                }

                desiredBLengthDuct = value;
                _lastValidB = value;
                OnPropertyChanged();

                if (!isUpdatingSize)
                {
                    isUpdatingSize = true;
                    double newA = ResizeRectangular.Ductulate(0.1, DesiredDiameterDuct, desiredBLengthDuct);

                    if (newA >= MinInches)
                    {
                        desiredALengthDuct = newA;
                        _lastValidA = desiredALengthDuct;
                        OnPropertyChanged(nameof(DesiredALengthDuct));
                    }
                    isUpdatingSize = false;
                }
            }
        }

        private void UpdateShape(ElementId elemId)
        {
            CurrentductShape = CurrentDuctShape.GetDuctShape(elemId);
        }

        private void UpdateVisibility()
        {
            if (currentductShape == DuctShapeEnum.Round)
            {
                VisibilityRound = System.Windows.Visibility.Visible;
                VisibilitySquare = System.Windows.Visibility.Collapsed;
            }
            else
            {
                VisibilityRound = System.Windows.Visibility.Collapsed;
                VisibilitySquare = System.Windows.Visibility.Visible;
            }
        }

        private void InitialCalculation(string shape)
        {
            if (shape == "Round")
            {
                desiredDiameterDuct = OverallSizes.RoundSize(currentElm);
                double temresult = desiredDiameterDuct;
                double ductSide = DuctSide_equiv.Side_equiv(temresult);
                SetDesiredALengthWithoutUpdatingB(ductSide);
                SetDesiredBLengthWithoutUpdatingA(ductSide);
            }
            else
            {
                SetDesiredALengthWithoutUpdatingB(OverallSizes.HeigthSize(currentElm));
                SetDesiredBLengthWithoutUpdatingA(OverallSizes.WidthSize(currentElm));
                desiredDiameterDuct = DuctDiamEquiv.Diam_equiv(desiredALengthDuct, desiredBLengthDuct);
            }
        }


        private void UpdateShapeCalculation()
        {

            if (currentductShape == DuctShapeEnum.Round)
            {
                if (desiredALengthDuct != 0 && desiredBLengthDuct != 0)
                {
                    desiredDiameterDuct = DuctDiamEquiv.Diam_equiv(desiredALengthDuct, desiredBLengthDuct);
                }
            }
            else
            {
                double temresult = desiredDiameterDuct;
                double tempvalue = DuctSide_equiv.Side_equiv(temresult);
                SetDesiredALengthWithoutUpdatingB(tempvalue);
                SetDesiredBLengthWithoutUpdatingA(tempvalue);
            }
        }

        public double GetDiameterModelUnit()
        {
            return UnitUtils.Convert(DesiredDiameterDuct, SelectedUnit.RevitUnit, UnitTypeId.Feet);
        }

        public double GetLengthAModelUnit()
        {
            return UnitUtils.Convert(DesiredALengthDuct, SelectedUnit.RevitUnit, UnitTypeId.Feet);
        }

        public double GetLengthBModelUnit()
        {
            return UnitUtils.Convert(DesiredBLengthDuct, SelectedUnit.RevitUnit, UnitTypeId.Feet);
        }

        public void SetDesiredALengthWithoutUpdatingB(double value)
        {
            var prev = isUpdatingSize;
            try
            {
                isUpdatingSize = true;   
                DesiredALengthDuct = value;
            }
            finally
            {
                isUpdatingSize = prev; 
            }
        }

        public void SetDesiredBLengthWithoutUpdatingA(double value)
        {
            var prev = isUpdatingSize;
            try
            {
                isUpdatingSize = true;
                DesiredBLengthDuct = value;
            }
            finally
            {
                isUpdatingSize = prev;
            }
        }
    }
}
