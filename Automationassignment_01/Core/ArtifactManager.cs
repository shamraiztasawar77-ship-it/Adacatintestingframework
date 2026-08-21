using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace Automationassignment_01.Core
{
    internal static class ArtifactManager
    {
        private static readonly object Sync = new();
        private static bool _executionInfoConfigured;

        public static string ReportDirectory { get; } =
            GetReportDirectory();

        private static readonly ExtentReports Report =
            CreateReport();

        public static ExtentTest CreateTest(
            string testName,
            string? description)
        {
            lock (Sync)
            {
                return Report.CreateTest(
                    testName,
                    description);
            }
        }

        public static void SetExecutionInfo(
            string browser,
            bool headless)
        {
            lock (Sync)
            {
                if (_executionInfoConfigured)
                {
                    return;
                }

                Report.AddSystemInfo(
                    "Browser",
                    browser);

                Report.AddSystemInfo(
                    "Execution Mode",
                    headless ? "Headless" : "Headed");

                _executionInfoConfigured = true;
            }
        }

        public static void Flush()
        {
            lock (Sync)
            {
                Report.Flush();
            }
        }

        private static ExtentReports CreateReport()
        {
            Directory.CreateDirectory(ReportDirectory);

            string reportFile = Path.Combine(
                ReportDirectory,
                "index.html");

            var sparkReporter =
                new ExtentSparkReporter(reportFile);

            sparkReporter.Config.DocumentTitle =
                "Selenium Test Report";

            sparkReporter.Config.ReportName =
                "Automation Assignment Results";

            sparkReporter.Config.CSS = """
        body {
            font-family: Arial, Helvetica, sans-serif;
        }

        .container {
            width: 95% !important;
            max-width: 1400px;
            margin: 0 auto;
        }

        .card-panel {
            padding: 20px !important;
            margin: 16px 0 !important;
            border-radius: 8px;
        }

        .test-list-item {
            padding: 14px 18px !important;
        }

        .test-content {
            padding: 20px !important;
        }

        table {
            width: 100% !important;
            table-layout: auto;
            border-collapse: collapse;
        }

        table th,
        table td {
            padding: 12px !important;
            text-align: left;
            vertical-align: top;
            word-break: break-word;
        }

        .details {
            width: 100%;
            overflow-x: auto;
        }

        img {
            display: block;
            max-width: 100% !important;
            height: auto !important;
            margin: 12px auto;
            border-radius: 6px;
        }

        pre,
        code {
            white-space: pre-wrap;
            word-break: break-word;
        }

        @media only screen and (max-width: 768px) {
            .container {
                width: 98% !important;
            }

            .card-panel,
            .test-content {
                padding: 12px !important;
            }

            table th,
            table td {
                padding: 8px !important;
            }
        }
        """;

            var extentReports = new ExtentReports();

            extentReports.AttachReporter(sparkReporter);

            extentReports.AddSystemInfo(
                "Operating System",
                Environment.OSVersion.ToString());

            extentReports.AddSystemInfo(
                ".NET Runtime",
                Environment.Version.ToString());

            return extentReports;
        }

        private static string GetReportDirectory()
        {
            string? environmentPath =
                Environment.GetEnvironmentVariable(
                    "TEST_REPORT_DIR");

            if (!string.IsNullOrWhiteSpace(environmentPath))
            {
                return Path.GetFullPath(
                    environmentPath);
            }

            return Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "TestResults",
                "Html");
        }
    }
}
