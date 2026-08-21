using OpenQA.Selenium;
using System;

namespace Automationassignment_01.Pages
{
    public sealed class SelectHotelPage : BasePage
    {
        private readonly By _selectHotelTitle =
            By.XPath("//*[normalize-space()='Select Hotel']");

        private readonly By _location =
            By.Id("location_0");

        private readonly By _hotelName =
            By.Id("hotel_name_0");

        private readonly By _roomType =
            By.Id("room_type_0");

        private readonly By _numberOfRooms =
            By.Id("rooms_0");

        private readonly By _firstHotelRadioButton =
            By.XPath("//input[@id='radiobutton_0']");

        private readonly By _continueButton =
            By.Id("continue");

        private readonly By _cancelButton =
            By.Id("cancel");

        private readonly By _selectionError =
            By.XPath("//label[@id='radiobutton_span']");

        private readonly By _selecthotelheading =
            By.Id("//td[@class='login_title']"); 

        public SelectHotelPage(
            IWebDriver driver,
            TimeSpan timeout)
            : base(driver, timeout)
        {
        }
        

        public bool IsLoaded()
        {
            return IsDisplayed(_hotelName);
        }
        
        public string getselecthotelheading() {

            return GetText(_selecthotelheading);
        
        }
        public string GetLocation()
        {
            return GetValue(_location);
        }

        public string GetHotelName()
        {
            return GetValue(_hotelName);
        }

        public string GetRoomType()
        {
            return GetValue(_roomType);
        }

        public string GetNumberOfRooms()
        {
            return GetValue(_numberOfRooms);
        }

        public void SelectFirstHotel()
        {
            Click(_firstHotelRadioButton);
        }

        public void ClickContinue()
        {
            Click(_continueButton);
        }

        public void ClickCancel()
        {
            Click(_cancelButton);
        }

        public void SelectFirstHotelAndContinue()
        {
            SelectFirstHotel();
            ClickContinue();
        }

        public string GetSelectionError()
        {
            return GetText(_selectionError);
        }
    }
}