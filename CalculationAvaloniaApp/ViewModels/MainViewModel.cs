using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CalculationAvaloniaApp.Models;
using CalculationAvaloniaApp.Models.Drawers;
using CalculationAvaloniaApp.Models.Calculations;
using CalculationAvaloniaApp.Models.Arguments;

namespace CalculationAvaloniaApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private int _width = 1000;

    private int _height = 1000;

    private ICalculation<double, double> _calculation;

    private IEnumerable<ICalculation<double, double>> _calculations;

    private ArgumentsSet _calculationArgumentsSet;

    private IEnumerable<IDrawer<double, int>> _drawers;

    private IDrawer<double, int> _drawer;

    private bool _isGpu = true;

    private double _x1 = -2;

    private double _x2 = 1;

    private double _y1 = -1;

    private double _y2 = 1;

    private Bitmap? _image;

    public int Width
    {
        get => _width;
        set
        {
            if (UpdateProperty(ref _width, value))
                UpdateImage();
        }
    }

    public IEnumerable<ICalculation<double, double>> Calculations => _calculations;

    public ICalculation<double, double> Calculation
    {
        get => _calculation;
        set
        {
            if (UpdateProperty(ref _calculation, value))
            {
                CalculationArgumentsSet = (ArgumentsSet)Calculation.GetArgumentSet();
                UpdateImage();
            }
        }
    }

    public ArgumentsSet CalculationArgumentsSet
    {
        get => _calculationArgumentsSet;
        set
        {
            var oldValue = _calculationArgumentsSet;
            if (UpdateProperty(ref _calculationArgumentsSet, value))
            {
                if(oldValue != null)
                {
                    oldValue.ArgumentChanged -=
                        CalculationArgumentsSet_ArgumentChanged;
                }
                _calculationArgumentsSet.ArgumentChanged +=
                    CalculationArgumentsSet_ArgumentChanged;
            }
        }
    }

    public IEnumerable<IDrawer<double, int>> Drawers => _drawers;

    public IDrawer<double, int> Drawer
    {
        get => _drawer;
        set
        {
            if (UpdateProperty(ref _drawer, value))
                UpdateImage();
        }
    }

    public bool IsGpu
    {
        get => _isGpu;
        set
        {
            if (UpdateProperty(ref _isGpu, value))
                UpdateImage();
        }
    }

    public int Height
    {
        get => _height;
        set
        {
            if (UpdateProperty(ref _height, value))
                UpdateImage();
        }
    }

    public double X1
    {
        get => _x1;
        set
        {
            if (UpdateProperty(ref _x1, value))
                UpdateImage();
        }
    }

    public double X2 
    {
        get => _x2;
        set
        {
            if (UpdateProperty(ref _x2, value))
                UpdateImage();
        }
    }

    public double Y1
    {
        get => _y1;
        set
        {
            if (UpdateProperty(ref _y1, value))
                UpdateImage();
        }
    }

    public double Y2
    {
        get => _y2;
        set
        {
            if (UpdateProperty(ref _y2, value))
                UpdateImage();
        }
    }

    public Bitmap? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }

    public ReactiveCommand<Unit, Unit> SaveImageCommand { get; }

    public async Task SaveImage()
    {
        if (Image == null)
        {
            return;
        }

        await using (var fileStream = new FileStream("C:\\Users\\Darkonge\\Desktop\\Рисунки\\" +
            $"save_{DateTime.Now.ToFileTime()}.png", FileMode.Create))
        {
            Image.Save(fileStream);
        }
    }

    public MainViewModel()
    {
        _calculations = SingletonCollection<ICalculation<double, double>>.
            Instances.Select(s => s.Value);
        _calculation = _calculations.ElementAt(0);
        CalculationArgumentsSet = (ArgumentsSet)Calculation.GetArgumentSet();
        _drawers = SingletonCollection<IDrawer<double, int>>.
            Instances.Select(s => s.Value);
        Drawer = _drawers.ElementAt(0);
        UpdateImage();
    }

    private Bitmap CreateImage(int[] values, int width, int height)
    {
        var format = PixelFormat.Bgra8888;
        var bitmap = new WriteableBitmap(new PixelSize(Width, Height),
            new Vector(96, 96), format);
        using (var lockedBitmap = bitmap.Lock())
        {
            Marshal.Copy(values, 0, lockedBitmap.Address, values.Length);
        }
        return bitmap;
    }

    private void UpdateImage()
    {
        var buffer = new int[Width * Height];
        CalculationManager.ChangeAccelerator(IsGpu);
        CalculationManager.Calculate(buffer, CalculationArgumentsSet.GetArray(), Width, Height,
            Calculation, Drawer, X1, X2, Y1, Y2);
        Image = CreateImage(buffer, Width, Height);
    }

    private void CalculationArgumentsSet_ArgumentChanged(object? sender, IArgument<double> e)
    {
        UpdateImage();
    }
}
