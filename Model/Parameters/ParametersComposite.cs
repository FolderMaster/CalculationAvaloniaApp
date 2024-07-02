using System.ComponentModel;

namespace Model.Parameters
{
    public class ParametersComposite : IParametersComposite<double>,
        INotifyPropertyChanged
    {
        public IEnumerable<object> Parameters { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ParametersComposite(IEnumerable<object> parameters)
        {
            Parameters = parameters;
            foreach(var item in Parameters)
            {
                if (item is INotifyPropertyChanged notify)
                {
                    notify.PropertyChanged += Notify_PropertyChanged;
                }
            }
        }

        private void Notify_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(nameof(GetArguments)));
        }

        public double[] GetArguments()
        {
            var result = new List<double>();
            foreach(var item in Parameters)
            {
                if (item is IParameter<double> parameter)
                {
                    result.Add(parameter.Value);
                }
                else if (item is IParametersSet<double> set)
                {
                    for (var i = 0; i < set.Count; ++i)
                    {
                        result.Add(set[i]);
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            return result.ToArray();
        }
    }
}
