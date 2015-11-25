using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace Protractor.Test
{
    [TestFixture]
    public class BasicTests
    {
        private StringBuilder verificationErrors = new StringBuilder();
        private IWebDriver driver;
        private NgWebDriver ngDriver;
        private String base_url = "http://www.angularjs.org";

        
                [SetUp]
        public void SetUp()
        {
            driver = new PhantomJSDriver();
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));
        
            // Using NuGet Package 'WebDriver.ChromeDriver.win32'
            //driver = new ChromeDriver();

            // Using NuGet Package 'WebDriver.IEDriverServer.win32'
            //var options = new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true };
            //driver = new InternetExplorerDriver(options);

            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));
            ngDriver = new NgWebDriver(driver);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception){} /* Ignore cleanup errors */
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void ShouldWaitForAngular()
        {
            ngDriver.Navigate().GoToUrl(base_url);
            IWebElement element = ngDriver.FindElement(NgBy.Model("yourName"));
            Assert.IsTrue(((NgWebElement)element).Displayed);
        }

        [Test]
        public void ShouldSetLocation()
        {
        	ngDriver.Navigate().GoToUrl(base_url );
        	String loc = "misc/faq";
        	NgNavigation nav = new NgNavigation(ngDriver, ngDriver.Navigate());
            nav.SetLocation(null,loc);
            Assert.IsTrue(ngDriver.Url.ToString().Contains(loc));
        }

        
        [Test]
        public void ShouldGreetUsingBinding()
        {
            IWebDriver ngDriver = new NgWebDriver(driver);
            ngDriver.Navigate().GoToUrl(base_url );
            ngDriver.FindElement(NgBy.Model("yourName")).SendKeys("Julie");
            Assert.AreEqual("Hello Julie!", ngDriver.FindElement(NgBy.Binding("yourName")).Text);
        }
        
        /*
        [Test]
        public void ShouldTestForAngular()
        {
            var ngDriver = new NgWebDriver(driver);
            object isAngularApp =  ngDriver.jsExecutor.ExecuteAsyncScript(ClientSideScripts.TestForAngular, 100);
            Assert.AreEqual(true, isAngularApp);
        }
        */
       
        [Test]
        public void ShouldListTodos()
        {
            var ngDriver = new NgWebDriver(driver);
            ngDriver.Navigate().GoToUrl("http://www.angularjs.org");
            var elements = ngDriver.FindElements(NgBy.Repeater("todo in todoList.todos"));
            Assert.AreEqual("build an angular app", elements[1].Text);
            Assert.AreEqual(false, elements[1].Evaluate("todo.done"));
        }
    }
}
