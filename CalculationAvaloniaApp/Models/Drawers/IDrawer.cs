using ILGPU;

namespace CalculationAvaloniaApp.Models.Drawers
{
    public interface IDrawer<T, R>
        where T : unmanaged
        where R : unmanaged
    {
        public void Draw(Index1D index, ArrayView<T> values, ArrayView<R> result);
    }
}
