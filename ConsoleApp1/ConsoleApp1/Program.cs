using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var path = args[0];
            // var fi = new FileInfo(path);
            FileInfo fi = new(path);

            var fileContent = new List<string>();
            using (StreamReader stream = new(fi.OpenRead()))
            {
                // Analogicznie do ""
                //string line = string.Empty;
                string line = null;

                while ((line = await stream.ReadLineAsync()) != null)
                {
                    fileContent.Add(line);
                }

            }

            foreach (var item in fileContent)
            {
                Console.WriteLine(item);
            }

            // DateTime - typ dla daty: metody Parse() i TryParse()
            // DateTime.Parse("2022-03-20")

            // string.isNullOrWhiteSpace(str)

            // studenci bedą przechowywani w HashSet (należy napisać comparator studentów)

            // zapisywanie do pliku: StreamWriter stream... stream.WriteLine(line)

            // serializacja do JSON: var json = JsonSerializer.Serializer(set)

        }
    }
}
