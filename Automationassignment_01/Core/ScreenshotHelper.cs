using OpenQA.Selenium;

namespace Automationassignment_01.Core
{
    internal static class ScreenshotHelper
    {
        public static string Capture(
            IWebDriver driver,
            string testName)
        {
            if (driver is not ITakesScreenshot screenshotDriver)
            {
                throw new InvalidOperationException(
                    "The WebDriver does not support screenshots.");
            }

            string screenshotDirectory =
                Path.Combine(
                    ArtifactManager.ReportDirectory,
                    "screenshots");

            Directory.CreateDirectory(
                screenshotDirectory);

            string safeTestName =
                MakeSafeFileName(testName);

            string fileName =
                $"{safeTestName}-{DateTime.UtcNow:yyyyMMdd-HHmmssfff}.png";

            string fullPath =
                Path.Combine(
                    screenshotDirectory,
                    fileName);

            screenshotDriver
                .GetScreenshot()
                .SaveAsFile(fullPath);

            // The HTML report requires a relative path.
            return Path.Combine(
                "screenshots",
                fileName);
        }

        private static string MakeSafeFileName(
            string fileName)
        {
            char[] invalidCharacters =
                Path.GetInvalidFileNameChars();

            return string.Concat(
                fileName.Select(character =>
                    invalidCharacters.Contains(character)
                        ? '_'
                        : character));
        }
    }
}