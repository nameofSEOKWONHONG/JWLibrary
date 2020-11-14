using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNET.FFMPEG {
    internal class RecEngineConst {
        public static readonly string FFMPEG_PROCESS_NAME = "ffmpeg";
        public static readonly string FFMPEG_FILE_NAME = "ffmpeg.exe";
        public static readonly string STOP_COMMAND = "q";
        public static readonly string DROP_KEYWORD = "FRAME DROPPED!";
        public static readonly string AUDIO_SNIFFER_FILE_NAME = "audio_sniffer.dll";
        public static readonly string SCREEN_CAPTURE_RECORDER_FILE_NAME = "screen_capture_recorder.dll";
        public static readonly string LIBRARY_REGISTER_FILE_NAME = "library-register.bat";
        public static readonly string LIBRARY_UNREGISTER_FILE_NAME = "library-unregister.bat";
    }
}
