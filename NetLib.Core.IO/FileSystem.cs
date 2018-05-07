using System.IO;

namespace FrHello.NetLib.Core.IO
{
    /// <summary>
    /// 文件系统
    /// todo:未完成
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// 解析绝对路径
        /// todo:未完成
        /// </summary>
        /// <param name="currentPath"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string ResolveAbsolutePath(string currentPath, string relativePath)
        {
            var p = Path.GetFullPath(relativePath);
            return null;
        }
    }
}
