using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Ductulator.Model;
using Ductulator.Core;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Ductulator.Views_Cs;
using Ductulator.Views;
using Ductulator.Utils;

namespace Ductulator
{
   
    public partial class MainForm : Window
    {
        public static UIDocument uidoc { get; set; }
        public static Document doc { get; set; }
        public static Element elm { get; set; }
        public static double factor = 0;
        public static double Vfactor = 0;
        private Dictionary<string, ElementId> 
            ductTypes = new Dictionary<string, ElementId>();
        public Dictionary<string, ElementId> DuctTypes
        {
            get
            {
                return ductTypes;
            }
        }
        public string unitAbrev = null;
        public string UnitAbrev
        {
            get
            {
                return unitAbrev;
            }


        }


        public MainForm(ExternalCommandData cmddata_p)
        {
            uidoc = cmddata_p.Application.ActiveUIDocument;
            doc = uidoc.Document;

            //Assign current selection
            elm = GetCurrentSelection.elem(doc, uidoc);

            //Assing abreviaion in current UI
            string NameUnits = null;
            ModelUnits.unitsName(elm, ref NameUnits,
                ref factor, ref unitAbrev, ref Vfactor);

           
            this.DataContext = this;
            InitializeComponent();

            
            dctSize_textBox.Text = 
                GetCalculatedSize.ElmCalSize(elm).AsString();

            //Add duct types in current model
            ductTypes = ModelDuctTypes.elmnt(doc);

            //Current selection inf Duct Type
            int termIndex = Array.IndexOf(ductTypes.Keys.ToArray(),
                CurrentDuctType.ElmType(elm).ToString());
            DuctType_comboBox.SelectedIndex = termIndex;

            //Assing sizes to elements in current UI
            nDctHeight_textBox.Text = OverallSizes.elmSize(elm)[0];
            nDctWidth_textBox.Text = OverallSizes.elmSize(elm)[1];
            rndDuct_Textbox.Text = OverallSizes.elmSize(elm)[2];

            nDctHeight_textBox.Select(nDctHeight_textBox.Text.Length, 0);
            nDctWidth_textBox.Select(nDctWidth_textBox.Text.Length, 0);
        }

        public string projectVersion = CommonAssemblyInfo.Number;
        public string ProjectVersion
        {
            get { return projectVersion; }
            set { projectVersion = value; }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Title_Link(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process
                .Start("https://apps.autodesk.com/RVT/en/Detail" +
                "/Index?id=6272106374266176068&appLang=en&os=Win64");
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Transform_Click(object sender, RoutedEventArgs e)
        {
            if (nDctHeight_textBox.Text == "" || nDctWidth_textBox.Text == "")
            {
                TaskDialog.Show("Warning", "One or more values are empty");
            }
            else
            {
                TransformAction.transform();
            }
        }

        /// <summary>
        /// Ductulate and validate values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            

            nDctWidth_textBox.TextChanged -= textChangedEventHandlerWidth;
            MainFormControllers.textBox_handler(Vfactor, nDctHeight_textBox,
                   nDctWidth_textBox, OverallSizes.elmSize(elm)[2]);
            nDctWidth_textBox.TextChanged += textChangedEventHandlerWidth;

           

        }

        /// <summary>
        /// Ductulate and validate values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void textChangedEventHandlerWidth(object sender,
            TextChangedEventArgs args)
        {
           
            nDctHeight_textBox.TextChanged -= textChangedEventHandler;
            MainFormControllers.textBox_handler(Vfactor, nDctWidth_textBox,
                nDctHeight_textBox, OverallSizes.elmSize(elm)[2]);
            nDctHeight_textBox.TextChanged += textChangedEventHandler;

        }
    }


}
