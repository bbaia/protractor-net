using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.PageObjects;

namespace Protractor.Samples.Basic
{
    [TestFixture]
    public class BasicTests
    {
        private IWebDriver driver;
        private IWebDriver ngDriver;

        [FindsBy(How = How.Custom, CustomFinderType = typeof(Input), Using = "yourName")]
        private IWebElement yourNameInput;

        [FindsBy(How = How.Custom, CustomFinderType = typeof(Binding), Using = "yourName")]
        private IWebElement yourNameOutput;

        [FindsBy(How = How.Custom, CustomFinderType = typeof(Repeater), Using = "todo in todos")]
        private IList<IWebElement> todoElements;

        [SetUp]
        public void SetUp()
        {
            // Using NuGet Package 'PhantomJS'
            driver = new PhantomJSDriver();

            // Using NuGet Package 'WebDriver.ChromeDriver.win32'
            //driver = new ChromeDriver();
            ngDriver = new NgWebDriver(driver);
            
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));

            //Initializing the page factory
            PageFactory.InitElements(ngDriver, this);
        }

        [TearDown] 
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void ShouldGreetUsingBinding()
        {
            ngDriver.Navigate().GoToUrl("http://www.angularjs.org");
            ngDriver.FindElement(NgBy.Input("yourName")).SendKeys("Julie");
            Assert.AreEqual("Hello Julie!", ngDriver.FindElement(NgBy.Binding("yourName")).Text);
        }

        [Test]
        public void ShouldListTodos()
        {
            ngDriver.Navigate().GoToUrl("http://www.angularjs.org");
            var elements = ngDriver.FindElements(NgBy.Repeater("todo in todos"));
            Assert.AreEqual("build an angular app", elements[1].Text);
            Assert.AreEqual(false, ((NgWebElement)elements[1]).Evaluate("todo.done"));
        }

        [Test]
        public void ShouldGreetUsingBindingByPageFactory()
        {
            ngDriver.Navigate().GoToUrl("http://www.angularjs.org");
            yourNameInput.SendKeys("Julie");
            Assert.AreEqual("Hello Julie!", yourNameOutput.Text);
        }

        [Test]
        public void ShouldListTodosByPageFactory()
        {
            ngDriver.Navigate().GoToUrl("http://www.angularjs.org");
            Assert.AreEqual("build an angular app", todoElements[1].Text);
            Assert.AreEqual(false, ((NgWebElement)todoElements[1]).Evaluate("todo.done"));
        }
    }
}
