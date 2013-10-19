using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace Protractor
{
    public class NgWebDriver : IWebDriver
    {
        private const string AngularDeferBootstrap = "NG_DEFER_BOOTSTRAP!";

        private IWebDriver driver;
        private IJavaScriptExecutor jsExecutor;
        private string rootElement;
        private NgModule[] mockModules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="mockModules">
        /// The modules to load before Angular whenever Url setter or Navigate().GoToUrl() is called.
        /// </param>
        public NgWebDriver(IWebDriver driver, params NgModule[] mockModules)
            : this(driver, "body", mockModules)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
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
            this.driver = driver;
            this.jsExecutor = (IJavaScriptExecutor)driver;
            this.rootElement = rootElement;
            this.mockModules = mockModules;
        }

        /// <summary>
        /// Gets the wrapped IWebDriver instance.
        /// <para/>
        /// Use this to interact with pages that do not contain Angular (such as a log-in screen).
        /// </summary>
        public IWebDriver WrappedDriver
        {
            get { return this.driver; }
        }

        /// <summary>
        /// Gets the CSS selector for an element on which to find Angular. 
        /// <para/>
        /// This is usually 'body' but if your ng-app is on a subsection of the page it may be a subelement.
        /// </summary>
        public string RootElement
        {
            get { return this.rootElement; }
        }

        /// <summary>
        /// If true, Protractor will not attempt to synchronize with the page before performing actions. 
        /// This can be harmful because Protractor will not wait until $timeouts and $http calls have been processed, 
        /// which can cause tests to become flaky. 
        /// This should be used only when necessary, such as when a page continuously polls an API using $timeout.
        /// </summary>
        public bool IgnoreSynchronization { get; set; }

        #region IWebDriver Members

        public string CurrentWindowHandle
        {
            get { return this.driver.CurrentWindowHandle; }
        }

        public string PageSource
        {
            get
            {
                this.WaitForAngular();
                return this.driver.PageSource;
            }
        }

        public string Title
        {
            get
            {
                this.WaitForAngular();
                return this.driver.Title;
            }
        }

        public string Url
        {
            get
            {
                this.WaitForAngular();
                return this.driver.Url;
            }
            set
            {
                // TODO: test Safari & Android
                IHasCapabilities hcDriver = this.driver as IHasCapabilities;
                if (hcDriver != null && 
                    (hcDriver.Capabilities.BrowserName == "internet explorer" ||
                     hcDriver.Capabilities.BrowserName == "phantomjs"))
                {
                    // Internet Explorer & PhantomJS
                    this.jsExecutor.ExecuteScript("window.name += '" + AngularDeferBootstrap + "';");
                    this.driver.Url = value;
                }
                else
                {
                    // Chrome & Firefox
                    this.driver.Url = "about:blank";
                    this.jsExecutor.ExecuteScript("window.name += '" + AngularDeferBootstrap + "'; window.location.href = '" + value + "';");
                }

                // Make sure the page is an Angular page.
                object isAngularApp = this.jsExecutor.ExecuteAsyncScript(ClientSideScripts.TestForAngular, 10);
                if (isAngularApp is bool && (bool)isAngularApp)
                {
                    // At this point, Angular will pause for us, until angular.resumeBootstrap is called.

                    // Register extra modules
                    foreach (NgModule ngModule in this.mockModules)
                    {
                        this.jsExecutor.ExecuteScript(ngModule.Script);
                    }
                    // Resume Angular bootstrap
                    this.jsExecutor.ExecuteScript(ClientSideScripts.ResumeAngularBootstrap,
                        String.Join(",", this.mockModules.Select(m => m.Name)));
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format("Angular could not be found on the page '{0}'", value));
                }
            }
        }

        public ReadOnlyCollection<string> WindowHandles
        {
            get { return this.driver.WindowHandles; }
        }

        public void Close()
        {
            this.driver.Close();
        }

        public IOptions Manage()
        {
            return this.driver.Manage();
        }

        public INavigation Navigate()
        {
            return new NgNavigation(this, this.driver.Navigate());
        }

        public void Quit()
        {
            this.driver.Quit();
        }

        public ITargetLocator SwitchTo()
        {
            return this.driver.SwitchTo();
        }

        public NgWebElement FindElement(By by)
        {
            this.WaitForAngular();
            return new NgWebElement(this, this.driver.FindElement(by));
        }

        public ReadOnlyCollection<NgWebElement> FindElements(By by)
        {
            this.WaitForAngular();
            return new ReadOnlyCollection<NgWebElement>(this.driver.FindElements(by).Select(e => new NgWebElement(this, e)).ToList());
        }

        IWebElement ISearchContext.FindElement(By by)
        {
            return this.FindElement(by);
        }

        ReadOnlyCollection<IWebElement> ISearchContext.FindElements(By by)
        {
            this.WaitForAngular();
            return new ReadOnlyCollection<IWebElement>(this.driver.FindElements(by).Select(e => (IWebElement)new NgWebElement(this, e)).ToList());
        }

        public void Dispose()
        {
            this.driver.Dispose();
        }

        #endregion

        internal void WaitForAngular()
        {
            if (!this.IgnoreSynchronization)
            {
                this.jsExecutor.ExecuteAsyncScript(ClientSideScripts.WaitForAngular, this.rootElement);
            }
        }
    }
}
