using System;

namespace CalculationAvaloniaApp.Models.Arguments
{
    public interface IArgument<T>
    {
        public T Value { get; set; }

        public event EventHandler<T>? Changed;
    }
}
