using ILGPU;
using Model.Parameters;

namespace Model.Calculations
{
    public interface ICalculation<T, A>
        where T : unmanaged
        where A : unmanaged
    {
        public IParametersComposite<A> GetArgumentsSet();

        public void Calculate(Index1D index, int width, int height,
            ArrayView<A> args, ArrayView<T> output);
    }
}
