using Avalonia.Controls;
using ViewModel;

namespace View.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Image_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var context = (MainViewModel)DataContext;
        context.Width = e.NewSize.Width;
        context.Height = e.NewSize.Height;
    }
}
