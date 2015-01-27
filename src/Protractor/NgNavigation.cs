using System;

using OpenQA.Selenium;

namespace Protractor
{
    /// <summary>
    /// Provides a mechanism for navigating against an AngularJS application.
    /// </summary>
    public class NgNavigation : INavigation
    {
        private readonly NgWebDriver _ngDriver;
        private readonly INavigation _navigation;

        /// <summary>
        /// Creates a new instance of <see cref="NgNavigation"/> by wrapping a <see cref="INavigation"/> instance.
        /// </summary>
        /// <param name="ngDriver">The <see cref="NgWebDriver"/> in use.</param>
        /// <param name="navigation">The existing <see cref="INavigation"/> instance.</param>
        public NgNavigation(NgWebDriver ngDriver, INavigation navigation)
        {
            _ngDriver = ngDriver;
            _navigation = navigation;
        }

        /// <summary>
        /// Gets the wrapped <see cref="INavigation"/> instance.
        /// </summary>
        public INavigation WrappedNavigation
        {
            get { return _navigation; }
        }

        #region INavigation Members

        /// <summary>
        /// Move back a single entry in the browser's history.
        /// </summary>
        public void Back()
        {
            _navigation.Back();
        }

        /// <summary>
        /// Move a single "item" forward in the browser's history.
        /// </summary>
        public void Forward()
        {
            _navigation.Forward();
        }

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load.</param>
        public void GoToUrl(Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url", "URL cannot be null.");
            }
            _ngDriver.Url = url.ToString();
        }

        /// <summary>
        ///  Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load. It is best to use a fully qualified URL</param>
        public void GoToUrl(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url", "URL cannot be null.");
            }
            _ngDriver.Url = url;
        }

        /// <summary>
        /// Refreshes the current page.
        /// </summary>
        public void Refresh()
        {
            _ngDriver.WaitForAngular();
            _navigation.Refresh();
        }

        #endregion
    }
}
