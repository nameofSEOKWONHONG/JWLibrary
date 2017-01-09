using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace JWLibrary.OSI
{

	public class DisplaySetting {

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAY_DEVICE {
			public int cb;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string DeviceName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceString;

			public int StateFlags;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceID;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceKey;

			public DISPLAY_DEVICE (int flags) {
				cb = 0;
				StateFlags = flags;
				DeviceName = new string((char)32, 32);
				DeviceString = new string((char)32, 128);
				DeviceID = new string((char)32, 128);
				DeviceKey = new string((char)32, 128);
				cb = Marshal.SizeOf(this);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DEVMODE {

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmDeviceName;

			public short dmSpecVersion;
			public short dmDriverVersion;
			public short dmSize;
			public short dmDriverExtra;
			public int dmFields;
			public short dmOrientation;
			public short dmPaperSize;
			public short dmPaperLength;
			public short dmPaperWidth;
			public short dmScale;
			public short dmCopies;
			public short dmDefaultSource;
			public short dmPrintQuality;
			public short dmColor;
			public short dmDuplex;
			public short dmYResolution;
			public short dmTTOption;
			public short dmCollate;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmFormName;

			public short dmUnusedPadding;
			public short dmBitsPerPel;
			public int dmPelsWidth;
			public int dmPelsHeight;
			public int dmDisplayFlags;
			public int dmDisplayFrequency;
		}

		[DllImport("User32.dll")]
		private static extern bool EnumDisplayDevices (
			IntPtr lpDevice, int iDevNum,
			ref DISPLAY_DEVICE lpDisplayDevice, int dwFlags);

		[DllImport("User32.dll")]
		private static extern bool EnumDisplaySettings (
			string devName, int modeNum, ref DEVMODE devMode);

		[DllImport("user32.dll")]
		public static extern int ChangeDisplaySettings (
			ref DEVMODE devMode, int flags);

		public DisplaySetting () {
		}

		public static List<DisplayDeviceResolutionModel> EnumModes (int devNum) {
			List<DisplayDeviceResolutionModel> models = new List<DisplayDeviceResolutionModel>();

			string devName = GetDeviceName(devNum);
			DEVMODE devMode = new DEVMODE();
			int modeNum = 0;
			bool result = true;
			do {
				result = EnumDisplaySettings(devName,
					modeNum, ref devMode);

				if (result) {
					string item = DevmodeToString(devMode);
					models.Add(new DisplayDeviceResolutionModel {
						Idx = modeNum
						,
						DevModeName = item
						,
						Width = devMode.dmPelsWidth
						,
						Height = devMode.dmPelsHeight
						,
						Bits = devMode.dmBitsPerPel
						,
						DisplayFrequency = devMode.dmDisplayFrequency
						,
						DefaultSource = devMode.dmDefaultSource
					});
				}
				modeNum++;
			} while (result);

			/*if (model.Count > 0) {
				DEVMODE current = GetDevmode(devNum, -1);
				int selected = model.Find(item => item.DevModeName ==  DevmodeToString(current));
					DevmodeToString(current));
				if (selected >= 0)
					listSettings.SetSelected(selected, true);
			}*/

			return models;
		}

		public static DisplayDeviceResolutionModel GetDeviceResolution (int devNum, string devModeName) {
			DisplayDeviceResolutionModel model = null;

			DEVMODE current = GetDevmode(devNum, -1);

			if (DevmodeToString(current) == devModeName) {
				model = new DisplayDeviceResolutionModel() {
					DevModeName = DevmodeToString(current),
					Idx = devNum
				};
			}

			return model;
		}

		public static DEVMODE GetDevmode (int devNum, int modeNum) { //populates DEVMODE for the specified device and mode
			DEVMODE devMode = new DEVMODE();
			string devName = GetDeviceName(devNum);
			EnumDisplaySettings(devName, modeNum, ref devMode);
			return devMode;
		}

		private static string DevmodeToString (DEVMODE devMode) {
			return devMode.dmPelsWidth.ToString() +
				" x " + devMode.dmPelsHeight.ToString() +
				", " + devMode.dmBitsPerPel.ToString() +
				" bits, " +
				devMode.dmDisplayFrequency.ToString() + " Hz";
		}

		public static List<DisplayDeviceSettingModel> EnumDevices () { //populates Display Devices list
			List<DisplayDeviceSettingModel> dislplayList = new List<DisplayDeviceSettingModel>();

			DISPLAY_DEVICE d = new DISPLAY_DEVICE(0);

			int devNum = 0;
			bool result;
			do {
				result = EnumDisplayDevices(IntPtr.Zero, devNum, ref d, 0);

				if (result) {
					string item = devNum.ToString() + ". " + d.DeviceString.Trim();
					if ((d.StateFlags & 4) != 0) item += " - main";

					dislplayList.Add(new DisplayDeviceSettingModel { Idx = devNum, DeviceName = d.DeviceString, DeviceMain = ((d.StateFlags & 4) != 0) });
				}
				devNum++;
			} while (result);

			return dislplayList;
		}

		public static string GetDeviceName (int devNum) {
			DISPLAY_DEVICE d = new DISPLAY_DEVICE(0);
			bool result = EnumDisplayDevices(IntPtr.Zero,
				devNum, ref d, 0);
			return (result ? d.DeviceName.Trim() : "#error#");
		}

		public static bool MainDevice (int devNum) { //whether the specified device is the main device
			DISPLAY_DEVICE d = new DISPLAY_DEVICE(0);
			if (EnumDisplayDevices(IntPtr.Zero, devNum, ref d, 0)) {
				return ((d.StateFlags & 4) != 0);
			}
			return false;
		}

		public static void SetResolution (int devNum, int modeNum) {
			DEVMODE d = GetDevmode(devNum, modeNum);

			if (d.dmBitsPerPel != 0 & d.dmPelsWidth != 0
				& d.dmPelsHeight != 0) {
				ChangeDisplaySettings(ref d, 0);
			}
		}
	}

	public class DisplayDeviceSettingModel {
		public int Idx { get; set; }
		public string DeviceName { get; set; }
		public bool BoxMode { get; set; }
		public bool DeviceMain { get; set; }
	}

	public class DisplayDeviceResolutionModel {
		public int Idx { get; set; }
		public string DevModeName { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int Bits { get; set; }
		public int DisplayFrequency { get; set; }
		public int DefaultSource { get; set; }
	}
}