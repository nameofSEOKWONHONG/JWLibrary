using JWLibrary.FFmpeg.Properties;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace JWLibrary.FFmpeg
{
    public class FFMpegCaptureAV : IDisposable
    {
        #region delegate events        
        public event EventHandler<FFmpegDataReceiveArgs> FFmpegDataReceived;
        protected virtual void OnDataReceived(object sender, FFmpegDataReceiveArgs e)
        {
            if (FFmpegDataReceived != null)
            {
                FFmpegDataReceived(this, e);
            }
        }

        public event EventHandler<EventArgs> FrameDroped;
        protected virtual void OnFrameDroped(object sender, EventArgs e)
        {
            if (FrameDroped != null)
            {
                FrameDroped(this, e);
            }
        }

        public event EventHandler<EventArgs> ErrorOccured;
        protected virtual void OnErrorOccured(object sender, EventArgs e)
        {
            if(ErrorOccured != null)
            {
                ErrorOccured(this, e);
            }
        }
        #endregion

        #region variable
        bool disposed = false;
        private CLIHelper _cmdHelper;
        private FrameDropChecker _frameDropChecker;
        private string[] _param;

        private const string FFMPEG_PROCESS_NAME = "ffmpeg";
        private const string FFMPEG_FILE_NAME = "ffmpeg.exe";
        private const string STOP_COMMAND = "q";
        private const string DROP_KEYWORD = "FRAME DROPPED!";
        private const string AUDIO_SNIFFER_FILE_NAME = "audio_sniffer.dll";
        private const string SCREEN_CAPTURE_RECORDER_FILE_NAME = "screen_capture_recorder.dll";
        private const string LIBRARY_REGISTER_FILE_NAME = "library-register.bat";
        private const string LIBRARY_UNREGISTER_FILE_NAME = "library-unregister.bat";
        #endregion

        #region property
        public bool IsRunning { get; private set; }
        #endregion

        #region constructor
        public FFMpegCaptureAV()
        {
            this._cmdHelper = new CLIHelper();
            this._frameDropChecker = new FrameDropChecker();
            this._cmdHelper.CommandDataReceived += cmdHelper_CommandDataReceived;
            this._frameDropChecker.FrameDroped += _frameDropChecker_FrameDroped;            
        }
        #endregion

        public bool Initialize()
        {
            var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var ffmpegFilePath = Path.Combine(executablePath, FFMPEG_FILE_NAME);

            if (!File.Exists(ffmpegFilePath))
            {
                File.WriteAllBytes(ffmpegFilePath, Resources.ffmpeg);
            }

            var audioSnifferFilePath = Path.Combine(executablePath, AUDIO_SNIFFER_FILE_NAME);
            if (!File.Exists(audioSnifferFilePath))
            {
                File.WriteAllBytes(audioSnifferFilePath, Resources.audio_sniffer);
            }

            var scrFilePath = Path.Combine(executablePath, SCREEN_CAPTURE_RECORDER_FILE_NAME);
            if (!File.Exists(scrFilePath))
            {
                File.WriteAllBytes(scrFilePath, Resources.screen_capture_recorder);
            }

            return IsRequireFile();
        }

        /// <summary>
        /// Call after initialization.
        /// </summary>
        public void Register()
        {
            var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var registerPath = Path.Combine(executablePath, LIBRARY_REGISTER_FILE_NAME);

            var audioSnifferFilePath = Path.Combine(executablePath, AUDIO_SNIFFER_FILE_NAME);
            var scrFilePath = Path.Combine(executablePath, SCREEN_CAPTURE_RECORDER_FILE_NAME);

            string contents = Resources.library_register
                                .Replace("@mode", "/s")
                                .Replace("@audio_sniffer_file", audioSnifferFilePath)
                                .Replace("@screen_capture_recorder_file", scrFilePath);
            File.WriteAllText(registerPath, contents);

            //process
            var processInfo = new ProcessStartInfo("cmd.exe", " /c " + @"""" + registerPath + @"""");
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.Verb = "runas";

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                Debug.WriteLine("output>>" + e.Data);
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                Debug.WriteLine("error>>" + e.Data);
            process.BeginErrorReadLine();

            process.WaitForExit();

            Debug.WriteLine("ExitCode: {0}", process.ExitCode);
            process.Close();

            File.Delete(registerPath);

            RegistryWrite();
        }

        private void RegistryWrite()
        {
            var screen = Screen.PrimaryScreen;
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey("software\\screen-capture-recorder", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }

            var height = key.GetValue("capture_height");

            if (Convert.ToDouble(height) != screen.Bounds.Height)
            {
                key.SetValue("capture_height", screen.Bounds.Height, RegistryValueKind.DWord);
            }

            var width = key.GetValue("capture_width");
            if (Convert.ToDouble(width) != screen.Bounds.Width)
            {
                key.SetValue("capture_width", screen.Bounds.Width, RegistryValueKind.DWord);
            }
            key.SetValue("start_x", 0, RegistryValueKind.DWord);
            key.SetValue("start_y", 0, RegistryValueKind.DWord);
            key.SetValue("default_max_fps", 60, RegistryValueKind.DWord);
        }

        public void SetFPS(int fps)
        {            
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);

            if (key == null)
            {
                RegistryWrite();
            }

            key.SetValue("default_max_fps", fps, RegistryValueKind.DWord);
        }

        /// <summary>
        /// Call before dispose.
        /// </summary>
        public void UnRegister()
        {
            var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var registerPath = Path.Combine(executablePath, LIBRARY_REGISTER_FILE_NAME);

            var audioSnifferFilePath = Path.Combine(executablePath, AUDIO_SNIFFER_FILE_NAME);
            var scrFilePath = Path.Combine(executablePath, SCREEN_CAPTURE_RECORDER_FILE_NAME);

            string contents = Resources.library_register
                                .Replace("@mode", "/u /s")
                                .Replace("@audio_sniffer_file", audioSnifferFilePath)
                                .Replace("@screen_capture_recorder_file", scrFilePath);
            File.WriteAllText(registerPath, contents);

            //process
            var processInfo = new ProcessStartInfo("cmd.exe", " /c " + @"""" + registerPath + @"""");
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                Debug.WriteLine("output>>" + e.Data);
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                Debug.WriteLine("error>>" + e.Data);
            process.BeginErrorReadLine();

            process.WaitForExit();

            Debug.WriteLine("ExitCode: {0}", process.ExitCode);
            process.Close();            

            File.Delete(registerPath);
            Thread.Sleep(500);

            try
            {
                File.Delete(audioSnifferFilePath);
                Thread.Sleep(500);
            }
            catch
            {
                //why happen delete error???
                //for only twitch mode
            }

            File.Delete(scrFilePath);
            Thread.Sleep(500);
        }

        /// <summary>
        /// Check required files.
        /// </summary>
        /// <returns></returns>
        public bool IsRequireFile()
        {
            var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var ffmpegFilePath = Path.Combine(executablePath, FFMPEG_FILE_NAME);

            if (!File.Exists(ffmpegFilePath))
            {
                return false;
            }

            var audioSnifferFilePath = Path.Combine(executablePath, AUDIO_SNIFFER_FILE_NAME);
            if (!File.Exists(audioSnifferFilePath))
            {
                return false;
            }

            var scrFilePath = Path.Combine(executablePath, SCREEN_CAPTURE_RECORDER_FILE_NAME);
            if (!File.Exists(scrFilePath))
            {
                return false;
            }

            return true;
        }

        #region events
        private void _frameDropChecker_FrameDroped(object sender, EventArgs e)
        {
            FrameDroped(this, new EventArgs());
        }

        private void cmdHelper_CommandDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.ToUpper().Contains(DROP_KEYWORD))
                {
                    this._frameDropChecker.FrameDropCount++;
                }

                if (e.Data.Contains("time="))
                {
                    int startIndex = e.Data.IndexOf("time=") + 5;
                    _param = e.Data.Split(new string[] { "=", " " }, StringSplitOptions.RemoveEmptyEntries);
                    
                    OnDataReceived(this, 
                        new FFmpegDataReceiveArgs(
                            e.Data.Substring(startIndex, 11), _param[3], _param[1]));
                }         
                
                if(e.Data.ToUpper().Contains("ERROR"))
                {
                    OnErrorOccured(this, e);
                }
            }
        }
        #endregion

        #region functions
        public void RecordingStart(string arguments)
        {
            if (IsRequireFile())
            {
                this._cmdHelper.ExecuteCommand(/*workingDir*/null, "ffmpeg.exe", arguments, true);
                this._frameDropChecker.FrameDropCheckStart();

                IsRunning = true;
            }
        }

        /// <summary>
        /// ffmpeg recording stop.
        /// </summary>        
        public void RecordingStop()
        {
            IsRunning = false;

            this._cmdHelper.CommandLineStandardInput(STOP_COMMAND);
            this._frameDropChecker.FrameDropCheckStop();
        }

        /// <summary>
        /// ffmpeg command process force stop.
        /// (process killed.)
        /// </summary>
        public void RecordingForceStop()
        {
            this._cmdHelper.CommandLineStop();
            this._frameDropChecker.FrameDropCheckStop();
        }

        public bool IsProcessHasExited()
        {
            return this._cmdHelper.RunProcess.HasExited;
        }
        #endregion

        #region dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            //memory free is here
            if (disposing)
            {
                System.Diagnostics.Process[] procs =
                     System.Diagnostics.Process.GetProcessesByName(FFMPEG_PROCESS_NAME);

                foreach (var item in procs)
                {
                    item.StandardInput.Write(STOP_COMMAND);
                    item.WaitForExit();
                }

                if (_cmdHelper != null)
                {
                    _cmdHelper.CommandDataReceived -= cmdHelper_CommandDataReceived;
                    _cmdHelper.Dispose();
                    _cmdHelper = null;
                }

                if (_frameDropChecker != null)
                {
                    _frameDropChecker.FrameDroped -= _frameDropChecker_FrameDroped;
                    _frameDropChecker.Dispose();
                    _frameDropChecker = null;
                }
            }

            disposed = true;
        }
        #endregion
    }

    public class FFmpegDataReceiveArgs : EventArgs {
        public string Time { get; set; }
        public string Fps { get; set; }
        public string Frame { get; set; }

        public FFmpegDataReceiveArgs(string time, string fps, string frame)
        {
            this.Time = time;
            this.Fps = fps;
            this.Frame = frame;
        }
    }

}
