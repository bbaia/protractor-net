// From https://github.com/angular/protractor/blob/master/lib/clientsidescripts.js

namespace Protractor
{
    /**
     * All scripts to be run on the client via ExecuteAsyncScript or
     * ExecuteScript should be put here. These scripts are transmitted over
     * the wire using their toString representation, and cannot reference
     * external variables. They can, however use the array passed in to
     * arguments. Instead of params, all functions on clientSideScripts
     * should list the arguments array they expect.
     */
    internal class ClientSideScripts
    {
        /**
         * Wait until Angular has finished rendering and has
         * no outstanding $http calls before continuing.
         *
         * arguments[0] {string} The selector housing an ng-app
         * arguments[1] {function} callback
         */
        public const string WaitForAngular = @"
var el = document.querySelector(arguments[0]);
var callback = arguments[1];
angular.element(el).injector().get('$browser').notifyWhenNoOutstandingRequests(callback);";

        /**
         * Tests whether the angular global variable is present on a page. 
         * Retries in case the page is just loading slowly.
         *
         * arguments[0] {string} none.
         */
        public const string TestForAngular = @"
var attempts = arguments[0];
var callback = arguments[arguments.length - 1];
var check = function(n) {
    if (window.angular && window.angular.resumeBootstrap) {
        callback(true);
    } else if (n < 1) {
        callback(false);
    } else {
        window.setTimeout(function() {check(n - 1)}, 1000);
    }
};
check(attempts);";

        /**
         * Continue to bootstrap Angular. 
         * 
         * arguments[0] {array} The module names to load.
         */
        public const string ResumeAngularBootstrap = @"
angular.resumeBootstrap(arguments[0].length ? arguments[0].split(',') : []);";

        /**
         * Return the current url using $location.absUrl().
         * 
         * arguments[0] {string} The selector housing an ng-app
         */
        public const string GetLocationAbsUrl = @"
var el = document.querySelector(arguments[0]);
return angular.element(el).injector().get('$location').absUrl();";

        /**
         * Evaluate an Angular expression in the context of a given element.
         *
         * arguments[0] {Element} The element in whose scope to evaluate.
         * arguments[1] {string} The expression to evaluate.
         *
         * @return {?Object} The result of the evaluation.
         */
        public const string Evaluate = @"
var element = arguments[0];
var expression = arguments[1];
return angular.element(element).scope().$eval(expression);";

        #region Locators

        /**
         * Find  elements by passed in identifiers name.
         * arguments[0] {Element} The scope of the search.
         * arguments[1] {string} The TagName.
         * arguments[2] {string} The ng Type.
         * arguments[3] {string} The SearchText.
         * @return {Array.WebElement} The matching select element.
         */
        public const string Find = @"
        var using = arguments[0] || document;
        var tagName = arguments[1];
        var identifier = arguments[2];
        var searchText = arguments[3];
        var prefixes = ['ng-', 'ng_', 'data-ng-', 'x-ng-', 'ng\\:'];
        for (var p = 0; p < prefixes.length; ++p) 
        {
            var selector =  tagName + '[' + prefixes[p] + identifier + '=""' + searchText + '""]';
            var inputs = using.querySelectorAll(selector);
            if (inputs.length) 
            {
                return inputs;
            }
        }";

        /* Find buttons by textual content. 
        * @param {string} searchText The exact text to match.
        * @param {Element} using The scope of the search.  Defaults to 'document'.
        * @return {Array.<Element>} The matching elements.
        */
        public const string FindButton =
            "var using = arguments[0]  || document; var searchText = arguments[1];" +
            "var elements = using.querySelectorAll('button, input[type=\"button\"], " +
            "input[type=\"submit\"]');  " +
            "var matches = [];  " +
            "for (var i = 0; i < elements.length; ++i) {    " +
            "var element = elements[i];    " +
            "var elementText;    " +
            "if (element.tagName.toLowerCase() == 'button') {      " +
            "elementText = element.innerText || element.textContent;    } " +
            "else {      elementText = element.value;    }    " +
            "if (elementText.trim() === searchText) {      " +
            "matches.push(element);    }  }  return matches; ";


        /* Find buttons by textual content.
       * @param {string} searchText The exact text to match.
       * @param {Element} using The scope of the search.  Defaults to 'document'.
       * @return {Array.<Element>} The matching elements.
        */
        public const string FindButtonByPartialText =
        "var using = arguments[0]  || document; var searchText = arguments[1];" +
        "var elements = using.querySelectorAll('button, input[type=\"button\"], " +
        "input[type=\"submit\"]');  " +
        "var matches = [];  " +
        "for (var i = 0; i < elements.length; ++i) {    " +
        "var element = elements[i];    " +
        "var elementText;    " +
        "if (element.tagName.toLowerCase() == 'button') {      " +
        "elementText = element.innerText || element.textContent;    } " +
        "else {      elementText = element.value;    }    " +
        "if (elementText.indexOf(searchText)  > -1) {      " +
        "matches.push(element);    }  }  return matches; ";

