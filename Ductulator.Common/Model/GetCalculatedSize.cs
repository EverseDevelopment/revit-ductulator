using Autodesk.Revit.DB;

namespace Ductulator.Model
{
    public static class GetCalculatedSize
    { 
        public static string ElmCalSize(Element elm)
        {
            Parameter result = elm.get_Parameter(BuiltInParameter.RBS_CALCULATED_SIZE);
            return result.AsString();
        }
    }
}
