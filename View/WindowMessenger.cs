using Avalonia.Controls;
using System;
using System.Threading.Tasks;
using View.Views;
using ViewModel;

namespace View
{
    public class WindowMessenger : IMessenger
    {
        public Window? MainWindow { get; set; }

        public async Task Message(string message)
        {
            ArgumentNullException.ThrowIfNull(MainWindow);
            var window = new MessageWindow()
            {
                Message = message
            };
            await window.ShowDialog(MainWindow);
        }
    }
}
