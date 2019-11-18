using System.Collections.Generic;
using OpenQA.Selenium;

namespace Protractor.Samples.PageObjects.Support
{
    /*
     * Page Object that represents the the AngularJS tutorial Step 7 page: 
     * http://docs.angularjs.org/tutorial/step_07
     */
    public class TutorialStep7Page
    {
        public IWebElement QueryInput
        {
            get { return this.ngDriver.FindElement(NgBy.Model("$ctrl.query")); }
        }

        public IWebElement SortBySelect
        {
            get { return this.ngDriver.FindElement(NgBy.Model("$ctrl.orderProp")); }
        }

        public IList<IWebElement> PhonesList
        {
            get { return this.ngDriver.FindElements(NgBy.Repeater("phone in $ctrl.phones")); }
        }

        IWebDriver ngDriver;

        public TutorialStep7Page(IWebDriver driver, string url)
        {
            ngDriver = new NgWebDriver(driver);
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
            return PhonesList.Count;
        }

        public string GetResultsPhoneName(int index)
        {
            return PhonesList[index].FindElement(NgBy.Binding("phone.name")).Text;
        }
    }
}
