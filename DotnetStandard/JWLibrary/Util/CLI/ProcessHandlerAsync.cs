using JWLibrary.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Util.CLI {
    /// <summary>
    ///     execute command line base
    ///     (this method execute target fileName (process name))
    /// </summary>
    public class ProcessHandlerAsync {

        public static async Task<int> RunAsync(string fileName, string args, Action<string> outputReceived,
            Action<string> errorReceived)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8,
                    WindowStyle = ProcessWindowStyle.Normal
                },
                EnableRaisingEvents = true
            };

            return await RunAsync(process, outputReceived, errorReceived).ConfigureAwait(false);
        }

        private static Task<int> RunAsync(Process process, Action<string> outputReceived, Action<string> errorReceived) {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += (s, e) => tcs.SetResult(process.ExitCode);
            process.OutputDataReceived += (s, e) => {
                if (!e.Data.jIsNullOrEmpty())
                    outputReceived(e.Data);
            };
            process.ErrorDataReceived += (s, e) => {
                if (!e.Data.jIsNullOrEmpty())
                    errorReceived(e.Data);
            };

            var isStarted = process.Start();
            if (!isStarted) throw new InvalidOperationException("Could not start process");

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return tcs.Task;
        }
    }
}