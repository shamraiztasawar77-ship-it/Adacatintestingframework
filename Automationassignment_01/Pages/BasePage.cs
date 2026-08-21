using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Automationassignment_01.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver { get; }
        protected WebDriverWait Wait { get; }

        protected BasePage(
            IWebDriver driver,
            TimeSpan timeout)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, timeout);
        }

        protected IWebElement WaitUntilVisible(By locator)
        {
            return Wait.Until(driver =>
            {
                try
                {
                    IWebElement element =
                        driver.FindElement(locator);

                    return element.Displayed
                        ? element
                        : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            })!;
        }

        protected IWebElement WaitUntilClickable(By locator)
        {
            return Wait.Until(driver =>
            {
                try
                {
                    IWebElement element =
                        driver.FindElement(locator);

                    return element.Displayed && element.Enabled
                        ? element
                        : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            })!;
        }

        protected void EnterText(
            By locator,
            string value)
        {
            IWebElement element =
                WaitUntilVisible(locator);

            element.Clear();
            element.SendKeys(value);
        }

        protected void Click(By locator)
        {
            WaitUntilClickable(locator).Click();
        }

        protected void SelectByText(
            By locator,
            string visibleText)
        {
            IWebElement element =
                WaitUntilVisible(locator);

            var dropdown =
                new SelectElement(element);

            dropdown.SelectByText(visibleText);
        }

        protected void SelectByValue(
            By locator,
            string value)
        {
            IWebElement element =
                WaitUntilVisible(locator);

            var dropdown =
                new SelectElement(element);

            dropdown.SelectByValue(value);
        }

        protected void SelectByIndex(
            By locator,
            int index)
        {
            IWebElement element =
                WaitUntilVisible(locator);

            var dropdown =
                new SelectElement(element);

            dropdown.SelectByIndex(index);
        }

        protected string GetText(By locator)
        {
            return WaitUntilVisible(locator)
                .Text
                .Trim();
        }

        protected string GetValue(By locator)
        {
            return WaitUntilVisible(locator)
                .GetAttribute("value")?
                .Trim() ?? string.Empty;
        }

        protected string GetSelectedText(By locator)
        {
            IWebElement element =
                WaitUntilVisible(locator);

            var dropdown =
                new SelectElement(element);

            return dropdown
                .SelectedOption
                .Text
                .Trim();
        }

        protected bool IsDisplayed(By locator)
        {
            try
            {
                return WaitUntilVisible(locator)
                    .Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}