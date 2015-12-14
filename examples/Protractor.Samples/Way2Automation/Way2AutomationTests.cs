using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
namespace Protractor.Test
{

    [TestFixture]
    public class Way2AutomationTests
    {
        private StringBuilder verificationErrors = new StringBuilder();
        private IWebDriver driver;
        private NgWebDriver ngDriver;
        private String login_url = "http://way2automation.com/way2auto_jquery/index.php";
        private String base_url = "http://www.way2automation.com/angularjs-protractor/banking";
        [TestFixtureSetUp]
        public void SetUp()
        {
            // driver = new PhantomJSDriver();
            driver = new FirefoxDriver();
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(60));
            ngDriver = new NgWebDriver(driver);
        }

        [SetUp]
        public void LogintToWay2AutomationSite()
        {
            const string signup_css_selector = "div#load_box.popupbox form#load_form a.fancybox[href='#login']";
            const string login_username_css_selector = "div#login.popupbox form#load_form input[name='username']";
            const string login_password_css_selector = "div#login.popupbox form#load_form input[type='password'][name='password']";
            const string login_button_css_selector = "div#login.popupbox form#load_form [value='Submit']";
            string username = "sergueik";
            string password = "i011155";
            Actions actions = new Actions(driver);

            driver.Navigate().GoToUrl(login_url);

            var signup_element = driver.FindElement(By.CssSelector(signup_css_selector));
            actions.MoveToElement(signup_element).Build().Perform();
            highlight(signup_element);
            signup_element.Click();

            var login_username = driver.FindElement(By.CssSelector(login_username_css_selector));
            highlight(login_username);
            login_username.SendKeys(username);

            var login_password_element = driver.FindElement(By.CssSelector(login_password_css_selector));
            highlight(signup_element);
            login_password_element.SendKeys(password);

            var login_button_element = driver.FindElement(By.CssSelector(login_button_css_selector));
            actions.MoveToElement(login_button_element).Build().Perform();
            highlight(login_button_element);
            login_button_element.Click();
            driver.Navigate().GoToUrl(base_url);
            ngDriver.Url = driver.Url;
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            try
            {
                driver.Close();
                driver.Quit();
            }
            catch (Exception) { } /* Ignore cleanup errors */
            Assert.IsEmpty(verificationErrors.ToString());
        }

        [Test]
        public void ShouldFindBankManagerLoginButton()
        {
            NgWebElement ng_login_button_element = ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login"));
            StringAssert.IsMatch("Bank Manager Login", ng_login_button_element.Text);
        }

        [Test]
        public void ShouldFindCustomersButton()
        {
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            NgWebElement ng_customers_button_element = ngDriver.FindElement(NgBy.PartialButtonText("Customers"));
            StringAssert.IsMatch("Customers", ng_customers_button_element.Text);
        }

        [Test]
        public void ShouldFindAddCustomerForm()
        {
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            NgWebElement ng_add_customer_button_element = ngDriver.FindElement(NgBy.PartialButtonText("Add Customer"));
            StringAssert.IsMatch("Add Customer", ng_add_customer_button_element.Text);
            ng_add_customer_button_element.Click();
            IWebElement ng_first_name_element = ngDriver.FindElement(NgBy.Model("fName"));
            highlight(ng_first_name_element);
            StringAssert.IsMatch("First Name", ng_first_name_element.GetAttribute("placeholder"));
            IWebElement ng_last_name_element = ngDriver.FindElement(NgBy.Model("lName"));
            highlight(ng_last_name_element);
            StringAssert.IsMatch("Last Name", ng_last_name_element.GetAttribute("placeholder"));
            IWebElement ng_post_code_element = ngDriver.FindElement(NgBy.Model("postCd"));
            highlight(ng_post_code_element);
            StringAssert.IsMatch("Post Code", ng_post_code_element.GetAttribute("placeholder"));
            NgWebElement ng_add_dustomer_button_element = ngDriver.FindElement(NgBy.PartialButtonText("Add Customer"));
            highlight(ng_add_dustomer_button_element);
            StringAssert.IsMatch("Add Customer", ng_add_customer_button_element.Text);
        }

        [Test]
        public void ShouldAddCustomer()
        {
            // switch to "Add Customer" screen
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Add Customer")).Click();
            // fill new Customer data 
            ngDriver.FindElement(NgBy.Model("fName")).SendKeys("John");
            ngDriver.FindElement(NgBy.Model("lName")).SendKeys("Doe");
            ngDriver.FindElement(NgBy.Model("postCd")).SendKeys("11011");
            // NOTE: there are two 'Add Customer' buttons on this form
            NgWebElement ng_add_dustomer_button_element = ngDriver.FindElements(NgBy.PartialButtonText("Add Customer"))[1];
            Actions actions = new Actions(ngDriver.WrappedDriver);
            actions.MoveToElement(ng_add_dustomer_button_element.WrappedElement).Build().Perform();
            highlight(ng_add_dustomer_button_element.WrappedElement);
            ng_add_dustomer_button_element.WrappedElement.Submit();
            // confirm
            ngDriver.WrappedDriver.SwitchTo().Alert().Accept();
            // switch to "Customers" screen
            ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();

            // find customers
            ReadOnlyCollection<NgWebElement> ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            // discover newly added customer 
            var ng_customers_enumerator = ng_customers.GetEnumerator();
            var status = false;
            ng_customers_enumerator.Reset();
            while (ng_customers_enumerator.MoveNext())
            {
                NgWebElement ng_customer = (NgWebElement)ng_customers_enumerator.Current;
                if (Regex.IsMatch(ng_customer.Text, "John Doe.*"))
                {
                    status = true;
                }
            }
            Assert.IsTrue(status);
        }

        [Test]
        public void ShouldShowCustomersAccounts()
        {
            string cust_repeater = "cust in Customers";
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();
            ReadOnlyCollection<NgWebElement> ng_accounts = ngDriver.FindElements(NgBy.Repeater(cust_repeater));
            Assert.IsTrue(ng_accounts[0].Displayed);
            StringAssert.Contains("Granger", ng_accounts[0].Text);
        }

        public void highlight(IWebElement element, int px = 3, string color = "yellow")
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].style.border='" + px + "px solid " + color + "'", element);
        }
    }
}
