using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Fabrication;
using Ductulator.Views;
using Ductulator.Model;
using static Ductulator.Common.Views.ViewModels.MainFormViewModel;

namespace Ductulator.Core
{
    public static class TransformElm
    {
        public static void Apply(Element elm, ElementId typeId, DuctShapeEnum ductShape,
                                 double roundValue, double heightValue, double widthValue)
        {
            if (elm == null) throw new ArgumentNullException(nameof(elm));

            if (string.Equals(App.typeDuct, "Duct", StringComparison.OrdinalIgnoreCase))
            {
                TransformDuct(elm, typeId, ductShape, roundValue, heightValue, widthValue);
            }
            else
            {
                TransformFabricationPart(elm, typeId, ductShape, roundValue, heightValue, widthValue);
            }
        }

        private static void TransformDuct(Element elm, ElementId typeId, DuctShapeEnum ductShape,
                                          double roundValue, double heightValue, double widthValue)
        {
            var doc = elm.Document;
            var duct = elm as Duct ?? throw new InvalidOperationException("Element is not a Duct.");

            // Change type + dimensions in one transaction
            InTransaction(doc, "Transform Duct", () =>
            {
                // Change type (safer/more general than assigning DuctType)
                if (typeId != ElementId.InvalidElementId && typeId != duct.GetTypeId())
                {
                    duct.ChangeTypeId(typeId);
                }

                if (ductShape == DuctShapeEnum.Round)
                {
                    TrySetParameter(duct, BuiltInParameter.RBS_CURVE_DIAMETER_PARAM, roundValue);
                }
                else
                {
                    TrySetParameter(duct, BuiltInParameter.RBS_CURVE_WIDTH_PARAM, widthValue);
                    TrySetParameter(duct, BuiltInParameter.RBS_CURVE_HEIGHT_PARAM, heightValue);
                }
            });
        }

        private static void TransformFabricationPart(Element elm, ElementId typeId, DuctShapeEnum ductShape,
                                                     double roundValue, double heightValue, double widthValue)
        {
            var doc = elm.Document;
            var fabPart = elm as FabricationPart ?? throw new InvalidOperationException("Element is not a FabricationPart.");

            try
            {
                InTransaction(doc, "Transform Fabrication Part", () =>
                {
                    // Type change first (throws if connected/locked)
                    if (typeId != ElementId.InvalidElementId && typeId != fabPart.GetTypeId())
                    {
                        fabPart.ChangeTypeId(typeId);
                    }

                    // Then set dimensions
                    if (ductShape == DuctShapeEnum.Round)
                    {
                        var diameterDef = FabPartDefinition.elmDefinition(fabPart, "Diameter");
                        if (diameterDef != null)
                            fabPart.SetDimensionValue(diameterDef, roundValue);
                    }
                    else
                    {
                        var widthDef = FabPartDefinition.elmDefinition(fabPart, "Width");
                        var depthDef = FabPartDefinition.elmDefinition(fabPart, "Depth");
                        if (widthDef != null)
                            fabPart.SetDimensionValue(widthDef, widthValue);
                        if (depthDef != null)
                            fabPart.SetDimensionValue(depthDef, heightValue);
                    }
                });
            }
            catch
            {

                MessageWindow.Show("Fab parts type cannot be changed if they are connected.");
            }
        }

        private static void InTransaction(Document doc, string name, Action body)
        {
            using (var t = new Transaction(doc, name))
            {
                t.Start();
                body();
                t.Commit();
            }
        }

        private static bool TrySetParameter(Element e, BuiltInParameter bip, double value)
        {
            var p = e.get_Parameter(bip);
            if (p == null || p.IsReadOnly) return false;
            return p.Set(value);
        }
    }
}
