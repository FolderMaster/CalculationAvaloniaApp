using ILGPU;

namespace CalculationAvaloniaApp.Models.Drawers
{
    public class ColorfulDrawer : IDrawer<double, int>
    {
        public void Draw(Index1D index, ArrayView<double> values, ArrayView<int> result)
        {
            result[index] = (int)GetColor(values[index]);
        }

        private Color GetColor(double ratio) =>
            new Color(255, (byte)(9 * (1 - ratio) * ratio * ratio * ratio * 255),
                (byte)(15 * (1 - ratio) * (1 - ratio) * ratio * ratio * 255),
                (byte)(8.5 * (1 - ratio) * (1 - ratio) * (1 - ratio) * ratio * 255));

        public override string ToString() => "Colorful";
    }
}
