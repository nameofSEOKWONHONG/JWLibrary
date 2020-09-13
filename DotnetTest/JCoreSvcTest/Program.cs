using System;
using System.Linq;
using System.Threading.Tasks;

namespace JCoreSvcTest {
    class Program {
        static void Main(string[] args) {
            char word = '*';

            Enumerable
                .Range(0, 100)
                .ToList()
                .ForEach(i => {
                    Enumerable.Range(0, 100)
                    .ToList()
                    .ForEach(j => {
                        Console.Write(word);
                    });
                    Console.WriteLine();
                });
        }
    }
}
