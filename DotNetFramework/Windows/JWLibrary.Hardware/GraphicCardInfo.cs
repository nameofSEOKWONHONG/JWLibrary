using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.OSI
{
    public class GraphicCardInfo
    {
        public List<VideoCardInfo> GetVideoCardInfo()
        {
            ManagementObjectSearcher objvide = new ManagementObjectSearcher("select * from Win32_VideoController");
            List<VideoCardInfo> videoCardInfos = null;

            var objs = objvide.Get();
            if (objs != null && objs.Count > 0)
            {
                videoCardInfos = new List<VideoCardInfo>();
                foreach (ManagementObject obj in objvide.Get())
                {
                    VideoCardInfo vci = new VideoCardInfo();
                    vci.Name = obj["Name"].ToString();
                    vci.DeviceID = obj["DeviceID"].ToString();
                    vci.AdapterRAM = obj["AdapterRAM"].ToString();
                    vci.AdapterDACType = obj["AdapterDACType"].ToString();
                    vci.Monochrome = obj["Monochrome"].ToString();
                    vci.InstalledDisplayDrivers = obj["InstalledDisplayDrivers"].ToString();
                    vci.DriverVersion = obj["DriverVersion"].ToString();
                    vci.VideoProcessor = obj["VideoProcessor"].ToString();
                    vci.VideoArchitecture = obj["VideoArchitecture"].ToString();
                    vci.VideoMemoryType = obj["VideoMemoryType"].ToString();
                    videoCardInfos.Add(vci);
                }
            }

            return videoCardInfos;
        }
    }

    public class VideoCardInfo
    {
        public string Name { get; set; }
        public string DeviceID { get; set; }
        public string AdapterRAM { get; set; }
        public string AdapterDACType { get; set; }
        public string Monochrome { get; set; }
        public string InstalledDisplayDrivers { get; set; }
        public string DriverVersion { get; set; }
        public string VideoProcessor { get; set; }
        public string VideoArchitecture { get; set; }
        public string VideoMemoryType { get; set; }
    }
}
