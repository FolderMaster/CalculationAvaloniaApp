using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

namespace View.Views
{
    public partial class RangeSlider : UserControl
    {
        public static readonly DirectProperty<RangeSlider, double>
            MinimumProperty = AvaloniaProperty.RegisterDirect<RangeSlider, double>
            (nameof(Minimum), c => c.Minimum, (c, v) => c.Minimum = v,
                0, BindingMode.TwoWay);

        public static readonly DirectProperty<RangeSlider, double>
            MaximumProperty = AvaloniaProperty.RegisterDirect<RangeSlider, double>
            (nameof(Maximum), c => c.Maximum, (c, v) => c.Maximum = v,
                100, BindingMode.TwoWay);

        public static readonly DirectProperty<RangeSlider, double>
            LowerValueProperty = AvaloniaProperty.RegisterDirect<RangeSlider, double>
            (nameof(LowerValue), c => c.LowerValue, (c, v) => c.LowerValue = v,
                10, BindingMode.TwoWay);

        public static readonly DirectProperty<RangeSlider, double>
            UpperValueProperty = AvaloniaProperty.RegisterDirect<RangeSlider, double>
            (nameof(UpperValue), c => c.UpperValue, (c, v) => c.UpperValue = v,
                90, BindingMode.TwoWay);

        public static readonly DirectProperty<RangeSlider, double>
            MinimumDistanceProperty = AvaloniaProperty.RegisterDirect<RangeSlider, double>
            (nameof(MinimumDistance), c => c.MinimumDistance, (c, v) => c.MinimumDistance = v,
                0, BindingMode.TwoWay);

        public static readonly StyledProperty<Orientation> OrientationProperty =
            AvaloniaProperty.Register<RangeSlider, Orientation>(nameof(Orientation));

        public static readonly StyledProperty<double> BarSizeProperty =
            AvaloniaProperty.Register<RangeSlider, double>(nameof(BarSize), 50);

        private double _valueToPixelRatio;

        private double _pixelToValueRatio;

        private double _minimum = 0;

        private double _maximum = 100;

        private double _lowerValue = 10;

        private double _upperValue = 90;

        private double _minimumDistance = 0;

        public double Minimum
        {
            get => _minimum;
            set
            {
                if (SetAndRaise(MinimumProperty, ref _minimum, value))
                {
                    UpdateSize();
                }
            }
        }

        public double Maximum
        {
            get => _maximum;
            set
            {
                if (SetAndRaise(MaximumProperty, ref _maximum, value))
                {
                    UpdateSize();
                }
            }
        }

        public double LowerValue
        {
            get => _lowerValue;
            set
            {
                if (value < Minimum)
                {
                    value = Minimum;
                }
                else if (value > UpperValue - MinimumDistance)
                {
                    value = UpperValue - MinimumDistance;
                }
                if (SetAndRaise(LowerValueProperty, ref _lowerValue, value))
                {
                    UpdateLowerBarValue();
                    UpdateMiddleBarValue();
                }
            }
        }

        public double UpperValue
        {
            get => _upperValue;
            set
            {
                if (value > Maximum)
                {
                    value = Maximum;
                }
                else if (value < LowerValue + MinimumDistance)
                {
                    value = LowerValue + MinimumDistance;
                }
                if (SetAndRaise(UpperValueProperty, ref _upperValue, value))
                {
                    UpdateMiddleBarValue();
                    UpdateUpperBarValue();
                }
            }
        }

        public double MinimumDistance
        {
            get => _minimumDistance;
            set
            {
                SetAndRaise(MinimumDistanceProperty, ref _minimumDistance, value);
            }
        }

        public Orientation Orientation
        {
            get => GetValue(OrientationProperty);
            set
            {
                SetValue(OrientationProperty, value);
                UpdateSize();
            }
        }

        public double BarSize
        {
            get => GetValue(BarSizeProperty);
            set
            {
                SetValue(BarSizeProperty, value);
                UpdateBarsSizes();
            }
        }

