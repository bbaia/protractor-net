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
using System.Drawing;
using System.Windows.Forms;

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
            driver.Manage().Window.Size = new System.Drawing.Size(600, 400);
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
                public void ShouldLogintToWay2AutomationSite()
                {
        
                    String login_url = "http://way2automation.com/way2auto_jquery/index.php";
                    string username = "sergueik";
                    string password = "i011155";
        
                    driver.Navigate().GoToUrl(login_url);
                    // signup
                    var signup_element = driver.FindElement(By.CssSelector("div#load_box.popupbox form#load_form a.fancybox[href='#login']"));
                    actions.MoveToElement(signup_element).Build().Perform();
                    highlight(signup_element);
                    signup_element.Click();
                    // enter username
                    var login_username = driver.FindElement(By.CssSelector("div#login.popupbox form#load_form input[name='username']"));
                    highlight(login_username);
                    login_username.SendKeys(username);
                    // enter password
                    var login_password_element = driver.FindElement(By.CssSelector("div#login.popupbox form#load_form input[type='password'][name='password']"));
                    highlight(signup_element);
                    login_password_element.SendKeys(password);
                    // click "Login"
                    var login_button_element = driver.FindElement(By.CssSelector("div#login.popupbox form#load_form [value='Submit']"));
                    actions.MoveToElement(login_button_element).Build().Perform();
                    highlight(login_button_element);
                    login_button_element.Click();
                }


        [Test]
        public void ShouldDeposit()
        {
            ngDriver.FindElement(NgBy.ButtonText("Customer Login")).Click();
            ReadOnlyCollection<NgWebElement> ng_customers = ngDriver.FindElement(NgBy.Model("custId")).FindElements(NgBy.Repeater("cust in Customers"));
            // select customer to log in
            ng_customers.First(cust => Regex.IsMatch(cust.Text, "Harry Potter")).Click();

            ngDriver.FindElement(NgBy.ButtonText("Login")).Click();
            ngDriver.FindElement(NgBy.Options("account for account in Accounts")).Click();


            NgWebElement ng_account_number_element = ngDriver.FindElement(NgBy.Binding("accountNo"));
            theReg = new Regex(@"(?<account_id>\d+)$");
            int account_id = 0;
            theMatches = theReg.Matches(ng_account_number_element.Text);
            foreach (Match theMatch in theMatches)
            {
                if (theMatch.Length != 0)
                {

                    foreach (Capture theCapture in theMatch.Groups["account_id"].Captures)
                    {
                        int.TryParse(theCapture.ToString(), out account_id);
                    }
                }
            }
            Assert.AreNotEqual(0, account_id);


            NgWebElement ng_account_amount_element = ngDriver.FindElement(NgBy.Binding("amount"));
            int account_amount = -1;
            theReg = new Regex(@"(?<account_amount>\d+)$");
            theMatches = theReg.Matches(ng_account_amount_element.Text);
            foreach (Match theMatch in theMatches)
            {
                if (theMatch.Length != 0)
                {

                    foreach (Capture theCapture in theMatch.Groups["account_amount"].Captures)
                    {
                        int.TryParse(theCapture.ToString(), out account_amount);
                    }
                }
            }
            Assert.AreNotEqual(-1, account_amount);

            ngDriver.FindElement(NgBy.PartialButtonText("Deposit")).Click();

            // core Selenium
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("form[name='myForm']")));
            NgWebElement ng_form_element = new NgWebElement(ngDriver, driver.FindElement(By.CssSelector("form[name='myForm']")));


            NgWebElement ng_deposit_amount_element = ng_form_element.FindElement(NgBy.Model("amount"));
            ng_deposit_amount_element.SendKeys("100");

            NgWebElement ng_deposit_button_element = ng_form_element.FindElement(NgBy.ButtonText("Deposit"));
            highlight(ng_deposit_button_element);
            ng_deposit_button_element.Click();
            // inspect message
            /* 
             view-source:http://www.way2automation.com/angularjs-protractor/banking/depositTx.html
             <span class="error" ng-show="message" >{{message}}</span><br>            
             http://www.way2automation.com/angularjs-protractor/banking/depositController.js
            if (txObj.success) {
                $scope.message = "Deposit Successful";
            } else {
                $scope.message = "Something went wrong. Please try again.";
            }
             */

            var ng_message_element = ngDriver.FindElement(NgBy.Binding("message"));
            StringAssert.Contains("Deposit Successful", ng_message_element.Text);
            highlight(ng_message_element);

            // re-read the amount 
            ng_account_amount_element = ngDriver.FindElement(NgBy.Binding("amount"));
            int updated_account_amount = -1;
            theReg = new Regex(@"(?<account_amount>\d+)$");
            theMatches = theReg.Matches(ng_account_amount_element.Text);
            foreach (Match theMatch in theMatches)
            {
                if (theMatch.Length != 0)
                {

                    foreach (Capture theCapture in theMatch.Groups["account_amount"].Captures)
                    {
                        int.TryParse(theCapture.ToString(), out updated_account_amount);
                    }
                }
            }
            Assert.AreEqual(updated_account_amount, account_amount + 100);
        }

        [Test]
        public void ShouldLoginCustomer()
        {

            NgWebElement ng_customer_login_button_element = ngDriver.FindElement(NgBy.ButtonText("Customer Login"));
            StringAssert.IsMatch("Customer Login", ng_customer_login_button_element.Text);
            highlight(ng_customer_login_button_element);
            // core Selenium
            IWebElement customer_login_button_element = driver.FindElement(By.XPath("//button[contains(.,'Customer Login')]"));
            StringAssert.IsMatch("Customer Login", customer_login_button_element.Text);
            highlight(customer_login_button_element);

            ng_customer_login_button_element.Click();
            NgWebElement ng_user_select_element = ngDriver.FindElement(NgBy.Model("custId"));
            StringAssert.IsMatch("userSelect", ng_user_select_element.GetAttribute("id"));
            ReadOnlyCollection<NgWebElement> ng_customers = ng_user_select_element.FindElements(NgBy.Repeater("cust in Customers"));
            // select customer to log in
            NgWebElement first_customer = ng_customers.First();
            Assert.IsTrue(first_customer.Displayed);
            StringAssert.Contains("Granger", first_customer.Text);
            first_customer.Click();
            NgWebElement ng_login_button_element = ngDriver.FindElement(NgBy.ButtonText("Login"));
            // login button
            Assert.IsTrue(ng_login_button_element.Displayed);
            Assert.IsTrue(ng_login_button_element.Enabled);
            highlight(ng_login_button_element);
            ng_login_button_element.Click();

            NgWebElement ng_greeting_element = ngDriver.FindElement(NgBy.Binding("user"));
            Assert.IsNotNull(ng_greeting_element);
            StringAssert.IsMatch("Hermoine Granger", ng_greeting_element.Text);
            highlight(ng_greeting_element);

            NgWebElement ng_account_number_element = ngDriver.FindElement(NgBy.Binding("accountNo"));
            Assert.IsNotNull(ng_account_number_element);
            theReg = new Regex(@"(?<account_id>\d+)$");
            Assert.IsTrue(theReg.IsMatch(ng_account_number_element.Text));
            highlight(ng_account_number_element);

            NgWebElement ng_account_amount_element = ngDriver.FindElement(NgBy.Binding("amount"));
            Assert.IsNotNull(ng_account_amount_element);
            theReg = new Regex(@"(?<account_amount>\d+)$");
            Assert.IsTrue(theReg.IsMatch(ng_account_amount_element.Text));
            highlight(ng_account_amount_element);

            NgWebElement ng_account_currency_element = ngDriver.FindElement(NgBy.Binding("currency"));
            Assert.IsNotNull(ng_account_currency_element);
            theReg = new Regex(@"(?<account_currency>(?:Dollar|Pound|Rupee))$");
            Assert.IsTrue(theReg.IsMatch(ng_account_currency_element.Text));
            highlight(ng_account_currency_element);
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
            highlight(ng_add_dustomer_button_element);
            ng_add_dustomer_button_element.Submit();

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
            highlight(ng_add_dustomer_button_element);
            ng_add_dustomer_button_element.Submit();
            // confirm
            ngDriver.WrappedDriver.SwitchTo().Alert().Accept();
            // switch to "Home" screen

            ngDriver.FindElement(NgBy.ButtonText("Home")).Click();
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();
            // customers
            ReadOnlyCollection<NgWebElement> ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));

            NgWebElement newly_added_customer = ng_customers.Single(cust => Regex.IsMatch(cust.Text, "John Doe"));

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
            StringAssert.IsMatch("userSelect", ng_customer_select_element.GetAttribute("id"));
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
            NgWebElement ng_customer_element = ng_customers.First(cust => Regex.IsMatch(cust.Text, "Harry Potter"));
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
            highlight(account_matching);
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
