using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace JWLibrary.FFmpeg
{
    public class RecCmdBuilder
    {
        private RecCmd _ffmpegCmdModel = null;
        private RecTypes _type;

        public RecCmdBuilder()
        {
            _ffmpegCmdModel = new RecCmd();
        }

        public RecCmdBuilder SetRecordingType(RecTypes type)
        {
            _type = type;
            return this;
        }

        public RecCmdBuilder SetVideoSource(string videoSourceName = "screen-capture-recorder")
        {
            _ffmpegCmdModel.VideoSource = videoSourceName;
            return this;
        }

        public RecCmdBuilder SetAudioSource(string audioSourceName = "virtual-audio-capturer")
        {
            _ffmpegCmdModel.AudioSource = audioSourceName;
            return this;
        }

        public RecCmdBuilder SetOffsetX(string offsetX)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey("software\\screen-capture-recorder", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            key.SetValue("start_x", offsetX , RegistryValueKind.DWord);
            
            return this;
        }

        public RecCmdBuilder SetOffsetY(string offsetY)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey("software\\screen-capture-recorder", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            key.SetValue("start_y", offsetY, RegistryValueKind.DWord);
            
            return this;
        }

        public RecCmdBuilder SetWidth(string width)
        {            
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey("software\\screen-capture-recorder", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            key.SetValue("capture_width", LengthNormalize(width), RegistryValueKind.DWord);

            return this;
        }

        public RecCmdBuilder SetHeight(string height)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\screen-capture-recorder", true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey("software\\screen-capture-recorder", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            key.SetValue("capture_height", LengthNormalize(height), RegistryValueKind.DWord);

            return this;
        }

        public RecCmdBuilder SetFrameRate(string framerate)
        {
            _ffmpegCmdModel.FrameRate = framerate;
            return this;
        }

        public RecCmdBuilder SetPreset(string preset)
        {
            _ffmpegCmdModel.Preset = preset;
            return this;
        }

        public RecCmdBuilder SetAudioQuality(string audioquality)
        {
            _ffmpegCmdModel.AudioQuality = audioquality;
            return this;
        }

        public RecCmdBuilder SetFileFormat(string format = "mp4")
        {
            _ffmpegCmdModel.Format = format;
            return this;
        }

        public RecCmdBuilder SetOption(string option)
        {
            _ffmpegCmdModel.Option1 = option;
            return this;
        }

        public RecCmdBuilder SetFileName(string filename)
        {
            _ffmpegCmdModel.FullFileName = filename;
            return this;
        }

        public RecCmdBuilder SetCpuCore(string corenum)
        {
            _ffmpegCmdModel.CpuCore = corenum;
            return this;
        }

        public RecCmdBuilder SetNVENC(bool isNVENC)
        {
            _ffmpegCmdModel.IsNVENC = isNVENC;
            return this;
        }

        public bool Build(out string cmd)
        {
            cmd = string.Empty;
            if(_type == RecTypes.Local)
            {
                if(!_ffmpegCmdModel.IsNVENC)
                    cmd = RecCmdConst.GET_REC_CMD();
                else
                    cmd = RecCmdConst.GET_REC_CMD_FOR_NVENC();
            }
            else if(_type == RecTypes.TwitchTV)
            {
                if (!_ffmpegCmdModel.IsNVENC)
                    cmd = RecCmdConst.GET_TWITCH_CMD();
                else
                    cmd = RecCmdConst.GET_TWITCH_CMD_FOR_NVENC();
            }
            else //youtube
            {
                if (!_ffmpegCmdModel.IsNVENC)
                    cmd = RecCmdConst.GET_YOUTUBE_CMD();
                else
                    cmd = RecCmdConst.GET_YOUTUBE_CMD_FOR_NVENC();
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
