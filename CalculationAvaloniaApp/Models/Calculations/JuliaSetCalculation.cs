using CalculationAvaloniaApp.Models.Arguments;
using ILGPU;

namespace CalculationAvaloniaApp.Models.Calculations
{
    public class JuliaSetCalculation : ICalculation<double, double>
    {
        private static IArgumentsSet<double> _argumentsSet =
            new ArgumentsSet([new Argument("Iteration", 1, 1000),
            new Argument("Real", -1, 1), new Argument("Imaginary", -1, 1)]);

        public void Calculate(Index1D index, int width, int height,
            double x1, double x2, double y1, double y2,
            ArrayView<double> args, ArrayView<double> output)
        {
            var xi = index % width;
            var yi = index / width;

            var x0 = x1 + xi * (x2 - x1) / width;
            var y0 = y1 + yi * (y2 - y1) / height;
            var x = x0;
            var y = y0;
            var iteration = 0;
            var maxIterations = args[0];

            while ((x * x + y * y <= 4) && (iteration <= maxIterations))
            {
                var temp = x * x - y * y + args[1];
                y = 2 * x * y + args[2];
                x = temp;
                iteration += 1;
            }
            output[index] = iteration / maxIterations;
        }

        public IArgumentsSet<double> GetArgumentSet() => _argumentsSet;

        public override string ToString() => "Julia set";
    }
}
