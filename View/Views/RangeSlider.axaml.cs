using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using System;

namespace View.Views
{
    [TemplatePart("PART_Panel", typeof(StackPanel))]
    [TemplatePart("PART_LowerBar", typeof(Control))]
    [TemplatePart("PART_LowerThumb", typeof(Thumb))]
    [TemplatePart("PART_MiddleBar", typeof(Control))]
    [TemplatePart("PART_UpperThumb", typeof(Thumb))]
    [TemplatePart("PART_UpperBar", typeof(Control))]
    public partial class RangeSlider : TemplatedControl
    {
        public static readonly StyledProperty<double> MinimumProperty =
            AvaloniaProperty.Register<RangeSlider, double>(nameof(Minimum), coerce: CoerceMinimum);

        public static readonly StyledProperty<double> MaximumProperty =
            AvaloniaProperty.Register<RangeSlider, double>(nameof(Maximum), coerce: CoerceMaximum);

        public static readonly StyledProperty<double> LowerValueProperty =
            AvaloniaProperty.Register<RangeSlider, double>(nameof(LowerValue),
                defaultBindingMode: BindingMode.TwoWay, coerce: CoerceLowerValue);

        public static readonly StyledProperty<double> UpperValueProperty =
            AvaloniaProperty.Register<RangeSlider, double>(nameof(UpperValue),
                defaultBindingMode: BindingMode.TwoWay, coerce: CoerceUpperValue);

        public static readonly StyledProperty<double> MinimumDistanceProperty =
            AvaloniaProperty.Register<RangeSlider, double>(nameof(MinimumDistance),
                coerce: CoerceMinimumDistance);

        public static readonly StyledProperty<double> BarSizeProperty =
            AvaloniaProperty.Register<RangeSlider, double>(nameof(BarSize));

        public static readonly StyledProperty<Orientation> OrientationProperty =
            AvaloniaProperty.Register<RangeSlider, Orientation>(nameof(Orientation));

        private StackPanel _panel;

        private Control _lowerBar;

        private Thumb _lowerThumb;

        private Control _middleBar;

        private Thumb _upperThumb;

        private Control _upperBar;

        private double _valueToPixelRatio;

        private double _pixelToValueRatio;

