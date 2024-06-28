using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using CalculationAvaloniaApp.Models.Arguments;

namespace CalculationAvaloniaApp.Views
{
    public partial class ArgumentsControl : UserControl
    {
        public static readonly DirectProperty<ArgumentsControl, ArgumentsSet?>
            ArgumentsSetProperty = AvaloniaProperty.RegisterDirect<ArgumentsControl, ArgumentsSet?>
            (nameof(ArgumentsSet), c => c.ArgumentsSet, (c, v) => c.ArgumentsSet = v,
                defaultBindingMode: BindingMode.TwoWay);

        private ArgumentsSet? _argumentsSet;

        public ArgumentsSet? ArgumentsSet
        {
            get => _argumentsSet;
            set => SetAndRaise(ArgumentsSetProperty, ref _argumentsSet, value);
        }

        public ArgumentsControl()
        {
            InitializeComponent();
        }
    }
}
