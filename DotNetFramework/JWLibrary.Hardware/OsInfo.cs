using JWLibrary.Core.StringToEnum;
using System;
using System.Management;

namespace JWLibrary.OSI
{
	public class OsInfo {

		public static WindowsType GetWindowsType () {
			WindowsType type = WindowsType.Unknown;

			string query = "SELECT * FROM Win32_OperatingSystem";

			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query)) {
				foreach (WindowsType item in Enum.GetValues(typeof(WindowsType))) {
					foreach (ManagementObject info in searcher.Get()) {
						if (info.Properties["Caption"].Value.ToString().Contains(StringEnum.GetStringValue(item))) {
							type = item;
							break;
						}
						/*Console.WriteLine("caption : {0}, version : {1}, sp major : {2}, sp minor : {3}", info.Properties["Caption"].Value.ToString().Trim()
						, info.Properties["Version"].Value.ToString()
						, info.Properties["ServicePackMajorVersion"].Value.ToString()
						, info.Properties["ServicePackMinorVersion"].Value.ToString());*/
					}
				}
			}

			return type;
		}
	}

	public enum WindowsType {

		[StringValue("Unknown")]
		Unknown,

		[StringValue("Windows XP")]
		WindowsXp,

		[StringValue("Windows Vista")]
		WindowsVista,

		[StringValue("Windows 7")]
		Windows7,

		[StringValue("Windows 10")]
		Windows10,
	}
}