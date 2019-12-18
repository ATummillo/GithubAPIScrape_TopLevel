using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GithubAPIScrape_TopLevel
{
    public class RepoProcessor
    {
        public static async Task LoadReposParallelAsync(DataTable dt)
        {
            List<Task> tasks = new List<Task>();

            int cnt = 0;

            foreach (DataRow row in dt.Rows)
            {
                cnt++;
                tasks.Add(LoadRepo(row));

                //if(cnt > 5)
                //{
                //    break;
                //}
            }

            await Task.WhenAll(tasks);

            File.WriteAllText(@"C:\Data\Github\Output\Success.txt", "Github API Scrape finished successfully!");

            Environment.Exit(0);
        }

        private static async Task LoadRepo(DataRow row)
        {
            int counter = 0;
            string user = row["GithubUser"].ToString();
            string coinID = row["CoinID"].ToString();

            string url = $"https://api.github.com/users/{user}/repos?client_id=6baefe59dc796de2fc29&client_secret=c542ede588ed74e9e8443b98a21b2283c6a986f9";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("SUCCESS");
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    string filePath = @"C:\Data\Github\Output\Success\" + coinID + ".csv";
                    File.WriteAllText(filePath, jsonStr);
                }
                else
                {
                    Console.WriteLine("FAILED");
                    string filePath = @"C:\Data\Github\Output\Failed\" + coinID + ".csv";
                    File.WriteAllText(filePath, coinID);
                }
            }
        }


    }
}
