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
         * Tries to find $$testability and possibly $injector for an ng1 app
         *
         * By default, doesn't care about $injector if it finds $$testability. 
         * However, these priorities can be reversed.
         *
         * @param {string=} selector The selector for the element with the injector.
         * If falsy, tries a variety of methods to find an injector
         * @param {boolean=} injectorPlease Prioritize finding an injector
         * @return {$$testability?: Testability, $injector?: Injector} Returns whatever
         *   ng1 app hooks it finds
         */
        private const string GetNg1HooksHelper = @"
function getNg1Hooks(selector, injectorPlease) {
    function tryEl(el) {
        try {
            if (!injectorPlease && angular.getTestability) {
                var $$testability = angular.getTestability(el);
                if ($$testability) {
                    return {$$testability: $$testability};
                }
            } else {
                var $injector = angular.element(el).injector();
                if ($injector) {
                    return {$injector: $injector};
                }
            }
        } catch(err) {} 
    }
    function trySelector(selector) {
        var els = document.querySelectorAll(selector);
        for (var i = 0; i < els.length; i++) {
            var elHooks = tryEl(els[i]);
            if (elHooks) {
                return elHooks;
            }
        }
    }

    if (selector) {
        return trySelector(selector);
    } else if (window.__TESTABILITY__NG1_APP_ROOT_INJECTOR__) {
        var $injector = window.__TESTABILITY__NG1_APP_ROOT_INJECTOR__;
        var $$testability = null;
        try {
            $$testability = $injector.get('$$testability');
        } catch (e) {}
        return {$injector: $injector, $$testability: $$testability};
    } else {
        return tryEl(document.body) ||
            trySelector('[ng-app]') || trySelector('[ng\\:app]') ||
            trySelector('[ng-controller]') || trySelector('[ng\\:controller]');
    }
};
";

        /**
         * Wait until Angular has finished rendering and has
         * no outstanding $http calls before continuing.
         *
         * arguments[0] {string} The selector housing an ng-app
         * arguments[1] {function} callback
         */
        public const string WaitForAngular = GetNg1HooksHelper + @"
var rootSelector = arguments[0];
var callback = arguments[1];
if (window.angular && !(window.angular.version && window.angular.version.major > 1)) {
    /* ng1 */
    var hooks = getNg1Hooks(rootSelector);
    if (hooks.$$testability) {
        hooks.$$testability.whenStable(callback);
    } else if (hooks.$injector) {
        hooks.$injector.get('$browser').
        notifyWhenNoOutstandingRequests(callback);
    } else if (!!rootSelector) {
        throw new Error('Could not automatically find injector on page: ""' +
            window.location.toString() + '"". Consider setting rootElement');
    } else {
    throw new Error('root element (' + rootSelector + ') has no injector.' +
        ' this may mean it is not inside ng-app.');
    }
} else if (rootSelector && window.getAngularTestability) {
    var el = document.querySelector(rootSelector);
    window.getAngularTestability(el).whenStable(callback);
} else if (window.getAllAngularTestabilities) {
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
    });
} else if (!window.angular) {
    throw new Error('window.angular is undefined.  This could be either ' +
        'because this is a non-angular page or because your test involves ' +
        'client-side navigation, which can interfere with Protractor\'s ' +
        'bootstrapping.  See http://git.io/v4gXM for details');
} else if (window.angular.version >= 2) {
    throw new Error('You appear to be using angular, but window.' +
        'getAngularTestability was never set.  This may be due to bad ' +
        'obfuscation.');
} else {
    throw new Error('Cannot get testability API for unknown angular ' +
        'version ""' + window.angular.version + '""');
}";

        /**
         * Tests whether the angular global variable is present on a page. 
         * Retries in case the page is just loading slowly.
         */
        public const string TestForAngular = @"
