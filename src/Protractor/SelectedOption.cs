using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Protractor
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectedOption : By
    {
        private object[] args;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public SelectedOption(string arg)
        {
            this.args = new object[] { arg };
        }

        /// <summary>
        /// 
        /// </summary>
        public IWebElement RootElement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IWebElement FindElement(ISearchContext context)
        {
            ReadOnlyCollection<IWebElement> elements = this.FindElements(context);
            return elements.Count > 0 ? elements[0] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            // Create script arguments
            object[] scriptArgs = new object[this.args.Length + 1];
            scriptArgs[0] = this.RootElement;
            Array.Copy(this.args, 0, scriptArgs, 1, this.args.Length);

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

            ReadOnlyCollection<IWebElement> elements = jsExecutor.ExecuteScript(ClientSideScripts.FindSelectedOptions, scriptArgs) as ReadOnlyCollection<IWebElement>;
            if (elements == null)
            {
                elements = new ReadOnlyCollection<IWebElement>(new List<IWebElement>(0));
            }
            return elements;
        }
    }
}
