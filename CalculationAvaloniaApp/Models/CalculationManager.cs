using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using CalculationAvaloniaApp.Models.Calculations;
using CalculationAvaloniaApp.Models.Drawers;

namespace CalculationAvaloniaApp.Models
{
    public static class CalculationManager
    {
        private static readonly Context _context;

        private static Accelerator _accelerator;

        static CalculationManager()
        {
            _context = Context.CreateDefault();
        }

        public static void ChangeAccelerator(bool isGpu)
        {
            if (_accelerator == null)
            {
                _accelerator = isGpu ? _context.CreateCudaAccelerator(0) :
                    _context.CreateCPUAccelerator(0);
            }
            else if (isGpu && _accelerator.AcceleratorType != AcceleratorType.Cuda)
            {
                _accelerator.Dispose();
                _accelerator = _context.CreateCudaAccelerator(0);
            }
            else if (!isGpu && _accelerator.AcceleratorType != AcceleratorType.CPU)
            {
                _accelerator.Dispose();
                _accelerator = _context.CreateCPUAccelerator(0);
            }
        }

        public static void Calculate<T, A, R>(R[] buffer, A[] args, int width, int height,
            ICalculation<T, A> calculation, IDrawer<T, R> drawer, 
            double x1, double x2, double y1, double y2)
            where T : unmanaged
            where A : unmanaged
            where R : unmanaged
        {
            var calculating = _accelerator.LoadAutoGroupedStreamKernel
                <Index1D, int, int, double, double, double,
                double, ArrayView<A>, ArrayView<T>>(calculation.Calculate);
            var drawing = _accelerator.LoadAutoGroupedStreamKernel
                <Index1D, ArrayView<T>, ArrayView<R>>(drawer.Draw);
            
            int count = buffer.Length;
            var values = _accelerator.Allocate1D<T>(count);
            var arguments = _accelerator.Allocate1D(args);
            var result = _accelerator.Allocate1D<R>(count);

            calculating(count, width, height, x1, x2,
                y1, y2, arguments.View, values.View);
            drawing(count, values.View, result.View);

            _accelerator.Synchronize();
            result.CopyToCPU(buffer);

            values.Dispose();
            arguments.Dispose();
            result.Dispose();
        }
    }
}