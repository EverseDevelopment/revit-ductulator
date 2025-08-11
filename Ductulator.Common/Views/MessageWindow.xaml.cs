namespace Ductulator.Views
{
    using System.Windows;
    using System.Windows.Input;
    using Ductulator.Utils;

    /// <summary>
    /// Interaction logic for Settings.xaml.
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            this.InitializeComponent();
            Theme.ApplyDarkLightMode(this.Resources.MergedDictionaries[0]);
        }

        public static void Show(string message)
        {
            MessageWindow dlg = new MessageWindow();
            dlg.labelMessage.Text = message;
            dlg.ShowDialog();
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
