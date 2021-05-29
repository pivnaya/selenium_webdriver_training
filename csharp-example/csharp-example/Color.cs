namespace csharp_example
{
    public class Color
    {
        public int R { get; }
        public int G { get; }
        public int B { get; }

        public bool IsGray()
        {
            return R == G && R == B;
        }

        public bool IsRed()
        {
            return G == 0 && B == 0;
        }

        public Color(string color)
        {
            string[] RGBa = color.Substring(5).Split(',');

            R = int.Parse(RGBa[0].Trim());
            G = int.Parse(RGBa[1].Trim());
            B = int.Parse(RGBa[2].Trim());
        }
    }
}
