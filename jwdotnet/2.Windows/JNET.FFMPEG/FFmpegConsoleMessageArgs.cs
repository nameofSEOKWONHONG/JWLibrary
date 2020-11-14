using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JNET.FFMPEG {
    public class FFmpegConsoleMessageArgs {
        public string Time { get; set; }
        public string Fps { get; set; }
        public string Frame { get; set; }

        public FFmpegConsoleMessageArgs(string time, string fps, string frame) {
            this.Time = time;
            this.Fps = fps;
            this.Frame = frame;
        }
    }
}
