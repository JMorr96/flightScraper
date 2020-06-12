using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Hosting;
using ScrapySharp.Network;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace flightScraper
{
    public class Program
    {
        static ScrapingBrowser scrapingBrowser = new ScrapingBrowser();
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var scraper = new ChromeScraper();
            var text = scraper.GetFlight("https://www.skyscanner.com/");
            foreach (var t in text)
            {
                Console.WriteLine(t.Text);
            }
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

    public class ChromeScraper
    {
        public ChromeDriver Driver { get; set; }

        public ChromeScraper()
        {
            var options = new ChromeOptions();
            options.AddArgument("ignore-certificate-errors");
            Driver = new ChromeDriver(options);
        }

        public IReadOnlyCollection<IWebElement> GetFlight(string url)
        {
            Driver.Navigate().GoToUrl(url);
            SearchFlight(Driver, "JFK", "Bali");
            return Driver.FindElementsByClassName("time");
            // while (true)
            // {
            //     var homeText = Driver.FindElementsByClassName("time");
            //     if (homeText.Count > 0)
            //         return homeText;
            // }

        }

        private void SearchFlight(ChromeDriver driver, string origin, string dest)
        {
            var originInput = driver.FindElement(By.XPath("//input[@id='fsc-origin-search']"));
            var destInput = driver.FindElement(By.XPath("//input[@id='fsc-destination-search']"));
            originInput.Clear();
            originInput.SendKeys(origin);
            destInput.Clear();
            destInput.SendKeys(dest);

            var departButton = driver.FindElement(By.XPath("//button[@id='depart-fsc-datepicker-button']"));
            departButton.Click();
            var departMonthDropdown =
                new SelectElement(
                    driver.FindElement(By.XPath("//select[@id='depart-calendar__bpk_calendar_nav_select']")));
            departMonthDropdown.SelectByValue("2020-07");
            
            
            var returnButton = driver.FindElement(By.XPath("//button[@id='return-fsc-datepicker-button']"));


        }
    }
}
