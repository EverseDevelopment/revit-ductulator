using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Ductulator.Views_Cs;
using System.Collections.Generic;
using Ductulator.Views;

namespace Ductulator
{
    [Autodesk.Revit.Attributes.Transaction(TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class MainCommand : IExternalCommand
    {
        Document _doc;

        // Declare Need variables.
        //
        private ExternalCommandData cre_cmddata;
        public ParameterSet ElementParameter = new ParameterSet();
        public List<Parameter> ElementParameterList = new List<Parameter>();
        public Element Selelement;
        public List<Element> Elems;
        public int NumberOfElements = 0;

        public static MainForm homewin;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // objects for the top level access
            //
            this.cre_cmddata = commandData;
            UIApplication uiApp = cre_cmddata.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            _doc = uiDoc.Document;

            ICollection<ElementId> selectedIds = uiDoc.Selection.GetElementIds();

            //Number of selected elements
            foreach (ElementId item in selectedIds)
            {
                NumberOfElements += 1;
            }


            if (NumberOfElements == 0)
            {
                MessageWindow.Show("You haven't selected any elements.");
            }
            else
            {
                if (NumberOfElements > 1)
                {
                    // If you have selected more than 1 element. 
                    MessageWindow.Show("You have selected more than 1 element");
                }
                else
                {
                    //item selected
                    foreach (ElementId item in selectedIds)
                    {
                        Selelement = _doc.GetElement(item);
                    }

                    int SelElmCategory;
                    #if REVIT2026
                    SelElmCategory = checked((int)Selelement.Category.Id.Value);
                    #else
                    SelElmCategory = Selelement.Category.Id.IntegerValue;
                    #endif



                    UIDocument ui_doc =
                            commandData.Application.ActiveUIDocument;
                    Autodesk.Revit.DB.Document doc = ui_doc.Document;

                    if (SelElmCategory == -2008000 ||
                        SelElmCategory == -2008193)
                    {
                        if (SelElmCategory == -2008000)
                        {
                            App.typeDuct = "Duct";
                            homewin = new MainForm(commandData);
                            homewin.ShowDialog();
                        }
                        else
                        {
                            if (FabDuctFiltering.straightSec(Selelement))
                            {
                                App.typeDuct = "FabPart";
                                homewin = new MainForm(commandData);
                                homewin.ShowDialog();
                            }
                            else
                            {
                                MessageWindow.Show("You have not selected a straight Duct");
                            }
                        }
                    }
                    else
                    {
                        MessageWindow.Show("You have not selected a Duct");
                    }
                }

            }

            return Autodesk.Revit.UI.Result.Succeeded;

        }
    }
}
