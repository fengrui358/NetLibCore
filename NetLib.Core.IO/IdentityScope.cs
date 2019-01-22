using System;
using System.Runtime.InteropServices;

namespace FrHello.NetLib.Core.IO
{
    /// <summary>
    /// 登陆局域网认证授权
    /// <example>
    /// using (new IdentityScope(Config.Instance.RemoteDirectory.UserName,
    ///     Config.Instance.RemoteDirectory.RemoteIpAddress, Config.Instance.RemoteDirectory.Password))
    /// {
    ///     File.Copy(zipFilePath,
    ///     Path.Combine(Config.Instance.RemoteDirectory.RemoteFullPath, zipFileName));
    /// }
    /// </example>
    /// </summary>
    public class IdentityScope : IDisposable
    {
        /// <summary>
        /// obtains user token
        /// </summary>
        /// <param name="pszUsername"></param>
        /// <param name="pszDomain"></param>
        /// <param name="pszPassword"></param>
        /// <param name="dwLogonType"></param>
        /// <param name="dwLogonProvider"></param>
        /// <param name="phToken"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        /// <summary>
        /// closes open handes returned by LogonUser
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("Advapi32.DLL")]
        private static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("Advapi32.DLL")]
        private static extern bool RevertToSelf();

        private const int Logon32ProviderDefault = 0;

        /// <summary>
        /// 域控中的需要用:Interactive = 2
        /// </summary>
        private const int Logon32LogonNewcredentials = 9;

        private bool _disposed;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="sUsername">用户名</param>
        /// <param name="sDomain">地址</param>
        /// <param name="sPassword">密码</param>
        public IdentityScope(string sUsername, string sDomain, string sPassword)
        {
            // initialize tokens  
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);

            try
            {
                // get handle to token  
                bool bImpersonated = LogonUser(sUsername, sDomain, sPassword,
                    Logon32LogonNewcredentials, Logon32ProviderDefault, ref pExistingTokenHandle);

                if (bImpersonated)
                {
                    if (!ImpersonateLoggedOnUser(pExistingTokenHandle))
                    {
                        int nErrorCode = Marshal.GetLastWin32Error();
                        throw new Exception("ImpersonateLoggedOnUser error;Code=" + nErrorCode);
                    }
                }
                else
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    throw new Exception("LogonUser error;Code=" + nErrorCode);
                }
            }
            finally
            {
                // close handle(s)  
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                RevertToSelf();
                _disposed = true;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}