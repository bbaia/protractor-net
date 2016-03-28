using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

using Protractor.Samples.PageObjects.Support;

namespace Protractor.Samples.PageObjects
{
    /*
     * E2E testing against the AngularJS tutorial Step 5 sample: 
     * http://docs.angularjs.org/tutorial/step_05
     */
    [TestFixture]
    public class PageObjectsTests
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            // Using PhantomJS
            driver = new PhantomJSDriver();

            // Using Chrome
            //driver = new ChromeDriver();

            // Using Internet Explorer
            //var options = new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true };
            //driver = new InternetExplorerDriver(options);

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
            var step5Page = new TutorialStep5Page(driver, "http://angular.github.io/angular-phonecat/step-5/app/");

            Assert.AreEqual(20, step5Page.GetResultsCount());

            step5Page.SearchFor("Motorola");
            Assert.AreEqual(8, step5Page.GetResultsCount());

            step5Page.SearchFor("Nexus");
            Assert.AreEqual(1, step5Page.GetResultsCount());
        }

        [Test(Description = "Should be possible to control phone order via the drop down select box")]
        public void ShouldSort()
        {
            var step5Page = new TutorialStep5Page(driver, "http://angular.github.io/angular-phonecat/step-5/app/");

            step5Page.SearchFor("tablet");
            Assert.AreEqual(2, step5Page.GetResultsCount());

            step5Page.SortByAge();
            Assert.AreEqual("Motorola XOOM™ with Wi-Fi", step5Page.GetResultsPhoneName(0));
            Assert.AreEqual("MOTOROLA XOOM™", step5Page.GetResultsPhoneName(1));

            step5Page.SortByName();
            Assert.AreEqual("MOTOROLA XOOM™", step5Page.GetResultsPhoneName(0));
            Assert.AreEqual("Motorola XOOM™ with Wi-Fi", step5Page.GetResultsPhoneName(1));
        }
    }
}
