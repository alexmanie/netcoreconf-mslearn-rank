using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;

namespace GS1ProductTracker.Logic.Webscraper.Selenium
{
    public class ChromeDriverManager : IDisposable
    {
        private ChromeDriver Browser { get; set; }

        public void Dispose()
        {
            Browser.Dispose();
        }

        public ChromeDriver GenerateBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            //options.AddArgument("--window-size=1420,1080");
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            //options.AddArgument("--disable-dev-shm-usage");

            options.LeaveBrowserRunning = false;
#if DEBUG
            options.BinaryLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\chromedriver.exe";
#endif
#if !DEBUG
            options.BinaryLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
#endif  
            //ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //service.SuppressInitialDiagnosticInformation = true;
            //service.HideCommandPromptWindow = true;

            //this.Browser = new ChromeDriver(service, options, TimeSpan.FromMinutes(2));

            this.Browser = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);


            return this.Browser;
        }
    }
}
