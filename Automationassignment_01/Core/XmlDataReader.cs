using Automationassignment_01.Models;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Automationassignment_01.Core
{
    public static class XmlDataReader
    {
        private static string ConfigurationDirectory =>
            Path.Combine(
                AppContext.BaseDirectory,
                "Configuration");

        private static string TestDataDirectory =>
            Path.Combine(
                ConfigurationDirectory,
                "TestData");

        public static string GetAppSetting(
            string elementName)
        {
            string filePath =
                Path.Combine(
                    ConfigurationDirectory,
                    "AppSettings.xml");

            XDocument document =
                LoadDocument(filePath);

            string? value = document
                .Root?
                .Element(elementName)?
                .Value;

            return RequireValue(
                value,
                $"App setting '{elementName}'");
        }

        public static bool GetBooleanAppSetting(
            string elementName)
        {
            string rawValue = GetAppSetting(elementName);

            if (!bool.TryParse(rawValue, out bool result))
            {
                throw new InvalidOperationException(
                    $"App setting '{elementName}' must be 'true' " +
                    $"or 'false'. Current value: '{rawValue}'.");
            }

            return result;
        }

        public static int GetPositiveIntegerAppSetting(
            string elementName)
        {
            string rawValue = GetAppSetting(elementName);

            if (!int.TryParse(rawValue, out int result) || result <= 0)
            {
                throw new InvalidOperationException(
                    $"App setting '{elementName}' must be a positive " +
                    $"integer. Current value: '{rawValue}'.");
            }

            return result;
        }

        public static LoginData GetLoginData(
            string userId)
        {
            string filePath =
                Path.Combine(
                    TestDataDirectory,
                    "LoginData.xml");

            XDocument document =
                LoadDocument(filePath);

            XElement? user = document
                .Root?
                .Elements("User")
                .FirstOrDefault(element =>
                    string.Equals(
                        (string?)element.Attribute("id"),
                        userId,
                        StringComparison.OrdinalIgnoreCase));

            if (user is null)
            {
                throw new InvalidOperationException(
                    $"Login user '{userId}' was not found " +
                    $"in '{filePath}'.");
            }

            return new LoginData
            {
                Id = userId,
                Username = RequireValue(
                    user.Element("Username")?.Value,
                    $"Username for login user '{userId}'"),
                Password = RequireValue(
                    user.Element("Password")?.Value,
                    $"Password for login user '{userId}'")
            };
        }

        public static SearchHotelData GetSearchHotelData(
            string testCaseId)
        {
            string filePath =
                Path.Combine(
                    TestDataDirectory,
                    "SearchHotelData.xml");

            XDocument document =
                LoadDocument(filePath);

            XElement testCase =
                FindTestCase(
                    document,
                    testCaseId,
                    "Search Hotel",
                    filePath);

            return new SearchHotelData
            {
                Id = testCaseId,

                Description =
                    GetOptionalValue(
                        testCase,
                        "Description"),

                Location =
                    GetOptionalValue(
                        testCase,
                        "Location"),

                Hotel =
                    GetOptionalValue(
                        testCase,
                        "Hotel"),

                RoomType =
                    GetOptionalValue(
                        testCase,
                        "RoomType"),

                NumberOfRooms =
                    GetOptionalValue(
                        testCase,
                        "NumberOfRooms"),

                CheckInOffsetDays =
                    GetIntegerValue(
                        testCase,
                        "CheckInOffsetDays"),

                CheckOutOffsetDays =
                    GetIntegerValue(
                        testCase,
                        "CheckOutOffsetDays"),

                AdultsPerRoom =
                    GetOptionalValue(
                        testCase,
                        "AdultsPerRoom"),

                ChildrenPerRoom =
                    GetOptionalValue(
                        testCase,
                        "ChildrenPerRoom"),

                expectedhotelname =
                GetOptionalValue(
                    testCase,
                    "expectedhotelname"),

                expectedlocation = GetOptionalValue(testCase,"expectedlocation"),

                ExpectedMessage =
                    GetOptionalValue(
                        testCase,
                        "ExpectedMessage")
            };
        }

        public static BookHotelData GetBookingValidationData(
            string testCaseId)
        {
            string filePath =
                Path.Combine(
                    TestDataDirectory,
                    "BookHotelData.xml");

            XDocument document =
                LoadDocument(filePath);

            XElement testCase =
                FindTestCase(
                    document,
                    testCaseId,
                    "Book Hotel validation",
                    filePath);

            return MapBookHotelData(testCase, testCaseId);
        }

        public static BookHotelData GetBookingLifecycleData(
            string testCaseId)
        {
            string filePath =
                Path.Combine(
                    TestDataDirectory,
                    "BookingLifecycleData.xml");

            XDocument document = LoadDocument(filePath);

            XElement testCase =
                FindTestCase(
                    document,
                    testCaseId,
                    "Booking lifecycle",
                    filePath);

            return MapBookHotelData(testCase, testCaseId);
        }

        private static BookHotelData MapBookHotelData(
            XElement testCase,
            string testCaseId)
        {
            return new BookHotelData
            {
                TestCaseId = testCaseId,
                FirstName = GetOptionalValue(testCase, "FirstName"),
                LastName = GetOptionalValue(testCase, "LastName"),
                Address = GetOptionalValue(testCase, "Address"),
                CreditCardNumber = GetOptionalValue(testCase, "CreditCardNumber"),
                CreditCardType = GetOptionalValue(testCase, "CreditCardType"),
                ExpiryMonth = GetOptionalValue(testCase, "ExpiryMonth"),
                ExpiryYear = GetOptionalValue(testCase, "ExpiryYear"),
                Cvv = GetOptionalValue(testCase, "Cvv"),
                ExpectedFirstNameMessage = GetOptionalValue(testCase, "ExpectedFirstNameMessage"),
                ExpectedLastNameMessage = GetOptionalValue(testCase, "ExpectedLastNameMessage"),
                ExpectedAddressMessage = GetOptionalValue(testCase, "ExpectedAddressMessage"),
                ExpectedCreditCardMessage = GetOptionalValue(testCase, "ExpectedCreditCardMessage"),
                ExpectedCardTypeMessage = GetOptionalValue(testCase, "ExpectedCardTypeMessage"),
                ExpectedExpiryMessage = GetOptionalValue(testCase, "ExpectedExpiryMessage"),
                ExpectedCvvMessage = GetOptionalValue(testCase, "ExpectedCvvMessage")
            };
        }

        private static XElement FindTestCase(
            XDocument document,
            string testCaseId,
            string dataDescription,
            string filePath)
        {
            if (string.IsNullOrWhiteSpace(testCaseId))
            {
                throw new ArgumentException(
                    "Test-case ID cannot be empty.",
                    nameof(testCaseId));
            }

            XElement? testCase = document
                .Root?
                .Elements("TestCase")
                .FirstOrDefault(element =>
                    string.Equals(
                        (string?)element.Attribute("id"),
                        testCaseId,
                        StringComparison.OrdinalIgnoreCase));

            if (testCase is null)
            {
                throw new InvalidOperationException(
                    $"{dataDescription} test case " +
                    $"'{testCaseId}' was not found " +
                    $"in '{filePath}'.");
            }

            return testCase;
        }

        private static XDocument LoadDocument(
            string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    $"XML file was not found: {filePath}",
                    filePath);
            }

            try
            {
                return XDocument.Load(filePath);
            }
            catch (Exception exception)
                when (exception is System.Xml.XmlException ||
                      exception is InvalidOperationException)
            {
                throw new InvalidOperationException(
                    $"XML file could not be loaded: {filePath}",
                    exception);
            }
        }

        private static string GetOptionalValue(
            XElement parent,
            string elementName)
        {
            return parent
                .Element(elementName)?
                .Value
                .Trim()
                ?? string.Empty;
        }

        private static int GetIntegerValue(
            XElement parent,
            string elementName)
        {
            string rawValue =
                GetOptionalValue(
                    parent,
                    elementName);

            if (!int.TryParse(
                    rawValue,
                    out int result))
            {
                throw new InvalidOperationException(
                    $"'{elementName}' must contain a valid integer. " +
                    $"Current value: '{rawValue}'.");
            }

            return result;
        }

        private static string RequireValue(
            string? value,
            string description)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"{description} is missing or empty.");
            }

            return value.Trim();
        }
    }
}
