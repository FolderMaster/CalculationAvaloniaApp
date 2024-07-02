using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Model.Parameters;

namespace View.Views
{
    public partial class MessageWindow : Window
    {
        public static readonly DirectProperty<MessageWindow, string>
            MessageProperty = AvaloniaProperty.RegisterDirect<MessageWindow, string>
            (nameof(ParametersComposite), c => c.Message, (c, v) => c.Message = v,
                defaultBindingMode: BindingMode.TwoWay);

        private string _message;

        public string Message
        {
            get => _message;
            set => SetAndRaise(MessageProperty, ref _message, value);
        }

        public MessageWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Close();
    }
}
