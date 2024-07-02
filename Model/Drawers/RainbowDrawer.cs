using ILGPU;
using ILGPU.Algorithms;
using Model.Parameters;

namespace Model.Drawers
{
    public class RainbowDrawer : IDrawer<double, double, int>
    {
        private static IParametersComposite<double> _argumentsSet =
            new ParametersComposite([new Parameter("Duration", 0, 1000, 2),
                new Parameter("Color", 0, XMath.PI, 2)]);

        public void Draw(Index1D index, ArrayView<double> values,
            ArrayView<double> args, ArrayView<int> result)
        {
            result[index] = (int)GetColor(values[index], args);
        }

        public IParametersComposite<double> GetArgumentsSet() => _argumentsSet;

        private Color GetColor(double ratio, ArrayView<double> args) =>
            new Color(255, (byte)(255 * XMath.Abs(XMath.Sin((float)(args[0] * ratio * XMath.PI)))),
                (byte)(255 * XMath.Abs(XMath.Sin((float)
                    (args[0] * ratio * XMath.PI + args[1])))),
                (byte)(255 * XMath.Abs(XMath.Sin((float)
                    (args[0] * ratio * XMath.PI + 2 * args[1])))));

        public override string ToString() => "Rainbow";
    }
}
