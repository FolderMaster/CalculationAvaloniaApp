using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;
using Model.Calculations;
using Model.Drawers;

namespace Model
{
    public static class CalculationManager
    {
        private static readonly Context _context;

        private static Accelerator _accelerator;

        static CalculationManager()
        {
            _context = Context.CreateDefault();
        }

        public static void SetAccelerator(bool isGpu)
        {
            if (_accelerator == null)
            {
                _accelerator = isGpu ? _context.CreateCudaAccelerator(0) :
                    _context.CreateCLAccelerator(0);
            }
            else if (isGpu && _accelerator.AcceleratorType != AcceleratorType.Cuda)
            {
                _accelerator.Dispose();
                _accelerator = _context.CreateCudaAccelerator(0);
            }
            else if (!isGpu && _accelerator.AcceleratorType != AcceleratorType.CPU)
            {
                _accelerator.Dispose();
                _accelerator = _context.CreateCLAccelerator(0);
            }
        }

        public static void Calculate<T, A1, R, A2>(R[] buffer, A1[] calculationArgs,
            A2[] drawerArgs, int width, int height,
            ICalculation<T, A1> calculation, IDrawer<T, A2, R> drawer)
            where T : unmanaged
            where A1 : unmanaged
            where R : unmanaged
            where A2 : unmanaged
        {
            int count = buffer.Length;
            var values = _accelerator.Allocate1D<T>(count);
            var calculationArguments = _accelerator.Allocate1D(calculationArgs);

            var calculating = _accelerator.LoadAutoGroupedStreamKernel
                <Index1D, int, int, ArrayView<A1>, ArrayView<T>>(calculation.Calculate);
            calculating(count, width, height, calculationArguments.View, values.View);
            _accelerator.Synchronize();

            var result = _accelerator.Allocate1D<R>(count);
            var drawerArguments = _accelerator.Allocate1D(drawerArgs);

            var drawing = _accelerator.LoadAutoGroupedStreamKernel
                <Index1D, ArrayView<T>, ArrayView<A2>, ArrayView<R>>(drawer.Draw);
            drawing(count, values.View, drawerArguments.View, result.View);

            _accelerator.Synchronize();
            result.CopyToCPU(buffer);

            values.Dispose();
            calculationArguments.Dispose();
            result.Dispose();
            drawerArguments.Dispose();
        }
    }
}