using System;
using System.Threading.Tasks;

namespace JWLibrary.FFmpeg
{
    public class FFmpegCommandBuilder
    {
        public static string BuildRecordingCommand(RecordingTypes rType, FFmpegCommandModel model, bool IsNEVNC)
        {
            switch (rType)
            {
                case RecordingTypes.Local:
                    return BuildRecordingCommandForLocal(model, IsNEVNC);
                case RecordingTypes.TwitchTV:
                    return BuildRecordingCommandForTwitchTV(model, IsNEVNC);
                case RecordingTypes.YouTube:
                    throw new Exception("This type was not implemented.");
                default:
                    throw new Exception("Unknown type.");
            }
        }

        /// <summary>
        /// making ffmpeg command string
        /// </summary>
        /// <param name="model"></param>
        /// <returns>builded ffmpeg command string</returns>
        private static string BuildRecordingCommandForLocal(FFmpegCommandModel model, bool IsNEVNC)
        {
            var tuple = ChecWHValue(model.Width, model.Height);

            string command = "";
            if(!IsNEVNC)
                command = CommandConst.GET_DESKTOP_RECORDING_COMMAND();
            else
                command = CommandConst.GET_DESKTOP_RECORDING_COMMAND_NVIDIA();

            command = command.Replace("@videoSource", model.VideoSource);
            command = command.Replace("@audioSource", model.AudioSource);
            command = command.Replace("@x", model.OffsetX);
            command = command.Replace("@y", model.OffsetY);
            command = command.Replace("@width", tuple.Item1);
            command = command.Replace("@height", tuple.Item2);
            command = command.Replace("@framerate", model.FrameRate);
            command = command.Replace("@preset", model.Preset);
            command = command.Replace("@audioRate", model.AudioQuality);
            command = command.Replace("@format", model.Format);
            command = command.Replace("@option1", model.Option1);
            command = command.Replace("@filename", model.FullFileName);
            command = command.Replace("@outputquality", model.OutPutQuality);
            command = command.Replace("@cpucore", model.CpuCore);

            return command;
        }

        private static Tuple<string, string> ChecWHValue(string width, string height)
        {
            var tempWidth = 0;
            var tempHeight = 0;

            Task t1 = new Task(() =>
            {
                if (int.TryParse(width, out tempWidth))
                {
                    while (true)
                    {
                        if (tempWidth % 2 == 0)
                            break;

                        tempWidth++;
                    }
                }
            });

            Task t2 = new Task(() =>
            {
                if (int.TryParse(height, out tempHeight))
                {
                    while (true)
                    {
                        if (tempHeight % 2 == 0)
                            break;

                        tempHeight++;

                    }
                }
            });

            t1.Start();t2.Start();
            Task.WaitAll(new[] { t1, t2 });


            return new Tuple<string, string>(tempWidth.ToString(), tempHeight.ToString());
        }

        /// <summary>
        /// not support yet.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>builded ffmpeg command string</returns>
        private static string BuildRecordingCommandForTwitchTV(FFmpegCommandModel model, bool IsNEVNC)
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
    }
}
