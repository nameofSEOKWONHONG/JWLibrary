using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.StaticMethod.CLI
{
    /// <summary>
    /// execute command line base
    /// (this method execute target fileName (process name))
    /// </summary>
    public class ProcessHandlerAsync
    {
        public static async Task<int> RunAsync(string fileName, string args, Action<string> outputReceived, Action<string> errorReceived)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo();
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                process.EnableRaisingEvents = true;

                return await RunAsync(process, outputReceived, errorReceived).ConfigureAwait(false);
            }
        }

        private static Task<int> RunAsync(Process process, Action<string> outputReceived, Action<string> errorReceived)
        {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += (s, e) => tcs.SetResult(process.ExitCode);
            process.OutputDataReceived += (s, e) => outputReceived(e.Data);
            process.ErrorDataReceived += (s, e) => errorReceived(e.Data);

            var isStarted = process.Start();
            if (!isStarted)
            {
                throw new InvalidOperationException("Could not start process");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return tcs.Task;
        }
    }
}
