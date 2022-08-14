using System.Linq;

namespace NSE.Core.Util
{
    public static class Utils
    {
        public static string ApenasNumeros(this string str, string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }
    }
}
