using System.Linq;
using System.Drawing;
using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace Protractor
{
    public class NgWebElement : IWebElement
    {
        private NgWebDriver ngDriver;
        private IWebElement element;

        public NgWebElement(NgWebDriver ngDriver, IWebElement element)
        {
            this.ngDriver = ngDriver;
            this.element = element;
        }

        public IWebElement WrappedElement
        {
            get { return this.element; }
        }

        #region IWebElement Members

        public bool Displayed
        {
            get 
            {
                this.ngDriver.WaitForAngular();
                return this.element.Displayed;
            }
        }

        public bool Enabled
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Enabled;
            }
        }

        public Point Location
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Location;
            }
        }

        public bool Selected
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Selected;
            }
        }

        public Size Size
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Size;
            }
        }

        public string TagName
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.TagName;
            }
        }

        public string Text
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Text;
            }
        }

        public void Clear()
        {
            this.ngDriver.WaitForAngular();
            this.element.Clear();
        }

        public void Click()
        {
            this.ngDriver.WaitForAngular();
            this.element.Click();
        }

        public string GetAttribute(string attributeName)
        {
            this.ngDriver.WaitForAngular();
            return this.element.GetAttribute(attributeName);
        }

        public string GetCssValue(string propertyName)
        {
            this.ngDriver.WaitForAngular();
            return this.element.GetCssValue(propertyName);
        }

        public void SendKeys(string text)
        {
            this.ngDriver.WaitForAngular();
            this.element.SendKeys(text);
        }

        public void Submit()
        {
            this.ngDriver.WaitForAngular();
            this.element.Submit();
        }

        public NgWebElement FindElement(By by)
        {
            if (by is JavaScriptBy)
            {
                ((JavaScriptBy)by).RootElement = this.element;
            }
            this.ngDriver.WaitForAngular();
            return new NgWebElement(this.ngDriver, this.element.FindElement(by));
        }

        public ReadOnlyCollection<NgWebElement> FindElements(By by)
        {
            if (by is JavaScriptBy)
            {
                ((JavaScriptBy)by).RootElement = this.element;
            }
            this.ngDriver.WaitForAngular();
            return new ReadOnlyCollection<NgWebElement>(this.element.FindElements(by).Select(e => new NgWebElement(this.ngDriver, e)).ToList());
        }

        IWebElement ISearchContext.FindElement(By by)
        {
            return this.FindElement(by);
        }

        ReadOnlyCollection<IWebElement> ISearchContext.FindElements(By by)
        {
            if (by is JavaScriptBy)
            {
                ((JavaScriptBy)by).RootElement = this.element;
            }
            this.ngDriver.WaitForAngular();
            return new ReadOnlyCollection<IWebElement>(this.element.FindElements(by).Select(e => (IWebElement)new NgWebElement(this.ngDriver, e)).ToList());
        }

        #endregion
    }
}
