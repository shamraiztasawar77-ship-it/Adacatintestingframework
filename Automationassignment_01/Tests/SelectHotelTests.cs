using Automationassignment_01.Core;
using Automationassignment_01.Models;
using Automationassignment_01.Pages;
using NUnit.Framework;

namespace Automationassignment_01.Tests
{
    [TestFixture]
    public sealed class SelectHotelTests : TestBase
    {
        [Test]
        public void TC07_UserCannotContinueWithoutSelectingHotel()
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

            selectHotelPage.ClickContinue();

            Assert.Multiple(() =>
            {
                Assert.That(
                    selectHotelPage.IsLoaded(),
                    Is.True,
                    "The application did not remain on the Select Hotel page.");

                Assert.That(
                    selectHotelPage.GetSelectionError(),
                    Is.EqualTo("Please Select a Hotel"),
                    "Hotel-selection validation message was incorrect.");
                }
            );
        }
    }
}