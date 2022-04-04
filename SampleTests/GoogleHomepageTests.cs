using NUnit.Framework;
using PlaywrightCSharp.PageObjects;
using System.Threading.Tasks;

namespace PlaywrightCSharp.SampleTests
{
    [TestFixture]
    public class GoogleHomepageTests : NUnitPlaywrightBase
    {
        private GoogleSearchPage googleSearchpage;
        private string pageUrl = "https://www.google.com/";

        [Test]
        public async Task Verify_Google_PageTitle()
        {
            await page.GotoAsync(pageUrl);

            googleSearchpage = new GoogleSearchPage(page);
            await googleSearchpage.ValidatePageTitle();            
        }

        [Test]
        public async Task Verify_UserCanSearchOnGoogleHomepage()
        {
            await page.GotoAsync(pageUrl);

            googleSearchpage = new GoogleSearchPage(page);
            await googleSearchpage.TypeSearchTextAndEnter("Playwright");
            await googleSearchpage.ClickFirstSearchResult();
            await googleSearchpage.ClickGetStartedOnPlaywrightPage();
        }
    }
}
