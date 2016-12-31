JWLibrary.FFmpeg

This library is desktop recording module.

This library was refer to below:

screen-capture-recorder[https://github.com/rdp/screen-capture-recorder-to-video-windows-free]

ffmpeg[http://www.ffmpeg.org/]

How to use :

/*
 * If you run this module, you must run it as an administrator.
 */

        [TestMethod]
        public void RunRecording()
        {
            JWLibrary.FFmpeg.FFMpegCaptureAV ffmpegCav = new FFmpeg.FFMpegCaptureAV();
            ffmpegCav.Initialize();

			//regsvr32 dll register
            ffmpegCav.Register();

            var model = new FFmpeg.FFmpegCommandModel()
            {
				//recording information.
            };

			var command = FFmpeg.BuildCommand.BuildRecordingCommand(FFmpeg.RecordingTypes.Local, model);
            ffmpegCav.FFmpegCommandExcute(command);

			//regsvr32 dll unregister
            ffmpegCav.UnRegister();
        }