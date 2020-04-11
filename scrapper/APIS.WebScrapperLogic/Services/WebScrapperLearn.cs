using APIS.WebScrapperLogic.Interfaces;
using APIS.WebScrapperLogic.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIS.WebScrapperLogic.Services
{
    public class WebScrapperLearn : IWebScrapper
    {
        RemoteWebDriver browser;

        public WebScrapperLearn(RemoteWebDriver remoteWebDriver)
        {
            browser = remoteWebDriver;
        }

        public static string Company_Name = "Learn";

        public bool CanWebscrape(string c, string p)
        {
            return true;
        }

        public WebScrappedData Webscrape(string hyperlink)
        {
            WebScrappedData scrappedData = new WebScrappedData();

            browser.Url = hyperlink;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Navigate()");
            INavigation navigation = browser.Navigate();
            Console.ResetColor();

            System.Threading.Tasks.Task.Delay(2000).GetAwaiter().GetResult();

            // Level
            string level = "//span[@id='level-status-text']";
            if (WebscraperUtils.IsElementPresent(browser, By.XPath(level)))
            {
                var levelElement = browser.FindElements(By.XPath(level)).FirstOrDefault();
                var levelData = WebscraperUtils.GetTextFromElement(levelElement);
                
                Console.WriteLine($"LEVEL -> {levelData}");
                scrappedData.Level = levelData;
            }

            // Level status points
            string levelStatusPoints = "//span[@id='level-status-points']";
            if (WebscraperUtils.IsElementPresent(browser, By.XPath(levelStatusPoints)))
            {
                var levelStatusPointsElement = browser.FindElements(By.XPath(levelStatusPoints)).FirstOrDefault();
                var levelStatusPointsData = WebscraperUtils.GetTextFromElement(levelStatusPointsElement);

                Console.WriteLine($"LEVEL STATUS POINTS -> {levelStatusPointsData}");
                scrappedData.LevelStatusPoints = levelStatusPointsData;
            }

            // Points
            string points = "//span[@id='level-status-points']/span[@class='has-text-weight-semibold']";
            if (WebscraperUtils.IsElementPresent(browser, By.XPath(points)))
            {
                var pointsElement = browser.FindElements(By.XPath(points)).FirstOrDefault();
                var pointsData = WebscraperUtils.GetTextFromElement(pointsElement);

                Console.WriteLine($"POINTS -> {pointsData}");
                scrappedData.Points = pointsData;
            }

            // Name
            string name = "//h1[@class='title has-margin-top-small has-margin-bottom-extra-small']";
            if (WebscraperUtils.IsElementPresent(browser, By.XPath(name)))
            {
                var nameElement = browser.FindElements(By.XPath(name)).FirstOrDefault();
                var nameData = WebscraperUtils.GetTextFromElement(nameElement);
                scrappedData.Username = nameData;
            }

            scrappedData.IsSuccess = true;

            return scrappedData;
        }

        public WebScrappedData FindAndWebscrape(string g, string internalCode, string description)
        {
            var urlList = Find(g, internalCode, description);
            if (!urlList.Any())
            {
                return new WebScrappedData() { IsSuccess = false, ErrorMessage = "User was not found" };
            }
            else
            {
                return Webscrape(urlList.FirstOrDefault());
            }
        }

        public List<string> Find(string g, string internalCode, string description)
        {
            return null;
        }

        private List<string> GetSiteNavigationResult(string searchKeyword)
        {
            return null;
        }

        private string CleanGtinFromLeadingZeroes(string g)
        {
            return g.TrimStart(new Char[] { '0' });
        }

        private string GetNutrientInfo(WebScrappedData ret, string infoBasePath, string nutrientName)
        {
            try
            {
                string nutrient = string.Empty;

                string nutrientXpath = string.Format(infoBasePath, nutrientName);
                if (WebscraperUtils.IsElementPresent(browser, By.XPath(nutrientXpath)))
                {
                    var nutrientElem = browser.FindElements(By.XPath(nutrientXpath)).FirstOrDefault();
                    if (nutrientElem != null)
                    {
                        nutrient = WebscraperUtils.GetTextFromElement(nutrientElem);
                    }
                }

                return nutrient;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
