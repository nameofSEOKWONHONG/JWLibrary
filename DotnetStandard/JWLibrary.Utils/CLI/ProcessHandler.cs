using JWLibrary.Core;
using JWLibrary.StaticMethod;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JWLibrary.Utils {
    /// <summary>
    /// execute command line base
    /// (this method execute cmd.exe base)
    /// </summary>
    public class ProcessHandler
    {
        readonly string[] _killProcessNames = null;

        public ProcessHandler(string[] killProcessNames)
        {
            this._killProcessNames = killProcessNames;
            this._killProcessNames = this._killProcessNames.jIfNull(x => x = new[] { "" });
        }

        public void Run(string fileName, string args, string workingDir)
        {
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo();
            proc.StartInfo.FileName = "cmd";
            proc.StartInfo.Arguments = string.Format("/{0} {1}", "k", fileName, args);
            //proc.StartInfo.CreateNoWindow = true;
            //proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.WorkingDirectory = workingDir;
            proc.Start();
            //Process.Start("cmd", string.Format("/{0} {1}", "k", fileName, args));
        }

        public void Stop()
        {
            var procs = Process.GetProcesses();
            var exist = procs.Where(p => _killProcessNames.Where(m => m == p.ProcessName).FirstOrDefault().jIsNotNull()).ToList();
            foreach (var proc in exist)
            {
                proc.CloseMainWindow();
                proc.Close();
            }
        }
    }
}
