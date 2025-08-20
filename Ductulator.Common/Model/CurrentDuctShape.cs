using Autodesk.Revit.DB;
using System.Linq;
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
                FabricationPart part = App.RevitCollectorService.GetDocument().GetElement(elm.Id) as FabricationPart;
                ConnectorManager cm = part.ConnectorManager;
                Connector first = cm.Connectors.Cast<Connector>().FirstOrDefault();
                ConnectorProfileType shape = first.Shape;

                if (shape == ConnectorProfileType.Round)
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

            if (App.typeDuct == "Duct")
            {
                Document doc = App.RevitCollectorService.GetDocument();
                MEPCurveType ductType = doc.GetElement(elementTypeId) as MEPCurveType;

                if (ductType.Shape == ConnectorProfileType.Rectangular || ductType.Shape == ConnectorProfileType.Oval)
                {
                    return DuctShapeEnum.Rectangular;
                }
                else
                {
                    return DuctShapeEnum.Round;
                }
            }
            else
            {
                Document doc = App.RevitCollectorService.GetDocument();
                FabricationPartType ductType = doc.GetElement(elementTypeId) as FabricationPartType;

                if (ductType.FamilyName.Contains("Oval"))
                {
                    return DuctShapeEnum.Rectangular;
                }


                if (ductType.FamilyName.Contains("Pipe") || ductType.FamilyName.Contains("Round"))
                {
                    return DuctShapeEnum.Round;
                }
                else
                {
                    return DuctShapeEnum.Rectangular;
                }

            }
        }
    }
}
