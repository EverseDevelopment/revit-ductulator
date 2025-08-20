using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Ductulator.Model;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Ductulator.Core
{
    public static class OverallSizes
    {

        public static List<string> elmSize(Element elm)
        {
            List<string> result = new List<string>();
            string elmShape = CurrentDuctShape.elmShape(elm);
            Connector elmConn = CurrentElmConn.elmConn(elm);

            if (elmShape == "Round")
            {
                var temresult = elmConn.Radius;
                double b_Side = DuctSide_equiv.Side_equiv(temresult) * factorConvertion(elm);
                result.Add(b_Side.ToString());
                result.Add(b_Side.ToString());
                temresult = temresult * 2 * factorConvertion(elm);
                result.Add(temresult.ToString());

            }
            else
            {      
                double b_Side = elmConn.Height * factorConvertion(elm);
                result.Add(Math.Round(b_Side, 2).ToString());
                double a_Side = elmConn.Width * factorConvertion(elm);
                result.Add(Math.Round(a_Side, 2).ToString());
                result.Add(DuctDiamEquiv.Diam_equiv(a_Side, b_Side).ToString());
            }

            return result;
        }


        public static double RoundSize(Element elm)
        {
            Connector elmConn = CurrentElmConn.elmConn(elm);
            var temresult = elmConn.Radius * 2;
            return Math.Round(temresult, 2);
        }

        public static double HeigthSize(Element elm)
        {
            Connector elmConn = CurrentElmConn.elmConn(elm);
            double b_Side = elmConn.Height;

            return b_Side;
        }

        public static double WidthSize(Element elm)
        {
            Connector elmConn = CurrentElmConn.elmConn(elm);
            double b_Side = elmConn.Width;

            return b_Side;
        }

        public static double factorConvertion(Element elm)
        {
            double result = 0;
            string NameUnits = null;
            string abrev = null;
            double vfactor = 0;

            ModelUnits.unitsName(elm, ref NameUnits, ref result, ref abrev, ref vfactor);

            return result;
        }

        public static ForgeTypeId GetUnitTypeId(Element elm)
        {
            Document doc = App.RevitCollectorService.GetDocument();

            Autodesk.Revit.DB.Parameter ductParameter = App.typeDuct == "Duct"
                ? elm.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH)
                : elm.get_Parameter(BuiltInParameter.FABRICATION_PART_LENGTH);

            if (ductParameter == null || ductParameter.Definition == null)
                throw new InvalidOperationException("Invalid parameter.");

            return doc.GetUnits()
                      .GetFormatOptions(ductParameter.Definition.GetDataType())
                      .GetUnitTypeId();
        }
    }
}
