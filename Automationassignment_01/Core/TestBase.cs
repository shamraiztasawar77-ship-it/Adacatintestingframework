using Automationassignment_01.Pages;
using AventStack.ExtentReports;
using Automationassignment_01.Models;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace Automationassignment_01.Core
{
    public abstract class TestBase
    {
        protected IWebDriver Driver { get; private set; } = null!;

        protected TimeSpan DefaultTimeout { get; private set; }

        private ExtentTest _extentTest = null!;

        [SetUp]
        public void SetUp()
        {
            string browser =
                XmlDataReader.GetAppSetting("Browser");

            bool headless =
                XmlDataReader.GetBooleanAppSetting("Headless");

            int explicitWaitSeconds =
                XmlDataReader.GetPositiveIntegerAppSetting(
                    "ExplicitWaitSeconds");

            DefaultTimeout =
                TimeSpan.FromSeconds(explicitWaitSeconds);

            _extentTest =
                ArtifactManager.CreateTest(
                    TestContext.CurrentContext.Test.Name,
                    TestContext.CurrentContext.Test.FullName);

            ArtifactManager.SetExecutionInfo(
                browser,
                headless);

            Driver =
                DriverFactory.CreateDriver(
                    browser,
                    headless);

            string applicationUrl =
                XmlDataReader.GetAppSetting(
                    "ApplicationUrl");

            Driver.Navigate().GoToUrl(
                applicationUrl);

            LoginData loginData =
                XmlDataReader.GetLoginData("ValidUser");

            var loginPage =
                new LoginPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                loginPage.IsLoaded(),
                Is.True,
                "Login page did not load.");

            loginPage.Login(
                loginData.Username,
                loginData.Password);

            var searchHotelPage =
                new SearchHotelPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                searchHotelPage.IsLoaded(),
                Is.True,
                "Login failed: Search Hotel page did not load.");
        }

        [TearDown]
        public void TearDown()
        {
            TestStatus status =
                TestContext.CurrentContext
                    .Result
                    .Outcome
                    .Status;

            string message =
                TestContext.CurrentContext
                    .Result
                    .Message ?? string.Empty;

            string stackTrace =
                TestContext.CurrentContext
                    .Result
                    .StackTrace ?? string.Empty;

            try
            {
                AddTestResultToReport(
                    status,
                    message,
                    stackTrace);
            }
            catch (Exception exception)
            {
                TestContext.Error.WriteLine(
                    $"Could not create report artifacts: {exception}");
            }
            finally
            {
                try
                {
                    Driver?.Quit();
                }
                catch (Exception exception)
                {
                    TestContext.Error.WriteLine(
                        $"Could not quit WebDriver: {exception}");
                }

                try
                {
                    Driver?.Dispose();
                }
                catch (Exception exception)
                {
                    TestContext.Error.WriteLine(
                        $"Could not dispose WebDriver: {exception}");
                }

                try
                {
                    ArtifactManager.Flush();
                }
                catch (Exception exception)
                {
                    TestContext.Error.WriteLine(
                        $"Could not flush the test report: {exception}");
                }
            }
        }

        private void AddTestResultToReport(
        TestStatus status,
        string message,
        string stackTrace)
            {
                string resultMessage =
                    string.IsNullOrWhiteSpace(message)
                        ? $"Test completed with status: {status}."
                        : message;

                switch (status)
                {
                    case TestStatus.Passed:
                        _extentTest.Pass(
                            "Test passed successfully.");
                        break;

                    case TestStatus.Skipped:
                        _extentTest.Skip(
                            resultMessage);
                        break;

                    case TestStatus.Inconclusive:

                    case TestStatus.Warning:
                        _extentTest.Warning(
                            resultMessage);
                        break;

                    default:
                        _extentTest.Fail(
                            resultMessage);

                        if (!string.IsNullOrWhiteSpace(stackTrace))
                        {
                            _extentTest.Fail(
                                $"<pre>{stackTrace}</pre>");
                        }

                        break;
                }

                if (Driver is null)
                {
                    return;
                }

                string relativeScreenshotPath =
                    ScreenshotHelper.Capture(
                        Driver,
                        TestContext.CurrentContext.Test.Name);

                _extentTest.AddScreenCaptureFromPath(
                    relativeScreenshotPath,
                    $"Browser state after test - {status}");
            }

    }
}
