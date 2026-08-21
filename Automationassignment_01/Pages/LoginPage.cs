using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Automationassignment_01.Pages
{
    public sealed class LoginPage : BasePage
    {
        private readonly By _username = By.Id("username");
        private readonly By _password = By.Id("password");
        private readonly By _loginButton = By.Id("login");
        private readonly By _logoutbutton = By.Id("logout");

        public LoginPage(
            IWebDriver driver,
            TimeSpan timeout)
            : base(driver, timeout)
        {
        }

        public void Login(string username, string password)
        {
            EnterText(_username, username);
            EnterText(_password, password);
            Click(_loginButton);
        }
        
        public bool IsLoaded()
        {
            return IsDisplayed(_username)
                && IsDisplayed(_password)
                && IsDisplayed(_loginButton);
        }
    }
}
