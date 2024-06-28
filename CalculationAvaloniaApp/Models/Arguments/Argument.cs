using System;
using System.ComponentModel;

namespace CalculationAvaloniaApp.Models.Arguments
{
    public class Argument : IArgument<double>, INotifyPropertyChanged
    {
        private double _value;

        public double Value
        {
            get => _value;
            set
            {
                if (_value != value && value >= Minimum &&
                    value <= Maximum)
                {
                    _value = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs(nameof(Value)));
                    Changed?.Invoke(this, value);
                }
            }
        }

        public double Minimum { get; private set; }

        public double Maximum { get; private set; }

        public string Name { get; private set; }

        public event EventHandler<double>? Changed;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Argument(string name, double minimum, double maximum, double defaultValue)
        {
            Name = name;
            Minimum = minimum;
            Maximum = maximum;
            Value = defaultValue;
        }

        public Argument(string name, double minimum, double maximum) :
            this(name, minimum, maximum, (maximum + minimum) / 2) { }
    }
}
