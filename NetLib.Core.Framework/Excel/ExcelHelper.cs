using System.Collections.Generic;
using System.Linq;

namespace FrHello.NetLib.Core.Framework.Excel
{
    /// <summary>
    /// ExcelHelper
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// Get invalid sheet name chars
        /// </summary>
        /// <returns></returns>
        public static char[] GetInvalidSheetNameChars()
        {
            return new[] {'/'};
        }

        /// <summary>
        /// Replace invalid characters in sheet name with specified characters, default is '-'
        /// </summary>
        /// <param name="sheetName">sheet name</param>
        /// <param name="replaceTarget">specified characters, default is '-'</param>
        /// <returns></returns>
        public static string ReplaceInvalidSheetName(string sheetName, char? replaceTarget = null)
        {
            if (!string.IsNullOrEmpty(sheetName))
            {
                var charts = sheetName.ToList();
                var invalidSheetNameChars = GetInvalidSheetNameChars();

                var removeIndex = new List<int>();

                for (var i = 0; i < charts.Count; i++)
                {
                    if (invalidSheetNameChars.Contains(charts[i]))
                    {
                        if (replaceTarget != null)
                        {
                            charts[i] = replaceTarget.Value;
                        }
                        else
                        {
                            removeIndex.Insert(0, i);
                        }
                    }
                }

                if (removeIndex.Any())
                {
                    foreach (var i in removeIndex)
                    {
                        charts.RemoveAt(i);
                    }
                }

                return new string(charts.ToArray());
            }

            return sheetName;
        }
    }
}
