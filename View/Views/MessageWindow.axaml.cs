using Avalonia.Controls;
using Avalonia.Interactivity;

namespace View.Views
{
    public partial class MessageWindow : Window
    {
        public string Message { get; private set; }

        public MessageWindow(string message)
        {
            Message = message;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
