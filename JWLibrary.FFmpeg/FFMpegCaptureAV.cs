using JWLibrary.CLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.FFmpeg
{
    public class FFMpegCaptureAV : IDisposable
    {
        #region delegate events
        public event EventHandler<System.Diagnostics.DataReceivedEventArgs> DataReceived;
        protected virtual void OnDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (DataReceived != null)
            {
                DataReceived(this, e);
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
        #endregion

        #region variable
        bool disposed = false;
        private CLIHelper _cmdHelper;
        private FrameDropChecker _frameDropChecker;
        public static string ExeFileName = @"ffmpeg.exe";
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

        #region events
        private void _frameDropChecker_FrameDroped(object sender, EventArgs e)
        {
            FrameDroped(this, new EventArgs());
        }

        private void cmdHelper_CommandDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                //Console.WriteLine(e.Data);
                if (e.Data.ToUpper().Contains("FRAME DROPPED!"))
                {
                    this._frameDropChecker.FrameDropCount++;
                }
            }
            OnDataReceived(this, e);
        }
        #endregion

        #region functions
        public void FFmpegCommandExcute(string workingDir, string exeFileName, string arguments, bool createNoWindow)
        {
            this._cmdHelper.ExecuteCommand(workingDir, exeFileName, arguments, createNoWindow);
            this._frameDropChecker.FrameDropCheckStart();
        }

        public void FFmpegCommandStop(string stopCommand)
        {
            this._cmdHelper.CommandLineStandardInput(stopCommand);
            this._frameDropChecker.FrameDropCheckStop();
        }

        public void FFmpegForceStop()
        {
            this._cmdHelper.CommandLineStop();
            this._frameDropChecker.FrameDropCheckStop();
        }

        public bool ProcessHasExited()
        {
            return this._cmdHelper.RunProcess.HasExited;
        }

        public string BuildRecordingCommand(FFmpegCommandModel model)
        {
            string command = CommandConst.GET_DESKTOP_RECODING_COMMAND();
            command = command.Replace("@videoSource", model.VideoSource);
            command = command.Replace("@audioSource", model.AudioSource);
            command = command.Replace("@x", model.OffsetX);
            command = command.Replace("@y", model.OffsetY);
            command = command.Replace("@width", model.Width);
            command = command.Replace("@height", model.Height);
            command = command.Replace("@framerate", model.FrameRate);
            command = command.Replace("@preset", model.Preset);
            command = command.Replace("@audioRate", model.AudioQuality);
            command = command.Replace("@format", model.Format);
            command = command.Replace("@option1", model.Option1);
            command = command.Replace("@filename", model.FullFileName);
            command = command.Replace("@outputquality", model.OutPutQuality);
            return command;
        }

        //not tested.
        public string BuildTwitchLiveCommnad(FFmpegCommandModel model)
        {
            string command = CommandConst.GET_TWITCH_LIVE_COMMNAD();
            command = command.Replace("@videoSource", model.VideoSource);
            command = command.Replace("@audioSource", model.AudioSource);
            command = command.Replace("@x", model.OffsetX);
            command = command.Replace("@y", model.OffsetY);
            command = command.Replace("@width", model.Width);
            command = command.Replace("@height", model.Height);
            command = command.Replace("@framerate", model.FrameRate);
            command = command.Replace("@preset", model.Preset);
            command = command.Replace("@audioRate", model.AudioQuality);
            command = command.Replace("@format", model.Format);
            command = command.Replace("@option1", model.Option1);
            command = command.Replace("@liveUrl", model.FullFileName);
            command = command.Replace("@outputquality", model.OutPutQuality);
            return command;
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
}
