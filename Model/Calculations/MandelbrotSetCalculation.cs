using ILGPU;
using Model.Arguments;

namespace Model.Calculations
{
    public class MandelbrotSetCalculation : ICalculation<double, double>
    {
        private static IArgumentsSet<double> _argumentsSet =
            new ArgumentsSet([new Argument("X1", -2, 1, -2),
                new Argument("X2", -2, 1, 1), new Argument("Y1", -1, 1, -1),
                new Argument("Y2", -1, 1, 1), new Argument("Iteration", 1, 1000)]);

        public void Calculate(Index1D index, int width, int height,
            ArrayView<double> args, ArrayView<double> output)
        {
            var x1 = args[0];
            var x2 = args[1];
            var y1 = args[2];
            var y2 = args[3];
            var maxIterations = args[4];

            var xi = index % width;
            var yi = index / width;

            var x0 = x1 + xi * (x2 - x1) / width;
            var y0 = y1 + yi * (y2 - y1) / height;
            var x = 0.0d;
            var y = 0.0d;
            var iteration = 0;

            while ((x * x + y * y <= 4) && (iteration <= maxIterations))
            {
                var temp = x * x - y * y + x0;
                y = 2 * x * y + y0;
                x = temp;
                iteration += 1;
            }
            output[index] = iteration / maxIterations;
        }

        public IArgumentsSet<double> GetArgumentsSet() => _argumentsSet;

        public override string ToString() => "Mandelbrot set";
    }
}
