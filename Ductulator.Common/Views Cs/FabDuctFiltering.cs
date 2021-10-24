using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ductulator.Views_Cs
{
    static class FabDuctFiltering
    {
        public static bool straightSec(Element elm)
        {
            bool result = false;

            FabricationPart fabPart = (FabricationPart)elm;

            if(fabPart.IsAStraight() == true)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}
