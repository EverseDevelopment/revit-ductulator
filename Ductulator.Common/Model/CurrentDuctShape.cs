using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using System;
using static Ductulator.Common.Views.ViewModels.MainFormViewModel;


namespace Ductulator.Model
{
    public static class CurrentDuctShape
    {
        public static string elmShape(Element elm)
        {
            string result;

            if(App.typeDuct == "Duct")
            {
                if (elm.get_Parameter(BuiltInParameter.RBS_CURVE_DIAMETER_PARAM)?.AsValueString() != null)
                {
                    result = "Round";

                }
                else
                {
                    result = "Rectangular";
                }
            }
            else
            {
                if (elm.get_Parameter(BuiltInParameter.FABRICATION_PART_DIAMETER_IN)?.AsValueString() != null)
                {
                    result = "Round";
                }
                else
                {
                    result = "Rectangular";
                }
            }
            return result;
        }

        public static DuctShapeEnum GetDuctShape(ElementId elementTypeId)
        {
            Document doc = App.RevitCollectorService.GetDocument();
            MEPCurveType ductType = doc.GetElement(elementTypeId) as MEPCurveType;
            if (ductType == null)
                throw new ArgumentException("ElementId is not a valid MEPCurveType.");

            if (ductType.Shape == ConnectorProfileType.Rectangular)
            {
                return DuctShapeEnum.Rectangular;
            }
            else
            {
                return DuctShapeEnum.Round;
            }

            throw new InvalidOperationException("Unable to determine duct shape.");
        }
    }
}