        public double Minimum
        {
            get => GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public double Maximum
        {
            get => GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double LowerValue
        {
            get => GetValue(LowerValueProperty);
            set => SetValue(LowerValueProperty, value);
        }

        public double UpperValue
        {
            get => GetValue(UpperValueProperty);
            set => SetValue(UpperValueProperty, value);
        }

        public double MinimumDistance
        {
            get => GetValue(MinimumDistanceProperty);
            set => SetValue(MinimumDistanceProperty, value);
        }

        public Orientation Orientation
        {
            get => GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public double BarSize
        {
            get => GetValue(BarSizeProperty);
            set => SetValue(BarSizeProperty, value);
        }

        static RangeSlider()
        {
            MinimumProperty.Changed.AddClassHandler<RangeSlider, double>
                ((o, e) => o.OnMinimumChanged(e));
            MaximumProperty.Changed.AddClassHandler<RangeSlider, double>
                ((o, e) => o.OnMaximumChanged(e));
            LowerValueProperty.Changed.AddClassHandler<RangeSlider, double>
                ((o, e) => o.OnLowerValueChanged(e));
            UpperValueProperty.Changed.AddClassHandler<RangeSlider, double>
                ((o, e) => o.OnUpperValueChanged(e));
            OrientationProperty.Changed.AddClassHandler<RangeSlider, Orientation>
                ((o, e) => o.OnOrientationChanged(e));
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _panel = e.NameScope.Find<StackPanel>("PART_Panel");
            _panel.SizeChanged += Panel_SizeChanged;
            _lowerBar = e.NameScope.Find<Control>("PART_LowerBar");
            _lowerBar.PointerPressed += LowerBar_PointerPressed;
            _lowerThumb = e.NameScope.Find<Thumb>("PART_LowerThumb");
            _lowerThumb.DragDelta += LowerThumb_DragDelta;
            _middleBar = e.NameScope.Find<Control>("PART_MiddleBar");
            _upperThumb = e.NameScope.Find<Thumb>("PART_UpperThumb");
            _upperThumb.DragDelta += UpperThumb_DragDelta;
            _upperBar = e.NameScope.Find<Control>("PART_UpperBar");
            _upperBar.PointerPressed += UpperBar_PointerPressed;
        }

        private static double CoerceMinimum(AvaloniaObject sender, double value)
        {
            var rangeSlider = (RangeSlider)sender;
            return ValidateDouble(value) ? Math.Min(value,
                rangeSlider.LowerValue) : rangeSlider.Minimum;
        }

        private static double CoerceMaximum(AvaloniaObject sender, double value)
        {
            var rangeSlider = (RangeSlider)sender;
            return ValidateDouble(value) ? Math.Max(value,
                rangeSlider.UpperValue) : rangeSlider.Maximum;
        }

        private static double CoerceLowerValue(AvaloniaObject sender, double value)
        {
            var rangeSlider = (RangeSlider)sender;
            return ValidateDouble(value) ? Math.Clamp(value, rangeSlider.Minimum,
                rangeSlider.UpperValue - rangeSlider.MinimumDistance) : rangeSlider.LowerValue;
        }

        private static double CoerceUpperValue(AvaloniaObject sender, double value)
        {
            var rangeSlider = (RangeSlider)sender;
            return ValidateDouble(value) ? Math.Clamp(value, rangeSlider.LowerValue +
                rangeSlider.MinimumDistance, rangeSlider.Maximum) : rangeSlider.UpperValue;
        }

        private static double CoerceMinimumDistance(AvaloniaObject sender, double value)
        {
            var rangeSlider = (RangeSlider)sender;
            return ValidateDouble(value) ? Math.Clamp(value, 0, rangeSlider.UpperValue -
                rangeSlider.LowerValue) : rangeSlider.MinimumDistance;
        }

        private static bool ValidateDouble(double value) =>
            !double.IsInfinity(value) && !double.IsNaN(value);

        private void OnMinimumChanged(AvaloniaPropertyChangedEventArgs<double> args)
        {
            if (IsInitialized)
            {
                UpdateSize();
            }
        }

        private void OnMaximumChanged(AvaloniaPropertyChangedEventArgs<double> args)
        {
            if (IsInitialized)
            {
                UpdateSize();
            }
        }

        private void OnLowerValueChanged(AvaloniaPropertyChangedEventArgs<double> args)
        {
            if (IsInitialized)
            {
                UpdateLowerBarValue();
                UpdateMiddleBarValue();
            }
        }

        private void OnUpperValueChanged(AvaloniaPropertyChangedEventArgs<double> args)
        {
            if (IsInitialized)
            {
                UpdateMiddleBarValue();
                UpdateUpperBarValue();
            }
        }

        private void OnOrientationChanged(AvaloniaPropertyChangedEventArgs<Orientation> args)
        {
            if (IsInitialized)
            {
                UpdateSize();
            }
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

        private void UpdateLowerBarValue() =>
            UpdateBarValue(_lowerBar, Minimum, LowerValue);

        private void UpdateMiddleBarValue() =>
            UpdateBarValue(_middleBar, LowerValue, UpperValue);

        private void UpdateUpperBarValue() =>
            UpdateBarValue(_upperBar, UpperValue, Maximum);

        private void UpdateBarsSizes()
        {
            if (Orientation == Orientation.Horizontal)
            {
                _upperBar.Height = BarSize;
                _middleBar.Height = BarSize;
                _lowerBar.Height = BarSize;
            }
            else
            {
                _upperBar.Width = BarSize;
                _middleBar.Width = BarSize;
                _lowerBar.Width = BarSize;
            }
        }

        private void UpdateSize()
        {
            if (Orientation == Orientation.Horizontal)
            {
                _pixelToValueRatio = (_panel.Bounds.Width - (_lowerThumb.Bounds.Width +
                    _upperThumb.Bounds.Width)) / (Maximum - Minimum);
                _valueToPixelRatio = (Maximum - Minimum) / (_panel.Bounds.Width -
                    (_lowerThumb.Bounds.Width + _upperThumb.Bounds.Width));
            }
            else
            {
                _pixelToValueRatio = (_panel.Bounds.Height - (_lowerThumb.Bounds.Height +
                    _upperThumb.Bounds.Height)) / (Maximum - Minimum);
                _valueToPixelRatio = (Maximum - Minimum) / (_panel.Bounds.Height -
                    (_lowerThumb.Bounds.Height + _upperThumb.Bounds.Height));
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
                if (sender == _lowerBar)
                {
                    value = _lowerBar.Bounds.Width +
                        _lowerThumb.Bounds.Width / 2 - point.X;
                }
                else
                {
                    value = point.X + _upperThumb.Bounds.Width / 2;
                }
            }
            else
            {
                if (sender == _lowerBar)
                {
                    value = _lowerBar.Bounds.Height +
                        _lowerThumb.Bounds.Height / 2 - point.Y;
                }
                else
                {
                    value = point.Y + _upperThumb.Bounds.Height / 2;
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

        private void Panel_SizeChanged(object? sender, SizeChangedEventArgs e) => UpdateSize();
    }
}
