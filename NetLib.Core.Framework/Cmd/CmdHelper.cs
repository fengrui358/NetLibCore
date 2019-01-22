using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FrHello.NetLib.Core.Framework.Cmd
{
    /// <summary>
    /// Cmd进程控制的包装器
    /// </summary>
    public class CmdHelper
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令</param>
        public static async Task<string> Excute(string command)
        {
            var cmd = GetCmd();
            var realCommand = $"{command} &exit";

            await cmd.StandardInput.WriteLineAsync(realCommand);
            cmd.StandardInput.AutoFlush = true;

            var result = cmd.StandardOutput.ReadToEnd();
            var outputStartIndex = result.IndexOf(realCommand, StringComparison.Ordinal);
            if (outputStartIndex > 0)
            {
                outputStartIndex += realCommand.Length;
            }

            cmd.WaitForExit();
            cmd.Close();

            return result.Substring(outputStartIndex, result.Length - outputStartIndex).Trim();
        }

        /// <summary>
        /// Ping命令封装
        /// </summary>
        /// <param name="ipAddress">Ip地址</param>
        /// <returns>是否Ping</returns>
        public static async Task<bool> Ping(IPAddress ipAddress)
        {
            var cmd = GetCmd();
            await cmd.StandardInput.WriteLineAsync($"ping {ipAddress} &exit");
            cmd.StandardInput.AutoFlush = true;

            while (!cmd.StandardOutput.EndOfStream)
            {
                var outLine = cmd.StandardOutput.ReadToEnd();
                Debug.WriteLine(outLine);
            }

            cmd.WaitForExit(); //等待程序执行完退出进程
            cmd.Close();

            return true;
        }

        /// <summary>
        /// 获取Cmd进程
        /// </summary>
        /// <returns></returns>
        public static Process GetCmd()
        {
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            return proc;
        }
    }
}