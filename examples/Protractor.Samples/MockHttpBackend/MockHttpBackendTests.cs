using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

using Protractor.Samples.MockHttpBackend.Support;

namespace Protractor.Samples.MockHttpBackend
{
    /*
     * E2E testing against the AngularJS tutorial Step 7 sample: 
     * http://docs.angularjs.org/tutorial/step_07
     */
    [TestFixture]
    public class MockHttpBackendTests
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

        [Test(Description = "Should filter the phone list as user types into the search box")]
        public void ShouldFilter()
        {
            // Fake backend with 2 phones
            NgMockE2EModule mockModule = new NgMockE2EModule(@"
// Requests for templates are handled by the real server
$httpBackend.whenGET('phone-list/phone-list.template.html').passThrough();

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
            var ngDriver = new NgWebDriver(driver, mockModule);
            ngDriver.Navigate().GoToUrl("http://angular.github.io/angular-phonecat/step-7/app/");
            Assert.AreEqual(2, ngDriver.FindElements(NgBy.Repeater("phone in $ctrl.phones")).Count);
            ngDriver.FindElement(NgBy.Model("$ctrl.query")).SendKeys("bravo");
            Assert.AreEqual(1, ngDriver.FindElements(NgBy.Repeater("phone in $ctrl.phones")).Count);
            ngDriver.FindElement(NgBy.Model("$ctrl.query")).SendKeys("!");
            Assert.AreEqual(0, ngDriver.FindElements(NgBy.Repeater("phone in $ctrl.phones")).Count);
        }
    }
}
