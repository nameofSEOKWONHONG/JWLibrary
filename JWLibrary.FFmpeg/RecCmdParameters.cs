﻿using System.Collections.Generic;

namespace JWLibrary.FFmpeg
{
    public class RecCmdParameters
    {
        public static List<string> GetSupportFPS()
        {
            List<string> sfps = new List<string>();
            sfps.Add("10");
            sfps.Add("20");
            sfps.Add("30");

            return sfps;
        }

        public static List<string> GetSupportFrameRate()
        {
            List<string> sfr = new List<string>();
            sfr.Add("10");
            sfr.Add("20");
            sfr.Add("30");
            sfr.Add("40");
            sfr.Add("50");
            sfr.Add("59");
            sfr.Add("60");

            return sfr;
        }

        public static List<string> GetSupportAudioQuality()
        {
            List<string> saq = new List<string>();
            saq.Add("11025");
            saq.Add("22050");
            saq.Add("44100");

            return saq;
        }

        public static List<string> GetSupportPreset()
        {
            List<string> srs = new List<string>();
            srs.Add("ultrafast");
            srs.Add("fast");
            srs.Add("medium");
            srs.Add("slow");
            srs.Add("veryslow");

            return srs;
        }

    }

}
