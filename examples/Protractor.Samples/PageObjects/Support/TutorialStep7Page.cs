using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Protractor.Samples.PageObjects.Support
{
    /*
     * Page Object that represents the the AngularJS tutorial Step 7 page: 
     * http://docs.angularjs.org/tutorial/step_07
     */
    public class TutorialStep7Page
    {
        [FindsBy(How = How.Custom, CustomFinderType = typeof(NgByModel), Using = "$ctrl.query")]
        public IWebElement QueryInput { get; set; }

        [FindsBy(How = How.Custom, CustomFinderType = typeof(NgByModel), Using = "$ctrl.orderProp")]
        public IWebElement SortBySelect { get; set; }

        NgWebDriver ngDriver;

        public TutorialStep7Page(IWebDriver driver, string url)
        {
            ngDriver = new NgWebDriver(driver);
            PageFactory.InitElements(ngDriver, this);

            ngDriver.Navigate().GoToUrl(url);
        }

        public TutorialStep7Page SearchFor(string query)
        {
            QueryInput.Clear();
            QueryInput.SendKeys(query);
            return this;
        }

        public TutorialStep7Page SortByName()
        {
            // Alternative: Use OpenQA.Selenium.Support.UI.SelectElement from Selenium.Support package
            SortBySelect
                .FindElement(By.XPath("//option[@value='name']"))
                .Click();
            return this;
        }

        public TutorialStep7Page SortByAge()
        {
            // Alternative: Use OpenQA.Selenium.Support.UI.SelectElement from Selenium.Support package
            SortBySelect
                .FindElement(By.XPath("//option[@value='age']"))
                .Click();
            return this;
        }

        public int GetResultsCount()
        {
            return ngDriver.FindElements(NgBy.Repeater("phone in $ctrl.phones")).Count;
        }

        public string GetResultsPhoneName(int index)
        {
            return ngDriver.FindElements(NgBy.Repeater("phone in $ctrl.phones"))[index].Evaluate("phone.name") as string;
        }
    }
}
