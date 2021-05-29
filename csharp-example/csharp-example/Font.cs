using OpenQA.Selenium;

namespace csharp_example
{
    public class Font
    {
        public string Decoration { get; }

        public int Weight { get; }

        public float Size { get; }

        public bool IsStrikeThrough()
        {
            return Decoration == "line-through";
        }

        public bool IsBold()
        {
            return Weight > 400;
        }

        public Font(IWebElement element)
        {
            Decoration = element.GetCssValue("text-decoration-line");
            Weight = int.Parse(element.GetCssValue("font-weight"));
            Size = float.Parse(element.GetCssValue("font-size").Replace(".",",").Replace("px",""));
        }
    }
}
