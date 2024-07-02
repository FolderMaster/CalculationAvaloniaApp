using System.ComponentModel;

namespace Model.Parameters
{
    public class Parameter : IParameter<double>, INotifyPropertyChanged
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
                }
            }
        }

        public double Minimum { get; private set; }

        public double Maximum { get; private set; }

        public string Name { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Parameter(string name, double minimum, double maximum, double defaultValue)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException(nameof(maximum));
            }
            if (defaultValue < minimum || defaultValue > maximum)
            {
                throw new ArgumentException(nameof(defaultValue));
            }
            Name = name;
            Minimum = minimum;
            Maximum = maximum;
            Value = defaultValue;
        }

        public Parameter(string name, double minimum, double maximum) :
            this(name, minimum, maximum, (maximum + minimum) / 2) { }
    }
}
