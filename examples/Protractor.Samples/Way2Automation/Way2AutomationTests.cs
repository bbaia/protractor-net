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
using Protractor.Extensions;

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
        private const int wait_seconds = 3;
        private Actions actions;
        private const String base_url = "http://www.way2automation.com/angularjs-protractor/banking";

        [TestFixtureSetUp]
        public void SetUp()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Size = new System.Drawing.Size(800, 600);
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

        //        [Test]
        //        public void ShouldLogintToWay2AutomationSite()
        //        {
        //            String login_url = "http://way2automation.com/way2auto_jquery/index.php";
        //            string username = "sergueik";
        //            string password = "i011155";
        //
        //            driver.Navigate().GoToUrl(login_url);
        //            // signup
        //            var signup_link = driver.FindElement(By.CssSelector("div#load_box.popupbox form#load_form a.fancybox[href='#login']"));
        //            actions.MoveToElement(signup_link).Build().Perform();
        //            ngDriver.Highlight(signup_link);
        //            signup_link.Click();
        //            // enter username
        //            var login_username = driver.FindElement(By.CssSelector("div#login.popupbox form#load_form input[name='username']"));
        //            ngDriver.Highlight(login_username);
        //            login_username.SendKeys(username);
        //            // enter password
        //            var login_password = driver.FindElement(By.CssSelector("div#login.popupbox form#load_form input[type='password'][name='password']"));
        //            ngDriver.Highlight(login_password);
        //            login_password.SendKeys(password);
        //            // click "Login"
        //            actions.MoveToElement(driver.FindElement(By.CssSelector("div#login.popupbox form#load_form [value='Submit']"))).Click().Build().Perform();
        //            // block until the login popup box disappears
        //            wait.Until(d => (d.FindElements(By.CssSelector("div#login.popupbox")).Count == 0));
        //        }

        [Test]
        public void ShouldDeposit()
        {
            ngDriver.FindElement(NgBy.ButtonText("Customer Login")).Click();
            ReadOnlyCollection<NgWebElement> ng_customers = ngDriver.FindElement(NgBy.Model("custId")).FindElements(NgBy.Repeater("cust in Customers"));
            // select customer to log in
            ng_customers.First(cust => Regex.IsMatch(cust.Text, "Harry Potter")).Click();

            ngDriver.FindElement(NgBy.ButtonText("Login")).Click();
            ngDriver.FindElement(NgBy.Options("account for account in Accounts")).Click();

            // inspect the account
            NgWebElement ng_account_number = ngDriver.FindElement(NgBy.Binding("accountNo"));
            int account_id = 0;
            int.TryParse(ng_account_number.Text.FindMatch(@"(?<account_number>\d+)$"), out account_id);
            Assert.AreNotEqual(0, account_id);

            int account_balance = -1;
            int.TryParse(ngDriver.FindElement(NgBy.Binding("amount")).Text.FindMatch(@"(?<account_balance>\d+)$"), out account_balance);
            Assert.AreNotEqual(-1, account_balance);
            NgWebElement ng_deposit_button = ngDriver.FindElement(NgBy.PartialButtonText("Deposit"));
            Assert.IsTrue(ng_deposit_button.Displayed);

            actions.MoveToElement(ng_deposit_button.WrappedElement).Build().Perform();
            Thread.Sleep(500);
            ng_deposit_button.Click();

            // core Selenium
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("form[name='myForm']")));
            NgWebElement ng_form_element = new NgWebElement(ngDriver, driver.FindElement(By.CssSelector("form[name='myForm']")));

            // deposit amount
            NgWebElement ng_deposit_amount = ng_form_element.FindElement(NgBy.Model("amount"));
            ng_deposit_amount.SendKeys("100");

            wait.Until(ExpectedConditions.ElementIsVisible(NgBy.ButtonText("Deposit")));
            ng_deposit_button = ng_form_element.FindElement(NgBy.ButtonText("Deposit"));
            actions.MoveToElement(ng_deposit_button.WrappedElement).Build().Perform();
            ngDriver.Highlight(ng_deposit_button);

            ng_deposit_button.Click();
            // http://www.way2automation.com/angularjs-protractor/banking/depositTx.html

            // inspect message
            var ng_message = ngDriver.FindElement(NgBy.Binding("message"));
            StringAssert.Contains("Deposit Successful", ng_message.Text);
            ngDriver.Highlight(ng_message);

            // re-read the amount
            int updated_account_balance = -1;
            int.TryParse(ngDriver.FindElement(NgBy.Binding("amount")).Text.FindMatch(@"(?<account_balance>\d+)$"), out updated_account_balance);
            Assert.AreEqual(updated_account_balance, account_balance + 100);
        }


        [Test]
        public void ShouldWithdraw()
        {
            ShouldDeposit();
            int account_balance = -1;
            int.TryParse(ngDriver.FindElement(NgBy.Binding("amount")).Text.FindMatch(@"(?<account_balance>\d+)$"), out account_balance);
            Assert.AreNotEqual(-1, account_balance);

            ngDriver.FindElement(NgBy.PartialButtonText("Withdrawl")).Click();

            // core Selenium
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("form[name='myForm']")));
            NgWebElement ng_form_element = new NgWebElement(ngDriver, driver.FindElement(By.CssSelector("form[name='myForm']")));
            NgWebElement ng_withdrawl_amount = ng_form_element.FindElement(NgBy.Model("amount"));
            ng_withdrawl_amount.SendKeys((account_balance + 100).ToString());

            NgWebElement ng_withdrawl_button = ng_form_element.FindElement(NgBy.ButtonText("Withdraw"));
            ngDriver.Highlight(ng_withdrawl_button);
            ng_withdrawl_button.Click();

            // inspect message
            var ng_message = ngDriver.FindElement(NgBy.Binding("message"));
            StringAssert.Contains("Transaction Failed. You can not withdraw amount more than the balance.", ng_message.Text);
            ngDriver.Highlight(ng_message);

            // re-read the amount
            int updated_account_balance = -1;
            int.TryParse(ngDriver.FindElement(NgBy.Binding("amount")).Text.FindMatch(@"(?<account_balance>\d+)$"), out updated_account_balance);
            Assert.AreEqual(account_balance, updated_account_balance);

            // core Selenium
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("form[name='myForm']")));
            ng_form_element = new NgWebElement(ngDriver, driver.FindElement(By.CssSelector("form[name='myForm']")));

            ng_form_element.FindElement(NgBy.Model("amount")).SendKeys((account_balance - 10).ToString());
            ng_form_element.FindElement(NgBy.ButtonText("Withdraw")).Click();
            // inspect message
            ng_message = ngDriver.FindElement(NgBy.Binding("message"));
            StringAssert.Contains("Transaction successful", ng_message.Text);
            ngDriver.Highlight(ng_message);

            // re-read the amount
            int.TryParse(ngDriver.FindElement(NgBy.Binding("amount")).Text.FindMatch(@"(?<account_balance>\d+)$"), out updated_account_balance);
            Assert.AreEqual(10, updated_account_balance);

        }

        [Test]
        public void ShouldLoginCustomer()
        {
            NgWebElement ng_customer_login_button = ngDriver.FindElement(NgBy.ButtonText("Customer Login"));
            StringAssert.IsMatch("Customer Login", ng_customer_login_button.Text);
            ngDriver.Highlight(ng_customer_login_button);
            // core Selenium
            IWebElement customer_login_button = driver.FindElement(By.XPath("//button[contains(.,'Customer Login')]"));
            StringAssert.IsMatch("Customer Login", customer_login_button.Text);
            ngDriver.Highlight(customer_login_button);

            ng_customer_login_button.Click();
            NgWebElement ng_user_select = ngDriver.FindElement(NgBy.Input("custId"));
            StringAssert.IsMatch("userSelect", ng_user_select.GetAttribute("id"));
            ReadOnlyCollection<NgWebElement> ng_customers = ng_user_select.FindElements(NgBy.Repeater("cust in Customers"));
            Assert.AreNotEqual(0, ng_customers.Count);
            // pick a customer
            NgWebElement first_customer = ng_customers.First();
            Assert.IsTrue(first_customer.Displayed);

            // the {{user}} is composed from first and last name
            StringAssert.IsMatch("(?:[^ ]+) +(?:[^ ]+)", first_customer.Text);
            string user = first_customer.Text;
            first_customer.Click();

            // login button
            NgWebElement ng_login_button = ngDriver.FindElement(NgBy.ButtonText("Login"));
            Assert.IsTrue(ng_login_button.Displayed && ng_login_button.Enabled);
            ngDriver.Highlight(ng_login_button);
            ng_login_button.Click();

            NgWebElement ng_greeting = ngDriver.FindElement(NgBy.Binding("user"));
            Assert.IsNotNull(ng_greeting);
            StringAssert.IsMatch(user, ng_greeting.Text);
            ngDriver.Highlight(ng_greeting);

            NgWebElement ng_account_number = ngDriver.FindElement(NgBy.Binding("accountNo"));
            Assert.IsNotNull(ng_account_number);
            theReg = new Regex(@"(?<account_id>\d+)$");
            Assert.IsTrue(theReg.IsMatch(ng_account_number.Text));
            ngDriver.Highlight(ng_account_number);

            NgWebElement ng_account_balance = ngDriver.FindElement(NgBy.Binding("amount"));
            Assert.IsNotNull(ng_account_balance);
            theReg = new Regex(@"(?<account_balance>\d+)$");
            Assert.IsTrue(theReg.IsMatch(ng_account_balance.Text));
            ngDriver.Highlight(ng_account_balance);

            NgWebElement ng_account_currency = ngDriver.FindElement(NgBy.Binding("currency"));
            Assert.IsNotNull(ng_account_currency);
            theReg = new Regex(@"(?<account_currency>(?:Dollar|Pound|Rupee))$");
            Assert.IsTrue(theReg.IsMatch(ng_account_currency.Text));
            ngDriver.Highlight(ng_account_currency);
        }

        [Test]
        public void ShouldAddCustomer()
        {
            // switch to "Add Customer" screen
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Add Customer")).Click();

            // fill new Customer data
            IWebElement ng_first_name = ngDriver.FindElement(NgBy.Model("fName"));
            ngDriver.Highlight(ng_first_name);
            StringAssert.IsMatch("First Name", ng_first_name.GetAttribute("placeholder"));
            ng_first_name.SendKeys("John");

            IWebElement ng_last_name = ngDriver.FindElement(NgBy.Model("lName"));
            ngDriver.Highlight(ng_last_name);
            StringAssert.IsMatch("Last Name", ng_last_name.GetAttribute("placeholder"));
            ng_last_name.SendKeys("Doe");

            IWebElement ng_post_code = ngDriver.FindElement(NgBy.Model("postCd"));
            ngDriver.Highlight(ng_post_code);
            StringAssert.IsMatch("Post Code", ng_post_code.GetAttribute("placeholder"));
            ng_post_code.SendKeys("11011");

            // NOTE: there are two 'Add Customer' buttons on this form
            NgWebElement ng_add_customer_button = ngDriver.FindElements(NgBy.PartialButtonText("Add Customer"))[1];
            actions.MoveToElement(ng_add_customer_button.WrappedElement).Build().Perform();
            ngDriver.Highlight(ng_add_customer_button);
            ng_add_customer_button.Submit();

            // confirm
            string alert_text = null;
            try
            {
                alert = ngDriver.WrappedDriver.SwitchTo().Alert();
                alert_text = alert.Text;
                StringAssert.StartsWith("Customer added successfully with customer id :", alert_text);
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

            int customer_id = 0;
            int.TryParse(alert_text.FindMatch(@"(?<customer_id>\d+)$"), out customer_id);
            Assert.AreNotEqual(0, customer_id);

            // switch to "Customers" screen
            ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();

            // discover newly added customer
            ReadOnlyCollection<NgWebElement> ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            int customer_count = ng_customers.Count;
            NgWebElement ng_new_customer = ng_customers.First(cust => Regex.IsMatch(cust.Text, "John Doe"));
            Assert.IsNotNull(ng_new_customer);

            actions.MoveToElement(ng_new_customer.WrappedElement).Build().Perform();
            ngDriver.Highlight(ng_new_customer);

            // confirm searching for the customer name
            ngDriver.FindElement(NgBy.Model("searchCustomer")).SendKeys("John");
            ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            Assert.AreEqual(1, ng_customers.Count);

            // show all customers again
            ngDriver.FindElement(NgBy.Model("searchCustomer")).Clear();

            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(NgBy.Repeater("cust in Customers")));
            // discover newly added customer again
            ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            ng_new_customer = ng_customers.First(cust => Regex.IsMatch(cust.Text, "John Doe"));
            // delete new customer
            NgWebElement ng_delete_button = ng_new_customer.FindElement(NgBy.ButtonText("Delete"));
            Assert.IsNotNull(ng_delete_button);
            actions.MoveToElement(ng_delete_button.WrappedElement).Build().Perform();
            ngDriver.Highlight(ng_delete_button, 1000);
            // in slow motion
            actions.MoveToElement(ng_delete_button.WrappedElement).ClickAndHold().Build().Perform();
            Thread.Sleep(1000);
            actions.Release();
            // sometimes actions do not work - for example in this test 
            ng_delete_button.Click();
            // wait for customer list to reload
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(NgBy.Repeater("cust in Customers")));
            // count the remaining customers            
            ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            int new_customer_count = ng_customers.Count;
            // conrirm the customer count changed
            Assert.IsTrue(customer_count - 1 == new_customer_count);
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
            NgWebElement ng_add_customer_button = ngDriver.FindElements(NgBy.PartialButtonText("Add Customer"))[1];
            actions.MoveToElement(ng_add_customer_button.WrappedElement).Build().Perform();
            ngDriver.Highlight(ng_add_customer_button);
            ng_add_customer_button.Submit();
            // confirm
            ngDriver.WrappedDriver.SwitchTo().Alert().Accept();

            // switch to "Home" screen
            ngDriver.FindElement(NgBy.ButtonText("Home")).Click();
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();

            // found new customer
            ReadOnlyCollection<NgWebElement> ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            // collect all customers
            ReadOnlyCollection<NgWebElement> ng_users = ngDriver.FindElements(NgBy.RepeaterColumn("cust in Customers", "user"));

            NgWebElement new_customer = ng_customers.Single(cust => Regex.IsMatch(cust.Text, "John Doe"));
            Assert.IsNotNull(new_customer);

            // remove button
            NgWebElement ng_delete_customer_button = new_customer.FindElement(NgBy.ButtonText("Delete"));
            StringAssert.IsMatch("Delete", ng_delete_customer_button.Text);
            actions.MoveToElement(ng_delete_customer_button.WrappedElement).Build().Perform();
            ng_delete_customer_button.Click();

            // confirm the cusomer is no loger present
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
            NgWebElement ng_customer_select = ngDriver.FindElement(NgBy.Model("custId"));
            StringAssert.IsMatch("userSelect", ng_customer_select.GetAttribute("id"));
            ReadOnlyCollection<NgWebElement> ng_customers = ng_customer_select.FindElements(NgBy.Repeater("cust in Customers"));

            // select customer to log in
            NgWebElement account_customer = ng_customers.First(cust => Regex.IsMatch(cust.Text, "Harry Potter*"));
            Assert.IsNotNull(account_customer);
            account_customer.Click();

            NgWebElement ng_currencies_select = ngDriver.FindElement(NgBy.Model("currency"));
            // use core Selenium
            SelectElement currencies_select = new SelectElement(ng_currencies_select.WrappedElement);
            IList<IWebElement> account_currencies = currencies_select.Options;
            IWebElement account_currency = account_currencies.First(cust => Regex.IsMatch(cust.Text, "Dollar"));
            Assert.IsNotNull(account_currency);
            currencies_select.SelectByText("Dollar");

            // add the account
            var submit_button = ngDriver.FindElement(By.XPath("/html/body//form/button[@type='submit']"));
            StringAssert.IsMatch("Process", submit_button.Text);
            submit_button.Click();

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

            // get customers
            ng_customers = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            // discover customer
            NgWebElement ng_customer = ng_customers.First(cust => Regex.IsMatch(cust.Text, "Harry Potter"));
            Assert.IsNotNull(ng_customer);

            // extract the account id from the alert message
            string account_id = alert_text.FindMatch(@"(?<account_id>\d+)$");
            Assert.IsNotNullOrEmpty(account_id);
            // search accounts of specific customer
            ReadOnlyCollection<NgWebElement> ng_customer_accounts = ng_customer.FindElements(NgBy.Repeater("account in cust.accountNo"));

            NgWebElement account_matching = ng_customer_accounts.First(acc => String.Equals(acc.Text, account_id));
            Assert.IsNotNull(account_matching);
            ngDriver.Highlight(account_matching);
        }

        [Test]
        public void ShouldSortCustomersAccounts()
        {
            ngDriver.FindElement(NgBy.ButtonText("Bank Manager Login")).Click();
            ngDriver.FindElement(NgBy.PartialButtonText("Customers")).Click();

            wait.Until(ExpectedConditions.ElementExists(NgBy.Repeater("cust in Customers")));
            // alterntive locator using core selenium
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("tr[ng-repeat*='cust in Customers']")));

            IWebElement sort_link = ngDriver.FindElement(By.CssSelector("a[ng-click*='sortType'][ng-click*= 'fName']"));
            StringAssert.Contains("First Name", sort_link.Text);
            ngDriver.Highlight(sort_link);
            sort_link.Click();

            ReadOnlyCollection<NgWebElement> ng_accounts = ngDriver.FindElements(NgBy.Repeater("cust in Customers"));
            // inspect first and last customers
            List<String> ng_account_names = ng_accounts.Select(element => element.Text).ToList();
            String last_customer_name = ng_account_names.FindLast(element => true);
            ngDriver.Highlight(sort_link);
            sort_link.Click();
            // confirm the customers are sorted in reverse order now
            StringAssert.Contains(last_customer_name, ngDriver.FindElements(NgBy.Repeater("cust in Customers")).First().Text);
        }

        [Test]
        public void ShouldEvaluateTransactionDetails()
        {
            ngDriver.FindElement(NgBy.ButtonText("Customer Login")).Click();
            // select customer/account with transactions
            ngDriver.FindElement(NgBy.Model("custId")).FindElements(NgBy.Repeater("cust in Customers")).First(cust => Regex.IsMatch(cust.Text, "Hermoine Granger")).Click();
            ngDriver.FindElement(NgBy.ButtonText("Login")).Click();
            ngDriver.FindElements(NgBy.Options("account for account in Accounts")).First(account => Regex.IsMatch(account.Text, "1001")).Click();

            // switch to transactions
            NgWebElement ng_transaction_button = ngDriver.FindElement(NgBy.PartialButtonText("Transactions"));
            StringAssert.Contains("Transactions", ng_transaction_button.Text);
            ngDriver.Highlight(ng_transaction_button);
            ng_transaction_button.Click();

            // wait for transaction information to be loaded and rendered
            wait.Until(ExpectedConditions.ElementExists(NgBy.Repeater("tx in transactions")));

            // examine first few transactions using Evaluate            
            ReadOnlyCollection<NgWebElement> ng_transactions = ngDriver.FindElements(NgBy.Repeater("tx in transactions"));
            int cnt = 0;
            foreach (NgWebElement ng_current_transaction in ng_transactions)
            {
                if (cnt++ > 5) { break; }
                StringAssert.IsMatch("(?i:credit|debit)", ng_current_transaction.Evaluate("tx.type").ToString());
                StringAssert.IsMatch(@"(?:\d+)", ng_current_transaction.Evaluate("tx.amount").ToString());
                // 'tx.date' is in Javascript UTC format similar to UniversalSorta­bleDateTimePat­tern in C# 
                var transaction_date = ng_current_transaction.Evaluate("tx.date");
                StringAssert.IsMatch(@"(?:\d{4}\-\d{2}\-\d{2}T\d{2}:\d{2}:\d{2}.\d{3}Z)", transaction_date.ToString());
            }
        }

        [Test]
        public void ShouldListTransactions()
        {
            ngDriver.FindElement(NgBy.ButtonText("Customer Login")).Click();
            // select customer/account with transactions
            ngDriver.FindElement(NgBy.Model("custId")).FindElements(NgBy.Repeater("cust in Customers")).First(cust => Regex.IsMatch(cust.Text, "Hermoine Granger")).Click();
            ngDriver.FindElement(NgBy.ButtonText("Login")).Click();
            ngDriver.FindElements(NgBy.Options("account for account in Accounts")).First(account => Regex.IsMatch(account.Text, "1001")).Click();

            // switch to transactions
            NgWebElement ng_transaction_button = ngDriver.FindElement(NgBy.PartialButtonText("Transactions"));
            StringAssert.Contains("Transactions", ng_transaction_button.Text);
            ngDriver.Highlight(ng_transaction_button);
            ng_transaction_button.Click();
            // http://www.way2automation.com/angularjs-protractor/banking/listTx.html

            // wait for transaction information to be loaded and rendered
            wait.Until(ExpectedConditions.ElementExists(NgBy.Repeater("tx in transactions")));

            // highlight transaction type cells in the page differently for Credit or Debit using RepeaterColumn            
            ReadOnlyCollection<NgWebElement> ng_transaction_type_columns = ngDriver.FindElements(NgBy.RepeaterColumn("tx in transactions", "tx.type"));
            Assert.IsNotEmpty(ng_transaction_type_columns);

            foreach (NgWebElement ng_current_transaction_type in ng_transaction_type_columns)
            {
                if (String.IsNullOrEmpty(ng_current_transaction_type.Text))
                {
                    break;
                }
                ngDriver.Highlight(ng_current_transaction_type, 1000, 3, ng_current_transaction_type.Text.Equals("Credit") ? "green" : "blue");
            }
        }
    }
}
