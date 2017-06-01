using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace JWLibrary.FFmpeg
{
    public class FFmpegCommandBuilder
    {
        private FFmpegCommandModel _ffmpegCmdModel = null;
        private RecordingTypes _type;

        public FFmpegCommandBuilder()
        {
            _ffmpegCmdModel = new FFmpegCommandModel();
        }

        public FFmpegCommandBuilder SetRecordingType(RecordingTypes type)
        {
            _type = type;
            return this;
        }

        public FFmpegCommandBuilder SetVideoSource(string videoSourceName)
        {
            _ffmpegCmdModel.VideoSource = videoSourceName;
            return this;
        }

        public FFmpegCommandBuilder SetAudioSource(string audioSourceName)
        {
            _ffmpegCmdModel.AudioSource = audioSourceName;
            return this;
        }

        public FFmpegCommandBuilder SetOffsetX(string offsetX)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey("software\\screen-capture-recorder", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            key.SetValue("start_x", offsetX , RegistryValueKind.DWord);
            
            return this;
        }

        public FFmpegCommandBuilder SetOffsetY(string offsetY)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey("software\\screen-capture-recorder", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            key.SetValue("start_y", offsetY, RegistryValueKind.DWord);
            
            return this;
        }

        public FFmpegCommandBuilder SetWidth(string width)
        {            
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey("software\\screen-capture-recorder", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            key.SetValue("capture_width", LengthNormalize(width), RegistryValueKind.DWord);

            return this;
        }

        public FFmpegCommandBuilder SetHeight(string height)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey("software\\screen-capture-recorder", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            key.SetValue("capture_height", LengthNormalize(height), RegistryValueKind.DWord);

            return this;
        }

        public FFmpegCommandBuilder SetFrameRate(string framerate)
        {
            _ffmpegCmdModel.FrameRate = framerate;
            return this;
        }

        public FFmpegCommandBuilder SetPreset(string preset)
        {
            _ffmpegCmdModel.Preset = preset;
            return this;
        }

        public FFmpegCommandBuilder SetAudioQuality(string audioquality)
        {
            _ffmpegCmdModel.AudioQuality = audioquality;
            return this;
        }

        public FFmpegCommandBuilder SetFileFormat(string format = "mp4")
        {
            _ffmpegCmdModel.Format = format;
            return this;
        }

        public FFmpegCommandBuilder SetOption(string option)
        {
            _ffmpegCmdModel.Option1 = option;
            return this;
        }

        public FFmpegCommandBuilder SetFileName(string filename)
        {
            _ffmpegCmdModel.FullFileName = filename;
            return this;
        }

        public FFmpegCommandBuilder SetCpuCore(string corenum)
        {
            _ffmpegCmdModel.CpuCore = corenum;
            return this;
        }

        public FFmpegCommandBuilder SetNVENC(bool isNVENC)
        {
            _ffmpegCmdModel.IsNVENC = isNVENC;
            return this;
        }

        public bool Build(out string cmd)
        {
            cmd = string.Empty;
            if(_type == RecordingTypes.Local)
            {
                if(!_ffmpegCmdModel.IsNVENC)
                    cmd = CommandConst.GET_DESKTOP_RECORDING_COMMAND();
                else
                    cmd = CommandConst.GET_DESKTOP_RECORDING_COMMAND_NVIDIA();
            }
            else if(_type == RecordingTypes.TwitchTV)
            {
                if (!_ffmpegCmdModel.IsNVENC)
                    cmd = CommandConst.GET_TWITCH_LIVE_COMMNAD();
                else
                    cmd = CommandConst.GET_TWITCH_LIVE_COMMNAD_NVIDIA();
            }

            cmd = cmd.Replace("@videoSource", _ffmpegCmdModel.VideoSource);
            cmd = cmd.Replace("@audioSource", _ffmpegCmdModel.AudioSource);
            //cmd = cmd.Replace("@x", _ffmpegCmdModel.OffsetX);
            //cmd = cmd.Replace("@y", _ffmpegCmdModel.OffsetY);
            //cmd = cmd.Replace("@width", tuple.Item1);
            //cmd = cmd.Replace("@height", tuple.Item2);
            cmd = cmd.Replace("@framerate", _ffmpegCmdModel.FrameRate);
            cmd = cmd.Replace("@preset", _ffmpegCmdModel.Preset);
            cmd = cmd.Replace("@audioquality", _ffmpegCmdModel.AudioQuality);
            cmd = cmd.Replace("@format", _ffmpegCmdModel.Format);
            cmd = cmd.Replace("@option1", _ffmpegCmdModel.Option1);
            cmd = cmd.Replace("@filename", _ffmpegCmdModel.FullFileName);            
            cmd = cmd.Replace("@cpucore", _ffmpegCmdModel.CpuCore);

            if (cmd.Contains("@")) return false;

            return true;
        }

        private static string LengthNormalize(string length)
        {
            var tempLength = 0;            

            //Task t1 = new Task(() =>
            //{
            //    if (int.TryParse(length, out tempLength))
            //    {
            //        while (true)
            //        {
            //            if (tempLength % 2 == 0)
            //                break;

            //            tempLength++;
            //        }
            //    }
            //});

            //t1.Start();
            //Task.WaitAll(new[] { t1 });

            if (int.TryParse(length, out tempLength))
            {
                while (true)
                {
                    if (tempLength % 2 == 0)
                        break;

                    tempLength++;
                }
            }


            return tempLength.ToString();
        }
    }
}
