namespace Ductulator.Views
{
    using System.Windows;
    using System.Windows.Input;
    using Ductulator.Common.Views.ViewModels;
    using Ductulator.Utils;

    /// <summary>
    /// Interaction logic for Settings.xaml.
    /// </summary>
    public partial class UnitsWindow : Window
    {
        public UnitsViewModel ViewModel { get; private set; }
        public UnitsWindow()
        {
            this.ViewModel = new UnitsViewModel();
            this.DataContext = ViewModel;

            this.InitializeComponent();
            Theme.ApplyDarkLightMode(this.Resources.MergedDictionaries[0]);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Title_Link(object sender, RoutedEventArgs e)
        {

        }
    }
}
