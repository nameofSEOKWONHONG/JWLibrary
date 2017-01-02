JWLibrary.FFmpeg

This library is desktop recording module.

This library was refer to below:

screen-capture-recorder[https://github.com/rdp/screen-capture-recorder-to-video-windows-free]

ffmpeg[http://www.ffmpeg.org/]

How to use : 
[update : 2017-01-02]

   /* If you run this module, you must run it as an administrator. */

    public partial class Form1 : Form
    {
        JWLibrary.FFmpeg.FFMpegCaptureAV _ffmpegCav
            = new FFmpeg.FFMpegCaptureAV();

        public Form1()
        {
            InitializeComponent();

			// add event code
            _ffmpegCav.FFmpegDataReceived += (s, e) =>
            {
                this.Invoke(
                    new MethodInvoker(delegate ()
                    {
                        lblFps.Text = e.Fps;
                        lblFrame.Text = e.Frame;
                        lblTime.Text = e.Time;
                    })                  
                );
            };

			// add event code
            _ffmpegCav.FrameDroped += (s, e) =>
            {
                Console.WriteLine("Frame drop!!!");
            };
        }

        private void btnRecStart_Click(object sender, EventArgs e)
        {
		    // add state code
            if (!_ffmpegCav.IsRunning)
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
                        FullFileName = @"E:\test.mp4"
                    };
                    var command = JWLibrary.FFmpeg.FFmpegCommandBuilder.BuildRecordingCommand(FFmpeg.RecordingTypes.Local, model);
                    _ffmpegCav.FFmpegCommandExcute(command);
                }
            }
        }

        private void btnRecStop_Click(object sender, EventArgs e)
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