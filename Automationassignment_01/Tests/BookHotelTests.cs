using Automationassignment_01.Core;
using Automationassignment_01.Models;
using Automationassignment_01.Pages;
using NUnit.Framework;

namespace Automationassignment_01.Tests
{
    [TestFixture]
    public sealed class BookHotelTests : TestBase
    {
        private BookHotelPage NavigateToBookHotelPage(
            string searchDataId)
        {
            SearchHotelData searchData =
                XmlDataReader.GetSearchHotelData(searchDataId);


            var searchHotelPage =
                new SearchHotelPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                searchHotelPage.IsLoaded(),
                Is.True,
                "Search Hotel page did not load.");

            searchHotelPage.Search(searchData);

            // Select Hotel page
            var selectHotelPage =
                new SelectHotelPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                selectHotelPage.IsLoaded(),
                Is.True,
                "Select Hotel page did not load after searching.");

            selectHotelPage.SelectFirstHotelAndContinue();

            // Book Hotel page
            var bookHotelPage =
                new BookHotelPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                bookHotelPage.IsLoaded(),
                Is.True,
                "Book Hotel page did not load after selecting the hotel.");

            return bookHotelPage;
        }

        [Test]
        public void TC08_TotalPriceExcludingGstIsCalculatedCorrectly()
        {
            BookHotelPage bookHotelPage =
                NavigateToBookHotelPage("TC-08.01");

            decimal pricePerNight =
                bookHotelPage.GetPricePerNight();

            int numberOfRooms =
                bookHotelPage.GetNumberOfRooms();

            int numberOfDays =
                bookHotelPage.GetTotalDays();

            decimal expectedTotalPrice =
                pricePerNight *
                numberOfRooms *
                numberOfDays;

            decimal actualTotalPrice =
                bookHotelPage.GetTotalPrice();

            Assert.That(
                actualTotalPrice,
                Is.EqualTo(expectedTotalPrice),
                $"Expected total price to be {expectedTotalPrice}, " +
                $"but displayed price was {actualTotalPrice}.");
        }

        [Test]
        [Category("Regression")]
        [Category("Validation")]

        public void TC09_FinalBilledPriceIncludingGstIsCalculatedCorrectly()
        {
            BookHotelPage bookHotelPage =
                NavigateToBookHotelPage("TC-09.01");

            decimal totalPrice =
                bookHotelPage.GetTotalPrice();

            decimal gst =
                bookHotelPage.GetGst();

            decimal expectedFinalPrice =
                totalPrice + gst;

            decimal actualFinalPrice =
                bookHotelPage.GetFinalBilledPrice();

            Assert.That(
                actualFinalPrice,
                Is.EqualTo(expectedFinalPrice),
                $"Expected final price to be {expectedFinalPrice}, " +
                $"but displayed price was {actualFinalPrice}.");
        }

        [Test]
        [Category("Regression")]
        [Category("Validation")]

        public void TC10_MandatoryGuestAndPaymentFieldsAreValidated()
        {
            BookHotelData bookingData =
                XmlDataReader.GetBookingValidationData("TC-10.01");

            BookHotelPage bookHotelPage =
                NavigateToBookHotelPage("TC-10.01");

            // Leave all mandatory fields empty.
            bookHotelPage.ClickBookNow();

            Assert.Multiple(() =>
            {
                Assert.That(
                    bookHotelPage.GetFirstNameError(),
                    Is.EqualTo(bookingData.ExpectedFirstNameMessage),
                    "First-name validation message was incorrect.");

                Assert.That(
                    bookHotelPage.GetLastNameError(),
                    Is.EqualTo(bookingData.ExpectedLastNameMessage),
                    "Last-name validation message was incorrect.");

                Assert.That(
                    bookHotelPage.GetAddressError(),
                    Is.EqualTo(bookingData.ExpectedAddressMessage),
                    "Address validation message was incorrect.");

                Assert.That(
                    bookHotelPage.GetCreditCardNumberError(),
                    Is.EqualTo(bookingData.ExpectedCreditCardMessage),
                    "Credit-card validation message was incorrect.");

                Assert.That(
                    bookHotelPage.GetCreditCardTypeError(),
                    Is.EqualTo(bookingData.ExpectedCardTypeMessage),
                    "Credit-card-type validation message was incorrect.");

                Assert.That(
                    bookHotelPage.GetExpiryDateError(),
                    Is.EqualTo(bookingData.ExpectedExpiryMessage),
                    "Expiry-date validation message was incorrect.");

                Assert.That(
                    bookHotelPage.GetCvvError(),
                    Is.EqualTo(bookingData.ExpectedCvvMessage),
                    "CVV validation message was incorrect.");
            });
        }
        [Test]
        [Category("Regression")]
        [Category("Smoke")]
        [Category("E2E")]
        public void TC_18LIFECYCLE()
        {

            var searchhotelpage = new SearchHotelPage(Driver, DefaultTimeout);
            SearchHotelData data = XmlDataReader.GetSearchHotelData("TC-08.01");
            searchhotelpage.Search(data);
            var selecthotelpage = new SelectHotelPage(Driver, DefaultTimeout);
            selecthotelpage.SelectFirstHotelAndContinue();
            var bookhotelpage = new BookHotelPage(Driver, DefaultTimeout);
            BookHotelData bookdata = XmlDataReader.GetBookingLifecycleData("TC-11.01");
            bookhotelpage.EnterBookingDetails(bookdata);
            bookhotelpage.ClickBookNow();
            var bookingconfirmationpage = new BookingConfirmationPage(Driver, DefaultTimeout);
            string orderid = bookingconfirmationpage.GetOrderId();
            bookingconfirmationpage.logout();
            bookingconfirmationpage.loginagain();
            var login = new LoginPage(Driver, DefaultTimeout);
            LoginData logindata = XmlDataReader.GetLoginData("ValidUser");
            login.Login(logindata.Username, logindata.Password);
            searchhotelpage.itinarypage();
            var itinarypage = new BookedItineraryPage(Driver, DefaultTimeout);
            itinarypage.SearchByOrderId(orderid);
            Assert.Multiple(() =>
            {
                Assert.That(
                    itinarypage.IsOrderDisplayed(orderid),
                    Is.True, ""
                    );

                Assert.That(
                itinarypage.IsLoaded(), Is.True, "page did not loaded "

                    );
                Assert.That(
                    itinarypage.gethotelname(),
                    Is.EqualTo(data.expectedhotelname), "the name of hotel dosent match "

                    );
                Assert.That(
                    itinarypage.gethotellocation(),
                    Is.EqualTo(data.expectedlocation), "the location is wrong"
                    );
            });

            itinarypage.SelectOrder(orderid);
            itinarypage.CancelSelectedOrder();
            itinarypage.SearchByOrderId(orderid);
            Assert.That(
                itinarypage.IsOrderDisplayed(orderid),
                Is.False, ""

                );

        }
    }
}
