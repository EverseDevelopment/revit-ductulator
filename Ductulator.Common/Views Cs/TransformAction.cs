using Autodesk.Revit.DB;
using Ductulator.Core;
using Ductulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ductulator;

namespace Ductulator.Views_Cs
{
    public static class TransformAction
    {
        public static void transform()
        {

            ElementId typeSelect =
                    (ElementId)MainCommand.homewin.DuctType_comboBox.SelectedValue;
            TransformElm.Apply(MainForm.elm, MainForm.doc, typeSelect, 
                MainCommand.homewin.rndDuct_Textbox.Text,
                MainCommand.homewin.nDctHeight_textBox.Text,
                MainCommand.homewin.nDctWidth_textBox.Text, MainForm.factor);
            MainCommand.homewin.dctSize_textBox.Text =
            GetCalculatedSize.ElmCalSize(MainForm.elm).AsString();

        }
    }
}
