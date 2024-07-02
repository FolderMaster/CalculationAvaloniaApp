using ILGPU;
using Model.Parameters;

namespace Model.Drawers
{
    public interface IDrawer<T, A, R>
        where T : unmanaged
        where A : unmanaged
        where R : unmanaged
    {
        public void Draw(Index1D index, ArrayView<T> values,
            ArrayView<A> args, ArrayView<R> result);

        public IParametersComposite<A> GetArgumentsSet();
    }
}
