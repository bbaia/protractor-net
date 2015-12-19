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
using System.Linq;

namespace Protractor.Test
{

    [TestFixture]
    public class Way2AutomationTests
    {
        private StringBuilder verificationErrors = new StringBuilder();
        private IWebDriver driver;
        private NgWebDriver ngDriver;
        private WebDriverWait wait;
        private IAlert alert;
        private string alert_text;
        private Regex theReg;
        private MatchCollection theMatches;
        private Match theMatch;
        private Capture theCapture;
        private int wait_seconds = 3;
        private int highlight_timeout = 100;
        private Actions actions;
        private String base_url = "http://www.way2automation.com/angularjs-protractor/banking";

        [TestFixtureSetUp]
        public void SetUp()
        {
            // driver = new PhantomJSDriver();
            driver = new FirefoxDriver();
            //driver = new ChromeDriver();
            //var options = new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true };
            //driver = new InternetExplorerDriver(options);

            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(60));
            ngDriver = new NgWebDriver(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(wait_seconds));
            actions = new Actions(driver);
        }

        [SetUp]
        public void NavigateToBankingExamplePage()
        {
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
        public void ShouldLoginCustomer()
        {

            NgWebElement ng_customer_login_button_element = ngDriver.FindElement(NgBy.ButtonText("Customer Login"));
            StringAssert.IsMatch("Customer Login", ng_customer_login_button_element.Text);
            highlight(ng_customer_login_button_element);
            ng_customer_login_button_element.Click();
            NgWebElement ng_user_select_element = ngDriver.FindElement(NgBy.Model("custId"));
            StringAssert.IsMatch("userSelect", ng_user_select_element.WrappedElement.GetAttribute("id"));
            ReadOnlyCollection<NgWebElement> ng_customers = ng_user_select_element.FindElements(NgBy.Repeater("cust in Customers"));
            // select customer to log in
            NgWebElement first_customer = ng_customers.First();
            Assert.IsTrue(first_customer.Displayed);
            StringAssert.Contains("Granger", first_customer.Text);
            first_customer.WrappedElement.Click();
            NgWebElement ng_login_button_element = ngDriver.FindElement(NgBy.ButtonText("Login"));
            // login button
            Assert.IsTrue(ng_login_button_element.WrappedElement.Displayed);
            Assert.IsTrue(ng_login_button_element.WrappedElement.Enabled);
            highlight(ng_login_button_element);
            ng_login_button_element.Click();
            // use core Selenium 
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div strong span.fontBig[class*='ng-binding']")));
            IWebElement greeting_elemment = ngDriver.WrappedDriver.FindElement(By.CssSelector("div strong span.fontBig[class*='ng-binding']"));
            StringAssert.IsMatch("Hermoine Granger", greeting_elemment.Text);
            highlight(greeting_elemment);
        }

        [Test]
        public void ShouldAddCustomer()
        {
            // switch to "Add Customer" screen
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Add Customer")).Click();

            // fill new Customer data            
            IWebElement ng_first_name_element = ngDriver.FindElement(NgBy.Model("fName"));
            highlight(ng_first_name_element);
            StringAssert.IsMatch("First Name", ng_first_name_element.GetAttribute("placeholder"));
            ng_first_name_element.SendKeys("John");

            IWebElement ng_last_name_element = ngDriver.FindElement(NgBy.Model("lName"));
            highlight(ng_last_name_element);
            StringAssert.IsMatch("Last Name", ng_last_name_element.GetAttribute("placeholder"));
            ng_last_name_element.SendKeys("Doe");


            IWebElement ng_post_code_element = ngDriver.FindElement(NgBy.Model("postCd"));
            highlight(ng_post_code_element);
            StringAssert.IsMatch("Post Code", ng_post_code_element.GetAttribute("placeholder"));
            ng_post_code_element.SendKeys("11011");

            // NOTE: there are two 'Add Customer' buttons on this form
            NgWebElement ng_add_dustomer_button_element = ngDriver.FindElements(NgBy.PartialButtonText("Add Customer"))[1];
            actions.MoveToElement(ng_add_dustomer_button_element.WrappedElement).Build().Perform();
            highlight(ng_add_dustomer_button_element.WrappedElement);
            ng_add_dustomer_button_element.WrappedElement.Submit();

            // confirm
            try
            {
                ngDriver.WrappedDriver.SwitchTo().Alert().Accept();
                // switch to "Customers" screen
                ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();

                // customers
                ReadOnlyCollection<NgWebElement> ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
                // discover newly added customer            
                NgWebElement newly_added_customer = ng_customers.First(cust => Regex.IsMatch(cust.Text, "John Doe.*"));
                Assert.IsNotNull(newly_added_customer);
            }
            catch (NoAlertPresentException ex)
            {
                // Alert not present
                verificationErrors.Append(ex.StackTrace);
            }
            catch (WebDriverException ex)
            {
                // Alert not handled by PhantomJS
                verificationErrors.Append(ex.StackTrace);
            }
        }

        [Test]
        public void ShouldDeleteCustomer()
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
            actions.MoveToElement(ng_add_dustomer_button_element.WrappedElement).Build().Perform();
            highlight(ng_add_dustomer_button_element.WrappedElement);
            ng_add_dustomer_button_element.WrappedElement.Submit();
            // confirm
            ngDriver.WrappedDriver.SwitchTo().Alert().Accept();
            // switch to "Home" screen

            ngDriver.FindElement(NgBy.ButtonText("Home")).Click();
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();
            // customers
            ReadOnlyCollection<NgWebElement> ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));

            NgWebElement newly_added_customer = ng_customers.Single(cust => Regex.IsMatch(cust.Text, "John Doe.*"));

            Assert.IsNotNull(newly_added_customer);
            NgWebElement ng_delete_customer_button_element = newly_added_customer.FindElement(NgBy.ButtonText("Delete"));
            StringAssert.IsMatch("Delete", ng_delete_customer_button_element.Text);
            actions.MoveToElement(ng_delete_customer_button_element.WrappedElement).Build().Perform();
            ng_delete_customer_button_element.Click();
            ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            IEnumerable<NgWebElement> removed_customer = ng_customers.TakeWhile(cust => Regex.IsMatch(cust.Text, "John Doe.*"));
            Assert.IsEmpty(removed_customer);

        }
        [Test]
        public void ShouldOpenAccount()
        {
            // switch to "Add Customer" screen
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Open Account")).Click();
            
            // fill new Account data
            NgWebElement ng_customer_select_element = ngDriver.FindElement(NgBy.Model("custId"));
            StringAssert.IsMatch("userSelect", ng_customer_select_element.WrappedElement.GetAttribute("id"));
            ReadOnlyCollection<NgWebElement> ng_customers = ng_customer_select_element.FindElements(NgBy.Repeater("cust in Customers"));
            
            // select customer to log in
            NgWebElement account_customer = ng_customers.First(cust => Regex.IsMatch(cust.Text, "Harry Potter*"));
            Assert.IsNotNull(account_customer);
            account_customer.Click();
            
            
            NgWebElement ng_currencies_select_element = ngDriver.FindElement(NgBy.Model("currency"));
            // use core Selenium
            SelectElement currencies_select_element = new SelectElement(ng_currencies_select_element.WrappedElement);
            IList<IWebElement> account_currencies = currencies_select_element.Options;
            IWebElement account_currency = account_currencies.First(cust => Regex.IsMatch(cust.Text, "Dollar"));
            Assert.IsNotNull(account_currency);            
            currencies_select_element.SelectByText("Dollar");
            
            // add the account
            var submit_button_element = ngDriver.WrappedDriver.FindElement(By.XPath("/html/body//form/button[@type='submit']"));
            StringAssert.IsMatch("Process", submit_button_element.Text);
            submit_button_element.Click();

            try
            {
                alert = driver.SwitchTo().Alert();
                alert_text = alert.Text;
                StringAssert.StartsWith("Account created successfully with account Number", alert_text);
                alert.Accept();
            }
            catch (NoAlertPresentException ex)
            {
                // Alert not present
                verificationErrors.Append(ex.StackTrace);
            }
            catch (WebDriverException ex)
            {
                // Alert not handled by PhantomJS
                verificationErrors.Append(ex.StackTrace);
            }

            // Confirm account added for customer
            Assert.IsEmpty(verificationErrors.ToString());

            // switch to "Customers" screen
            ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();

            // customers
            ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            // discover customer            
            NgWebElement ng_customer_element = ng_customers.First(cust => Regex.IsMatch(cust.Text, "Harry Potter.*"));
            Assert.IsNotNull(ng_customer_element);

            // extract the account id from the alert message            
            string account_id = "";
            theReg = new Regex(@"(?<account_id>\d+)$");
            theMatches = theReg.Matches(alert_text);
            foreach (Match theMatch in theMatches)
            {
                if (theMatch.Length != 0)
                {

                    foreach (Capture theCapture in theMatch.Groups["account_id"].Captures)
                    {
                        account_id = theCapture.ToString();
                    }
                }
            }
            Assert.IsNotNullOrEmpty(account_id);
            // search accounts of specific customer
            ReadOnlyCollection<NgWebElement> ng_customer_accounts = ng_customer_element.FindElements(NgBy.Repeater("account in cust.accountNo"));

            NgWebElement account_matching = ng_customer_accounts.First(acc => String.Equals(acc.Text, account_id));
            Assert.IsNotNull(account_matching);
            highlight(account_matching.WrappedElement);
        }

        [Test]
        public void ShouldSortCustomersAccounts()
        {
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();
            // core Selenium
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("a[ng-click*='sortType'][ng-click*= 'fName']")));
            IWebElement sort_first_name_element = ngDriver.WrappedDriver.FindElement(By.CssSelector("a[ng-click*='sortType'][ng-click*= 'fName']"));
            StringAssert.Contains("First Name", sort_first_name_element.Text);
            highlight(sort_first_name_element);
            sort_first_name_element.Click();

            ReadOnlyCollection<NgWebElement> ng_accounts = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            NgWebElement first_customer = ng_accounts.First();
            StringAssert.Contains("Ron", first_customer.Text);
            sort_first_name_element.Click();

            ng_accounts = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            first_customer = ng_accounts.First();
            StringAssert.Contains("Albus", first_customer.Text);
        }

        public void highlight(IWebElement element, int px = 3, string color = "yellow")
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].style.border='" + px + "px solid " + color + "'", element);
            Thread.Sleep(highlight_timeout);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].style.border=''", element);
        }
    }
}
