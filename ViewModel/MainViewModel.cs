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
using Model;
using Model.Drawers;
using Model.Calculations;
using Model.Arguments;
using static System.Net.Mime.MediaTypeNames;

namespace ViewModel;

public class MainViewModel : ViewModelBase
{
    private double _width = 1;

    private double _height = 1;

    private IMessenger _messenger;

    private ICalculation<double, double> _calculation;

    private IEnumerable<ICalculation<double, double>> _calculations;

    private ArgumentsSet _calculationArgumentsSet;

    private IEnumerable<IDrawer<double, double, int>> _drawers;

    private IDrawer<double, double, int> _drawer;

    private ArgumentsSet _drawerArgumentsSet;

    private bool _isGpu = true;

    private Bitmap? _image;

    public IEnumerable<ICalculation<double, double>> Calculations => _calculations;

    public ICalculation<double, double> Calculation
    {
        get => _calculation;
        set
        {
            if (UpdateProperty(ref _calculation, value))
            {
                CalculationArgumentsSet = (ArgumentsSet)Calculation.GetArgumentsSet();
                UpdateImage();
            }
        }
    }

    public ArgumentsSet CalculationArgumentsSet
    {
        get => _calculationArgumentsSet;
        private set
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

    public IEnumerable<IDrawer<double, double, int>> Drawers => _drawers;

    public IDrawer<double, double, int> Drawer
    {
        get => _drawer;
        set
        {
            if (UpdateProperty(ref _drawer, value))
            {
                DrawerArgumentsSet = (ArgumentsSet)Drawer.GetArgumentsSet();
                UpdateImage();
            }
        }
    }

    public ArgumentsSet DrawerArgumentsSet
    {
        get => _drawerArgumentsSet;
        private set
        {
            var oldValue = _drawerArgumentsSet;
            if (UpdateProperty(ref _drawerArgumentsSet, value))
            {
                if (oldValue != null)
                {
                    oldValue.ArgumentChanged -=
                        CalculationArgumentsSet_ArgumentChanged;
                }
                _drawerArgumentsSet.ArgumentChanged +=
                    CalculationArgumentsSet_ArgumentChanged;
            }
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

    public double Width
    {
        get => _width;
        set
        {
            if (UpdateProperty(ref _width, value))
                UpdateImage();
        }
    }

    public double Height
    {
        get => _height;
        set
        {
            if (UpdateProperty(ref _height, value))
                UpdateImage();
        }
    }

    public Bitmap? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }

    public string SavePath { get; set; }

    public ReactiveCommand<Unit, Unit> SaveImageCommand { get; }

    public async Task SaveImage()
    {
        if (Image == null)
        {
            return;
        }
        try
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            await using (var fileStream = new FileStream(SavePath +
                $"\\save_{DateTime.Now.ToFileTime()}.png", FileMode.Create))
            {
                Image.Save(fileStream);
            }
        }
        catch (Exception ex)
        {
            _messenger.Message(ex.Message);
        }
    }

    public MainViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        _calculations = SingletonCollection<ICalculation<double, double>>.
            Instances.Select(s => s.Value);
        _calculation = _calculations.ElementAt(0);
        CalculationArgumentsSet = (ArgumentsSet)Calculation.GetArgumentsSet();
        _drawers = SingletonCollection<IDrawer<double, double, int>>.
            Instances.Select(s => s.Value);
        Drawer = _drawers.ElementAt(0);
        SavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        UpdateImage();
    }

    private Bitmap CreateImage(int[] values, int width, int height)
    {
        var format = PixelFormat.Bgra8888;
        var bitmap = new WriteableBitmap(new PixelSize(width, height),
            new Vector(96, 96), format);
        using (var lockedBitmap = bitmap.Lock())
        {
            Marshal.Copy(values, 0, lockedBitmap.Address, values.Length);
        }
        return bitmap;
    }

    private void UpdateImage()
    {
        var buffer = new int[(int)Width * (int)Height];
        CalculationManager.ChangeAccelerator(IsGpu);
        CalculationManager.Calculate(buffer, CalculationArgumentsSet.GetArray(),
            DrawerArgumentsSet.GetArray(), (int)Width, (int)Height,
            Calculation, Drawer);
        Image = CreateImage(buffer, (int)Width, (int)Height);
    }

    private void CalculationArgumentsSet_ArgumentChanged(object? sender, IArgument<double> e)
    {
        UpdateImage();
    }
}
