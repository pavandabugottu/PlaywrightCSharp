using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PlaywrightCSharp
{
    [TestFixture]
    public abstract class NUnitPlaywrightBase
    {
        protected IPlaywright playwright;
        protected IBrowser browser;
        protected IPage page;
        protected IBrowserContext context;
        private string CurrentTestFolder = TestContext.CurrentContext.TestDirectory;
        private DirectoryInfo LogsFolder;
        private DirectoryInfo ScreenshotsFolder;

        [OneTimeSetUp]
        public void CreateiPlaywrightAndiBrowserContextInstances()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(PlaywrightBrowserSettings));
            var playwrightConfig = section.Get<PlaywrightBrowserSettings>();
            CurrentTestFolder = string.IsNullOrEmpty(playwrightConfig.LogFolderPath) ? CurrentTestFolder : playwrightConfig.LogFolderPath;
            LogsFolder = Directory.CreateDirectory(Path.Combine(CurrentTestFolder, "Logs"));
            ScreenshotsFolder = Directory.CreateDirectory(Path.Combine(CurrentTestFolder, "Screenshots"));

            LaunchBrowser(playwrightConfig.Browser, playwrightConfig.Headeless);
        }

        [SetUp]
        public async Task CreateiBrowserContextAndiPageContextInstances()
        {
            context = await browser.NewContextAsync();
            page = await context.NewPageAsync();
            await page.SetViewportSizeAsync(1920, 1080);
        }                

        [TearDown]
        public async Task DisposeiPageContextAndiBrowserContextInstances()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;

            // Take screenshot and log the test results to a log file if the test fails.
            if (testResult.Status.Equals(TestStatus.Failed) || testResult.Status.Equals(ResultState.Error))
            {
                var testSpecificScreenshotFolder = Directory.CreateDirectory(Path.Combine(ScreenshotsFolder.FullName, TestContext.CurrentContext.Test.Name));
                var screenshotPath = Path.Combine(testSpecificScreenshotFolder.FullName, $"TestFailure_{DateTime.Now.ToString("yyyyMMddHHmmss")}.png");
                //Take a screenshot
                await page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                //var testSpecificLogsFolder = Directory.CreateDirectory(Path.Combine(LogsFolder.FullName, TestContext.CurrentContext.Test.Name));
            }

            //await page.CloseAsync();
            await context.CloseAsync();
        }

        [OneTimeTearDown]
        public void DisposeiBrowserContextAndiPlaywrightContextInstances()
        {
            //await page.CloseAsync();
            //await browser.CloseAsync();
            playwright?.Dispose();
        }

        #region Private Methods        

        private void LaunchBrowser(string browsertype, bool headless = false)
        {
            if (browser == null)
            {
                if (headless == false)
                {
                    browser = Task.Run(() => GetBrowserAsync(browsertype, headless: false)).Result;                    
                }
                else
                {
                    browser = Task.Run(() => GetBrowserAsync(browsertype, headless: true)).Result;
                }
            }
        }

        private async Task<IBrowser> GetBrowserAsync(string browserName, bool headless = false)
        {
            var playwright = await Playwright.CreateAsync();

            IBrowser browser;

            switch (browserName)
            {
                case "chrome":
                    if (headless)
                    {
                        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = true });
                    }
                    else
                    {
                        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = false, SlowMo = 1000 });
                    }
                    break;
                case "msedge":
                    if (headless)
                    {
                        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = true });
                    }
                    else
                    {
                        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = false, SlowMo = 1000 });
                    }
                    break;
                case "firefox":
                    if (headless)
                    {
                        browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = true });
                    }
                    else
                    {
                        browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = false, SlowMo = 1000 });
                    }
                    break;
                case "safari":
                    if (headless)
                    {
                        browser = await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = true });
                    }
                    else
                    {
                        browser = await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = false, SlowMo = 1000 });
                    }
                    break;

                case "iphone12":
                    if (headless)
                    {
                        browser = await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = true });
                    }
                    else
                    {
                        browser = await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Channel = browserName, Headless = false, SlowMo = 1000 });
                    }
                    break;

                default:
                    throw new Exception("Invalid value for parameter named browser in the config file");
            }

            return browser;
        }

        #endregion
    }
}