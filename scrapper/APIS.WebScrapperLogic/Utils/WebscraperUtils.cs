using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace APIS.WebScrapperLogic.Utils
{
    public class WebscraperUtils
    {
        static WebscraperUtils _instance = null;

        public static WebscraperUtils GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WebscraperUtils();
            }
            return _instance;
        }

        protected WebscraperUtils() { }

        public static bool IsElementPresent(IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            // catch (Exception ex)
            // {
            //     Console.ForegroundColor = ConsoleColor.Red;
            //     Console.WriteLine($"[EXCEPTION] {ex.Message}");
            //     Console.ResetColor();

            //     throw;
            // }
        }

        /// <summary>
        /// Find by CSS Selector and return the text
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="selectorString"></param>
        /// <returns></returns>
        public static string GetTextByCssSelector(RemoteWebDriver browser, string selectorString)
        {
            try
            {
                var element = browser.FindElement(By.CssSelector(selectorString));
                return GetTextFromElement(element);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if element is displayed and return the text
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetTextFromElement(IWebElement element)
        {
            try
            {
                if (element == null) return string.Empty;

                if (element.Displayed)
                    return element.Text.Trim();
                else
                    return element.GetAttribute("textContent").Trim();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
