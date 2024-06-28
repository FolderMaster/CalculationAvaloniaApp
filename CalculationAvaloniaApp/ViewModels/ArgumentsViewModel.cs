using CalculationAvaloniaApp.Models.Arguments;

namespace CalculationAvaloniaApp.ViewModels
{
    public class ArgumentsViewModel : ViewModelBase
    {
        public IArgumentsSet<double> ArgumentSet { get; set; }
    }
}
