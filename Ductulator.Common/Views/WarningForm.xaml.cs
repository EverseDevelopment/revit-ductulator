using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ductulator.Views_Cs;

namespace Ductulator.Views
{
    /// <summary>
    /// Interaction logic for WarningForm.xaml
    /// </summary>
    public partial class WarningForm : Window
    {
        public string warningString;
        public string WarningString
        {
            get { return warningString; }
            set { warningString = value; }
        }

        public WarningForm(string input)
        {
            warningString = input;
            this.DataContext = this;
            InitializeComponent();
            this.Topmost = true;

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Transform_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
