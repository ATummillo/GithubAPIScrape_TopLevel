using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GithubAPIScrape_TopLevel
{
    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; } = new HttpClient();

        public static void initializeClient()
        {
            ApiClient = new HttpClient();
            //ApiClient.BaseAddress = new Uri("https://api.github.com/");

            ApiClient.Timeout = TimeSpan.FromMinutes(10);

            ApiClient.DefaultRequestHeaders.UserAgent.Clear();
            ApiClient.DefaultRequestHeaders.Add("User-Agent", "CRE-OAuthApp-2 by CRE-Developer");

            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
    }
}
