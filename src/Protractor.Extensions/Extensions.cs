using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using NUnit.Framework;
using OpenQA.Selenium;

namespace Protractor.Extensions
{
    public static class Extensions
    {

        private static string result = null;
        private static Regex Reg;
        private static MatchCollection Matches;

        public static string FindMatch(this string element_text, string match_string, string match_name)
        {
            result = null;
            Reg = new Regex(match_string,
                                   RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

            Matches = Reg.Matches(element_text);
            foreach (Match Match in Matches)
            {
                if (Match.Length != 0)
                {

                    foreach (Capture Capture in Match.Groups[match_name].Captures)
                    {
                        if (result == null)
                        {
                            result = Capture.ToString();
                        }
                    }
                }
            }
            return result;
        }

        public static string FindMatch(this string element_text, string match_string)
        {
            string match_name = match_string.FindMatch("(?:<(?<result>[^>]+)>)", "result");
            result = null;
            Reg = new Regex(match_string,
                                   RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

            Matches = Reg.Matches(element_text);

            foreach (Match Match in Matches)
            {
                if (Match.Length != 0)
                {

                    foreach (Capture Capture in Match.Groups[match_name].Captures)
                    {
                        if (result == null)
                        {
                            result = Capture.ToString();
                        }
                    }
                }
            }

            return result;

        }

        public static void Highlight(this NgWebDriver driver, IWebElement element, int highlight_timeout = 100, int px = 3, string color = "yellow")
        {
            IWebDriver context = driver.WrappedDriver;
            ((IJavaScriptExecutor)context).ExecuteScript("arguments[0].style.border='" + px + "px solid " + color + "'", element);
            Thread.Sleep(highlight_timeout);
            ((IJavaScriptExecutor)context).ExecuteScript("arguments[0].style.border=''", element);
        }

        public static string CssSelectorOf(this NgWebElement ngWebElement)
        {
            string getCssSelectorOfElement = @"
		var get_css_selector_of = function(element) {
    if (!(element instanceof Element))
        return;
    var path = [];
    while (element.nodeType === Node.ELEMENT_NODE) {
        var selector = element.nodeName.toLowerCase();
        if (element.id) {
            if (element.id.indexOf('-') > -1) {
                selector += '[id = ""' + element.id + '""]';
            } else {
                selector += '#' + element.id;
            }
            path.unshift(selector);
            break;
        } else {
            var element_sibling = element;
            var sibling_cnt = 1;
            while (element_sibling = element_sibling.previousElementSibling) {
                if (element_sibling.nodeName.toLowerCase() == selector)
                    sibling_cnt++;
            }
            if (sibling_cnt != 1)
                selector += ':nth-of-type(' + sibling_cnt + ')';
        }
        path.unshift(selector);
        element = element.parentNode;
    }
    return path.join(' > ');
}
return get_css_selector_of(arguments[0]);
			";
            return ((IJavaScriptExecutor)ngWebElement.NgDriver.WrappedDriver).ExecuteScript(getCssSelectorOfElement, ngWebElement.WrappedElement).ToString();
        }

        public static string XPathOf(this NgWebElement ngWebElement)
        {
            string getXpathOfElement = @"
		var get_xpath_of = function(element) {
    var elementTagName = element.tagName.toLowerCase();
    if (element.id != '') {
        return '//' + elementTagName + '[@id=""' + element.id + '""]';
    } else if (element.name && document.getElementsByName(element.name).length === 1) {
        return '//' + elementTagName + '[@name=""' + element.name + '""]';
    }
    if (element === document.body) {
        return '/html/' + elementTagName;
    }
    var sibling_count = 0;
    var siblings = element.parentNode.childNodes;
    siblings_length = siblings.length;
    for (cnt = 0; cnt < siblings_length; cnt++) {
        var sibling_element = siblings[cnt];
        if (sibling_element.nodeType !== 1) { // not ELEMENT_NODE
            continue;
        }
        if (sibling_element === element) {
            return sibling_count > 0 ? get_xpath_of(element.parentNode) + '/' + elementTagName + '[' + (sibling_count + 1) + ']' : get_xpath_of(element.parentNode) + '/' + elementTagName;
        }
        if (sibling_element.nodeType === 1 && sibling_element.tagName.toLowerCase() === elementTagName) {
            sibling_count++;
        }
    }
    return;
};
return get_xpath_of(arguments[0]);
			";
            return ((IJavaScriptExecutor)ngWebElement.NgDriver.WrappedDriver).ExecuteScript(getXpathOfElement, ngWebElement.WrappedElement).ToString();
        }


        public static T Execute<T>(this IWebDriver driver, string script, params Object[] args)
        {
            return (T)((IJavaScriptExecutor)driver).ExecuteScript(script, args);
        }

        public static List<Dictionary<String, String>> ScopeOf(this NgWebElement ngWebElement)
        {
            string getScopeOfElement = "return angular.element(arguments[0]).scope();";
            IWebDriver driver = ngWebElement.NgDriver.WrappedDriver;
            List<Dictionary<String, String>> result = new List<Dictionary<string, string>>();
            IEnumerable<Object> raw_data = driver.Execute<IEnumerable<Object>>(getScopeOfElement, ngWebElement.WrappedElement);
            foreach (var element in (IEnumerable<Object>)raw_data)
            {
                Dictionary<String, String> row = new Dictionary<String, String>();
                Dictionary<String, Object> dic = (Dictionary<String, Object>)element;
                foreach (object key in dic.Keys)
                {
                    Object val = null;
                    if (!dic.TryGetValue(key.ToString(), out val)) { val = ""; }
                    row.Add(key.ToString(), val.ToString());
                }
                result.Add(row);
            }
            return result;
        }

        public static List<Dictionary<String, String>> ScopeDataOf(this NgWebElement ngWebElement, string scopeData)
        {
            string getScopeData = String.Format("return angular.element(arguments[0]).scope().{0};", scopeData);
            IWebDriver driver = ngWebElement.NgDriver.WrappedDriver;
            List<Dictionary<String, String>> result = new List<Dictionary<string, string>>();
            IEnumerable<Object> raw_data = driver.Execute<IEnumerable<Object>>(getScopeData, ngWebElement.WrappedElement);
            foreach (var element in (IEnumerable<Object>)raw_data)
            {
                Dictionary<String, String> row = new Dictionary<String, String>();
                Dictionary<String, Object> dic = (Dictionary<String, Object>)element;
                foreach (object key in dic.Keys)
                {
                    Object val = null;
                    if (!dic.TryGetValue(key.ToString(), out val)) { val = ""; }
                    row.Add(key.ToString(), val.ToString());
                }
                result.Add(row);
            }
            return result;
        }

        public static String IdentityOf(this NgWebElement ngWebElement)
        {
            string getIdentityOfElement = "return angular.identity(angular.element(arguments[0])).html();";
            return ((IJavaScriptExecutor)ngWebElement.NgDriver.WrappedDriver).ExecuteScript(getIdentityOfElement, ngWebElement.WrappedElement).ToString();
        }
    }
}