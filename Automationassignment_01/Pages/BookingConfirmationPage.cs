using OpenQA.Selenium;
using System;

namespace Automationassignment_01.Pages
{
    public sealed class BookingConfirmationPage : BasePage
    {
        private readonly By _orderId =
            By.Id("order_no");

        private readonly By _hotelName =
            By.Id("hotel_name");

        private readonly By _location =
            By.Id("location");

        private readonly By _roomType =
            By.Id("room_type");
      
        private readonly By _firstName =
            By.Id("first_name");

        private readonly By _lastName =
            By.Id("last_name");

        private readonly By _finalPrice =
            By.Id("final_price");

        private readonly By _myItineraryButton =
            By.Id("my_itinerary");
        private readonly By _logoutbutton = By.Id("logout");
        private readonly By _newloginlink = By.XPath("//a[normalize-space()='Click here to login again']");
        private new readonly By _searchbutton = By.Id("search_hotel");
        public BookingConfirmationPage(
            IWebDriver driver,
            TimeSpan timeout)
            : base(driver, timeout)
        {
        }

        public bool IsLoaded()
        {
            return IsDisplayed(_orderId);
        }

        public string GetOrderId()
        {
            return GetValue(_orderId).Trim();
        }

        public string GetHotelName()
        {
            return GetValue(_hotelName).Trim();
        }

        public string GetLocation()
        {
            return GetValue(_location).Trim();
        }

        public string GetRoomType()
        {
            return GetValue(_roomType).Trim();
        }

        public string GetFirstName()
        {
            return GetValue(_firstName).Trim();
        }

        public string GetLastName()
        {
            return GetValue(_lastName).Trim();
        }

        public string GetFinalPrice()
        {
            return GetValue(_finalPrice).Trim();
        }
        public void logout() {

            Click(_logoutbutton);
        }
        public void searchbuttton() {
            Click(_searchbutton);
        }

        public void loginagain() {

            Click(_newloginlink);
        }

        public void OpenBookedItinerary()
        {
            Click(_myItineraryButton);
        }
    }
}