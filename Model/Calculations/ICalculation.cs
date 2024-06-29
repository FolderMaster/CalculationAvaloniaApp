using ILGPU;
using Model.Arguments;

namespace Model.Calculations
{
    public interface ICalculation<T, A>
        where T : unmanaged
        where A : unmanaged
    {
        public IArgumentsSet<A> GetArgumentsSet();

        public void Calculate(Index1D index, int width, int height,
            ArrayView<A> args, ArrayView<T> output);
    }
}
