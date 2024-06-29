using ILGPU;
using Model.Arguments;

namespace Model.Calculations
{
    public class JuliaSetCalculation : ICalculation<double, double>
    {
        private static IArgumentsSet<double> _argumentsSet =
            new ArgumentsSet([new Argument("X1", -2, 2, -2),
                new Argument("X2", -2, 2, 2), new Argument("Y1", -2, 2, -2),
                new Argument("Y2", -2, 2, 2), new Argument("Iteration", 1, 1000),
                new Argument("Real", -1, 1), new Argument("Imaginary", -1, 1)]);

        public void Calculate(Index1D index, int width, int height,
            ArrayView<double> args, ArrayView<double> output)
        {
            var x1 = args[0];
            var x2 = args[1];
            var y1 = args[2];
            var y2 = args[3];
            var maxIterations = args[4];
            var real = args[5];
            var imaginary = args[6];

            var xi = index % width;
            var yi = index / width;

            var x0 = x1 + xi * (x2 - x1) / width;
            var y0 = y1 + yi * (y2 - y1) / height;
            var x = x0;
            var y = y0;
            var iteration = 0;

            while ((x * x + y * y <= 4) && (iteration <= maxIterations))
            {
                var temp = x * x - y * y + real;
                y = 2 * x * y + imaginary;
                x = temp;
                iteration += 1;
            }
            output[index] = iteration / maxIterations;
        }

        public IArgumentsSet<double> GetArgumentsSet() => _argumentsSet;

        public override string ToString() => "Julia set";
    }
}