        /**
         * Find a list of elements in the page by their angular binding.
         *
         * arguments[0] {Element} The scope of the search.
         * arguments[1] {string} The binding, e.g. {{cat.name}}.
         *
         * @return {Array.WebElement} The elements containing the binding.
         */
        public const string FindBindings = @"
var using = arguments[0] || document;
var binding = arguments[1];
var bindings = using.getElementsByClassName('ng-binding');
var matches = [];
for (var i = 0; i < bindings.length; ++i) {
    var bindingName = angular.element(bindings[i]).data().$binding[0].exp ||
        angular.element(bindings[i]).data().$binding;
    if (bindingName.indexOf(binding) != -1) {
        matches.push(bindings[i]);
    }
}
return matches;";

        /**
         * Find input elements by model name.
         *
         * arguments[0] {Element} The scope of the search.
         * arguments[1] {string} The model name.
         *
         * @return {Array.WebElement} The matching input elements.
         */
        public const string FindInputs = @"
var using = arguments[0] || document;
var model = arguments[1];
var prefixes = ['ng-', 'ng_', 'data-ng-', 'x-ng-', 'ng\\:'];
for (var p = 0; p < prefixes.length; ++p) {
    var selector = 'input[' + prefixes[p] + 'model=""' + model + '""]';
    var inputs = using.querySelectorAll(selector);
    if (inputs.length) {
        return inputs;
    }
}";

        /// <summary>
        /// Find label elements by the text content
        /// </summary>
        public const string FindByLabelText =
          "var using = arguments[0]  || document; " +
          "var searchText = arguments[1];" +
          "var elements = using.querySelectorAll('label');" +
          "var matches = [];  " +
          "for (var i = 0; i < elements.length; ++i) {    " +
          "var element = elements[i];    " +
          "var elementText;    " +
          "if (element.tagName.toLowerCase() == 'label') {      " +
          "elementText = element.innerText || element.textContent;    } " +
          "else {      elementText = element.value;    }    " +
          "if (elementText.trim() === searchText) {      " +
          "matches.push(element);    }  }  return matches; ";

        /**
         * Find multiple select elements by model name.
         *
         * arguments[0] {Element} The scope of the search.
         * arguments[1] {string} The model name.
         *
         * @return {Array.WebElement} The matching select elements.
         */
        public const string FindSelects = @"
var using = arguments[0] || document;
var model = arguments[1];
var prefixes = ['ng-', 'ng_', 'data-ng-', 'x-ng-', 'ng\\:'];
for (var p = 0; p < prefixes.length; ++p) {
    var selector = 'select[' + prefixes[p] + 'model=""' + model + '""]';
    var inputs = using.querySelectorAll(selector);
    if (inputs.length) {
        return inputs;
    }
}";

        /**
         * Find selected option elements by model name.
         *
         * arguments[0] {Element} The scope of the search.
         * arguments[1] {string} The model name.
         *
         * @return {Array.WebElement} The matching select elements.
         */
        public const string FindSelectedOptions = @"
var using = arguments[0] || document;
var model = arguments[1];
var prefixes = ['ng-', 'ng_', 'data-ng-', 'x-ng-', 'ng\\:'];
for (var p = 0; p < prefixes.length; ++p) {
    var selector = 'select[' + prefixes[p] + 'model=""' + model + '""] option:checked';
    var inputs = using.querySelectorAll(selector);
    if (inputs.length) {
        return inputs;
    }
}";

        /**
         * Find textarea elements by model name.
         *
         * arguments[0] {Element} The scope of the search.
         * arguments[1] {String} The model name.
         *
         * @return {Array.WebElement} The matching textarea elements.
         */
        public const string FindTextArea = @"
var using = arguments[0] || document;
var model = arguments[1];
var prefixes = ['ng-', 'ng_', 'data-ng-', 'x-ng-', 'ng\\:'];
for (var p = 0; p < prefixes.length; ++p) {
    var selector = 'textarea[' + prefixes[p] + 'model=""' + model + '""]';
    var textareas = using.querySelectorAll(selector);
    if (textareas.length) {
        return textareas;
    }
}";

        /**
         * Find all rows of an ng-repeat.
         *
         * arguments[0] {Element} The scope of the search.
         * arguments[1] {string} The text of the repeater, e.g. 'cat in cats'.
         *
         * @return {Array.WebElement} All rows of the repeater.
         */
        public const string FindAllRepeaterRows = @"
var using = arguments[0] || document;
var repeater = arguments[1];
var rows = [];
var prefixes = ['ng-', 'ng_', 'data-ng-', 'x-ng-', 'ng\\:'];
for (var p = 0; p < prefixes.length; ++p) {
    var attr = prefixes[p] + 'repeat';
    var repeatElems = using.querySelectorAll('[' + attr + ']');
    attr = attr.replace(/\\/g, '');
    for (var i = 0; i < repeatElems.length; ++i) {
        if (repeatElems[i].getAttribute(attr).indexOf(repeater) != -1) {
            rows.push(repeatElems[i]);
        }
    }
}
return rows;";

        #endregion
    }
}