var asyncCallback = arguments[0];
var callback = function(args) {
    setTimeout(function() {
        asyncCallback(args);
    }, 0);
};
var definitelyNg1 = false;
var definitelyNg2OrNewer = false;
var check = function() {
    /* Figure out which version of angular we're waiting on */
    if (!definitelyNg1 && !definitelyNg2OrNewer) {
        if (window.angular && !(window.angular.version && window.angular.version.major > 1)) {
            definitelyNg1 = true;
        } else if (window.getAllAngularTestabilities) {
            definitelyNg2OrNewer = true;
        }
    }
    /* See if our version of angular is ready */
    if (definitelyNg1) {
        if (window.angular && window.angular.resumeBootstrap) {
            return callback(1);
        }
    } else if (definitelyNg2OrNewer) {
        if (true /* ng2 has no resumeBootstrap() */) {
            return callback(2);
        }
    }
    /* Try again (or fail) */
    if (definitelyNg1 && window.angular) {
        throw new Error('angular never provided resumeBootstrap');
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
window.__TESTABILITY__NG1_APP_ROOT_INJECTOR__ = 
    angular.resumeBootstrap(arguments[0].length ? arguments[0].split(',') : []);";

        /**
         * Return the current location using $location.url().
         *
         * arguments[0] {string} The selector housing an ng-app
         */
        public const string GetLocation = GetNg1HooksHelper + @"
var hooks = getNg1Hooks(arguments[0]);
if (angular.getTestability) {
    return hooks.$$testability.getLocation();
}
return hooks.$injector.get('$location').getLocation();";

        /**
         * Browse to another page using in-page navigation.
         *
         * arguments[0] {string} The selector housing an ng-app
         * arguments[1] {string} In page URL using the same syntax as $location.url()
         */
        public const string SetLocation = GetNg1HooksHelper + @"
var hooks = getNg1Hooks(arguments[0]);
var url = arguments[1];
if (angular.getTestability) {
    return hooks.$$testability.setLocation(url);
}
var $injector = hooks.$injector;
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
         * arguments[1] {boolean} Whether the binding needs to be matched exactly
         * arguments[2] {string} The selector to use for the root app element.
         * arguments[3] {Element} The scope of the search.
         *
         * @return {Array.WebElement} The elements containing the binding.
         */
        public const string FindBindings = GetNg1HooksHelper + @"
var binding = arguments[0];
var exactMatch = arguments[1];
var using = arguments[3] || document;
if (angular.getTestability) {
    return getNg1Hooks(arguments[2]).$$testability.
        findBindings(using, binding, exactMatch);
}
var bindings = using.getElementsByClassName('ng-binding');
var matches = [];
for (var i = 0; i < bindings.length; ++i) {
    var dataBinding = angular.element(bindings[i]).data('$binding');
    if (dataBinding) {
        var bindingName = dataBinding.exp || dataBinding[0].exp || dataBinding;
        if (exactMatch) {
            var matcher = new RegExp('({|\\s|^|\\|)' +
                /* See http://stackoverflow.com/q/3561711 */
                binding.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, '\\$&') +
                '(}|\\s|$|\\|)');
            if (matcher.test(bindingName)) {
                matches.push(bindings[i]);
            }
        } else {
            if (bindingName.indexOf(binding) != -1) {
                matches.push(bindings[i]);
            }
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
        public const string FindModel = GetNg1HooksHelper + @"
var model = arguments[0];
var using = arguments[2] || document;
if (angular.getTestability) {
    return getNg1Hooks(arguments[1]).$$testability.
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
         * arguments[1] {boolean} Whether the repeater needs to be matched exactly
         * arguments[1] {string} The selector to use for the root app element.
         * arguments[2] {Element} The scope of the search.
         *
         * @return {Array.WebElement} All rows of the repeater.
         */
        public const string FindAllRepeaterRows = @"
var repeaterMatch = function(ngRepeat, repeater, exact) {
    if (exact) {
        return ngRepeat.split(' track by ')[0].split(' as ')[0].split('|')[0].
            split('=')[0].trim() == repeater;
    } else {
        return ngRepeat.indexOf(repeater) != -1;
    }
};

var repeater = arguments[0];
var exactMatch = arguments[1];
var using = arguments[3] || document;
var rows = [];
var prefixes = ['ng-', 'ng_', 'data-ng-', 'x-ng-', 'ng\\:'];
for (var p = 0; p < prefixes.length; ++p) {
    var attr = prefixes[p] + 'repeat';
    var repeatElems = using.querySelectorAll('[' + attr + ']');
    attr = attr.replace(/\\/g, '');
    for (var i = 0; i < repeatElems.length; ++i) {
        if (repeaterMatch(repeatElems[i].getAttribute(attr), repeater, exactMatch)) {
            rows.push(repeatElems[i]);
        }
    }
}
for (var p = 0; p < prefixes.length; ++p) {
    var attr = prefixes[p] + 'repeat-start';
    var repeatElems = using.querySelectorAll('[' + attr + ']');
    attr = attr.replace(/\\/g, '');
    for (var i = 0; i < repeatElems.length; ++i) {
        if (repeaterMatch(repeatElems[i].getAttribute(attr), repeater, exactMatch)) {
            var elem = repeatElems[i];
            while (elem.nodeType != 8 || 
                    !repeaterMatch(elem.nodeValue, repeater)) {
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
