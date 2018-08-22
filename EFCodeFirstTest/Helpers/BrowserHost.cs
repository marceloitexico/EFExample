using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TestStack.Seleno.Configuration;

namespace BankingSite.FunctionalUITests
{
    public static class BrowserHost
    {
        public static readonly SelenoHost Instance = new SelenoHost();
        public static readonly string RootUrl;
        public static FirefoxDriver Driver { get; }
        static BrowserHost()
        {
            try
            {
                /* this configuration is for Seleno, but since version 3 has no compatibility with mozilla firefox.
                //var binary = new FirefoxBinary(@"C:\Program Files\Mozilla Firefox\firefox.exe");
                //IWebDriver driver = new FirefoxDriver();
                //FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\Users\Developer\Documents\iTexico\Goals 2018");
                //service.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox";
                // Instance.Run("BankingSite", 4223);
                */
                //CreateDefaultService, path to geckoDriver.exe
                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\Users\Developer\Documents\iTexico\Goals 2018");
                service.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                Driver = new FirefoxDriver(service);
                RootUrl = RootUrl = "http://localhost:1468/";
                
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
          
        }

        
    }
}
