using System;
using System.Collections.Generic;
using System.Management;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace JWLibrary.OSI {

    public class Network {

        /// <summary>
		/// 모든 IPv4 Address 구하기
		/// </summary>
		/// <returns>IPv4 Address 문자열 배열</returns>
		public static string[] GetAllIPv4Address() {
            List<string> ipv4s = new List<string>();
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

            foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList) {
                if (regex.IsMatch(ip.ToString())) {
                    ipv4s.Add(ip.ToString());
                }
            }

            return ipv4s.ToArray();
        }

        /// <summary>
        /// 유선 IPv4 Address 구하기
        /// </summary>
        /// <returns>유선 IPv4 Address 문자열 배열</returns>
        public static string[] GetWiredIPv4Address() {
            return GetIPv4Address(true);
        }

        /// <summary>
        /// 무선 IPv4 Address 구하기
        /// </summary>
        /// <returns>무선 IPv4 Address 문자열 배열</returns>
        public static string[] GetWirelessIPv4Address() {
            return GetIPv4Address(false);
        }

        /// <summary>
        /// 유선 or 무선 IPv4 Address 구하기
        /// </summary>
        /// <param name="wired">유선을 찾고 싶으면 true</param>
        /// <returns>IPv4 Address 문자열 배열</returns>
        private static string[] GetIPv4Address(bool wired) {
            // IPv4 검사할 정규식
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

            List<string> ipv4s = new List<string>();
            string[] wifiNames = GetWifiNetAdapterNames();

            try {
                ManagementObjectSearcher objSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
                ManagementObjectCollection objCollection = objSearcher.Get();

                foreach (ManagementObject obj in objCollection) {
                    // wired값에 따라 무선 or 유선인것만 찾기
                    if (wired && Array.IndexOf(wifiNames, (string)obj["Description"]) < 0 || !wired && Array.IndexOf(wifiNames, (string)obj["Description"]) > -1) {
                        foreach (string address in (string[])obj["IPAddress"]) {
                            if (regex.IsMatch(address) && address != "0.0.0.0") {
                                ipv4s.Add(address);
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("Error : " + ex.ToString());
            }

            // 반환
            return ipv4s.ToArray();
        }

        /// <summary>
        /// 무선 Network Adapter 이름 구하기
        /// </summary>
        /// <returns>무선 Network Adapter 문자열 배열</returns>
        public static string[] GetWifiNetAdapterNames() {
            List<string> names = new List<string>();

            try {
                ObjectQuery query = new ObjectQuery("SELECT * FROM MSNdis_80211_ReceivedSignalStrength Where active = true");
                ManagementScope scope = new ManagementScope("root\\wmi");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject obj in searcher.Get()) {
                    if ((bool)obj["Active"] == true) {
                        names.Add((string)obj["InstanceName"]);
                    }
                }
            } catch (ManagementException mex) {
                if (mex.ErrorCode != ManagementStatus.NotSupported) {
                    Console.WriteLine("Error : " + mex.ToString());
                }
            } catch (Exception ex) {
                Console.WriteLine("Error : " + ex.ToString());
            }

            return names.ToArray();
        }

        /// <summary>
        /// 네트워크 기기 정보
        /// </summary>
        /// <returns></returns>
        public static NetworkInterface[] GetAllNetworkInterfaces() {
            return NetworkInterface.GetAllNetworkInterfaces();
        }

        public static string GetMacAddress() {
            return NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
        }
    }
}