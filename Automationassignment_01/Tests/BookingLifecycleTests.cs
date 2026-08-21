using Automationassignment_01.Core;
using Automationassignment_01.Models;
using Automationassignment_01.Pages;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Xml;

namespace Automationassignment_01.Tests
{
    [TestFixture]
    public sealed class BookingLifecycleTests : TestBase
    {
        private BookHotelPage NavigateToBookHotelPage(
            string searchDataId)
        {
            SearchHotelData searchData =
                XmlDataReader.GetSearchHotelData(
                    searchDataId);

            var searchHotelPage =
                new SearchHotelPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                searchHotelPage.IsLoaded(),
                Is.True,
                "Search Hotel page did not load.");

            searchHotelPage.Search(searchData);

            var selectHotelPage =
                new SelectHotelPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                selectHotelPage.IsLoaded(),
                Is.True,
                "Select Hotel page did not load.");

            selectHotelPage
                .SelectFirstHotelAndContinue();

            var bookHotelPage =
                new BookHotelPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                bookHotelPage.IsLoaded(),
                Is.True,
                "Book Hotel page did not load.");

            return bookHotelPage;
        }

        private BookingConfirmationPage CreateBooking(
            string bookingDataId)
        {
            // This method already exists in XmlDataReader.
            BookHotelData bookingData =
                XmlDataReader.GetBookingLifecycleData(
                    bookingDataId);

            // TC-08.01 contains valid hotel-search data.
            BookHotelPage bookHotelPage =
                NavigateToBookHotelPage(
                    "TC-08.01");

            bookHotelPage.EnterBookingDetails(
                bookingData);

            bookHotelPage.ClickBookNow();

            var confirmationPage =
                new BookingConfirmationPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                confirmationPage.IsLoaded(),
                Is.True,
                "Booking Confirmation page did not load.");

            return confirmationPage;
        }

        [Test]
        public void TC11_SuccessfulBookingGeneratesValidOrderId()
        {
            BookHotelData bookingData =
                XmlDataReader.GetBookingLifecycleData(
                    "TC-11.01");

            BookingConfirmationPage confirmationPage =
                CreateBooking(
                    "TC-11.01");

            string orderId =
                confirmationPage.GetOrderId();

            Assert.Multiple(() =>
            {
                Assert.That(
                    orderId,
                    Is.Not.Null.And.Not.Empty,
                    "A non-empty Order ID was not generated.");

                Assert.That(
                    confirmationPage.GetFirstName(),
                    Is.EqualTo(bookingData.FirstName),
                    "Incorrect first name was displayed.");

                Assert.That(
                    confirmationPage.GetLastName(),
                    Is.EqualTo(bookingData.LastName),
                    "Incorrect last name was displayed.");

                Assert.That(
                    confirmationPage.GetHotelName(),
                    Is.Not.Null.And.Not.Empty,
                    "Hotel name was not displayed.");

                Assert.That(
                    confirmationPage.GetLocation(),
                    Is.Not.Null.And.Not.Empty,
                    "Location was not displayed.");

                Assert.That(
                    confirmationPage.GetRoomType(),
                    Is.Not.Null.And.Not.Empty,
                    "Room type was not displayed.");

                Assert.That(
                    confirmationPage.GetFinalPrice(),
                    Is.Not.Null.And.Not.Empty,
                    "Final price was not displayed.");
            });
        }
        [Test]
        public void TC12_BookingCanBeSearchedAndCancelledUsingOrderId()
        {
            // TC12 creates a separate booking so that it does not
            // depend on TC11 or its Order ID.
            BookingConfirmationPage confirmationPage =
                CreateBooking(
                    "TC-12.01");

            string orderId =
                confirmationPage.GetOrderId();

            Assert.That(
                orderId,
                Is.Not.Null.And.Not.Empty,
                "Order ID was not generated.");

            confirmationPage.OpenBookedItinerary();

            var itineraryPage =
                new BookedItineraryPage(
                    Driver,
                    DefaultTimeout);

            Assert.That(
                itineraryPage.IsLoaded(),
                Is.True,
                "Booked Itinerary page did not load.");

            itineraryPage.SearchByOrderId(
                orderId);

            Assert.That(
                itineraryPage.IsOrderDisplayed(orderId),
                Is.True,
                $"Booking '{orderId}' was not found.");

            itineraryPage.SelectOrder(
                orderId);

            itineraryPage.CancelSelectedOrder();

            // Search again to verify that the cancelled booking
            // is no longer present as an active booking.
            itineraryPage.SearchByOrderId(
                orderId);

            Assert.That(
                itineraryPage.IsOrderDisplayed(orderId),
                Is.False,
                $"Booking '{orderId}' remained active " +
                "after cancellation.");
        }
        //[Test]
        //public void TC013_testfailuretest()
        //{
        //    var searchHotelPage = new SearchHotelPage(
        //        Driver,
        //        DefaultTimeout);

        //    SearchHotelData data =
        //        XmlDataReader.GetSearchHotelData("TC-05.01");

        //    searchHotelPage.Search(data);

        //    var selectHotelPage = new SelectHotelPage(
        //        Driver,
        //        DefaultTimeout);
        //    string roomCount = data.NumberOfRooms
        //   .Split('-')[0]
        //      .Trim();

        //    string expectedNumberOfRooms =
        //  roomCount == "1"
        //      ? "1 Rooms"
        //      : $"{roomCount} Rooms";
        //    Assert.Fail();
        //}
        //[Test]
        //public void tc13() {

        //    var searchhotelpage = new SearchHotelPage(Driver, DefaultTimeout);
        //    SearchHotelData searchdata = XmlDataReader.GetSearchHotelData("TC-10.01");
        //    searchhotelpage.Search(searchdata);
        //    var selecthotelpage = new SelectHotelPage(Driver, DefaultTimeout);
        //    selecthotelpage.SelectFirstHotelAndContinue();
        //    var bookhotelpage = new BookHotelPage(Driver, DefaultTimeout);
        //    //bookhotelpage = NavigateToBookHotelPage("TC-08.01");
        //    BookHotelData bookdata = XmlDataReader.GetBookingLifecycleData("TC-11.01");
        //    bookhotelpage.EnterBookingDetails(bookdata);
        //    bookhotelpage.ClickBookNow();
        //    var bookingconfirmationpage = new BookingConfirmationPage(Driver, DefaultTimeout);
        //    string orderid = bookingconfirmationpage.GetOrderId();
        //    bookingconfirmationpage.logout();
        //    bookingconfirmationpage.loginagain();
        //    //bookingconfirmationpage.loginagain();
        //    var loginpage = new LoginPage(Driver, DefaultTimeout);
        //    LoginData logindata = XmlDataReader.GetLoginData("ValidUser");
        //    loginpage.Login(logindata.Username, logindata.Password);
        //    searchhotelpage.itinarypage();
        //    var bookeditinarypage = new BookedItineraryPage(Driver, DefaultTimeout);
        //    bookeditinarypage.SearchByOrderId(orderid);
        //    Assert.That(
        //       bookeditinarypage.IsOrderDisplayed(orderid),
        //       Is.True, "order id not found "


        //       );

        //}
        //[Test]

        //public void tc_14()
        //{ var searchhotelpage = new SearchHotelPage(Driver, DefaultTimeout);
        //    SearchHotelData searchdata = XmlDataReader.GetSearchHotelData("TC-08.01");
        //    searchhotelpage.Search(searchdata);

        //    var selecthotelpage = new SelectHotelPage(Driver, DefaultTimeout);
        //    //selecthotel.SelectFirstHotel();
        //    selecthotelpage.SelectFirstHotelAndContinue();
        //    var bookhotelpage = new BookHotelPage(Driver, DefaultTimeout);
        //    BookHotelData bookingdata = XmlDataReader.GetBookingLifecycleData("TC-11.01");
        //    bookhotelpage.EnterBookingDetails(bookingdata);
        //    bookhotelpage.ClickBookNow();
        //    var bookingconfiramtionpage = new BookingConfirmationPage(Driver, DefaultTimeout);
        //    string orderid = bookingconfiramtionpage.GetOrderId();
        //    bookingconfiramtionpage.searchbuttton();
        //    SearchHotelData searchdata2 = XmlDataReader.GetSearchHotelData("TC-08.01");
        //    searchhotelpage.Search(searchdata2);
        //    //searchhotelpage.ClickSearch();
        //    //selecthotel.SelectFirstHotel();
        //    selecthotelpage.SelectFirstHotelAndContinue();
        //    BookHotelData data2 = XmlDataReader.GetBookingLifecycleData("TC-12.01");
        //    bookhotelpage.EnterBookingDetails(data2);
        //    bookhotelpage.ClickBookNow();
        //    string orderis1 = bookingconfiramtionpage.GetOrderId();
        //    var bookitinarypage = new BookedItineraryPage(Driver, DefaultTimeout);
        //    bookingconfiramtionpage.OpenBookedItinerary();
        //    bookitinarypage.SearchByOrderId(orderid);
        //    Assert.That(
        //        bookitinarypage.IsOrderDisplayed(orderid),
        //        Is.True, ""
        //        );
        //    bookitinarypage.SearchByOrderId(orderis1);
        //    Assert.That(
        //        bookitinarypage.IsOrderDisplayed(orderis1),
        //        Is.True, ""
        //       );
        //}
        //[Test]
        //public void tc_15() {
        //    var searchhotelpage = new SearchHotelPage(Driver, DefaultTimeout);
        //    SearchHotelData searchdata = XmlDataReader.GetSearchHotelData("TC-08.01");
        //    searchhotelpage.Search(searchdata);
        //    var selecthotelpage = new SelectHotelPage(Driver, DefaultTimeout);
        //    selecthotelpage.SelectFirstHotel();
        //    selecthotelpage.SelectFirstHotelAndContinue();
        //    var bookhotelpage = new BookHotelPage(Driver, DefaultTimeout);
        //    BookHotelData bookdata = XmlDataReader.GetBookingLifecycleData("TC-11.01");
        //    bookhotelpage.EnterBookingDetails(bookdata);
        //    bookhotelpage.ClickBookNow();
        //    var bookingconfirmationpage = new BookingConfirmationPage(Driver,DefaultTimeout);
        //   string orderid= bookingconfirmationpage.GetOrderId();
        //    bookingconfirmationpage.logout();        
        //    bookingconfirmationpage.loginagain();
        //    var loginpage = new LoginPage(Driver,DefaultTimeout);
        //    LoginData data = XmlDataReader.GetLoginData("ValidUser");
        //    loginpage.Login(data.Username,data.Password);
        //    searchhotelpage.itinarypage();
        //    var bookeditinarypage = new BookedItineraryPage(Driver, DefaultTimeout);
        //    bookeditinarypage.SearchByOrderId(orderid);
        //    Assert.That(
        //        bookeditinarypage.IsOrderDisplayed(orderid),
        //        Is.True, ""
        //        );
        //    bookeditinarypage.SelectOrder(orderid);
        //    bookeditinarypage.CancelSelectedOrder();
        //    bookeditinarypage.SearchByOrderId(orderid);
        //    Assert.That(
        //        bookeditinarypage.IsOrderDisplayed(orderid),
        //        Is.False,""

        //        );
        //    bookeditinarypage.LOGOUT();

        //}


        //[Test]

        //public void tc15() { 
        //    var searchhotelpage = new SearchHotelPage(Driver, DefaultTimeout);
        //    SearchHotelData searchdata = XmlDataReader.GetSearchHotelData("TC-08.01");
        //    searchhotelpage.Search(searchdata);
        //    var selecthotelpage = new SelectHotelPage(Driver,DefaultTimeout);
        //    selecthotelpage.SelectFirstHotelAndContinue();
        //    var bookhotelpage = new BookHotelPage(Driver,DefaultTimeout);
        //    BookHotelData bookdata = XmlDataReader.GetBookingLifecycleData("TC-12.01");
        //    bookhotelpage.EnterBookingDetails(bookdata);
        //    bookhotelpage.ClickBookNow();   
        //    var bookingconfirmationpage = new BookingConfirmationPage(Driver, DefaultTimeout);
        //    bookingconfirmationpage.OpenBookedItinerary();
        //    var bookeditinarypage = new BookedItineraryPage(Driver, DefaultTimeout);
        //    bookeditinarypage.LOGOUT();
        //}
        //[Test]
        //public void TC16_VerifyBookingCanBeSearchedByOrderId()
        //{
        //    // Search Hotel
        //    var searchHotelPage =
        //        new SearchHotelPage(
        //            Driver,
        //            DefaultTimeout);

        //    SearchHotelData searchData =
        //        XmlDataReader.GetSearchHotelData(
        //            "TC-08.01");

        //    searchHotelPage.Search(searchData);

        //    // Select Hotel
        //    var selectHotelPage =
        //        new SelectHotelPage(
        //            Driver,
        //            DefaultTimeout);

        //    selectHotelPage.SelectFirstHotelAndContinue();

        //    // Book Hotel
        //    var bookHotelPage =
        //        new BookHotelPage(
        //            Driver,
        //            DefaultTimeout);

        //    BookHotelData bookingData =
        //        XmlDataReader.GetBookingLifecycleData(
        //            "TC-11.01");

        //    bookHotelPage.EnterBookingDetails(bookingData);
        //    bookHotelPage.ClickBookNow();

        //    // Booking Confirmation
        //    var confirmationPage =
        //        new BookingConfirmationPage(
        //            Driver,
        //            DefaultTimeout);

        //    string orderId =
        //        confirmationPage.GetOrderId();

        //    Assert.That(
        //        orderId,
        //        Is.Not.Null.And.Not.Empty,
        //        "Order ID was not generated.");

        //    // Open Booked Itinerary
        //    confirmationPage.OpenBookedItinerary();

        //    var bookedItineraryPage =
        //        new BookedItineraryPage(
        //            Driver,
        //            DefaultTimeout);

        //    // Search Booking
        //    bookedItineraryPage.SearchByOrderId(orderId);
        //    Assert.That(
        //        bookedItineraryPage.IsOrderDisplayed(orderId),
        //        Is.True,
        //        $"Booking '{orderId}' was not found in the Booked Itinerary.");
        //}
        //[Test]

        //public void tc_17checkingforpageheading() 
        //{
        //var searchhotelpage = new SearchHotelPage(Driver,DefaultTimeout);
        //    SearchHotelData seqarcdata = XmlDataReader.GetSearchHotelData("TC-08.01");
        //    Assert.That(
        //        searchhotelpage.gethsearchotelheading(),
        //        Is.EqualTo(seqarcdata.ExpectedMessage), "did not match)"

        //        );
        //    searchhotelpage.Search(seqarcdata);
        //    var selecthotelpage = new SelectHotelPage(Driver,DefaultTimeout);
        //    Assert.That(
        //           selecthotelpage.IsLoaded(),
        //           Is.True,
        //           "Select Hotel page did not load.");
        //    //Assert.That(
        //    //    selecthotelpage.getselecthotelheading(),
        //    //    Does.Contain("Select Hotel"), ""

        //    //    );

        //    selecthotelpage.SelectFirstHotelAndContinue();
        //    var bookhotelconfirmationpage = new BookHotelPage(Driver,DefaultTimeout);
        //    BookHotelData bookdata = XmlDataReader.GetBookingLifecycleData("TC-11.01");
        //    bookhotelconfirmationpage.EnterBookingDetails(bookdata);
        //    bookhotelconfirmationpage.ClickBookNow();

        //    var bookingconfirmation = new BookingConfirmationPage(Driver, DefaultTimeout);
        //   string orderid = bookingconfirmation.GetOrderId();
        //    bookingconfirmation.OpenBookedItinerary();


        //    var bookedit = new BookedItineraryPage(Driver, DefaultTimeout);
        //    bookedit.SelectOrder(orderid);
        //    bookedit.CancelSelectedOrder();

























        //}
        [Test]


        public void tc_13()
        {
            var searchhotelpage = new SearchHotelPage(Driver,DefaultTimeout);
            SearchHotelData searchdata = XmlDataReader.GetSearchHotelData("TC-08.01");
            searchhotelpage.Search(searchdata);

            var selecthotelpage = new SelectHotelPage(Driver, DefaultTimeout);
            selecthotelpage.SelectFirstHotelAndContinue();

            var bookhotelpage = new BookHotelPage(Driver, DefaultTimeout);
            BookHotelData bookdata = XmlDataReader.GetBookingLifecycleData("TC-11.01");
            bookhotelpage.EnterBookingDetails(bookdata);
            bookhotelpage.ClickBookNow();

            var confirmationpage = new BookingConfirmationPage(Driver,DefaultTimeout);
            string orderid = confirmationpage.GetOrderId();

            confirmationpage.logout();
            confirmationpage.loginagain();

            var login = new LoginPage(Driver,DefaultTimeout);
            LoginData logindata = XmlDataReader.GetLoginData("ValidUser");
            login.Login(logindata.Username,logindata.Password);

            searchhotelpage.itinarypage();

            var itinarypage = new BookedItineraryPage(Driver,DefaultTimeout);
            itinarypage.SearchByOrderId(orderid);
            Assert.That(
                itinarypage.IsOrderDisplayed(orderid),
                Is.True,"the order id is not present "

                );
        
        
        
        }
    }

   
}
   

