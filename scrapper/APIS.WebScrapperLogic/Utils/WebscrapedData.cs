namespace APIS.WebScrapperLogic.Utils
{
    public class WebScrappedData
    {
        public WebScrappedData()
        {
            IsSuccess = false;
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public string Username { get; set; }
        public string Level { get; set; }
        public string Points { get; set; }
        public string LevelStatusPoints { get; set; }
    }
}

