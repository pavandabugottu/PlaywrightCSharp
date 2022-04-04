using Microsoft.Playwright;
using Shouldly;
using System.Threading.Tasks;

namespace PlaywrightCSharp.PageObjects
{
    public class GoogleSearchPage
    {
        private readonly string searchbox = "[aria-label =\"Search\"]";
        private readonly string firstsearchOption = "xpath=//h3[contains(text(), 'Playwright: Fast and reliable end-to-end testing')]";
        private readonly string getStartedText = "text = Get started";
        private readonly string targetPageUrl = "https://playwright.dev/";
        private readonly string playwrightIntroPageUrl = "https://playwright.dev/docs/intro";
        private IPage page;

        public GoogleSearchPage(IPage ipage)
        {
            page = ipage;
        }

        public async Task ValidatePageTitle()
        {
            var title = await page.TitleAsync();
            title.ShouldBe("Google");
        }

        public async Task TypeSearchTextAndEnter(string searchText)
        {
            await page.FillAsync(searchbox, searchText, null);
            await page.PressAsync(searchbox, "Enter");
        }

        public async Task ClickFirstSearchResult()
        {
            await page.ClickAsync(firstsearchOption);
            page.Url.ShouldBe(targetPageUrl);
        }

        public async Task ClickGetStartedOnPlaywrightPage()
        {
            await page.ClickAsync(getStartedText);
            page.Url.ShouldBe(playwrightIntroPageUrl);
        }
    }
}
