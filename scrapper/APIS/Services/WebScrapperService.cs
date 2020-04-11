using APIS.WebScrapperLogic.Interfaces;
using APIS.WebScrapperLogic.Services;
using APIS.WebScrapperLogic.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;

namespace APIS.Services
{
    public class WebScrapperService
    {
        static ChromeDriver _browser;

        static ChromeDriver browser
        {
            get
            {
                ChromeOptions options = new ChromeOptions();
                // options.AddArgument("--no-sandbox");
                // options.AddArgument("--window-size=1420,1080");
                // options.AddArgument("--headless");
                // options.AddArgument("--disable-gpu");

                // +INFO: https://stackoverflow.com/questions/48450594/selenium-timed-out-receiving-message-from-renderer
                options.AddArgument("--window-size=1420,1080");
                options.AddArgument("start-maximized");
                options.AddArgument("enable-automation");
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-infobars");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-browser-side-navigation");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--disable-extensions");
                options.AddArgument("--dns-prefetch-disable");
                options.AddArgument("--aggressive-cache-discard");
                options.AddArgument("--disable-cache");
                options.AddArgument("--disable-application-cache");
                options.AddArgument("--disable-offline-load-stale-cache");
                options.AddArgument("--disk-cache-size=0");
                options.AddArgument("--no-proxy-server");
                options.AddArgument("--log-level=3");
                options.AddArgument("--silent");
                options.Proxy = null;

                // options.PageLoadStrategy = PageLoadStrategy.Normal;
                options.PageLoadStrategy = PageLoadStrategy.Eager;
                // options.PageLoadStrategy = PageLoadStrategy.None;
                
                options.LeaveBrowserRunning = false;

                if (_browser == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("ChromeDriver created!");
                    Console.ResetColor();

                    _browser = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options, TimeSpan.FromSeconds(60));
                }

                return _browser;
            }
        }

        /// <summary>
        /// Do scrap!
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static WebScrappedData WebScrappe(string path)
        {
            try
            {
                IWebScrapper scrapper = new WebScrapperLearn(browser);
                WebScrappedData scrapResult = scrapper.Webscrape(path);
                return scrapResult;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Exit browser");
                Console.ResetColor();
            }
        }
    }
}
