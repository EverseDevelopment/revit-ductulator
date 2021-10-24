using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ductulator.Model
{
    public static class ModelDuctTypes
    {
        public static Dictionary<string, ElementId> elmnt (Document doc)
        {
            Dictionary<string, ElementId> ductTypeList = new Dictionary<string, ElementId> { };

            if (App.typeDuct == "Duct")
            { 
                var collectorround = new FilteredElementCollector(doc).
                    OfCategory(BuiltInCategory.OST_DuctCurves).OfClass(typeof(ElementType)).ToElementIds();

                foreach (var d in collectorround)
                {
                ductTypeList.Add(doc.GetElement(d).get_Parameter(BuiltInParameter.ALL_MODEL_FAMILY_NAME).AsString() 
                    + " - " + doc.GetElement(d).Name.ToString(), d);
                }
            }
            else
            {
                var collectorround = new FilteredElementCollector(doc).
                 OfCategory(BuiltInCategory.OST_FabricationDuctwork).OfClass(typeof(ElementType)).ToElementIds();


                ElementId typeId = MainForm.elm.GetTypeId();
                FabricationPartType fabPartType =  (FabricationPartType)doc.GetElement(typeId);
                ICollection<ElementId> listTypes = fabPartType.GetSimilarTypes();

                foreach (ElementId d in listTypes)
                {
                    ductTypeList.Add(doc.GetElement(d).get_Parameter
                        (BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString(), d);

                }
            }
            return ductTypeList;
        }
    }
}
