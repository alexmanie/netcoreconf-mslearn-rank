using APIS.Services;
using APIS.WebScrapperLogic.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebScrapperController : ControllerBase
    {
        [HttpGet]
        public ActionResult<WebScrappedData> Webscrape(string path)
        {
            try
            {
                WebScrappedData result = WebScrapperService.WebScrappe(path);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}