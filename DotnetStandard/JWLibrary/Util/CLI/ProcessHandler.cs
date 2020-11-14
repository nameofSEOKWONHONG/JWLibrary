using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace JWLibrary.Utils {

    /// <summary>
    ///     execute command line base
    ///     (this method execute target fileName (process name) and repeated event)
    /// </summary>
    public class ProcessHandler : IDisposable {

        #region variables

        private bool disposed;

        #endregion variables

        #region Constructor

        public ProcessHandler() {
            RunProcess = new Process();
        }

        #endregion Constructor

        #region properties

        public Process RunProcess { get; private set; }

        #endregion properties

        #region define events

        public event EventHandler<DataReceivedEventArgs> CommandDataReceived;

        public virtual void OnCommandDataReceived(object sender, DataReceivedEventArgs e) {
            if (CommandDataReceived != null) CommandDataReceived(this, e);
        }

        #endregion define events

        #region Functions

        public void ExecuteCommand(string workingDir, string exeFullFileName, string arguments, bool createNoWindow) {
            SetCommandLine(workingDir, exeFullFileName, arguments, createNoWindow);
            CommandLineStart();
        }

        private void SetCommandLine(string workingDir, string exeFullFileName, string arguments, bool createNoWindow) {
            RunProcess.StartInfo.WorkingDirectory = workingDir;
            RunProcess.StartInfo.FileName = exeFullFileName;
            RunProcess.StartInfo.Arguments = arguments;
            RunProcess.StartInfo.CreateNoWindow = createNoWindow;
            RunProcess.StartInfo.UseShellExecute = false;
            RunProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            RunProcess.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            RunProcess.StartInfo.RedirectStandardError = true;
            RunProcess.StartInfo.RedirectStandardOutput = true;
            RunProcess.StartInfo.RedirectStandardInput = true;
            RunProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            RunProcess.ErrorDataReceived += (s, e) => { OnCommandDataReceived(this, e); };
        }

        private void CommandLineStart() {
            RunProcess.Start();
            RunProcess.BeginOutputReadLine();
            RunProcess.BeginErrorReadLine();
        }

        public void CommandLineStandardInput(string command) {
            RunProcess.StandardInput.Write(command);
        }

        public void CommandLineStop() {
            if (!RunProcess.HasExited) {
                RunProcess.Kill();
                Thread.Sleep(100);
            }
        }

        #endregion Functions

        #region dispose

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing)
                if (RunProcess != null) {
                    RunProcess.Dispose();
                    RunProcess = null;
                }

            disposed = true;
        }

        #endregion dispose
    }
}