using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Protractor.Samples.Basic
{
    [TestFixture]
    public class BasicTests
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            // Using PhantomJS
            //driver = new PhantomJSDriver();

            // Using Chrome
            driver = new ChromeDriver();

            // Using Internet Explorer
            //var options = new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true };
            //driver = new InternetExplorerDriver(options);

            // Using Microsoft Edge
            //driver = new EdgeDriver();

            // Using Firefox
            //driver = new FirefoxDriver();

            // Required for TestForAngular and WaitForAngular scripts
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void ShouldGreetUsingBinding()
        {
            var ngDriver = new NgWebDriver(driver);
            ngDriver.Navigate().GoToUrl("http://www.angularjs.org");
            ngDriver.FindElement(NgBy.Model("yourName")).SendKeys("Julie");
            Assert.AreEqual("Hello Julie!", ngDriver.FindElement(NgBy.Binding("yourName")).Text);
        }

        [Test]
        public void ShouldListTodos()
        {
            var ngDriver = new NgWebDriver(driver);
            ngDriver.Navigate().GoToUrl("http://www.angularjs.org");
            var elements = ngDriver.FindElements(NgBy.Repeater("todo in todoList.todos"));
            Assert.AreEqual("build an AngularJS app", elements[1].Text.Trim());
            Assert.AreEqual(false, elements[1].Evaluate("todo.done"));
        }

        [Test]
        public void Angular2Test()
        {
            var ngDriver = new NgWebDriver(driver);
            ngDriver.Navigate().GoToUrl("https://material.angular.io/");
            ngDriver.FindElement(By.XPath("//a[@routerlink='guide/getting-started']")).Click();
            Assert.AreEqual("https://material.angular.io/guide/getting-started", ngDriver.Url);
        }

        [Test]
        public void NonAngularPageShouldBeSupported()
        {
            Assert.DoesNotThrow(() =>
            {
                var ngDriver = new NgWebDriver(driver);
                ngDriver.IgnoreSynchronization = true;
                ngDriver.Navigate().GoToUrl("http://www.google.com");
                ngDriver.IgnoreSynchronization = false;
            });
        }
    }
}
