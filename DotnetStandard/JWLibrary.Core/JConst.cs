using System.Threading;

namespace JWLibrary.Core
{
    public class JConst
    {
        public static int SLEEP_INTERVAL = 1;
        public static int LOOP_WARNING_COUNT = 5000;
        public static int LOOP_LIMIT = 500;
        public static void SetInterval(int interval) {
            Thread.Sleep(interval);
        }
    }
}