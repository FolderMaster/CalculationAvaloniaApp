namespace CalculationAvaloniaApp.Models.Drawers
{
    public struct Color
    {
        public byte A { get; set; }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public static explicit operator int(Color color) =>
            color.A << 24 | color.R << 16 | color.G << 8 | color.B;
    }
}
