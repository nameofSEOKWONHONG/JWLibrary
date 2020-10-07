using JWLibrary.Utils.Files;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JCoreSvcTest {
    class Program {
        static void Main(string[] args) {
            var fswProvider = new FileSystemWatcherProvider(@"D:\database");
            fswProvider.Created((s, e, fi) => {
                Console.WriteLine(e.ChangeType.ToString());
                Console.WriteLine(e.FullPath);
            }).Changed((s, e, fi) => {
                Console.WriteLine(e.ChangeType.ToString());
                Console.WriteLine(e.FullPath);
            }).Start();

            Console.ReadLine();

            fswProvider.Stop();
            fswProvider.Dispose();

        }
    }
}
