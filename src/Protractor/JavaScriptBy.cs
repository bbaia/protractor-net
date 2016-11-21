using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Protractor
{
    /// <summary>
    /// Provides a mechanism by which to find elements within a document using JavaScript.
    /// </summary>
    public class JavaScriptBy : By
    {
        private string script;
        private object[] args;

        /// <summary>
        /// Creates a new instance of <see cref="JavaScriptBy"/>.
        /// </summary>
        /// <param name="script">
        /// The JavaScript code to execute.
        /// </param>
        /// <param name="args">
        /// The arguments to the script.
        /// </param>
        public JavaScriptBy(string script, params object[] args)
        {
            this.script = script;
            this.args = args;
            this.Description = "Protractor.JavaScriptBy";
        }

        /// <summary>
        /// Gets or sets any additional arguments to the script.
        /// </summary>
        public object[] AdditionalScriptArguments { get; set; }

        /// <summary>
        /// Finds the first element matching the criteria.
        /// </summary>
        /// <param name="context">
        /// An <see cref="ISearchContext"/> object to use to search for the elements.
        /// </param>
        /// <returns>
        /// The first matching <see cref="IWebElement"/> on the current context.
        /// </returns>
        public override IWebElement FindElement(ISearchContext context)
        {
            ReadOnlyCollection<IWebElement> elements = this.FindElements(context);
            if (elements.Count == 0)
            {
                throw new NoSuchElementException(String.Format("Unable to locate element: {{ {0} }}.", this.Description));
            }
            return elements[0];
        }

        /// <summary>
        /// Finds all elements matching the criteria.
        /// </summary>
        /// <param name="context">
        /// An <see cref="ISearchContext"/> object to use to search for the elements.
        /// </param>
        /// <returns>
        /// A collection of all <see cref="OpenQA.Selenium.IWebElement"/> matching the current criteria, 
        /// or an empty list if nothing matches.
        /// </returns>
        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            // Create script arguments
            object[] scriptArgs = this.args;
            if (this.AdditionalScriptArguments != null && this.AdditionalScriptArguments.Length > 0)
            {
                // Add additionnal script arguments
                scriptArgs = new object[this.args.Length + this.AdditionalScriptArguments.Length];
                this.args.CopyTo(scriptArgs, 0);
                this.AdditionalScriptArguments.CopyTo(scriptArgs, this.args.Length);
            }

            // Get JS executor
            IJavaScriptExecutor jsExecutor = context as IJavaScriptExecutor;
            if (jsExecutor == null)
            {
                IWrapsDriver wrapsDriver = context as IWrapsDriver;
                if (wrapsDriver != null)
                {
                    jsExecutor = wrapsDriver.WrappedDriver as IJavaScriptExecutor;
                }
            }
            if (jsExecutor == null)
            {
                throw new NotSupportedException("Could not get an IJavaScriptExecutor instance from the context.");
            }

            ReadOnlyCollection<IWebElement> elements = jsExecutor.ExecuteScript(this.script, scriptArgs) as ReadOnlyCollection<IWebElement>;
            if (elements == null)
            {
                elements = new ReadOnlyCollection<IWebElement>(new List<IWebElement>(0));
            }
            return elements;
        }
    }
}