        static RangeSlider()
        {
        }

        public RangeSlider()
        {
            InitializeComponent();
        }

        private void UpdateBarValue(Control bar, double lowerValue, double upperValue)
        {
            var value = _pixelToValueRatio * (upperValue - lowerValue);
            if (Orientation == Orientation.Horizontal)
            {
                bar.Width = value;
            }
            else
            {
                bar.Height = value;
            }
        }

        private void UpdateBarsSizes()
        {
            if (Orientation == Orientation.Horizontal)
            {
                upperBar.Height = BarSize;
                middleBar.Height = BarSize;
                lowerBar.Height = BarSize;
            }
            else
            {
                upperBar.Width = BarSize;
                middleBar.Width = BarSize;
                lowerBar.Width = BarSize;
            }
        }

        private void UpdateLowerBarValue() => UpdateBarValue(lowerBar, Minimum, LowerValue);

        private void UpdateMiddleBarValue() => UpdateBarValue(middleBar, LowerValue, UpperValue);

        private void UpdateUpperBarValue() => UpdateBarValue(upperBar, UpperValue, Maximum);

        private void UpdateSize()
        {
            if (Orientation == Orientation.Horizontal)
            {
                _pixelToValueRatio = (stack.Bounds.Width - (lowerThumb.Bounds.Width +
                    upperThumb.Bounds.Width)) / (Maximum - Minimum);
                _valueToPixelRatio = (Maximum - Minimum) / (stack.Bounds.Width -
                    (lowerThumb.Bounds.Width + upperThumb.Bounds.Width));
                
            }
            else
            {
                _pixelToValueRatio = (stack.Bounds.Height - (lowerThumb.Bounds.Height +
                    upperThumb.Bounds.Height)) / (Maximum - Minimum);
                _valueToPixelRatio = (Maximum - Minimum) / (stack.Bounds.Height -
                    (lowerThumb.Bounds.Height + upperThumb.Bounds.Height));
            }
            UpdateBarsSizes();
            UpdateLowerBarValue();
            UpdateMiddleBarValue();
            UpdateUpperBarValue();
        }

        private double GetChangeValueByThumbVector(Vector vector) => _valueToPixelRatio *
            (Orientation == Orientation.Horizontal ? vector.X : vector.Y);

        private double GetChangeValueByBarPoint(object sender, PointerPressedEventArgs e)
        {
            var point = e.GetPosition((Visual)sender);
            double value;
            if(Orientation == Orientation.Horizontal)
            {
                if (sender == lowerBar)
                {
                    value = lowerBar.Bounds.Width +
                        lowerThumb.Bounds.Width / 2 - point.X;
                }
                else
                {
                    value = point.X + upperThumb.Bounds.Width / 2;
                }
            }
            else
            {
                if (sender == lowerBar)
                {
                    value = lowerBar.Bounds.Height +
                        lowerThumb.Bounds.Height / 2 - point.Y;
                }
                else
                {
                    value = point.Y + upperThumb.Bounds.Height / 2;
                }
            }
            return _valueToPixelRatio * value;
        }

        private void LowerThumb_DragDelta(object sender, VectorEventArgs e) =>
            LowerValue += GetChangeValueByThumbVector(e.Vector);

        private void UpperThumb_DragDelta(object sender, VectorEventArgs e) =>
            UpperValue += GetChangeValueByThumbVector(e.Vector);

        private void LowerBar_PointerPressed(object sender, PointerPressedEventArgs e) =>
            LowerValue -= GetChangeValueByBarPoint(sender, e);

        private void UpperBar_PointerPressed(object sender, PointerPressedEventArgs e) =>
            UpperValue += GetChangeValueByBarPoint(sender, e);

        private void Stack_SizeChanged(object? sender, SizeChangedEventArgs e) => UpdateSize();
    }
}
