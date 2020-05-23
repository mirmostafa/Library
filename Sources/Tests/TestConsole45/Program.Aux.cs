using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Mohammad.Collections.ObjectModel;
using Mohammad.Diagnostics;
using Mohammad.Helpers;
using Mohammad.Helpers.Console;
using Mohammad.Threading.Tasks;

namespace TestConsole45
{
    public partial class App : ConsoleProgramBase<App>
    {
        //struct DataStaructure
        //{
        //    public int Id { get; set; }
        //    public int Min { get; set; }
        //    public int Max { get; set; }
        //    public override string ToString() => $"Id: {this.Id}, Min: {this.Min}, Max: {this.Max}";
        //}
        private App()
        {
        }

        [STAThread]
        private static void Main(string[] args)
        {
            CallMeOnMain(args);
        }

        [Obsolete("Not working anymore. Just to get the idea.", true)]
        private static void PriceLive()
        {
            Func<string, string, int> getValue = (html, item) =>
            {
                var line = html.Split('\n').First(s => s.Contains(item));
                return
                    int.Parse(
                        line.Substring(line.LastIndexOf(">", StringComparison.Ordinal) + 1,
                                line.Length - line.LastIndexOf(">", StringComparison.Ordinal) - "</td>".Length - 1)
                            .Replace(",", ""));
            };
            "Downloading...".WriteLine();
            var client = new WebClient();
            var siteHtml = client.DownloadString("http://www.tala.ir/webservice/price_live.php");
            "Downloaded.".WriteLine();
            ConsoleHelper.WriteLine();
            //"Tala Bazar:\t".Write();
            //NumericHelper.Separate(GetValue(html, "bazartehran").ToInt()).WriteLine();

            "Geram 18 Ayar:\t".Write();
            getValue(siteHtml, "gold_18k").ToInt().ToCommaSeparate().WriteLine();

            "Geram 24 Ayar:\t".Write();
            getValue(siteHtml, "gold_24k").ToInt().ToCommaSeparate().WriteLine();

            "sekke gadim:\t".Write();
            getValue(siteHtml, "sekke-gad").ToInt().ToCommaSeparate().WriteLine();
        }

        private static void Puya()
        {
            string[] lines = null;
            Action action = () => lines = File.ReadAllLines(@"E:\match.txt");
            Diag.RunDebug(action, "Read match file records");
            $"Lines count: {lines.Length}".WriteLine();
            var data = new SortedList<int, int>(lines.Length);
            action = () => lines.FastForEach(line =>
            {
                var cols = line.Split(',');
                data.Add(int.Parse(cols[1]), int.Parse(cols[2]));
            });
            Diag.RunDebug(action, "Add to array");
            IEnumerable<int> questions = null;
            action = () => questions = File.ReadAllLines(@"E:\questions.txt").Select(int.Parse);
            Diag.RunDebug(action, "Read questions");
            var result = new PairList<int, int>();
            action = () =>
                questions.FastForEach(question => result.Add(question, data.First(d => d.Key > question).Value));
            Diag.RunDebug(action, "Find questions");
            action = () => File.WriteAllLines(@"E:\answers.txt", result.Select(r => $"{r.Value1} : {r.Value2}"));
            Diag.RunDebug(action, "Write answers");
        }
        //private void WriteDb()
        //{
        //    var cs = @"Data Source=192.168.88.31;Initial Catalog=SatrapS2;Integrated Security=True";
        //    var db = Database.GetDatabase(cs);
        //    db.Logger.Out       = Console.Out;
        //    db.Logger.IsEnabled = true;

        //    var output = FileSystemHelper.GetDirectory(@"C:\Temp\Practicing", false)
        //                                 .DeleteDirectory()
        //                                 .Wait(1000)
        //                                 .CreateDirectory();

        //    output.CreateFile("Tables.reading")
        //          .Write(db.Tables.Select(t => t.Name).Merge(Environment.NewLine))
        //          .ChangeExtension("lst");

        //    db.Tables.ForEach(table =>
        //    {
        //        this.WaitIfRequired();
        //        if (this.IsCancellationRequested) return;
        //        $"Processing {table}....".WriteLine();
        //        output.CreateFile(table.ToString().Replace("*", "_").Replace("?", "_").Add(".reading"))
        //              .Write($"{"name".Enlarge(40)}{"Type".Enlarge(15)}{"Null".Enlarge(7)}{"ID".Enlarge(7)}{"Frgn".Enlarge(7)}")
        //              .Append(Environment.NewLine)
        //              .Append(table.Columns
        //                           .Select(col =>
        //                                       $"{col.Name.Enlarge(40)}{col.DataType.Enlarge(15)}{col.IsNullable.ToString().Enlarge(7)}{col.IsIdentity.ToString().Enlarge(7)}{col.IsForeignKey}")
        //                           .Merge(Environment.NewLine))
        //              .ChangeExtension(".table");
        //    });
        //}
    }
}