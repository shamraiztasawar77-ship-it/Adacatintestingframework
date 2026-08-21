using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Automationassignment_01.Core;
public static class DriverFactory
{
    public static IWebDriver CreateDriver(
        string browser,
        bool headless)
    {
        if (string.IsNullOrWhiteSpace(browser))
        {
            throw new ArgumentException(
                "Browser name cannot be empty.",
                nameof(browser));
        }

        IWebDriver driver = browser
            .Trim()
            .ToLowerInvariant() switch
        {
            "chrome" => CreateChromeDriver(headless),
            "edge" => CreateEdgeDriver(headless),
            "firefox" => CreateFirefoxDriver(headless),

            _ => throw new NotSupportedException(
                $"Browser '{browser}' is not supported.")
        };

        driver.Manage().Timeouts().PageLoad =
            TimeSpan.FromSeconds(60);

        if (!headless)
        {
            driver.Manage().Window.Maximize();
        }

        return driver;
    }

    private static IWebDriver CreateChromeDriver(bool headless)
    {
        var options = new ChromeOptions();

        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");
        options.AddArgument("--disable-extensions");

        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
        }

        string driverDirectory = AppContext.BaseDirectory;
        string driverPath = Path.Combine(
            driverDirectory,
            "chromedriver.exe");

        if (!File.Exists(driverPath))
        {
            throw new FileNotFoundException(
                $"ChromeDriver was not found at '{driverPath}'. " +
                "Build the project so the ChromeDriver package is " +
                "copied to the output directory.",
                driverPath);
        }

        ChromeDriverService service =
            ChromeDriverService.CreateDefaultService(
                driverDirectory);

        service.HideCommandPromptWindow = true;

        return new ChromeDriver(
            service,
            options);
    }

    private static IWebDriver CreateEdgeDriver(bool headless)
    {
        var options = new EdgeOptions();

        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");

        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
        }

        return new EdgeDriver(options);
    }

    private static IWebDriver CreateFirefoxDriver(bool headless)
    {
        var options = new FirefoxOptions();

        if (headless)
        {
            options.AddArgument("-headless");
        }

        return new FirefoxDriver(options);
    }
}

