using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace ConsoleTokenizer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            #region Loading Data
            Console.WriteLine("Loading input data...");
            var standardWords = new mesEntities().Database.SqlQuery<string>("select * from Standard").ToList();
            var texts = new mesEntities().Database.SqlQuery<string>("select * from Query").ToList();
            #endregion

            #region Tokenizing
            Console.WriteLine("Analyzing...");

            var tokenizer = new Tokenizer.TextAnalyzer();
            tokenizer.Initialize(texts, standardWords);
            tokenizer.Run();
            Console.WriteLine("Analyze Completed");
            #endregion

            Console.WriteLine("Creating Json output file");
            using (var sFile = new SaveFileDialog())
            {

                
                if (sFile.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                var toJsonResult = tokenizer.Words.SelectMany(u =>
                   u.Value.Locations, (word, location) => new { word, location }
                    ).OrderBy(u => u.location.Row).ThenBy(u => u.location.CharNumber).Take(1000).Select(u => new
                    {
                        key = u.word.Key,
                        norm = u.word.Value.Normalized,
                        correct = u.word.Value.IsStandard,
                        suggest = u.word.Value.Suggestions
                    }).ToList();

                string jsonString = JsonConvert.SerializeObject(toJsonResult);
                toJsonResult = null;
                using (StreamWriter sw = new StreamWriter(File.Open(sFile.FileName, FileMode.Create), Encoding.Unicode))
                {
                    sw.Write(jsonString);
                    sw.Close();
                }
            }
            Console.WriteLine("Json file saved");

            Console.WriteLine("See u later :)");


            Console.ReadKey();
        }
    }

}

