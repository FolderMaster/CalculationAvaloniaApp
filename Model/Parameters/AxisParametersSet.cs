using System.ComponentModel;

namespace Model.Parameters
{
    public class AxisParametersSet : IParametersSet<double>, INotifyPropertyChanged
    {
        public double _value1;

        public double _value2;

        public int Count => 2;

        public double this[int index]
        {
            get
            {
                if(index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return index == 0 ? _value1 : _value2;
            }
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (value >= Minimum && value <= Maximum)
                {
                    if(index == 0)
                    {
                        if(value < _value2 - MinimumDistance)
                        {
                            _value1 = value;
                            PropertyChanged?.Invoke(this,
                                new PropertyChangedEventArgs("[0]"));
                        }
                    }
                    else
                    {
                        if (value > _value1 + MinimumDistance)
                        {
                            _value2 = value;
                            PropertyChanged?.Invoke(this,
                                new PropertyChangedEventArgs("[1]"));
                        }
                    }
                }
            }
        }

        public double Value1
        {
            get => _value1;
            set
            {
                if (value >= Minimum && value <= Maximum &&
                    value <= _value2 - MinimumDistance)
                {
                    _value1 = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs(nameof(Value1)));
                }
            }
        }

        public double Value2
        {
            get => _value2;
            set
            {
                if (value >= Minimum && value <= Maximum &&
                    value >= _value1 + MinimumDistance)
                {
                    _value2 = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs(nameof(Value2)));
                }
            }
        }

        public double Minimum { get; private set; }

        public double Maximum { get; private set; }

        public double MinimumDistance { get; private set; }

        public string Name { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public AxisParametersSet(string name, double minimum, double maximum,
            double minimumDistance, double defaultValue1, double defaultValue2)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException(nameof(maximum));
            }
            if (defaultValue1 < minimum || defaultValue1 > maximum)
            {
                throw new ArgumentException(nameof(defaultValue1));
            }
            if (defaultValue2 < minimum || defaultValue2 > maximum)
            {
                throw new ArgumentException(nameof(defaultValue2));
            }
            if (defaultValue1 > defaultValue2 - minimumDistance)
            {
                throw new ArgumentException(nameof(defaultValue2));
            }
            Name = name;
            Minimum = minimum;
            Maximum = maximum;
            MinimumDistance = minimumDistance;
            _value1 = defaultValue1;
            _value2 = defaultValue2;
        }

        public AxisParametersSet(string name, double minimum, double maximum,
            double minimumDistance) : this(name, minimum, maximum,
                minimumDistance, minimum, maximum) { }
    }
}
