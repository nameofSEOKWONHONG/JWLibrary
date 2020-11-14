using System;
using System.IO;
using JWLibrary;
using JWLibrary.Core;
using JWLibrary.Utils;

namespace JNET.FFMPEG {
    public class RecEngine : IDisposable {
        public Action<FFmpegConsoleMessageArgs> FFMpegEventAction { get; set; }
        public Action<string> FFMpegErrorAction { get; set; }
        private ProcessHandler processHandler = null;
        private bool disposed;

        #region [constructor]
        public RecEngine() {
            processHandler = new ProcessHandler();
            processHandler.CommandDataReceived += (s, e) => {
                if(FFMpegEventAction.jIsNotNull()) {
                    if(e.Data.jIsNotNull()) {
                        if(e.Data.Contains("ERROR", StringComparison.OrdinalIgnoreCase)) {
                            FFMpegErrorAction(e.Data);
                            return;
                        }

                        var startIndex = e.Data.IndexOf("time=") + 5;
                        var param = e.Data.Split(new string[] { "=", " " }, StringSplitOptions.RemoveEmptyEntries);
                        var time = e.Data.Substring(startIndex, 11);
                        FFMpegEventAction(new FFmpegConsoleMessageArgs(time, param[3], param[1]));
                    }
                }
            };
        }
        #endregion

        #region [rec methods]
        public void Init() {
            var executePath = "".jToAppPath();
            var ffmpegFilePath = Path.Combine(executePath, RecEngineConst.FFMPEG_FILE_NAME);

            if(!File.Exists(ffmpegFilePath)) {
                throw new Exception($"don't find {RecEngineConst.FFMPEG_FILE_NAME}.");
            }

            var audioSnifferFilePath = Path.Combine(executePath, RecEngineConst.AUDIO_SNIFFER_FILE_NAME);
            if(!File.Exists(audioSnifferFilePath)) {
                throw new Exception($"don't find {RecEngineConst.AUDIO_SNIFFER_FILE_NAME}.");
            }

            var screenCaptureRecorderFilePath = Path.Combine(executePath, RecEngineConst.SCREEN_CAPTURE_RECORDER_FILE_NAME);
            if(!File.Exists(screenCaptureRecorderFilePath)) {
                throw new Exception($"don't find {RecEngineConst.SCREEN_CAPTURE_RECORDER_FILE_NAME}.");
            }
        }

        public void Register() {

        }

        public void UnRegister() {

        }

        public bool RecStart() {
            return true;
        }

        public bool RecStop() {
            return true;
        }

        public bool RecForceStop() {
            if(processHandler.jIsNotNull()) {
                processHandler.CommandLineStop();
            }
            return true;
        }

        public bool IsProcessHasExited() {
            if (processHandler.jIsNull()) return false;
            return processHandler.RunProcess.HasExited;
        }

        #endregion

        #region [dispose]
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) return;

            if (disposing) {
                if (processHandler.jIsNotNull()) {
                    processHandler.Dispose();
                    processHandler = null;
                }

                System.Diagnostics.Process[] procs =
                     System.Diagnostics.Process.GetProcessesByName(RecEngineConst.FFMPEG_PROCESS_NAME);

                procs.jForEach(proc => {
                    proc.StandardInput.Write(RecEngineConst.STOP_COMMAND);
                    proc.WaitForExit();
                    return true;
                });
            }

            disposed = true;
        }

        #endregion
    }
}
