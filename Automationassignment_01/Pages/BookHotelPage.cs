using OpenQA.Selenium;
using System;
using System.Globalization;
using System.Linq;
using Automationassignment_01.Models;

namespace Automationassignment_01.Pages
{
    public sealed class BookHotelPage : BasePage
    {
        // Booking summary fields
        private readonly By _hotelName =
            By.Id("hotel_name_dis");

        private readonly By _location =
            By.Id("location_dis");

        private readonly By _roomType =
            By.Id("room_type_dis");

        private readonly By _numberOfRooms =
            By.Id("room_num_dis");

        private readonly By _totalDays =
            By.Id("total_days_dis");

        private readonly By _pricePerNight =
            By.Id("price_night_dis");

        private readonly By _totalPrice =
            By.Id("total_price_dis");

        private readonly By _gst =
            By.Id("gst_dis");

        private readonly By _finalBilledPrice =
            By.Id("final_price_dis");

        // Guest details
        private readonly By _firstName =
            By.Id("first_name");

        private readonly By _lastName =
            By.Id("last_name");

        private readonly By _billingAddress =
            By.Id("address");

        // Payment details
        private readonly By _creditCardNumber =
            By.Id("cc_num");

        private readonly By _creditCardType =
            By.Id("cc_type");

        private readonly By _expiryMonth =
            By.Id("cc_exp_month");

        private readonly By _expiryYear =
            By.Id("cc_exp_year");

        private readonly By _cvv =
            By.Id("cc_cvv");

        private readonly By _bookNowButton =
            By.Id("book_now");

        // Validation messages
        private readonly By _firstNameError =
            By.Id("first_name_span");

        private readonly By _lastNameError =
            By.Id("last_name_span");

        private readonly By _addressError =
            By.Id("address_span");

        private readonly By _creditCardNumberError =
            By.Id("cc_num_span");

        private readonly By _creditCardTypeError =
            By.Id("cc_type_span");

        private readonly By _expiryDateError =
            By.Id("cc_expiry_span");

        private readonly By _cvvError =
            By.Id("cc_cvv_span");

        public BookHotelPage(
            IWebDriver driver,
            TimeSpan timeout)
            : base(driver, timeout)
        {
        }

        public bool IsLoaded()
        {
            return IsDisplayed(_bookNowButton);
        }

        public int GetNumberOfRooms()
        {
            return ExtractInteger(GetValue(_numberOfRooms));
        }

        public int GetTotalDays()
        {
            return ExtractInteger(GetValue(_totalDays));
        }

        public decimal GetPricePerNight()
        {
            return ExtractDecimal(GetValue(_pricePerNight));
        }

        public decimal GetTotalPrice()
        {
            return ExtractDecimal(GetValue(_totalPrice));
        }

        public decimal GetGst()
        {
            return ExtractDecimal(GetValue(_gst));
        }

        public decimal GetFinalBilledPrice()
        {
            return ExtractDecimal(GetValue(_finalBilledPrice));
        }
        public void EnterBookingDetails(BookHotelData data)
            {
                EnterText(_firstName, data.FirstName);
                EnterText(_lastName, data.LastName);
                EnterText(_billingAddress, data.Address);
                EnterText(_creditCardNumber, data.CreditCardNumber);

                if (!string.IsNullOrWhiteSpace(data.CreditCardType))
                {
                    SelectByText(_creditCardType, data.CreditCardType);
                }

                if (!string.IsNullOrWhiteSpace(data.ExpiryMonth))
                {
                    SelectByText(_expiryMonth, data.ExpiryMonth);
                }

                if (!string.IsNullOrWhiteSpace(data.ExpiryYear))
                {
                    SelectByText(_expiryYear, data.ExpiryYear);
                }

                EnterText(_cvv, data.Cvv);
}

        public void ClickBookNow()
        {
            Click(_bookNowButton);
        }

        public string GetFirstNameError()
        {
            return GetText(_firstNameError);
        }

        public string GetLastNameError()
        {
            return GetText(_lastNameError);
        }

        public string GetAddressError()
        {
            return GetText(_addressError);
        }

        public string GetCreditCardNumberError()
        {
            return GetText(_creditCardNumberError);
        }

        public string GetCreditCardTypeError()
        {
            return GetText(_creditCardTypeError);
        }

        public string GetExpiryDateError()
        {
            return GetText(_expiryDateError);
        }

        public string GetCvvError()
        {
            return GetText(_cvvError);
        }

        private static int ExtractInteger(string value)
        {
            string digits =
                new string(value
                    .Where(char.IsDigit)
                    .ToArray());

            if (!int.TryParse(digits, out int result))
            {
                throw new FormatException(
                    $"Could not extract an integer from '{value}'.");
            }

            return result;
        }

        private static decimal ExtractDecimal(string value)
        {
            string numericValue =
                new string(value
                    .Where(character =>
                        char.IsDigit(character) ||
                        character == '.')
                    .ToArray());

            if (!decimal.TryParse(
                    numericValue,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out decimal result))
            {
                throw new FormatException(
                    $"Could not extract a decimal value from '{value}'.");
            }

            return result;
        }
    }
}