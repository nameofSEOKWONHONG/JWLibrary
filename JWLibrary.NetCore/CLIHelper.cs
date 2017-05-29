using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace JWLibrary.NetCore
{
    public class CLIHelper
    {
        public CLIHelper()
        {

        }

        public bool ExecuteCommand(string exeDir, string args, out string result)
        {
            bool isRet = false;
            result = string.Empty;

            try
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo();

                procStartInfo.FileName = exeDir;
                procStartInfo.Arguments = args;
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                procStartInfo.StandardOutputEncoding = Encoding.UTF8;

                using (Process process = new Process())
                {
                    process.StartInfo = procStartInfo;
                    process.Start();

                    process.WaitForExit();

                    result = process.StandardOutput.ReadToEnd();                    
                }
                isRet = true;
            }
            catch (Exception ex)
            {
                result = $"exeDir = {exeDir}, args = {args}, error = {ex.Message}";
            }

            return isRet;
        }
    }
}
