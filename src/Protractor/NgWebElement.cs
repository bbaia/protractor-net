using System;
using System.Linq;
using System.Drawing;
using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace Protractor
{
    /// <summary>
    /// Provides a mechanism to get elements off the page for test.
    /// </summary>
    public class NgWebElement : IWebElement, IWrapsElement
    {
        private readonly NgWebDriver ngDriver;
        private readonly IWebElement element;

        /// <summary>
        /// Creates a new instance of <see cref="NgWebElement"/> by wrapping a <see cref="IWebElement"/> instance.
        /// </summary>
        /// <param name="ngDriver">The <see cref="NgWebDriver"/> in use.</param>
        /// <param name="element">The existing <see cref="IWebElement"/> instance.</param>
        public NgWebElement(NgWebDriver ngDriver, IWebElement element)
        {
            this.ngDriver = ngDriver;
            this.element = element;
        }

        /// <summary>
        /// Gets the <see cref="NgWebDriver"/> instance used to initialize the element.
        /// </summary>
        public NgWebDriver NgDriver
        {
            get { return this.ngDriver; }
        }

        #region IWrapsElement Members

        /// <summary>
        /// Gets the wrapped <see cref="IWebElement"/> instance.
        /// </summary>
        public IWebElement WrappedElement
        {
            get { return this.element; }
        }

        #endregion

        #region IWebElement Members

        /// <summary>
        /// Gets a value indicating whether or not this element is displayed.
        /// </summary>
        /// <remarks>
        /// The <see cref="Displayed"/> property avoids the problem of having 
        /// to parse an element's "style" attribute to determine visibility of an element.
        /// </remarks>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public bool Displayed
        {
            get 
            {
                this.ngDriver.WaitForAngular();
                return this.element.Displayed;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this element is enabled.
        /// </summary>
        /// <remarks>
        /// The <see cref="Enabled"/> property will generally
        /// return <see langword="true"/> for everything except explicitly disabled input elements.
        /// </remarks>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public bool Enabled
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Enabled;
            }
        }

        /// <summary>
        /// Gets a <see cref="Point"/> object containing the coordinates of the upper-left corner
        /// of this element relative to the upper-left corner of the page.
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public Point Location
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Location;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this element is selected.
        /// </summary>
        /// <remarks>
        /// This operation only applies to input elements such as checkboxes,
        /// options in a select element and radio buttons.
        /// </remarks>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public bool Selected
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Selected;
            }
        }

        /// <summary>
        /// Gets a <see cref="Size"/> object containing the height and width of this element.
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public Size Size
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Size;
            }
        }

        /// <summary>
        /// Gets the tag name of this element.
        /// </summary>
        /// <remarks>
        /// The <see cref="TagName"/> property returns the tag name of the
        /// element, not the value of the name attribute. For example, it will return
        /// "input" for an element specified by the HTML markup &lt;input name="foo" /&gt;.
        /// </remarks>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public string TagName
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.TagName;
            }
        }

        /// <summary>
        /// Get the visible (i.e. not hidden by CSS) text of this element, including sub-elements.
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public string Text
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Text;
            }
        }

        /// <summary>
        /// Clears the content of this element.
        /// </summary>
        /// <remarks>
        /// If this element is a text entry element, the <see cref="Clear"/>
        /// method will clear the value. It has no effect on other elements. Text entry elements
        /// are defined as elements with INPUT or TEXTAREA tags.
        /// </remarks>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public void Clear()
        {
            this.ngDriver.WaitForAngular();
            this.element.Clear();
        }

        /// <summary>
        /// Clicks this element.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Click this element. If the click causes a new page to load, the <see cref="Click"/>
        ///     method will attempt to block until the page has loaded. After calling the
        ///     <see cref="Click"/> method, you should discard all references to this
        ///     element unless you know that the element and the page will still be present.
        ///     Otherwise, any further operations performed on this element will have an undefined.
        ///     behavior.
        ///   </para>
        ///   <para>
        ///     If this element is not clickable, then this operation is ignored. This allows you to
        ///     simulate a users to accidentally missing the target when clicking.
        ///   </para>
        /// </remarks>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public void Click()
        {
            this.ngDriver.WaitForAngular();
            this.element.Click();
        }

        /// <summary>
        /// Gets the value of the specified attribute for this element.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>
        /// The attribute's current value. Returns a <see langword="null"/> if the
        /// value is not set.
        /// </returns>
        /// <remarks>
        /// The <see cref="GetAttribute"/> method will return the current value
        /// of the attribute, even if the value has been modified after the page has been
        /// loaded. Note that the value of the following attributes will be returned even if
        /// there is no explicit attribute on the element:
        /// <list type="table">
        /// <listheader>
        /// <term>Attribute name</term>
        /// <term>Value returned if not explicitly specified</term>
        /// <term>Valid element types</term>
        /// </listheader>
        /// <item>
        /// <description>checked</description>
        /// <description>checked</description>
        /// <description>Check Box</description>
        /// </item>
        /// <item>
        /// <description>selected</description>
        /// <description>selected</description>
        /// <description>Options in Select elements</description>
        /// </item>
        /// <item>
        /// <description>disabled</description>
        /// <description>disabled</description>
        /// <description>Input and other UI elements</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public string GetAttribute(string attributeName)
        {
            this.ngDriver.WaitForAngular();
            return this.element.GetAttribute(attributeName);
        }

        /// <summary>
        /// Gets the value of a declared HTML attribute of this element.
        /// </summary>
        /// <param name="attributeName">The name of the HTML attribute to get the value of.</param>
        /// <returns>
        /// The HTML attribute's current value. Returns a <see langword="null"/> if the
        /// value is not set or the declared attribute does not exist.
        /// </returns>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <remarks>
        /// As opposed to the <see cref="GetAttribute(string)"/> method, this method
        /// only returns attributes declared in the element's HTML markup. To access the value
        /// of an IDL property of the element, either use the <see cref="GetAttribute(string)"/>
        /// method or the <see cref="GetDomProperty(string)"/> method.
        /// </remarks>
        public string GetDomAttribute(string attributeName)
        {
            this.ngDriver.WaitForAngular();
            return this.element.GetDomAttribute(attributeName);
        }

        /// <summary>
        /// Gets the value of a JavaScript property of this element.
        /// </summary>
        /// <param name="propertyName">The name of the JavaScript property to get the value of.</param>
        /// <returns>
        /// The JavaScript property's current value. Returns a <see langword="null"/> if the
        /// value is not set or the property does not exist.
        /// </returns>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public string GetDomProperty(string propertyName)
        {
            this.ngDriver.WaitForAngular();
            return this.element.GetDomProperty(propertyName);
        }

        /// <summary>
        /// Gets the value of a JavaScript property of this element.
        /// </summary>
        /// <param name="propertyName">The name JavaScript the JavaScript property to get the value of.</param>
        /// <returns>
        /// The JavaScript property's current value. Returns a <see langword="null"/> if the
        /// value is not set or the property does not exist.
        /// </returns>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        [Obsolete("Use the GetDomProperty method instead.")]
        public string GetProperty(string propertyName)
        {
            this.ngDriver.WaitForAngular();
            return this.element.GetProperty(propertyName);
        }

        /// <summary>
        /// Gets the representation of an element's shadow root for accessing the shadow DOM of a web component.
        /// </summary>
        /// <exception cref="NoSuchShadowRootException">Thrown when this element does not have a shadow root.</exception>
        /// <returns>A shadow root representation.</returns>
        public ISearchContext GetShadowRoot()
        {
            this.ngDriver.WaitForAngular();
            return this.element.GetShadowRoot();
        }

        /// <summary>
        /// Gets the value of a CSS property of this element.
        /// </summary>
        /// <param name="propertyName">The name of the CSS property to get the value of.</param>
        /// <returns>The value of the specified CSS property.</returns>
        /// <remarks>
        /// The value returned by the <see cref="GetCssValue"/>
        /// method is likely to be unpredictable in a cross-browser environment.
        /// Color values should be returned as hex strings. For example, a
        /// "background-color" property set as "green" in the HTML source, will
        /// return "#008000" for its value.
        /// </remarks>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public string GetCssValue(string propertyName)
        {
            this.ngDriver.WaitForAngular();
            return this.element.GetCssValue(propertyName);
        }

        /// <summary>
        /// Simulates typing text into the element.
        /// </summary>
        /// <param name="text">The text to type into the element.</param>
        /// <remarks>
        /// The text to be typed may include special characters like arrow keys,
        /// backspaces, function keys, and so on. Valid special keys are defined in
        /// <see cref="Keys"/>.
        /// </remarks>
        /// <seealso cref="Keys"/>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public void SendKeys(string text)
        {
            this.ngDriver.WaitForAngular();
            this.element.SendKeys(text);
        }

        /// <summary>
        /// Submits this element to the web server.
        /// </summary>
        /// <remarks>
        /// If this current element is a form, or an element within a form,
        /// then this will be submitted to the web server. If this causes the current
        /// page to change, then this method will block until the new page is loaded.
        /// </remarks>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public void Submit()
        {
            this.ngDriver.WaitForAngular();
            this.element.Submit();
        }

        /// <summary>
        /// Finds the first <see cref="NgWebElement"/> using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>The first matching <see cref="NgWebElement"/> on the current context.</returns>
        /// <exception cref="NoSuchElementException">If no element matches the criteria.</exception>
        public NgWebElement FindElement(By by)
        {
            if (by is JavaScriptBy jBy)
            {
                jBy.AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
            }
            this.ngDriver.WaitForAngular();
            return new NgWebElement(this.ngDriver, this.element.FindElement(by));
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
            if (by is JavaScriptBy jBy)
            {
                jBy.AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
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
            if (by is JavaScriptBy jBy)
            {
                jBy.AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
            }
            this.ngDriver.WaitForAngular();
            return new ReadOnlyCollection<IWebElement>(this.element.FindElements(by).Select(e => (IWebElement)new NgWebElement(this.ngDriver, e)).ToList());
        }

        #endregion

        /// <summary>
        /// Evaluates the expression as if it were on the scope of the current element.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The expression evaluated by Angular.</returns>
        public object Evaluate(string expression)
        {
            this.ngDriver.WaitForAngular();
            return this.ngDriver.ExecuteScript(ClientSideScripts.Evaluate, this.element, expression);
        }
    }
}
