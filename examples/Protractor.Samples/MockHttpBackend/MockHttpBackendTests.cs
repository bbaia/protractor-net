using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using Protractor.Samples.MockHttpBackend.Support;

namespace Protractor.Samples.MockHttpBackend
{
    /*
     * E2E testing against the AngularJS tutorial Step 5 sample: 
     * http://docs.angularjs.org/tutorial/step_05
     */
    [TestFixture]
    public class MockHttpBackendTests
    {
        private IWebDriver driver;
        private IWebDriver ngDriver;

        [FindsBy(How = How.Custom, CustomFinderType = typeof(Repeater), Using = "phone in phones")]
        private IList<IWebElement> phonesList;

        [SetUp]
        public void SetUp()
        {
            // Using NuGet Package 'PhantomJS'
            driver = new PhantomJSDriver();

            // Using NuGet Package 'WebDriver.ChromeDriver.win32'
            //driver = new ChromeDriver("C:\\Drivers");
            ngDriver = new NgWebDriver(driver);

            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));

            //Initializing the page factory
            OpenQA.Selenium.Support.PageObjects.PageFactory.InitElements(ngDriver, this);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test(Description = "Should filter the phone list as user types into the search box")]
        public void ShouldFilter()
        {
            // Fake backend with 2 phones
            NgMockE2EModule mockModule = new NgMockE2EModule(@"
$httpBackend.whenGET('phones/phones.json').respond(
[
    {
        age: 12, 
        carrier: 'AT&amp;T', 
        id: 'motorola-bravo-with-motoblur', 
        imageUrl: 'img/phones/motorola-bravo-with-motoblur.0.jpg', 
        name: 'MOTOROLA BRAVO\u2122 with MOTOBLUR\u2122', 
        snippet: 'An experience to cheer about.'
    }, 
    {
        age: 13, 
        carrier: 'T-Mobile', 
        id: 'motorola-defy-with-motoblur', 
        imageUrl: 'img/phones/motorola-defy-with-motoblur.0.jpg', 
        name: 'Motorola DEFY\u2122 with MOTOBLUR\u2122', 
        snippet: 'Are you ready for everything life throws your way?'
    }, 
]
);
");
            ngDriver.Navigate().GoToUrl("http://angular.github.io/angular-phonecat/step-5/app/");
            Assert.AreEqual(20, ngDriver.FindElements(NgBy.Repeater("phone in phones")).Count);
            ngDriver.FindElement(NgBy.Input("query")).SendKeys("bravo");
            Assert.AreEqual(1, ngDriver.FindElements(NgBy.Repeater("phone in phones")).Count);
            ngDriver.FindElement(NgBy.Input("query")).SendKeys("!");
            Assert.AreEqual(0, ngDriver.FindElements(NgBy.Repeater("phone in phones")).Count);
        }

        [Test(Description = "Should filter the phone list as user types into the search box")]
        public void ShouldFilterByPageFactory()
        {
            // Fake backend with 2 phones
            NgMockE2EModule mockModule = new NgMockE2EModule(@"
$httpBackend.whenGET('phones/phones.json').respond(
[
    {
        age: 12, 
        carrier: 'AT&amp;T', 
        id: 'motorola-bravo-with-motoblur', 
        imageUrl: 'img/phones/motorola-bravo-with-motoblur.0.jpg', 
        name: 'MOTOROLA BRAVO\u2122 with MOTOBLUR\u2122', 
        snippet: 'An experience to cheer about.'
    }, 
    {
        age: 13, 
        carrier: 'T-Mobile', 
        id: 'motorola-defy-with-motoblur', 
        imageUrl: 'img/phones/motorola-defy-with-motoblur.0.jpg', 
        name: 'Motorola DEFY\u2122 with MOTOBLUR\u2122', 
        snippet: 'Are you ready for everything life throws your way?'
    }, 
]
);
");
            ngDriver.Navigate().GoToUrl("http://angular.github.io/angular-phonecat/step-5/app/");
            Assert.AreEqual(20, phonesList.Count);
            ngDriver.FindElement(NgBy.Input("query")).SendKeys("bravo");
            Assert.AreEqual(1, phonesList.Count);
            ngDriver.FindElement(NgBy.Input("query")).SendKeys("!");
            Assert.AreEqual(0, phonesList.Count);
        }
    }
}
