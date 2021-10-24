using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ductulator.Model
{
    public static class FabPartDefinition
    {
        public static FabricationDimensionDefinition elmDefinition(Element elm, String PParam)
       {
        FabricationDimensionDefinition Dparam;
        FabricationPart fp = (FabricationPart)elm;
        IList<FabricationDimensionDefinition> 
                dimObj = fp.GetDimensions();
        Dparam = dimObj.First
                (item => item.Name.ToString() == PParam);
        return Dparam;
        }
    }
}
