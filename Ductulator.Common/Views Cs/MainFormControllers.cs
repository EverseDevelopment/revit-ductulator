using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Ductulator.Model;
using Ductulator.Core;
using System.Windows.Controls;
using System.Text.RegularExpressions;


namespace Ductulator.Views_Cs
{
    public static class MainFormControllers
    {
        public static void textBox_handler(double vFactor,
            System.Windows.Controls.TextBox currentTbx,
            System.Windows.Controls.TextBox oppositeTbx,
            string roundDuctEquiv)
        {

        string numberOnly = "0";
        int errorCounterDot = Regex.Matches
                (currentTbx.Text, @"[.]").Count;
            
            int errorCounter = Regex.Matches
                    (currentTbx.Text, @"[0-9]").Count;

            errorCounter = errorCounter + errorCounterDot;
            int TextLenght = currentTbx.Text.Length;

            if (errorCounter != TextLenght)
            {
                string s = currentTbx.Text;
                if (s.Length > 1)
                {
                    numberOnly = Regex.Replace(s, "[^0-9.]", "");
                    currentTbx.Text = numberOnly;
                }
                else
                {
                    currentTbx.Text = "";
                }    
                MessageBox.Show
                    ("Number field should contain only numbers");
            }
            else
            { 
                if (errorCounterDot > 1)
                {
                    string s = currentTbx.Text;
                    if (s.Length > 1)
                    {
                        rplcSecondDot(s, ref numberOnly);
                    }
                    currentTbx.Text = numberOnly;
                    MessageBox.Show
                        ("Only one dot is allowed");
                }
                else
                {

                    switch(currentTbx.Text)
                    {
                        case "":
                            oppositeTbx.Text = "";
                            break;
                        case "0":
                            oppositeTbx.Text = "0";
                            currentTbx.Text = "0";
                            break;
                        case "0.":
                            break;
                        case "0.0":
                            break;
                        case "0.00":
                            oppositeTbx.Text = "0.0";
                            currentTbx.Text = "0";
                            MessageBox.Show
                                ("Value cannot be null");
                            break;
                        case "00":
                            oppositeTbx.Text = "0";
                            currentTbx.Text = "0";
                            MessageBox.Show
                                ("Value cannot be null");
                            break;
                        default:
                            oppositeTbx.Text = ResizeRectangular.Ductulate
                            (vFactor, roundDuctEquiv, currentTbx.Text).ToString();
                            break;
                    }
                }
            }
        }

        private static void rplcSecondDot(string text,ref string txBoxText)
        {
            var index = text.IndexOf
                            ('.', text.IndexOf('.') + 1);
            txBoxText = string.Concat
                (text.Substring(0, index), "", text.Substring(index + 1));
        }
    }
}
