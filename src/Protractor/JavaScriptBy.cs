using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace Protractor
{
    internal class JavaScriptBy : By
    {
        private string script;
        private object[] args;

        public JavaScriptBy(string script, params object[] args)
        {
            this.script = script;
            this.args = args;
        }

        public IWebElement RootElement { get; set; }

        public override IWebElement FindElement(ISearchContext context)
        {
            ReadOnlyCollection<IWebElement> elements = this.FindElements(context);
            return elements.Count > 0 ? elements[0] : null;
        }

        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            // Create script arguments
            object[] scriptArgs = new object[this.args.Length + 1];
            scriptArgs[0] = this.RootElement;
            Array.Copy(this.args, 0, scriptArgs, 1, this.args.Length);

            ReadOnlyCollection<IWebElement> elements = ((IJavaScriptExecutor)context).ExecuteScript(this.script, scriptArgs) as ReadOnlyCollection<IWebElement>;
            if (elements == null)
            {
                elements = new ReadOnlyCollection<IWebElement>(new List<IWebElement>(0));
            }
            return elements;
        }
    }
}