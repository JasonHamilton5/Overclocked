using Microsoft.Playwright;
using Xunit;

namespace Overclocked.Tests.UITests
{
    public class ProductUITests : IAsyncLifetime
    {
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private IPage _page = null!;
        private const string BaseUrl = "http://localhost:5173";

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            _page = await _browser.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task Homepage_Loads_Successfully()
        {
            await _page.GotoAsync(BaseUrl);
            var title = await _page.TitleAsync();
            Assert.Equal("Overclocked", title);
        }

        [Fact]
        public async Task Homepage_Displays_Products()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForSelectorAsync(".col");
            var products = await _page.QuerySelectorAllAsync(".col");
            Assert.True(products.Count > 0);
        }

        [Fact]
        public async Task Clicking_Product_Navigates_To_Product_Page()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForSelectorAsync(".col a");
            await _page.Locator(".col a").First.ClickAsync();
            Assert.Contains("/product/", _page.Url);
        }

        [Fact]
        public async Task Product_Page_Displays_SKU()
        {
            await _page.GotoAsync($"{BaseUrl}/product/1");
            await _page.WaitForSelectorAsync("text=SKU");
            var sku = await _page.QuerySelectorAsync("text=SKU");
            Assert.NotNull(sku);
        }

        [Fact]
        public async Task Invalid_Page_Shows_404()
        {
            await _page.GotoAsync($"{BaseUrl}/somefakepage");
            await _page.WaitForSelectorAsync("text=404");
            var notFound = await _page.QuerySelectorAsync("text=404");
            Assert.NotNull(notFound);
        }

        [Fact]
        public async Task Search_Filters_Products()
        {
            await _page.GotoAsync(BaseUrl + "/products");
            await _page.WaitForSelectorAsync("input[placeholder='Search...']");
            await _page.FillAsync("input[placeholder='Search...']", "Mouse");
            var products = await _page.QuerySelectorAllAsync(".col");
            Assert.True(products.Count > 0);
        }
    }
}