using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSimulation
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Add Spaces to the given string before the capital laters
        /// Mainly used to print names as string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SpaceCapitals(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";
            var newText = new StringBuilder(str.Length * 2);
            newText.Append(str[0]);
            for (var i = 1; i < str.Length; i++)
            {
                // Current char is upper, previous char is not apper (Acronyms), and previous is not space
                if (char.IsUpper(str[i]) && !char.IsUpper(str[i - 1]) && str[i - 1] != ' ' )
                    newText.Append(' ');
                newText.Append(str[i]);
            }
            return newText.ToString();
        }
    }
}
