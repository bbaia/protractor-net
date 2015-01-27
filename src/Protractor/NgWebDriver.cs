using System;
using System.Linq;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Protractor
{
    /// <summary>
    /// Provides a mechanism to write tests against an AngularJS application.
    /// </summary>
    public class NgWebDriver : IWebDriver, IWrapsDriver, IJavaScriptExecutor, ITakesScreenshot
    {
        private const string AngularDeferBootstrap = "NG_DEFER_BOOTSTRAP!";

        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _jsExecutor;
        private readonly string _rootElement;
        private readonly NgModule[] _mockModules;

        /// <summary>
        /// Creates a new instance of <see cref="NgWebDriver"/> by wrapping a <see cref="IWebDriver"/> instance.
        /// </summary>
        /// <param name="driver">The configured webdriver instance.</param>
        /// <param name="mockModules">
        /// The modules to load before Angular whenever Url setter or Navigate().GoToUrl() is called.
        /// </param>
        public NgWebDriver(IWebDriver driver, params NgModule[] mockModules)
            : this(driver, "body", mockModules) { }

        /// <summary>
        /// Creates a new instance of <see cref="NgWebDriver"/> by wrapping a <see cref="IWebDriver"/> instance.
        /// </summary>
        /// <param name="driver">The configured webdriver instance.</param>
        /// <param name="rootElement">
        /// The CSS selector for an element on which to find Angular. 
        /// <para/>
        /// This is usually 'body' but if your ng-app is on a subsection of the page it may be a subelement.
        /// </param>
        /// <param name="mockModules">
        /// The modules to load before Angular whenever Url setter or Navigate().GoToUrl() is called.
        /// </param>
        public NgWebDriver(IWebDriver driver, string rootElement, params NgModule[] mockModules)
        {
            if (!(driver is IJavaScriptExecutor))
            {
                throw new NotSupportedException("The WebDriver instance must implement the IJavaScriptExecutor interface.");
            }
            _driver = driver;
            _jsExecutor = (IJavaScriptExecutor)driver;
            _rootElement = rootElement;
            _mockModules = mockModules;
        }

        /// <summary>
        /// Gets the CSS selector for an element on which to find Angular. 
        /// <para/>
        /// This is usually 'body' but if your ng-app is on a subsection of the page it may be a subelement.
        /// </summary>
        public string RootElement
        {
            get { return _rootElement; }
        }

        /// <summary>
        /// If true, Protractor will not attempt to synchronize with the page before performing actions. 
        /// This can be harmful because Protractor will not wait until $timeouts and $http calls have been processed, 
        /// which can cause tests to become flaky. 
        /// This should be used only when necessary, such as when a page continuously polls an API using $timeout.
        /// </summary>
        public bool IgnoreSynchronization { get; set; }

        #region IWebDriver Members

        /// <summary>
        /// Gets the current window handle, which is an opaque handle to this 
        /// window that uniquely identifies it within this driver instance.
        /// </summary>
        public string CurrentWindowHandle
        {
            get { return _driver.CurrentWindowHandle; }
        }

        /// <summary>
        /// Gets the source of the page last loaded by the browser.
        /// </summary>
        public string PageSource
        {
            get
            {
                WaitForAngular();
                return _driver.PageSource;
            }
        }

        /// <summary>
        /// Gets the title of the current browser window.
        /// </summary>
        public string Title
        {
            get
            {
                WaitForAngular();
                return _driver.Title;
            }
        }

        /// <summary>
        /// Gets or sets the URL the browser is currently displaying.
        /// </summary>
        public string Url
        {
            get
            {
                WaitForAngular();
                var hcDriver = _driver as IHasCapabilities;
                if (hcDriver != null && hcDriver.Capabilities.BrowserName == "internet explorer")
                {
                    // 'this.driver.Url' does not work on IE
                    return _jsExecutor.ExecuteScript(ClientSideScripts.GetLocationAbsUrl, _rootElement) as string;
                }
                return _driver.Url;
            }
            set
            {
                // TODO: test Safari & Android
                var hcDriver = _driver as IHasCapabilities;
                if (hcDriver != null &&
                    (hcDriver.Capabilities.BrowserName == "internet explorer" ||
                     hcDriver.Capabilities.BrowserName == "phantomjs"))
                {
                    // Internet Explorer & PhantomJS
                    _jsExecutor.ExecuteScript("window.name += '" + AngularDeferBootstrap + "';");
                    _driver.Url = value;
                }
                else
                {
                    // Chrome & Firefox
                    _driver.Url = "about:blank";
                    _jsExecutor.ExecuteScript("window.name += '" + AngularDeferBootstrap + "'; window.location.href = '" + value + "';");
                }

                // Make sure the page is an Angular page.
                object isAngularApp = _jsExecutor.ExecuteAsyncScript(ClientSideScripts.TestForAngular, 10);
                if (isAngularApp is bool && (bool)isAngularApp)
                {
                    // At this point, Angular will pause for us, until angular.resumeBootstrap is called.

                    // Register extra modules
                    foreach (NgModule ngModule in _mockModules)
                    {
                        _jsExecutor.ExecuteScript(ngModule.Script);
                    }
                    // Resume Angular bootstrap
                    _jsExecutor.ExecuteScript(ClientSideScripts.ResumeAngularBootstrap,
                        String.Join(",", _mockModules.Select(m => m.Name).ToArray()));
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format("Angular could not be found on the page '{0}'", value));
                }
            }
        }

        /// <summary>
        /// Gets the window handles of open browser windows.
        /// </summary>
        public ReadOnlyCollection<string> WindowHandles
        {
            get { return _driver.WindowHandles; }
        }

        /// <summary>
        /// Close the current window, quitting the browser if it is the last window currently open.
        /// </summary>
        public void Close()
        {
            _driver.Close();
        }

        /// <summary>
        /// Instructs the driver to change its settings.
        /// </summary>
        /// <returns>
        /// An <see cref="IOptions"/> object allowing the user to change the settings of the driver.
        /// </returns>
        public IOptions Manage()
        {
            return _driver.Manage();
        }

        /// <summary>
        /// Instructs the driver to navigate the browser to another location.
        /// </summary>
        /// <returns>
        /// An <see cref="INavigation"/> object allowing the user to access 
        /// the browser's history and to navigate to a given URL.
        /// </returns>
        public INavigation Navigate()
        {
            return new NgNavigation(this, _driver.Navigate());
        }

        /// <summary>
        /// Quits this driver, closing every associated window.
        /// </summary>
        public void Quit()
        {
            _driver.Quit();
        }

        /// <summary>
        /// Instructs the driver to send future commands to a different frame or window.
        /// </summary>
        /// <returns>
        /// An <see cref="ITargetLocator"/> object which can be used to select a frame or window.
        /// </returns>
        public ITargetLocator SwitchTo()
        {
            return _driver.SwitchTo();
        }

        /// <summary>
        /// Finds the first <see cref="NgWebElement"/> using the given mechanism. 
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>The first matching <see cref="NgWebElement"/> on the current context.</returns>
        /// <exception cref="NoSuchElementException">If no element matches the criteria.</exception>
        public NgWebElement FindElement(By by)
        {
            WaitForAngular();
            return new NgWebElement(this, _driver.FindElement(by));
        }

        /// <summary>
        /// Finds all <see cref="NgWebElement"/>s within the current context 
        /// using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>
        /// A <see cref="ReadOnlyCollection{T}"/> of all <see cref="NgWebElement"/>s 
        /// matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public ReadOnlyCollection<NgWebElement> FindElements(By by)
        {
            WaitForAngular();
            return new ReadOnlyCollection<NgWebElement>(_driver.FindElements(by).Select(e => new NgWebElement(this, e)).ToList());
        }

        IWebElement ISearchContext.FindElement(By by)
        {
            return FindElement(by);
        }

        ReadOnlyCollection<IWebElement> ISearchContext.FindElements(By by)
        {
            WaitForAngular();
            return new ReadOnlyCollection<IWebElement>(_driver.FindElements(by).Select(e => (IWebElement)new NgWebElement(this, e)).ToList());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _driver.Dispose();
        }

        #endregion

        #region IWrapsDriver Members

        /// <summary>
        /// Gets the wrapped <see cref="IWebDriver"/> instance.
        /// <para/>
        /// Use this to interact with pages that do not contain Angular (such as a log-in screen).
        /// </summary>
        public IWebDriver WrappedDriver
        {
            get { return _driver; }
        }

        #endregion

        #region IJavaScriptExecutor Members

        /// <summary>
        /// Executes JavaScript in the context of the currently selected frame or window
        /// </summary>
        /// <param name="script">The JavaScript code to execute</param>
        /// <param name="args">The arguments to the script</param>
        /// <returns>The value returned by the script</returns>
        public object ExecuteScript(string script, params object[] args)
        {
            return _jsExecutor.ExecuteScript(script, args);
        }

        /// <summary>
        /// Executes JavaScript asynchronously in the context of the currently selected frame or window
        /// </summary>
        /// <param name="script">The JavaScript code to execute</param>
        /// <param name="args">The arguments to the script</param>
        /// <returns>The value returned by the script</returns>
        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return _jsExecutor.ExecuteAsyncScript(script, args);
        }

        #endregion

        #region ITakesScreenshot Members

        public Screenshot GetScreenshot()
        {
            var takesScreenshot = _driver as ITakesScreenshot;
            if (takesScreenshot == null)
            {
                for (var wrapsDriver = _driver as IWrapsDriver; wrapsDriver != null; wrapsDriver = (wrapsDriver.WrappedDriver as IWrapsDriver))
                {
                    takesScreenshot = (wrapsDriver.WrappedDriver as ITakesScreenshot);
                    if (takesScreenshot != null)
                    {
                        break;
                    }
                }
            }
            if (takesScreenshot == null)
            {
                throw new NotSupportedException("The WrappedDriver must implement or wrap a driver that implements the ITakesScreenshot interface.");
            }
            return takesScreenshot.GetScreenshot();
        }

        #endregion

        internal void WaitForAngular()
        {
            if (!IgnoreSynchronization)
            {
                _jsExecutor.ExecuteAsyncScript(ClientSideScripts.WaitForAngular, _rootElement);
            }
        }
    }
}
