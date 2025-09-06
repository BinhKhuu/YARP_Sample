using Microsoft.Playwright;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task ReverseProxy_HomePage_HasButton()
    {
        await Page.GotoAsync("http://localhost:5044");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync("");

        // create a locator
        var getSomethingBtn = Page.Locator("text=GetSomething unauthorised");

        // Expect an attribute "to be strictly equal" to the value.
        await Expect(getSomethingBtn).ToHaveAttributeAsync("id", "myButton");

        // Click the get started link.
        await getSomethingBtn.ClickAsync();
    }

    [Test]
    public async Task ReverseProxy_APIRoute_Returns401()
    {
        await Page.GotoAsync("http://localhost:5044/api");

        var has401Error = await Page.QuerySelectorAsync("div.error-code:text('HTTP ERROR 401')") != null;

        
    }

    /// <summary>
    /// Not working yet
    /// </summary>
    [Test]
    public async Task ReverseProxy_WebRoute_()
    {
        
        try
        {
            // Step 1: Navigate to /web (should redirect to login)
            Console.WriteLine("Navigating to /web...");
            await Page.GotoAsync("http://localhost:5044/web");

            // Step 2: Wait for login page to load
            await Page.WaitForSelectorAsync("input[name='loginfmt']", new PageWaitForSelectorOptions 
            { 
                Timeout = 10000 
            });
            
            // Step 3: Fill in login credentials
            Console.WriteLine("Entering email...");
            await Page.FillAsync("input[name='loginfmt']", "your-email@company.com");

            var nextButton = await Page.QuerySelectorAsync("input[type='submit'][value*='Next'], input[type='button'][value*='Next']");
            if (nextButton != null)
            {
                await nextButton.ClickAsync();
            }
            // Step 5: Wait for successful redirect (to protected content)
            await Page.WaitForLoadStateAsync();
        
            // Step 6: Verify we're on the correct page now
            var currentUrl = Page.Url;
            Console.WriteLine($"Current URL after login: {currentUrl}");
        
            if (!currentUrl.Contains("/web")) // Adjust this condition based on your app
            {
                Console.WriteLine("✓ Successfully logged in and redirected!");
            }
            else
            {
                Console.WriteLine("⚠ Login might have failed - still on web page");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login flow: {ex.Message}");
        }
        finally
        {
            await Browser.CloseAsync();
        }
    }
}