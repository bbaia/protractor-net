using System;

using OpenQA.Selenium;

namespace Protractor
{
    public class NgNavigation : INavigation
    {
        private NgWebDriver ngDriver;
        private INavigation navigation;

        public NgNavigation(NgWebDriver ngDriver, INavigation navigation)
        {
            this.ngDriver = ngDriver;
            this.navigation = navigation;
        }

        public INavigation WrappedNavigation
        {
            get { return this.navigation; }
        }

        #region INavigation Members

        public void Back()
        {
            this.navigation.Back();
        }

        public void Forward()
        {
            this.navigation.Forward();
        }

        public void GoToUrl(Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url", "URL cannot be null.");
            }
            this.ngDriver.Url = url.ToString();
        }

        public void GoToUrl(string url)
        {
            this.ngDriver.Url = url;
        }

        public void Refresh()
        {
            this.navigation.Refresh();
        }

        #endregion
    }
}
