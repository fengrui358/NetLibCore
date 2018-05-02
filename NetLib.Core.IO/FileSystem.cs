using System.IO;

namespace FrHello.NetLib.Core.IO
{
    public static class FileSystem
    {
        public static string ResolveAbsolutePath(string currentPath, string relativePath)
        {
            var p = Path.GetFullPath(relativePath);
            return null;
        }
    }
}
