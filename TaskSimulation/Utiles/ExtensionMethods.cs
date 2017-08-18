using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TaskSimulation.Utiles
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


        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Add<TA, TB>(this IList<Tuple<TA, TB>> list, TA item1, TB item2)
        {
            list.Add(new Tuple<TA, TB>(item1, item2));
        }

        /// <summary>
        /// The assumption is that the list is orrdered
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <typeparam name="TB"></typeparam>
        /// <param name="list"></param>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public static void AddUnique<TA, TB>(this IList<Tuple<TA, TB>> list, TA item1, TB item2)
        {
            if (list.Count != 0)
            {
                var last = list.Last();
                if (last != null && last.Item1.Equals(item1))
                {
                    list.Remove(last);
                }
            }
            list.Add(item1, item2);
        }

        /// <summary>
        /// Get the element x from end (fromEnd=0 is the last)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="fromEnd"></param>
        /// <returns></returns>
        public static T GetFromEnd<T>(this IList<T> list, int fromEnd)
        {
            return list[list.Count - 1 - fromEnd];
        }
    }
}
