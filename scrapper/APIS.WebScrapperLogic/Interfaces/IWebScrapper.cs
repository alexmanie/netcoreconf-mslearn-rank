using System.Collections.Generic;
using APIS.WebScrapperLogic.Utils;

namespace APIS.WebScrapperLogic.Interfaces
{
    public interface IWebScrapper
    {
        bool CanWebscrape(string companyGLN, string productURL);
        WebScrappedData Webscrape(string hyperlink);
        WebScrappedData FindAndWebscrape(string gtin, string internalCode, string description);
        List<string> Find(string gtin, string internalCode, string description);
    }
}
