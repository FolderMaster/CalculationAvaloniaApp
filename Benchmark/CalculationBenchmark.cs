using BenchmarkDotNet.Attributes;
using Model;
using Model.Calculations;
using Model.Drawers;

namespace Benchmark
{
    [ThreadingDiagnoser]
    [MemoryDiagnoser]
    public class CalculationBenchmark
    {
        private int _width = 1000;

        public int _height = 1000;


        private ICalculation<double, double> _calculation =
            new JuliaSetCalculation();

        private IDrawer<double, double, int> _drawer =
            new RainbowDrawer();

        private int[] _buffer;

        [GlobalSetup]
        public void Setup()
        {
            _buffer = new int[_width * _height];
        }

        [Benchmark]
        public void CpuTest()
        {
            CalculationManager.SetAccelerator(false);
            CalculationManager.Calculate(_buffer, _calculation.GetArgumentsSet().GetArguments(),
                _drawer.GetArgumentsSet().GetArguments(), _width, _height, _calculation, _drawer);
        }

        [Benchmark]
        public void GpuTest()
        {
            CalculationManager.SetAccelerator(true);
            CalculationManager.Calculate(_buffer, _calculation.GetArgumentsSet().GetArguments(),
                _drawer.GetArgumentsSet().GetArguments(), _width, _height, _calculation, _drawer);
        }
    }
}
