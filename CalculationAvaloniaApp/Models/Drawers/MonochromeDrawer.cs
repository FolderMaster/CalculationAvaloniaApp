using ILGPU;

namespace CalculationAvaloniaApp.Models.Drawers
{
    public class MonochromeDrawer : IDrawer<double, int>
    {
        public void Draw(Index1D index, ArrayView<double> values, ArrayView<int> result)
        {
            result[index] = (int)GetColor(values[index]);
        }

        private Color GetColor(double ratio) =>
            new Color(255, (byte)(255 * ratio), (byte)(255 * ratio), (byte)(255 * ratio));

        public override string ToString() => "Monochrome";
    }
}
