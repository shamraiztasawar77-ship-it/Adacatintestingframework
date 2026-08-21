using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automationassignment_01.Models;
using OpenQA.Selenium;
using System.Globalization;
using RazorEngine.Configuration;

namespace Automationassignment_01.Pages
{
   public sealed class SearchHotelPage : BasePage
    {
        private readonly By _location = By.Id("location");
        private readonly By _hotel = By.Id("hotels");
        private readonly By _roomType = By.Id("room_type");
        private readonly By _numberOfRooms = By.Id("room_nos");
        private readonly By _checkInDate = By.Id("datepick_in");
        private readonly By _checkOutDate = By.Id("datepick_out");
        private readonly By _adultsPerRoom = By.Id("adult_room");
        private readonly By _childrenPerRoom = By.Id("child_room");

        private readonly By _searchButton = By.Id("Submit");
        private readonly By _resetButton = By.Id("Reset");
        private readonly By _itinarypage = By.XPath("//a[normalize-space()='Booked Itinerary']");

        private readonly By _locationError = By.Id("location_span");
        private readonly By _checkInDateError = By.Id("checkin_span");
        private readonly By _checkOutDateError = By.Id("checkout_span");
        private readonly By _searchpageheading = By.XPath("//td[@class='login_title']");

        public SearchHotelPage(
            IWebDriver driver,
            TimeSpan timeout)
            : base(driver, timeout)
        {
        }

        public bool IsLoaded()
        {
            return IsDisplayed(_location)
                && IsDisplayed(_hotel)
                && IsDisplayed(_roomType)
                && IsDisplayed(_searchButton);
        }

        public void SelectLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return;
            }
            SelectByText(_location, location);
        }

        public void SelectHotel(string hotel)
        {
            SelectByText(_hotel, hotel);
        }

        public void SelectRoomType(string roomType)
        {
            SelectByText(_roomType, roomType);
        }

        public void SelectNumberOfRooms(string numberOfRooms)
        {
            SelectByText(_numberOfRooms, numberOfRooms);
        }

        public void EnterCheckInDate(string checkInDate)
        {
            EnterText(_checkInDate, checkInDate);
        }

        public void EnterCheckOutDate(string checkOutDate)
        {
            EnterText(_checkOutDate, checkOutDate);
        }

        public void SelectAdultsPerRoom(string adultsPerRoom)
        {
            SelectByText(_adultsPerRoom, adultsPerRoom);
        }

        public void SelectChildrenPerRoom(string childrenPerRoom)
        {
            if (string.IsNullOrWhiteSpace(childrenPerRoom))
            {
                return;
            }

            string optionValue = childrenPerRoom
                .Split('-')[0]
                .Trim();

            SelectByValue(
                _childrenPerRoom,
                optionValue);
        }

        public void ClickSearch()
        {
            Click(_searchButton);
        }

        public void ClickReset()
        {
            Click(_resetButton);
        }
        public void itinarypage() {
            Click(_itinarypage);
        }

        public void FillSearchForm(SearchHotelData data)
        {
            SelectLocation(data.Location);
            SelectHotel(data.Hotel);
            SelectRoomType(data.RoomType);
            SelectNumberOfRooms(data.NumberOfRooms);

            string checkInDate = DateTime.Today
                .AddDays(data.CheckInOffsetDays)
                .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            string checkOutDate = DateTime.Today
                .AddDays(data.CheckOutOffsetDays)
                .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            EnterCheckInDate(checkInDate);
            EnterCheckOutDate(checkOutDate);

            SelectAdultsPerRoom(data.AdultsPerRoom);
            SelectChildrenPerRoom(data.ChildrenPerRoom);
        }
        public string gethsearchotelheading() {

            return GetText(_searchpageheading);
        }

        public void Search(SearchHotelData data)
        {
            FillSearchForm(data);
            ClickSearch();
        }

        public string GetLocationError()
        {
            return GetText(_locationError);
        }

        public string GetCheckInDateError()
        {
            return GetText(_checkInDateError);
        }

        public string GetCheckOutDateError()
        {
            return GetText(_checkOutDateError);
        }

        public string GetSelectedLocation()
        {
            return GetSelectedText(_location);
        }

        public string GetSelectedHotel()
        {
            return GetSelectedText(_hotel);
        }

        public string GetSelectedRoomType()
        {
            return GetSelectedText(_roomType);
        }

        public string GetSelectedNumberOfRooms()
        {
            return GetSelectedText(_numberOfRooms);
        }

        public string GetCheckInDate()
        {
            return GetValue(_checkInDate);
        }

        public string GetCheckOutDate()
        {
            return GetValue(_checkOutDate);
        }

        public string GetSelectedAdultsPerRoom()
        {
            return GetSelectedText(_adultsPerRoom);
        }

        public string GetSelectedChildrenPerRoom()
        {
            return GetSelectedText(_childrenPerRoom);
        }
    }
}
