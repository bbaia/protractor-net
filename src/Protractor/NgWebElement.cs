using System.Linq;
using System.Drawing;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Protractor
{
    /// <summary>
    /// Provides a mechanism to get elements off the page for test.
    /// </summary>
    public class NgWebElement : IWebElement, IWrapsElement, IJavaScriptExecutor
    {
        private readonly NgWebDriver _ngDriver;
        private readonly IWebElement _element;

        /// <summary>
        /// Creates a new instance of <see cref="NgWebElement"/> by wrapping a <see cref="IWebElement"/> instance.
        /// </summary>
        /// <param name="ngDriver">The <see cref="NgWebDriver"/> in use.</param>
        /// <param name="element">The existing <see cref="IWebElement"/> instance.</param>
        public NgWebElement(NgWebDriver ngDriver, IWebElement element)
        {
            _ngDriver = ngDriver;
            _element = element;
        }

        #region IWrapsElement

        /// <summary>
        /// Gets the wrapped <see cref="IWebElement"/> instance.
        /// </summary>
        public IWebElement WrappedElement
        {
            get { return _element; }
        }

        #endregion

        #region IWebElement Members

        /// <summary>
        /// Gets a value indicating whether or not this element is displayed.
        /// </summary>
        public bool Displayed
        {
            get
            {
                _ngDriver.WaitForAngular();
                return _element.Displayed;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this element is enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                _ngDriver.WaitForAngular();
                return _element.Enabled;
            }
        }

        /// <summary>
        /// Gets a <see cref="Point"/> object containing the coordinates of the upper-left corner
        /// of this element relative to the upper-left corner of the page.
        /// </summary>
        public Point Location
        {
            get
            {
                _ngDriver.WaitForAngular();
                return _element.Location;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this element is selected.
        /// </summary>
        public bool Selected
        {
            get
            {
                _ngDriver.WaitForAngular();
                return _element.Selected;
            }
        }

        /// <summary>
        /// Gets a <see cref="Size"/> object containing the height and width of this element.
        /// </summary>
        public Size Size
        {
            get
            {
                _ngDriver.WaitForAngular();
                return _element.Size;
            }
        }

        /// <summary>
        /// Gets the tag name of this element.
        /// </summary>
        public string TagName
        {
            get
            {
                _ngDriver.WaitForAngular();
                return _element.TagName;
            }
        }

        /// <summary>
        /// Gets the innerText of this element, without any leading or trailing whitespace,
        /// and with other whitespace collapsed.
        /// </summary>
        public string Text
        {
            get
            {
                _ngDriver.WaitForAngular();
                return _element.Text;
            }
        }

        /// <summary>
        /// Clears the content of this element.
        /// </summary>
        public void Clear()
        {
            _ngDriver.WaitForAngular();
            _element.Clear();
        }

        /// <summary>
        /// Clicks this element. 
        /// </summary>
        public void Click()
        {
            _ngDriver.WaitForAngular();
            _element.Click();
        }

        /// <summary>
        /// Gets the value of the specified attribute for this element.
        /// </summary>
        public string GetAttribute(string attributeName)
        {
            _ngDriver.WaitForAngular();
            return _element.GetAttribute(attributeName);
        }

        /// <summary>
        /// Gets the value of a CSS property of this element.
        /// </summary>
        public string GetCssValue(string propertyName)
        {
            _ngDriver.WaitForAngular();
            return _element.GetCssValue(propertyName);
        }

        /// <summary>
        /// Simulates typing text into the element.
        /// </summary>
        public void SendKeys(string text)
        {
            _ngDriver.WaitForAngular();
            _element.SendKeys(text);
        }

        /// <summary>
        /// Submits this element to the web server.
        /// </summary>
        public void Submit()
        {
            _ngDriver.WaitForAngular();
            _element.Submit();
        }

        /// <summary>
        /// Finds the first <see cref="NgWebElement"/> using the given mechanism. 
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>The first matching <see cref="NgWebElement"/> on the current context.</returns>
        /// <exception cref="NoSuchElementException">If no element matches the criteria.</exception>
        public NgWebElement FindElement(By by)
        {
            _ngDriver.WaitForAngular();
            return new NgWebElement(_ngDriver, _element.FindElement(by));
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
            _ngDriver.WaitForAngular();
            return new ReadOnlyCollection<NgWebElement>(_element.FindElements(by).Select(e => new NgWebElement(_ngDriver, e)).ToList());
        }

        IWebElement ISearchContext.FindElement(By by)
        {
            return FindElement(by);
        }

        ReadOnlyCollection<IWebElement> ISearchContext.FindElements(By by)
        {
            _ngDriver.WaitForAngular();
            return new ReadOnlyCollection<IWebElement>(_element.FindElements(by).Select(e => (IWebElement)new NgWebElement(_ngDriver, e)).ToList());
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
            return _ngDriver.ExecuteScript(script, args);
        }

        /// <summary>
        /// Executes JavaScript asynchronously in the context of the currently selected frame or window
        /// </summary>
        /// <param name="script">The JavaScript code to execute</param>
        /// <param name="args">The arguments to the script</param>
        /// <returns>The value returned by the script</returns>
        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return _ngDriver.ExecuteAsyncScript(script, args);
        }

        #endregion

        /// <summary>
        /// Evaluates the expression as if it were on the scope of the current element.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The expression evaluated by Angular.</returns>
        public object Evaluate(string expression)
        {
            _ngDriver.WaitForAngular();
            return ((IJavaScriptExecutor)_ngDriver.WrappedDriver).ExecuteScript(ClientSideScripts.Evaluate, _element, expression);
        }
    }
}
