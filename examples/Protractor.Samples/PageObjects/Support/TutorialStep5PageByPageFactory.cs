using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Protractor.Samples.PageObjects.Support
{
    public class TutorialStep5PageByPageFactory
    {
        NgWebDriver ngDriver;

        [FindsBy(How = How.Custom, CustomFinderType = typeof(Input), Using = "query")]
        private IWebElement queryElement;

        [FindsBy(How = How.Custom, CustomFinderType = typeof(Select), Using = "orderProp")]
        private IWebElement orderProp;

        [FindsBy(How = How.Custom, CustomFinderType = typeof(Repeater), Using = "phone in phones")]
        private IList<IWebElement> phoneList;


        public TutorialStep5PageByPageFactory(IWebDriver driver, string url)
        {
            ngDriver = new NgWebDriver(driver);

            ngDriver.Navigate().GoToUrl(url);

            PageFactory.InitElements(ngDriver, this);
        }

        public TutorialStep5PageByPageFactory SearchFor(string query)
        {
            var q = queryElement;
            q.Clear();
            q.SendKeys(query);
            return this;
        }

        public TutorialStep5PageByPageFactory SortByName()
        {
            // Alternative: Use OpenQA.Selenium.Support.UI.SelectElement from Selenium.Support package
            orderProp
                .FindElement(By.XPath("//option[@value='name']"))
                .Click();
            return this;
        }

        public TutorialStep5PageByPageFactory SortByAge()
        {
            // Alternative: Use OpenQA.Selenium.Support.UI.SelectElement from Selenium.Support package
            orderProp
                .FindElement(By.XPath("//option[@value='age']"))
                .Click();
            return this;
        }

        public int GetResultsCount()
        {
            return phoneList.Count;
        }

        public string GetResultsPhoneName(int index)
        {
            return ((NgWebElement)phoneList[index]).Evaluate("phone.name") as string;
        }
    }
}
