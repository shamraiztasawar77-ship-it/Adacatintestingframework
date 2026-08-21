using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Automationassignment_01.Pages
{
    public sealed class BookedItineraryPage : BasePage
    {
        private readonly IWebDriver _driver;
        private readonly TimeSpan _timeout;

        private readonly By _orderIdSearchField =
            By.Id("order_id_text");

        private readonly By _searchButton =
            By.Id("search_hotel_id");

        private readonly By _cancelSelectedButton =
            By.Name("cancelall");

        private readonly By _LOGOUT = 
            By.XPath("//a[normalize-space()='Logout']");
        private readonly By _hotelname =
            By.CssSelector("input[id^='hotel_name_']");

        private readonly By _locationname
            = By.CssSelector("input[id^='location_']");
        
        public BookedItineraryPage(
            IWebDriver driver,
            TimeSpan timeout)
            : base(driver, timeout)
        {
            _driver = driver;
            _timeout = timeout;
        }

        public bool IsLoaded()
        {
            return IsDisplayed(_orderIdSearchField);
        }

        public string gethotelname() {

            return GetValue(_hotelname);
        }
        public string gethotellocation() {

            return GetValue(_locationname);
        }
        public void SearchByOrderId(string orderId)
        {
            EnterText(_orderIdSearchField, orderId);
            Click(_searchButton);
        }
        public void LOGOUT() {
        
        Click(_LOGOUT);
        }


        public bool IsOrderDisplayed(string orderId)
        {
            By orderIdLocator =
                By.XPath(
                    $"//input[@value={CreateXPathLiteral(orderId)}]");

            try
            {
                var wait =
                    new WebDriverWait(
                        _driver,
                        TimeSpan.FromSeconds(5));

                return wait.Until(driver =>
                {
                    try
                    {
                        IWebElement element =
                            driver.FindElement(orderIdLocator);

                        return element.Displayed;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void SelectOrder(string orderId)
        {
            By orderCheckbox =
                By.XPath(
                    $"//input[@value={CreateXPathLiteral(orderId)}]" +
                    "/ancestor::tr//input[@type='checkbox']");

            Click(orderCheckbox);
        }

        public void CancelSelectedOrder()
        {
            Click(_cancelSelectedButton);

            var wait =
                new WebDriverWait(
                    _driver,
                    _timeout);

            IAlert alert =
                wait.Until(driver =>
                {
                    try
                    {
                        return driver.SwitchTo().Alert();
                    }
                    catch (NoAlertPresentException)
                    {
                        return null;
                    }
                })!;

            alert.Accept();

            WaitForAlertToClose();
        }

        private void WaitForAlertToClose()
        {
            var wait =
                new WebDriverWait(
                    _driver,
                    _timeout);

            wait.Until(driver =>
            {
                try
                {
                    driver.SwitchTo().Alert();

                    return false;
                }
                catch (NoAlertPresentException)
                {
                    return true;
                }
            });
        }

        private static string CreateXPathLiteral(
            string value)
        {
            if (!value.Contains('\''))
            {
                return $"'{value}'";
            }

            if (!value.Contains('"'))
            {
                return $"\"{value}\"";
            }

            string[] parts =
                value.Split('\'');

            return "concat('" +
                   string.Join(
                       "', \"'\", '",
                       parts) +
                   "')";
        }
    }
}