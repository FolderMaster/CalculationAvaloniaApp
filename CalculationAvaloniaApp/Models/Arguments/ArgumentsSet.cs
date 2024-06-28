using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculationAvaloniaApp.Models.Arguments
{
    public class ArgumentsSet : IArgumentsSet<double>
    {
        public IEnumerable<IArgument<double>> Arguments { get; private set; }

        public event EventHandler<IArgument<double>>? ArgumentChanged;

        public ArgumentsSet(IEnumerable<IArgument<double>> arguments)
        {
            Arguments = arguments;
            foreach(var argument in Arguments)
            {
                argument.Changed += Argument_Changed;
            }
        }

        private void Argument_Changed(object? sender, double e)
        {
            ArgumentChanged?.Invoke(this, (IArgument<double>)sender);
        }

        public double[] GetArray()
        {
            var count = Arguments.Count();
            var result = new double[count];
            for (var i = 0; i < count; ++i)
            {
                result[i] = Arguments.ElementAt(i).Value;
            }
            return result;
        }
    }
}
