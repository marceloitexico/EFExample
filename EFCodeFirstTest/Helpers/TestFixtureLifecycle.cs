using System;
using NUnit.Framework;

namespace BankingSite.FunctionalUITests
{
    [SetUpFixture]
    public class TestFixtureLifecycle : IDisposable
    {        
        public void Dispose()              
        {
            DemoHelperCode.Utilities.Wait(5000);

            // Cleanup and close browser
            BrowserHost.Driver.Dispose();
        }
    }
}
