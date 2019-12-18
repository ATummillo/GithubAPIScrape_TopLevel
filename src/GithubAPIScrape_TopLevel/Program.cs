using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Data;
using System.Text.RegularExpressions;

namespace GithubAPIScrape_TopLevel
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            PrepDirectories();
            ApiHelper.initializeClient();
            DataTable dt = ConvertCSVtoDataTable(@"C:\Data\Github\Input\TopLevelUserMap.csv");
            try
            {
                await RepoProcessor.LoadReposParallelAsync(dt);
            }
            catch(TaskCanceledException ex)
            {
                Console.WriteLine(ex.CancellationToken.IsCancellationRequested);
                Console.WriteLine("PAUSED");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("PAUSED");
            }

            Environment.Exit(1);
        }

        private static void PrepDirectories()
        {
            Directory.Delete(@"C:\Data\Github\Output\Success", true);
            Directory.Delete(@"C:\Data\Github\Output\Failed", true);

            Directory.CreateDirectory(@"C:\Data\Github\Output\Success");
            Directory.CreateDirectory(@"C:\Data\Github\Output\Failed");
        }

        private static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                DataColumn col = new DataColumn(header);

                switch (header)
                {
                    case "CoinID":
                        col.DataType = System.Type.GetType("System.Int32");
                        break;

                    case "CoinName":
                        col.DataType = System.Type.GetType("System.String");
                        break;

                    case "CoinSymbol":
                        col.DataType = System.Type.GetType("System.String");
                        break;

                    case "GithubUser":
                        col.DataType = System.Type.GetType("System.String");
                        break;

                    default:
                        throw new Exception("Error parsing to CSV!");
                }

                dt.Columns.Add(col);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    if (i == 0)
                    {
                        dr[i] = Int32.Parse(rows[i]);
                    }
                    else
                    {
                        dr[i] = rows[i];
                    }
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
