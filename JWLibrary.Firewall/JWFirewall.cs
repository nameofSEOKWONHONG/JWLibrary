using System;
using NetFwTypeLib;
using System.Collections;

namespace JWLibrary.Firewall
{
    public class JWFirewall
    {
        private int[] portsSocket = null;//{ 777, 3306 };
        private string[] portsName = null;//{ "AsyncPort", "MySqlPort" };
        private INetFwProfile fwProfile = null;

        public JWFirewall(int[] portsSocket, string[] portsName)
        {
            this.portsSocket = portsSocket;
            this.portsName = portsName;
        }

        public void OpenFirewall(string applictionName)
        {
            INetFwAuthorizedApplications authApps = null;
            INetFwAuthorizedApplication authApp = null;
            INetFwOpenPorts openPorts = null;
            INetFwOpenPort openPort = null;
            try
            {
                if (isAppFound(applictionName) == false)
                {
                    SetProfile();
                    authApps = fwProfile.AuthorizedApplications;
                    authApp = GetInstance("INetAuthApp") as INetFwAuthorizedApplication;
                    authApp.Name = applictionName;
                    authApp.ProcessImageFileName = applictionName;
                    authApps.Add(authApp);
                }

                if (isPortFound(portsSocket[0]) == false)
                {
                    SetProfile();
                    openPorts = fwProfile.GloballyOpenPorts;
                    openPort = GetInstance("INetOpenPort") as INetFwOpenPort;
                    openPort.Port = portsSocket[0];
                    openPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
                    openPort.Name = portsName[0];
                    openPorts.Add(openPort);
                }

                if (isPortFound(portsSocket[1]) == false)
                {
                    SetProfile();
                    openPorts = fwProfile.GloballyOpenPorts;
                    openPort = GetInstance("INetOpenPort") as INetFwOpenPort;
                    openPort.Port = portsSocket[1];
                    openPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
                    openPort.Name = portsName[1];
                    openPorts.Add(openPort);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (authApps != null) authApps = null;
                if (authApp != null) authApp = null;
                if (openPorts != null) openPorts = null;
                if (openPort != null) openPort = null;
            }
        }

        public void CloseFirewall(string applicationName)
        {
            INetFwAuthorizedApplications apps = null;
            INetFwOpenPorts ports = null;
            try
            {
                if (isAppFound(applicationName) == true)
                {
                    SetProfile();
                    apps = fwProfile.AuthorizedApplications;
                    apps.Remove(applicationName);
                }

                if (isPortFound(portsSocket[0]) == true)
                {
                    SetProfile();
                    ports = fwProfile.GloballyOpenPorts;
                    ports.Remove(portsSocket[0], NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
                }

                if (isPortFound(portsSocket[1]) == true)
                {
                    SetProfile();
                    ports = fwProfile.GloballyOpenPorts;
                    ports.Remove(portsSocket[1], NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (apps != null) apps = null;
                if (ports != null) ports = null;
            }
        }

        public bool isAppFound(string appName)
        {
            bool boolResult = false;
            Type progID = null;
            INetFwMgr firewall = null;
            INetFwAuthorizedApplications apps = null;
            INetFwAuthorizedApplication app = null;
            try
            {
                progID = Type.GetTypeFromProgID("HNetCfg.FwMgr");
                firewall = Activator.CreateInstance(progID) as INetFwMgr;
                if (firewall.LocalPolicy.CurrentProfile.FirewallEnabled)
                {
                    apps = firewall.LocalPolicy.CurrentProfile.AuthorizedApplications;
                    IEnumerator appEnumerate = apps.GetEnumerator();
                    while ((appEnumerate.MoveNext()))
                    {
                        app = appEnumerate.Current as INetFwAuthorizedApplication;
                        if (app.Name == appName)
                        {
                            boolResult = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (progID != null) progID = null;
                if (firewall != null) firewall = null;
                if (apps != null) apps = null;
                if (app != null) app = null;
            }
            return boolResult;
        }

        public bool isPortFound(int portNumber)
        {
            bool boolResult = false;
            INetFwOpenPorts ports = null;
            Type progID = null;
            INetFwMgr firewall = null;
            INetFwOpenPort currentPort = null;
            try
            {
                progID = Type.GetTypeFromProgID("HNetCfg.FwMgr");
                firewall = Activator.CreateInstance(progID) as INetFwMgr;
                ports = firewall.LocalPolicy.CurrentProfile.GloballyOpenPorts;
                IEnumerator portEnumerate = ports.GetEnumerator();
                while ((portEnumerate.MoveNext()))
                {
                    currentPort = portEnumerate.Current as INetFwOpenPort;
                    if (currentPort.Port == portNumber)
                    {
                        boolResult = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ports != null) ports = null;
                if (progID != null) progID = null;
                if (firewall != null) firewall = null;
                if (currentPort != null) currentPort = null;
            }
            return boolResult;
        }

        public void SetProfile()
        {
            INetFwMgr fwMgr = null;
            INetFwPolicy fwPolicy = null;
            try
            {
                fwMgr = GetInstance("INetFwMgr") as INetFwMgr;
                fwPolicy = fwMgr.LocalPolicy;
                fwProfile = fwPolicy.CurrentProfile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fwMgr != null) fwMgr = null;
                if (fwPolicy != null) fwPolicy = null;
            }
        }

        public object GetInstance(string typeName)
        {
            Type tpResult = null;
            switch (typeName)
            {
                case "INetFwMgr":
                    tpResult = Type.GetTypeFromCLSID(new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
                    return Activator.CreateInstance(tpResult);
                case "INetAuthApp":
                    tpResult = Type.GetTypeFromCLSID(new Guid("{EC9846B3-2762-4A6B-A214-6ACB603462D2}"));
                    return Activator.CreateInstance(tpResult);
                case "INetOpenPort":
                    tpResult = Type.GetTypeFromCLSID(new Guid("{0CA545C6-37AD-4A6C-BF92-9F7610067EF5}"));
                    return Activator.CreateInstance(tpResult);
                default:
                    return null;
            }
        }
    }
}
