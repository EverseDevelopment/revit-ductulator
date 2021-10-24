using Autodesk.Revit.DB;


namespace Ductulator.Model
{
    public static class ModelUnits
    {
       
        public static void unitsName(Element elm, ref string nameUnit, 
            ref double factor, ref string unitAbrev, ref double Vfactor)
        {
            if (App.typeDuct == "Duct")
            {
                string ductUnit = elm.get_Parameter
               (BuiltInParameter.CURVE_ELEM_LENGTH).DisplayUnitType.ToString();
                NameUnits(ductUnit, ref nameUnit, ref factor, ref unitAbrev, ref Vfactor);
            }
            else
            {
                string ductUnit = elm.get_Parameter
               (BuiltInParameter.FABRICATION_PART_LENGTH).DisplayUnitType.ToString();
                NameUnits(ductUnit, ref nameUnit, ref factor, ref unitAbrev, ref Vfactor);
            }
           
        }


        private static void NameUnits(string ductUnit, 
            ref string typeOfUnits, ref double factorvalue, 
            ref string SymbolUnits, ref double factor)
        {

            switch (ductUnit)
            {

                case string a when a.Contains("DUT_DECIMAL_FEET"):
                    factorvalue = 1;
                    typeOfUnits = "FEET";
                    SymbolUnits = "ft";
                    factor = 0.01;
                    break;
                case string b when b.Contains("DUT_FEET_FRACTIONAL_INCHES"):
                    factorvalue = 1;
                    typeOfUnits = "FEET";
                    SymbolUnits = "in";
                    factor = 0.01;
                    break;
                case string c when c.Contains("DUT_DECIMAL_INCHES"):
                    factorvalue = 12;
                    typeOfUnits = "INCHES";
                    SymbolUnits = "in";
                    factor = 0.1;
                    break;
                case string d when d.Contains("DUT_FRACTIONAL_INCHES"):
                    factorvalue = 12;
                    typeOfUnits = "INCHES";
                    SymbolUnits = "in";
                    factor = 0.1;
                    break;
                case string e when e.Contains("DUT_METERS"):
                    factorvalue = 0.3048;
                    typeOfUnits = "METERS";
                    SymbolUnits = "mt";
                    factor = 0.01;
                    break;
                case string f when f.Contains("DUT_DECIMETERS"):
                    factorvalue = 3.048;
                    typeOfUnits = "DECIMETERS";
                    SymbolUnits = "dm";
                    factor = 0.1;
                    break;
                case string g when g.Contains("DUT_CENTIMETERS"):
                    factorvalue = 30.48;
                    typeOfUnits = "CENTIMETERS";
                    SymbolUnits = "cm";
                    factor = 1;
                    break;
                case string h when h.Contains("DUT_MILLIMETERS"):
                    factorvalue = 304.8;
                    typeOfUnits = "MILIMETERS";
                    SymbolUnits = "mm";
                    factor = 1;
                    break;
                case string i when i.Contains("DUT_METERS_CENTIMETERS"):
                    factorvalue = 0.3048;
                    typeOfUnits = "METERS";
                    SymbolUnits = "cm";
                    factor = 0.01;
                    break;
                default:
                    factorvalue = 12;
                    typeOfUnits = "INCHES";
                    SymbolUnits = "in";
                    factorvalue = 0.1;
                    break;
            }
        }
    }
}
