using CalculationAvaloniaApp.Models.Arguments;
using ILGPU;

namespace CalculationAvaloniaApp.Models.Calculations
{
    public interface ICalculation<T, A>
        where T : unmanaged
        where A : unmanaged
    {
        public IArgumentsSet<A> GetArgumentSet();

        public void Calculate(Index1D index, int width, int height,
            double x1, double x2, double y1, double y2,
            ArrayView<A> args, ArrayView<T> output);
    }
}
