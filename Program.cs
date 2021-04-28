using System;
using Bertozzi.Mattia._4H.SaveRecord.Models;

namespace Bertozzi.Mattia._4H.SaveRecord
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SaveRecord - 2021 - di Mattia Bertozzi 4H");

            //1)
            //Leggere un file CSV con i comuni e trasformarlo in una List<Comune>
            
            Comuni c = new Comuni("Comuni.csv");
            Console.WriteLine($"Ho letto {c.Count} righe dal file csv");

            //2)
            //Scrivere la List<Comune> in un file binario
            c.Save();

            //3)
            //Rileggere il file binario in una List<Comune>
            c.Load();
            Console.WriteLine($"Ho letto {c.Count} righe dal file binario.");
        }
    }
}
