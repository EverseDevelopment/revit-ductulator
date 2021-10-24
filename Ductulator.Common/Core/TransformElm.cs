using Autodesk.Revit.DB;
using System;
using Ductulator.Model;
using Ductulator.Views;

namespace Ductulator.Core
{
    public static class TransformElm
    {
        public static void Apply(Element elm, Document doc, ElementId typeId, 
            string rndValue, string hghtValue, string wdthValue, double factor)
        {

            if (App.typeDuct == "Duct")
            {
                DuctTranform(elm, doc, typeId, 
                    rndValue, hghtValue, wdthValue, factor);
            }
            else
            {
                FabPartTranform(elm, doc, typeId,
                    rndValue, hghtValue, wdthValue, factor);
            }
        }

        private static void DuctTranform(Element elm, Document doc, ElementId typeId,
          string rndValue, string hghtValue, string wdthValue, double factor)
        {

            Autodesk.Revit.DB.Mechanical.Duct ductelm =
                elm as Autodesk.Revit.DB.Mechanical.Duct;


            Autodesk.Revit.DB.Mechanical.DuctType selectedDuctType = null;
            selectedDuctType = doc.GetElement(typeId)
                as Autodesk.Revit.DB.Mechanical.DuctType;


            using (Transaction t = new Transaction(doc, "transformDuct"))
            {
                t.Start("Transform");
                ductelm.DuctType = selectedDuctType;
                t.Commit();
            }
            if (CurrentDuctShape.elmShape(elm) == "Round")
            {
                Parameter newDiameter = elm.get_Parameter(BuiltInParameter.RBS_CURVE_DIAMETER_PARAM);

                using (Transaction tranround = new Transaction(doc, "parameter"))
                {
                    tranround.Start("param");
                    try
                    {
                        newDiameter.Set(Convert.ToDouble(rndValue) / factor);
                    }
                    catch
                    {

                    }
                    tranround.Commit();
                }
            }
            else
            {
                Parameter newWidth = elm.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM);
                Parameter newHeight = elm.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM);

                using (Transaction transac = new Transaction(doc, "parameter"))
                {
                    transac.Start("param");
                    try
                    {
                        newWidth.Set(Convert.ToDouble(wdthValue) / factor);
                        newHeight.Set(Convert.ToDouble(hghtValue) / factor);
                    }
                    catch
                    {
                    }
                    transac.Commit();
                }

            }
        }

        private static void FabPartTranform(Element elm, Document doc, ElementId typeId,
            string rndValue, string hghtValue, string wdthValue, double factor)
        {
            FabricationPart fabPart = (FabricationPart)elm;

            FabricationPartType selectedDuctType = null;
            selectedDuctType = doc.GetElement(typeId)
                as FabricationPartType;

            try
            { 
            using (Transaction t = new Transaction(doc, "transformDuct"))
            {
                t.Start("Transform");
                fabPart.ChangeTypeId(typeId);
                t.Commit();
                    if (CurrentDuctShape.elmShape(elm) == "Round")
                    {
                        using (Transaction transac = new Transaction(doc, "parameter"))
                        {
                            transac.Start("param");
                            FabricationDimensionDefinition Diameter =
                                FabPartDefinition.elmDefinition(elm, "Diameter");
                            fabPart.SetDimensionValue(Diameter, Convert.ToDouble(rndValue) / factor);
                            transac.Commit();
                        }
                    }
                    else
                    {
                        using (Transaction transac = new Transaction(doc, "parameter"))
                        {


                            transac.Start("param");
                            FabricationDimensionDefinition Width =
                                FabPartDefinition.elmDefinition(elm, "Width");
                            fabPart.SetDimensionValue(Width, Convert.ToDouble(wdthValue) / factor);
                            FabricationDimensionDefinition Height =
                                FabPartDefinition.elmDefinition(elm, "Depth");
                            fabPart.SetDimensionValue(Height, Convert.ToDouble(hghtValue) / factor);
                            transac.Commit();
                        }
                    }

                }
            }    
             catch
            {
                WarningForm homewin = new WarningForm
                    ("Fab parts type cannot be changed" +
                    " if they are connected");
                homewin.ShowDialog();

            }
        }
    }
}
