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
var rootSelector = arguments[0];
var el = document.querySelector(rootSelector);
var callback = arguments[1];
if (window.getAngularTestability) {
    window.getAngularTestability(el).whenStable(callback);
    return;
}
if (!window.angular) {
    throw new Error('angular could not be found on the window');
}
if (angular.getTestability) {
    angular.getTestability(el).whenStable(callback);
} else {
    if (!angular.element(el).injector()) {
        throw new Error('root element (' + rootSelector + ') has no injector.' +
            ' this may mean it is not inside ng-app.');
    }
angular.element(el).injector().get('$browser').
    notifyWhenNoOutstandingRequests(callback);
}";

        /**
         * Wait until all Angular2 applications on the page have become stable.
         *
         * arguments[0] {function} callback
         */
        public const string WaitForAllAngular2 = @"
var callback = arguments[0];
var testabilities = window.getAllAngularTestabilities();
var count = testabilities.length;
var decrement = function() {
    count--;
    if (count === 0) {
        callback();
    }
};
testabilities.forEach(function(testability) {
    testability.whenStable(decrement);
});";

        /**
         * Tests whether the angular global variable is present on a page. 
         * Retries in case the page is just loading slowly.
         */
        public const string TestForAngular = @"
var callback = arguments[0];
var check = function() {
    if (window.getAllAngularTestabilities) {
        callback(2);
    } else if (window.angular && window.angular.resumeBootstrap) {
        callback(1);
    } else {
        window.setTimeout(function() {check()}, 1000);
    }
};
check();";

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
         * Return the current location using $location.url().
         *
         * arguments[0] {string} The selector housing an ng-app
         */
        public const string GetLocation = @"
var el = document.querySelector(arguments[0]);
if (angular.getTestability) {
    return angular.getTestability(el).getLocation();
}
return angular.element(el).injector().get('$location').url();";

        /**
         * Browse to another page using in-page navigation.
         *
         * arguments[0] {string} The selector housing an ng-app
         * arguments[1] {string} In page URL using the same syntax as $location.url()
         */
        public const string SetLocation = @"
var el = document.querySelector(arguments[0]);
var url = arguments[1];
if (angular.getTestability) {
    angular.getTestability(el).setLocation(url);
}
var $injector = angular.element(el).injector();
var $location = $injector.get('$location');
var $rootScope = $injector.get('$rootScope');

if (url !== $location.url()) {
    $location.url(url);
    $rootScope.$digest();
}";

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
         * Find a list of elements in the page by their angular binding.
         *
         * arguments[0] {string} The binding, e.g. {{cat.name}}.
         * arguments[1] {string} The selector to use for the root app element.
         * arguments[2] {Element} The scope of the search.
         *
         * @return {Array.WebElement} The elements containing the binding.
         */
        public const string FindBindings = @"
var binding = arguments[0];
var root = document.querySelector(arguments[1]);
var using = arguments[2] || document;
if (angular.getTestability) {
    return angular.getTestability(root).
        findBindings(using, binding, false);
}
var bindings = using.getElementsByClassName('ng-binding');
var matches = [];
for (var i = 0; i < bindings.length; ++i) {
    var dataBinding = angular.element(bindings[i]).data('$binding');
    if (dataBinding) {
        var bindingName = dataBinding.exp || dataBinding[0].exp || dataBinding;
        if (bindingName.indexOf(binding) != -1) {
            matches.push(bindings[i]);
        }
    }
}
return matches;";

        /**
         * Find elements by model name.
         *
         * arguments[0] {string} The model name.
         * arguments[1] {string} The selector to use for the root app element.
         * arguments[2] {Element} The scope of the search.
         *
         * @return {Array.WebElement} The matching input elements.
         */
        public const string FindModel = @"
var model = arguments[0];
var root = document.querySelector(arguments[1]);
var using = arguments[2] || document;
if (angular.getTestability) {
    return angular.getTestability(root).
        findModels(using, model, true);
}
var prefixes = ['ng-', 'ng_', 'data-ng-', 'x-ng-', 'ng\\:'];
for (var p = 0; p < prefixes.length; ++p) {
    var selector = '[' + prefixes[p] + 'model=""' + model + '""]';
    var inputs = using.querySelectorAll(selector);
    if (inputs.length) {
        return inputs;
    }
}";

        /**
         * Find selected option elements by model name.
         *
         * arguments[0] {string} The model name.
         * arguments[1] {string} The selector to use for the root app element.
         * arguments[2] {Element} The scope of the search.
         *
         * @return {Array.WebElement} The matching select elements.
         */
        public const string FindSelectedOptions = @"
var model = arguments[0];
var root = document.querySelector(arguments[1]);
var using = arguments[2] || document;
var prefixes = ['ng-', 'ng_', 'data-ng-', 'x-ng-', 'ng\\:'];
for (var p = 0; p < prefixes.length; ++p) {
    var selector = 'select[' + prefixes[p] + 'model=""' + model + '""] option:checked';
    var inputs = using.querySelectorAll(selector);
    if (inputs.length) {
        return inputs;
    }
}";

        /**
         * Find all rows of an ng-repeat.
         *
         * arguments[0] {string} The text of the repeater, e.g. 'cat in cats'.
         * arguments[1] {string} The selector to use for the root app element.
         * arguments[2] {Element} The scope of the search.
         *
         * @return {Array.WebElement} All rows of the repeater.
         */
        public const string FindAllRepeaterRows = @"
var repeater = arguments[0];
var root = document.querySelector(arguments[1]);
var using = arguments[2] || document;
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
for (var p = 0; p < prefixes.length; ++p) {
    var attr = prefixes[p] + 'repeat-start';
    var repeatElems = using.querySelectorAll('[' + attr + ']');
    attr = attr.replace(/\\/g, '');
    for (var i = 0; i < repeatElems.length; ++i) {
        if (repeatElems[i].getAttribute(attr).indexOf(repeater) != -1) {
            var elem = repeatElems[i];
            while (elem.nodeType != 8 || 
                    !(elem.nodeValue.indexOf(repeater) != -1)) {
                if (elem.nodeType == 1) {
                    rows.push(elem);
                }
                elem = elem.nextSibling;
            }
        }
    }
}
return rows;";

        #endregion
    }
}
