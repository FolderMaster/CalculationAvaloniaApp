using ILGPU;
using ILGPU.Algorithms;

namespace CalculationAvaloniaApp.Models.Drawers
{
    public class RainbowDrawer : IDrawer<double, int>
    {
        public void Draw(Index1D index, ArrayView<double> values, ArrayView<int> result)
        {
            result[index] = (int)GetColor(values[index]);
        }

        private Color GetColor(double ratio) =>
            new Color(255, (byte)(255 * XMath.Abs(XMath.Sin((float)(2 * ratio * XMath.PI)))),
                (byte)(255 * XMath.Abs(XMath.Sin((float)
                    (2 * ratio * XMath.PI + 2 * XMath.PI / 3)))),
                (byte)(255 * XMath.Abs(XMath.Sin((float)
                    (2 * ratio * XMath.PI + 4 * XMath.PI / 3)))));

        public override string ToString() => "Rainbow";
    }
}
