using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Protractor
{
    /// <summary>
    /// Internal implementation of the By Selenium class, supporting custom JavaScript selection for angular components.
    /// </summary>
    public class JavaScriptBy : By
    {
        private readonly string _script;
        private readonly object[] _args;

        /// <summary>
        /// javaScriptBy ctor.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="args"></param>
        public JavaScriptBy(string script, params object[] args)
        {
            this._script = script;
            this._args = args;
        }

        /// <summary>
        /// RootElement.
        /// </summary>
        public IWebElement RootElement { get; set; }

        /// <summary>
        /// FindElement Implementation.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IWebElement FindElement(ISearchContext context)
        {
            ReadOnlyCollection<IWebElement> elements = this.FindElements(context);
            return elements.Count > 0 ? elements[0] : null;
        }

        /// <summary>
        /// FindElements Implementation.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            // Create script arguments
            object[] scriptArgs = new object[this._args.Length + 1];
            scriptArgs[0] = this.RootElement;
            Array.Copy(this._args, 0, scriptArgs, 1, this._args.Length);

            // Get JS executor
            var jsExecutor = context as IJavaScriptExecutor;
            if (jsExecutor == null)
            {
                var wrapsDriver = context as IWrapsDriver;
                if (wrapsDriver != null)
                {
                    jsExecutor = wrapsDriver.WrappedDriver as IJavaScriptExecutor;
                }
            }
            if (jsExecutor == null)
            {
                throw new NotSupportedException("Could not get an IJavaScriptExecutor instance from the context.");
            }

            var elements = jsExecutor.ExecuteScript(_script, scriptArgs) as ReadOnlyCollection<IWebElement>;
            if (elements == null)
            {
                elements = new ReadOnlyCollection<IWebElement>(new List<IWebElement>(0));
            }
            return elements;
        }
    }
}