using ILGPU;
using Model.Parameters;

namespace Model.Calculations
{
    public class JuliaSetCalculation : ICalculation<double, double>
    {
        private static IParametersComposite<double> _argumentsSet =
            new ParametersComposite([new AxisParametersSet("X", -2, 2, 1e-15),
                new AxisParametersSet("Y", -2, 2, 1e-15), new Parameter("Iteration", 1, 1000),
                new Parameter("Real", -1, 1), new Parameter("Imaginary", -1, 1)]);

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

        public IParametersComposite<double> GetArgumentsSet() => _argumentsSet;

        public override string ToString() => "Julia set";
    }
}
