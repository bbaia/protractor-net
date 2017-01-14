using System;

using OpenQA.Selenium;

namespace Protractor
{
    /// <summary>
    /// Provides a mechanism for navigating against an AngularJS application.
    /// </summary>
    public class NgNavigation : INavigation
    {
        private NgWebDriver ngDriver;
        private INavigation navigation;

        /// <summary>
        /// Creates a new instance of <see cref="NgNavigation"/> by wrapping a <see cref="INavigation"/> instance.
        /// </summary>
        /// <param name="ngDriver">The <see cref="NgWebDriver"/> in use.</param>
        /// <param name="navigation">The existing <see cref="INavigation"/> instance.</param>
        public NgNavigation(NgWebDriver ngDriver, INavigation navigation)
        {
            this.ngDriver = ngDriver;
            this.navigation = navigation;
        }

        /// <summary>
        /// Gets the wrapped <see cref="INavigation"/> instance.
        /// </summary>
        public INavigation WrappedNavigation
        {
            get { return this.navigation; }
        }

        #region INavigation Members

        /// <summary>
        /// Move back a single entry in the browser's history.
        /// </summary>
        public void Back()
        {
            this.ngDriver.WaitForAngular();
            this.navigation.Back();
        }

        /// <summary>
        /// Move a single "item" forward in the browser's history.
        /// </summary>
        public void Forward()
        {
            this.ngDriver.WaitForAngular();
            this.navigation.Forward();
        }

        void OpenQA.Selenium.INavigation.GoToUrl(Uri url)
        {
            GoToUrl(url, true);
        }

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load.</param>
        public void GoToUrl(Uri url)
        {
            GoToUrl(url, true);
        }

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load.</param>
        /// <param name="ensureAngularApp">Ensure the page is an Angular page by throwing an exception.</param>
        public void GoToUrl(Uri url, bool ensureAngularApp)
        {
            if (ensureAngularApp)
            {
                if (url == null)
                {
                    throw new ArgumentNullException("url", "URL cannot be null.");
                }
                this.ngDriver.Url = url.ToString();
            }
            else
            {
                this.navigation.GoToUrl(url);
            }
        }

        void OpenQA.Selenium.INavigation.GoToUrl(string url)
        {
            GoToUrl(url, true);
        }

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load. It is best to use a fully qualified URL</param>
        public void GoToUrl(string url)
        {
            GoToUrl(url, true);
        }

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load. It is best to use a fully qualified URL</param>
        /// <param name="ensureAngularApp">Ensure the page is an Angular page by throwing an exception.</param>
        public void GoToUrl(string url, bool ensureAngularApp)
        {
            if (ensureAngularApp)
            {
                this.ngDriver.Url = url;
            }
            else
            {
                this.navigation.GoToUrl(url);
            }
        }

        /// <summary>
        /// Refreshes the current page.
        /// </summary>
        public void Refresh()
        {
            if (this.ngDriver.IgnoreSynchronization)
            {
                this.navigation.Refresh();
            }
            else
            {
                string url = this.ngDriver.ExecuteScript("return window.location.href;") as string;
                this.ngDriver.Url = url;
            }
        }

        #endregion

        /// <summary>
        /// Browse to another page using in-page navigation.
        /// </summary>
        /// <param name="path">The path to load using the same syntax as '$location.url()'.</param>
        public void GoToLocation(string path)
        {
            this.ngDriver.Location = path;
        }
    }
}
