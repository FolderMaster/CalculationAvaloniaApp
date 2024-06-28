using ILGPU;
using CalculationAvaloniaApp.Models.Arguments;

namespace CalculationAvaloniaApp.Models.Calculations
{
    public class MandelbrotSetCalculation : ICalculation<double, double>
    {
        public void Calculate(Index1D index, int width, int height,
            double x1, double x2, double y1, double y2,
            ArrayView<double> args, ArrayView<double> output)
        {
            var xi = index % width;
            var yi = index / width;

            var x0 = x1 + xi * (x2 - x1) / width;
            var y0 = y1 + yi * (y2 - y1) / height;
            var x = 0.0d;
            var y = 0.0d;
            var iteration = 0;

            while ((x * x + y * y <= 4) && (iteration <= args[0]))
            {
                var temp = x * x - y * y + x0;
                y = 2 * x * y + y0;
                x = temp;
                iteration += 1;
            }
            output[index] = iteration / args[0];
        }

        public IArgumentsSet<double> GetArgumentSet() =>
            new ArgumentsSet([new Argument("Iteration", 1, 1000)]);

        public override string ToString() => "Mandelbrot set";
    }
}
