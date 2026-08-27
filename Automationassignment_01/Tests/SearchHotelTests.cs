using Automationassignment_01.Core;
using Automationassignment_01.Models;
using Automationassignment_01.Pages;


namespace Automationassignment_01.Tests
{
    [TestFixture]
    public sealed class SearchHotelTests : TestBase
    {
        [Test]
        [Category("Regression")]
        [Category("Validation")]
        public void TC01_LocationIsMandatory()
        {
            var searchHotelPage = new SearchHotelPage(
                Driver,
                DefaultTimeout);

            SearchHotelData data =
                XmlDataReader.GetSearchHotelData("TC-01.01");

            searchHotelPage.Search(data);

            Assert.That(
                searchHotelPage.GetLocationError(),
                Is.EqualTo(data.ExpectedMessage),
                "Location validation message was incorrect.");
        }

        [Test]
        [Category("Regression")]
        [Category("Validation")]
        public void TC02_CheckInDateCannotBeLaterThanCheckOutDate()
        {
            var searchHotelPage = new SearchHotelPage(
                Driver,
                DefaultTimeout);

            SearchHotelData data =
                XmlDataReader.GetSearchHotelData("TC-02.01");

            searchHotelPage.Search(data);

            Assert.That(
                searchHotelPage.GetCheckInDateError(),
                Is.EqualTo(data.ExpectedMessage),
                "Check-in validation message was incorrect.");
        }

        [Test]
        [Category("Regression")]
        [Category("Validation")]
        public void TC03_CheckInDateCannotBeInThePast()
        {
            var searchHotelPage = new SearchHotelPage(
                Driver,
                DefaultTimeout);

            SearchHotelData data =
                XmlDataReader.GetSearchHotelData("TC-03.01");

            searchHotelPage.Search(data);

            Assert.That(
                searchHotelPage.GetCheckInDateError(),
                Is.EqualTo(data.ExpectedMessage),
                "Past-date validation message was incorrect.");
        }

        [Test]
        [Category("Regression")]
        [Category("Validation")]
        public void TC04_CheckInAndCheckOutDatesCannotBeEqual()
        {
            var searchHotelPage = new SearchHotelPage(
                Driver,
                DefaultTimeout);

            SearchHotelData data =
                XmlDataReader.GetSearchHotelData("TC-04.01");

            searchHotelPage.Search(data);

            Assert.That(
                searchHotelPage.GetCheckInDateError(),
                Is.EqualTo(data.ExpectedMessage),
                "Same-date validation message was incorrect.");
        }

        [Test]
        [Category("Regression")]
        [Category("Validation")]
        public void TC05_SearchCriteriaAreDisplayedCorrectly()
        {
            var searchHotelPage = new SearchHotelPage(
                Driver,
                DefaultTimeout);

            SearchHotelData data =
                XmlDataReader.GetSearchHotelData("TC-05.01");

            searchHotelPage.Search(data);

            var selectHotelPage = new SelectHotelPage(
                Driver,
                DefaultTimeout);
                string roomCount = data.NumberOfRooms
               .Split('-')[0]
                  .Trim();

                  string expectedNumberOfRooms =
                roomCount == "1"
                    ? "1 Rooms"
                    : $"{roomCount} Rooms";
            
            Assert.Multiple(() =>
            {
                Assert.That(
                    selectHotelPage.IsLoaded(),
                    Is.True,
                    "Select Hotel page did not load.");

                Assert.That(
                    selectHotelPage.GetLocation(),
                    Is.EqualTo(data.Location),
                    "Location was incorrect.");

                Assert.That(
                    selectHotelPage.GetHotelName(),
                    Is.EqualTo(data.Hotel),
                    "Hotel was incorrect.");

                Assert.That(
                    selectHotelPage.GetRoomType(),
                    Is.EqualTo(data.RoomType),
                    "Room type was incorrect.");

                Assert.That(
                  selectHotelPage.GetNumberOfRooms(),
                  Is.EqualTo("2 Rooms"),
                  "Number of rooms was incorrect.");
            });
        }
        
        [Test]
        [Category("Regression")]
        [Category("Validation")]
        public void TC06_ResetClearsSearchHotelForm()
        {
            var searchHotelPage = new SearchHotelPage(
                Driver,
                DefaultTimeout);

            SearchHotelData data =
                XmlDataReader.GetSearchHotelData("TC-06.01");

            searchHotelPage.FillSearchForm(data);

            searchHotelPage.ClickReset();

            Assert.Multiple(() =>
            {
                Assert.That(
                    searchHotelPage.GetSelectedLocation(),
                    Is.EqualTo("- Select Location -"),
                    "Location was not reset.");

                Assert.That(
                    searchHotelPage.GetSelectedHotel(),
                    Is.EqualTo("- Select Hotel -"),
                    "Hotel was not reset.");

                Assert.That(
                    searchHotelPage.GetSelectedRoomType(),
                    Is.EqualTo("- Select Room Type -"),
                    "Room type was not reset.");

                Assert.That(
                    searchHotelPage.GetSelectedNumberOfRooms(),
                    Is.EqualTo("1 - One"),
                    "Number of rooms was not reset.");

                Assert.That(
                    searchHotelPage.GetSelectedAdultsPerRoom(),
                    Is.EqualTo("1 - One"),
                    "Adults per room was not reset.");
            });
        }
    }
}