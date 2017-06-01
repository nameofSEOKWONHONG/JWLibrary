namespace JWLibrary.FFmpeg
{
    public class CommandConst
    {
        public static string GET_AV_DEVICE_INFOMATION_COMMAND()
        {
            return "-list_devices true -f dshow -i dummy";
        }

        public static string GET_DESKTOP_RECORDING_COMMAND()
        {
            return @"-rtbufsize 2000M -f dshow -y -i video=""@videoSource"":audio=""@audioSource"" -vcodec libx264 -b:v 5M -pix_fmt yuv420p -r @framerate -threads @cpucore -preset @preset -crf 23 -acodec aac -ac 2 -ab 192k -ar @audioquality -ab 64k @option1 -f @format ""@filename""";
        }

        public static string GET_DESKTOP_RECORDING_COMMAND_NVIDIA()
        {
            return @"-rtbufsize 2000M -f dshow -y -i video=""@videoSource"":audio=""@audioSource"" -vcodec libx264 -b:v 5M -r @framerate -threads @cpucore -c:v h264_nvenc -profile:v high -preset:v llhq -pixel_format yuv444p -acodec aac -ac 2 -ab 192k -ar @audioquality -ab 64k @option1 -f @format ""@filename""";
        }

        public static string GET_CONVERT_FILE_COMMAND()
        {
            return @" -y -i @SRCPATH @DESTPATH";
        }

        public static string GET_TWITCH_LIVE_COMMNAD()
        {
            return @"-rtbufsize 2000M -f dshow -y -i video=""@videoSource"":audio=""@audioSource"" -vcodec libx264 -b:v 5M -pix_fmt yuv420p -r @framerate -threads @cpucore -preset @preset -crf 23 -acodec aac -ac 2 -ab 192k -ar @audioquality -ab 64k @option1-f flv ""rtmp://live.twitch.tv/app/@filename""";
        }

        public static string GET_TWITCH_LIVE_COMMNAD_NVIDIA()
        {
            return @"-rtbufsize 2000M -f dshow -y -i video=""@videoSource"":audio=""@audioSource"" -vcodec libx264 -b:v 5M -r @framerate -threads @cpucore -c:v h264_nvenc -profile:v high -preset:v llhq -pixel_format yuv444p -acodec aac -ac 2 -ab 192k -ar @audioquality -ab 64k @option1 -f flv ""rtmp://live.twitch.tv/app/@filename""";            
        }
    }
}
