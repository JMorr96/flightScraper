using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScrapySharp.Network;

namespace flightScraper
{
    public class Program
    {
        static ScrapingBrowser scrapingBrowser = new ScrapingBrowser();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        static HtmlNode GetHtml(string url)
        {
            WebPage webpage = scrapingBrowser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }
    }
}
