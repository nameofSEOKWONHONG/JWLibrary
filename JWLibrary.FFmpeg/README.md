JWLibrary.FFmpeg

This library is desktop recording module.

This library was refer to below:

screen-capture-recorder[https://github.com/rdp/screen-capture-recorder-to-video-windows-free]

ffmpeg[http://www.ffmpeg.org/]

How to use :

   /*
    * If you run this module, you must run it as an administrator.
    */
    public partial class Form1 : Form
    {
        JWLibrary.FFmpeg.FFMpegCaptureAV _ffmpegCav
            = new FFmpeg.FFMpegCaptureAV();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_ffmpegCav.Initialize())
            {
                _ffmpegCav.Register();

                JWLibrary.FFmpeg.FFmpegCommandModel model = new FFmpeg.FFmpegCommandModel
                {
                    AudioQuality = JWLibrary.FFmpeg.FFmpegCommandParameterSupport.GetSupportAudioQuality()[0],
                    Format = "mp4",
                    FrameRate = JWLibrary.FFmpeg.FFmpegCommandParameterSupport.GetSupportFrameRate()[0],
                    Height = "1440",
                    Width = "2560",
                    OffsetX = "0",
                    OffsetY = "0",
                    Preset = JWLibrary.FFmpeg.FFmpegCommandParameterSupport.GetSupportPreset()[0],
                    FullFileName = [local filename(with path)]
                };
                var command = JWLibrary.FFmpeg.BuildCommand.BuildRecordingCommand(FFmpeg.RecordingTypes.Local, model);
                _ffmpegCav.FFmpegCommandExcute(command);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _ffmpegCav.FFmpegCommandStop();
            _ffmpegCav.UnRegister();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_ffmpegCav != null)
            {
                _ffmpegCav.Dispose();
            }

            base.OnClosing(e);
        }
    }