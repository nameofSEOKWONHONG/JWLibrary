using JWLibrary.StaticMethod.CLI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace JWLibrary.StaticMethod.Network
{
    public class NetworkHelper
    {
        public static readonly int[] DYNAMIC_UDP_TCP_PORT_RANGE = Enumerable.Range(49152, 65535).ToArray();

        public static bool IsTcpPortAvailable(int tcpPort)
        {
            var ipgp = IPGlobalProperties.GetIPGlobalProperties();

            // Check ActiveConnection ports
            TcpConnectionInformation[] conns = ipgp.GetActiveTcpConnections();
            foreach (var cn in conns)
            {
                if (cn.LocalEndPoint.Port == tcpPort)
                {
                    return false;
                }
            }

            // Check LISTENING ports
            IPEndPoint[] endpoints = ipgp.GetActiveTcpListeners();
            foreach (var ep in endpoints)
            {
                if (ep.Port == tcpPort)
                {
                    return false;
                }
            }

            return true;
        }

        public static void OpenPort(string boundType, string portType, int port)
        {
            var arg = OpenPortCommand(boundType, portType, port);
            ProcessHandlerAsync.RunAsync("cmd.exe", arg,
                (output) =>
                {
                    Debug.WriteLine(output);
                },
                (error) =>
                {
                    Debug.WriteLine(error);
                })
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    Debug.WriteLine("done");
                });
        }

        public static void ClosePort(string boundType, string portType, int port)
        {
            var arg = ClosePortCommand(boundType, portType, port);
            ProcessHandlerAsync.RunAsync("cmd.exe", arg,
                (output) =>
                {
                    Debug.WriteLine(output);
                },
                (error) =>
                {
                    Debug.WriteLine(error);
                })
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    Debug.WriteLine("done");
                });
        }

        public static void EnableFirewall()
        {
            var arg = EnableFirewallCommand();
            ProcessHandlerAsync.RunAsync("cmd.exe", arg,
                (output) =>
                {
                    Debug.WriteLine(output);
                },
                (error) =>
                {
                    Debug.WriteLine(error);
                })
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    Debug.WriteLine("done");
                });
        }

        public static void DisableFirewall()
        {
            var arg = DisableFirewallCommand();
            ProcessHandlerAsync.RunAsync("cmd.exe", arg,
                (output) =>
                {
                    Debug.WriteLine(output);
                },
                (error) =>
                {
                    Debug.WriteLine(error);
                })
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    Debug.WriteLine("done");
                });
        }

        /// <summary>
        /// open port command
        /// </summary>
        /// <param name="boundType">in/out</param>
        /// <param name="portType">TCP/UDP</param>
        /// <param name="port"></param>
        /// <returns></returns>
        private static string OpenPortCommand(string boundType, string portType, int port)
        {
            return $"/C netsh advfirewall firewall add rule name=\"ACCC Server App Open Port {port}\" dir={boundType} action=allow protocol={portType} localport={port}";
        }

        private static string ClosePortCommand(string boundType, string portType, int port)
        {
            return $"/C netsh advfirewall firewall delete rule name=\"ACCC Server App Open Port {port}\" dir={boundType} protocol={portType} localport={port}";
        }

        private static string EnableFirewallCommand()
        {
            return "/C netsh advfirewall set allprofiles state on";
        }

        private static string DisableFirewallCommand()
        {
            return "/C netsh advfirewall set allprofiles state off";
        }
    }
}
