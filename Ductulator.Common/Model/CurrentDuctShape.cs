using Autodesk.Revit.DB;


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
    }
}
