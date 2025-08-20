using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Ductulator.Common.Utils;
using Ductulator.Common.Views.ViewModels;
using Ductulator.Core;
using Ductulator.Model;
using Ductulator.Utils;
using Ductulator.Views;
using Ductulator.Views_Cs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Ductulator.Common.Views.ViewModels.UnitsViewModel;

namespace Ductulator
{
   
    public partial class MainForm : Window
    {
        public static Element elm { get; set; }
        public static double Vfactor = 0;
        private Dictionary<string, ElementId> 
            ductTypes = new Dictionary<string, ElementId>();
        public string unitAbrev = null;

        public MainFormViewModel ViewModel { get; private set; }

        public MainForm(ExternalCommandData cmddata_p)
        {
            UIDocument uidoc = cmddata_p.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            elm = GetCurrentSelection.elem(doc, uidoc);

            this.ViewModel = new MainFormViewModel(elm);
            this.DataContext = ViewModel;

            InitializeComponent();

            Theme.ApplyDarkLightMode(this.Resources.MergedDictionaries[0]);
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
            Hyperlink.Run(Links.ductulatorWebsite);
        }

        private void Select_Units(object sender, RoutedEventArgs e)
        {
            var win = new UnitsWindow();
            win.Owner = Window.GetWindow(this);

            win.Closed += (_, __) =>
            {
                UnitOption selectedUnit = win.ViewModel.SelectedLengthUnit;
                ViewModel.SelectedUnit = selectedUnit;
            };

            win.ShowDialog();
        }

        private void Everse_Link(object sender, RoutedEventArgs e)
        {
            Hyperlink.Run(Links.everseWebsite);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Transform_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidNumber(nDctHeight_textBox.Text) || !IsValidNumber(nDctWidth_textBox.Text))
            {
                MessageWindow.Show("One or more values are invalid");
            }
            else
            {

                TransformElm.Apply(elm, 
                    ViewModel.SelectedDuctTypeId,
                    ViewModel.CurrentductShape,
                    ViewModel.GetDiameterModelUnit(),
                    ViewModel.GetLengthAModelUnit(),
                    ViewModel.GetLengthBModelUnit());

                this.Close();
            }
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            if (textBox != null)
            {
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        private static readonly Regex _regex =
            new Regex(@"^[0-9]*(?:[.,][0-9]*)?$");

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;
            string fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            e.Handled = !IsValidNumber(fullText);
        }

        private void NumberValidationTextBox_Paste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pasteText = (string)e.DataObject.GetData(typeof(string));
                System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;
                string fullText = textBox.Text.Insert(textBox.SelectionStart, pasteText);

                if (!IsValidNumber(fullText))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsValidNumber(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            if (!_regex.IsMatch(text))
                return false;

            if (double.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value > 0;
            }

            return false;
        }
    }
}
