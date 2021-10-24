using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ductulator.Model
{
    public static class GetCurrentSelection
    {
        public static Element elem(Document doc, UIDocument uiDoc)
        {
            Element elmnt = null;
            ICollection<ElementId> selectedIds = 
                uiDoc.Selection.GetElementIds();
            foreach (ElementId item in selectedIds)
            {
                elmnt = doc.GetElement(item);
            }

            return elmnt;

        }
    }
}
