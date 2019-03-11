using System;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core
{
    /// <summary>
    /// 字符串辅助
    /// </summary>
    public static partial class StringHelper
    {
        /// <summary>
        /// 针对拉丁字符，判断字符串相似度的算法，编辑距离算法
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns>返回值越小，相似度越高</returns>
        public static int LevenshteinDistanceCompute(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // initialize the top and right of the table to 0, 1, 2, ...
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 1; j <= m; d[0, j] = j++)
            {
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }
    }
}