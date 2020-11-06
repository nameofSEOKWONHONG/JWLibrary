using JWLibrary.Core;
using System.Diagnostics;
using System.Linq;

namespace JWLibrary.Utils {

    /// <summary>
    ///     execute command line base
    ///     (this method execute cmd.exe base)
    /// </summary>
    public class ProcessHandler {
        private readonly string[] _killProcessNames;

        public ProcessHandler(string[] killProcessNames) {
            _killProcessNames = killProcessNames;
            _killProcessNames = _killProcessNames.jIfNull(x => x = new[] { "" });
        }

        public void Run(string fileName, string args, string workingDir) {
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

        public void Stop() {
            var pros = Process.GetProcesses();
            var exist = pros.Where(p => _killProcessNames.FirstOrDefault(m => m == p.ProcessName).jIsNotNull())
                .ToList();
            foreach (var proc in exist) {
                proc.CloseMainWindow();
                proc.Close();
            }
        }
    }
}