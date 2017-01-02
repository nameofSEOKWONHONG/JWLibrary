using System;

namespace JWLibrary.FFmpeg
{
    public class FFmpegCommandBuilder
    {
        public static string BuildRecordingCommand(RecordingTypes rType, FFmpegCommandModel model)
        {
            switch (rType)
            {
                case RecordingTypes.Local:
                    return BuildRecordingCommandForLocal(model);
                case RecordingTypes.TwitchTV:
                    return BuildRecordingCommandForTwitchTV(model);
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
        private static string BuildRecordingCommandForLocal(FFmpegCommandModel model)
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

        /// <summary>
        /// not support yet.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>builded ffmpeg command string</returns>
        private static string BuildRecordingCommandForTwitchTV(FFmpegCommandModel model)
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
