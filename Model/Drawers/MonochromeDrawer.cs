using ILGPU;
using Model.Parameters;

namespace Model.Drawers
{
    public class MonochromeDrawer : IDrawer<double, double, int>
    {
        private static IParametersComposite<double> _argumentsSet =
            new ParametersComposite([new Parameter("Red", 0, 1, 1),
                new Parameter("Green", 0, 1, 1),
                new Parameter("Blue", 0, 1, 1)]);
        public void Draw(Index1D index, ArrayView<double> values,
            ArrayView<double> args, ArrayView<int> result)
        {
            result[index] = (int)GetColor(values[index], args);
        }

        public IParametersComposite<double> GetArgumentsSet() => _argumentsSet;

        private Color GetColor(double ratio, ArrayView<double> args) =>
            new Color(255, (byte)(args[0] * ratio * 255),
                (byte)(args[1] * ratio * 255),
                (byte)(args[2] * ratio * 255));

        public override string ToString() => "Monochrome";
    }
}
