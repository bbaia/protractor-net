using OpenQA.Selenium;

namespace Protractor.Samples.PageObjects.Support
{
    /*
     * Page Object that represents the the AngularJS tutorial Step 5 page: 
     * http://docs.angularjs.org/tutorial/step_05
     */
    public class TutorialStep5Page
    {
        NgWebDriver ngDriver;

        public TutorialStep5Page(IWebDriver driver, string url)
        {
            ngDriver = new NgWebDriver(driver);

            ngDriver.Navigate().GoToUrl(url);
        }

        public TutorialStep5Page SearchFor(string query)
        {
            var q = ngDriver.FindElement(NgBy.Input("query"));
            q.Clear();
            q.SendKeys(query);
            return this;
        }

        public TutorialStep5Page SortByName()
        {
            // Alternative: Use OpenQA.Selenium.Support.UI.SelectElement from Selenium.Support package
            ngDriver
                .FindElement(NgBy.Select("orderProp"))
                .FindElement(By.XPath("//option[@value='name']"))
                .Click();
            return this;
        }

        public TutorialStep5Page SortByAge()
        {
            // Alternative: Use OpenQA.Selenium.Support.UI.SelectElement from Selenium.Support package
            ngDriver
                .FindElement(NgBy.Select("orderProp"))
                .FindElement(By.XPath("//option[@value='age']"))
                .Click();
            return this;
        }

        public int GetResultsCount()
        {
            return ngDriver.FindElements(NgBy.Repeater("phone in phones")).Count;
        }

        public string GetResultsPhoneName(int index)
        {
            return ngDriver.FindElements(NgBy.Repeater("phone in phones"))[index].Evaluate("phone.name") as string;
        }
    }
}
