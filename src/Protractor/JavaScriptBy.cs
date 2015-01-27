using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Protractor
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JavaScriptBy : By
    {
        private readonly string _script;
        private readonly object[] _args;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="args"></param>
        protected JavaScriptBy(string script, params object[] args)
        {
            _script = script;
            _args = args;
        }

        public override IWebElement FindElement(ISearchContext context)
        {
            ReadOnlyCollection<IWebElement> elements = this.FindElements(context);
            return elements.Count > 0 ? elements[0] : null;
        }

        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            // Create script arguments
            object[] scriptArgs = new object[_args.Length + 1];
            scriptArgs[0] = context is IWebElement ? context : null;
            Array.Copy(_args, 0, scriptArgs, 1, _args.Length);

            // Get JS executor
            IJavaScriptExecutor jsExecutor = context as IJavaScriptExecutor;
            if (jsExecutor == null)
            {
                throw new NotSupportedException("Could not get an IJavaScriptExecutor instance from the context.");
            }

            ReadOnlyCollection<IWebElement> elements = jsExecutor.ExecuteScript(_script, scriptArgs) as ReadOnlyCollection<IWebElement>;
            if (elements == null)
            {
                elements = new ReadOnlyCollection<IWebElement>(new List<IWebElement>(0));
            }
            return elements;
        }
    }
}