using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Fabrication;
using Ductulator.Views;
using Ductulator.Model;

namespace Ductulator.Core
{
    public static class TransformElm
    {
        public static void Apply(Element elm, ElementId typeId,
                                 double roundValue, double heightValue, double widthValue)
        {
            if (elm == null) throw new ArgumentNullException(nameof(elm));

            // Decide once
            bool isRound = string.Equals(CurrentDuctShape.elmShape(elm), "Round", StringComparison.OrdinalIgnoreCase);

            if (string.Equals(App.typeDuct, "Duct", StringComparison.OrdinalIgnoreCase))
            {
                TransformDuct(elm, typeId, isRound, roundValue, heightValue, widthValue);
            }
            else
            {
                TransformFabricationPart(elm, typeId, isRound, roundValue, heightValue, widthValue);
            }
        }

        private static void TransformDuct(Element elm, ElementId typeId, bool isRound,
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

                if (isRound)
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

        private static void TransformFabricationPart(Element elm, ElementId typeId, bool isRound,
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
                    if (isRound)
                    {
                        var diameterDef = FabPartDefinition.elmDefinition(fabPart, "Diameter");
                        if (diameterDef != null)
                            fabPart.SetDimensionValue(diameterDef, roundValue);
                    }
                    else
                    {
                        var widthDef = FabPartDefinition.elmDefinition(fabPart, "Width");
                        var depthDef = FabPartDefinition.elmDefinition(fabPart, "Depth"); // Height in fabrication terms
                        if (widthDef != null)
                            fabPart.SetDimensionValue(widthDef, widthValue);
                        if (depthDef != null)
                            fabPart.SetDimensionValue(depthDef, heightValue);
                    }
                });
            }
            catch
            {
                // Keep existing UX behavior, but avoid failing silently
                var dlg = new WarningForm("Fab parts type cannot be changed if they are connected.");
                dlg.ShowDialog();
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
