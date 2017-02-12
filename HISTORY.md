0.10.2 / 2017-02-12
===================

* ([c30c04a](https://github.com/bbaia/protractor-net/commit/c30c04a262252dadd9c725c9e4be7a03ec4204a6)) Add NgByExactBinding & NgByExactRepeater for exact matching (Supports Page Objects pattern with FindsByAttribute)

0.10.1 / 2017-02-01
===================

* ([1b35fb9](https://github.com/bbaia/protractor-net/commit/1b35fb9d375fa6d19a607a60c6485c6d08229e32)) Add 'exactMatch' option to NgByBinding & NgByRepeater locators

0.10.0 / 2017-01-30
===================

* ([7b76ebd](https://github.com/bbaia/protractor-net/commit/7b76ebd4569a212ac5f588c7b747cbde5c615efc)) Throw NoSuchElementException in NgBy locators when element is not found
* ([c86613d](https://github.com/bbaia/protractor-net/commit/c86613de9914bc82229d1847db6c25bd69f91ffa)) Synchronize 'ClientSideScripts.cs' with latest Protractor 5
* ([d599176](https://github.com/bbaia/protractor-net/commit/d59917668c9d3e046a8f939d3f5053709889edce)) Add Firefox support

0.9.2 / 2017-01-17
==================

* ([d9753d9](https://github.com/bbaia/protractor-net/commit/d9753d93a8b85ebba40c9f8f3db7cf1ab2f9b3c9)) Add support for $compileProvider.debugInfoEnabled(false)

0.9.1 / 2016-12-01
==================

* ([dbb4074](https://github.com/bbaia/protractor-net/commit/dbb4074d4ad2fd263a414584fd8fa0ed56bea4fd)) Fix NgNavigation.Refresh() to reload the injected NgModules

0.9.0 / 2016-11-19
==================

* ([5eeb2fd](https://github.com/bbaia/protractor-net/commit/5eeb2fd61c2b7246d1be445a3fa93255c5cb337d)) Fix AngularJS tutorial based samples (broken since Angular 1.5 is used)
* ([83ae33d](https://github.com/bbaia/protractor-net/commit/83ae33d651603675c5d44aa5ef963643b2991393)) Update to Selenium.WebDriver v3.0.0
* ([9504185](https://github.com/bbaia/protractor-net/commit/95041854d166b3968d1431e71f5e898327873fe4)) Set By.Description property on NgBy locators to improve ToString() support

0.8.2 / 2016-10-21
==================

* ([95c0ddb](https://github.com/bbaia/protractor-net/commit/95c0ddb8502177ee8d1c31c94d58d930c16ec361)) Fix navigating to non-Angular page with IgnoreSynchronization set to true

0.8.1 / 2016-09-18
==================

* ([31d91ed](https://github.com/bbaia/protractor-net/commit/31d91edd7c3b933364dbee1c01835730b946a92b)) Add Microsoft Edge support
* ([842ea1d](https://github.com/bbaia/protractor-net/commit/842ea1dd44945110a983a2e37b38560ff2bd1d87)) Fix issue that caused Safari to sometimes hang on page load
* ([e9705d6](https://github.com/bbaia/protractor-net/commit/e9705d6651b581b6b06e4e9dd0d74c8b31836719)) Implement IJavaScriptExecutor in NgWebDriver

0.8.0 / 2016-05-11
==================

* ([1ffd373](https://github.com/bbaia/protractor-net/commit/1ffd373f071b426d3054cfa65149aa5f0b6c5d52)) Add initial Angular 2 support (NgBy locators not supported)
* ([e08080c](https://github.com/bbaia/protractor-net/commit/e08080ca4e76505e374a35d3855672b241d802df)) Use Angular testability API for NgBy.Model and NgBy.Bindings

0.7.1 / 2016-04-07
==================

* ([9034ae9](https://github.com/bbaia/protractor-net/commit/9034ae993321e64d241e0fe08ceb92538a13f829)) Add the NgWebDriver.Location property and NgNavigation.GoToLocation method to perform in-page navigation
* ([373437c](https://github.com/bbaia/protractor-net/commit/373437c3e350a0dcdb4cc215f2c3747c7fdeb19e)) Reset URL on IE and PhantomJS before navigation

0.7.0 / 2016-03-29
==================

* ([77e5191](https://github.com/bbaia/protractor-net/commit/77e519137117a41838c8000959afa4339f6ec4a2)) Use NgWebDriver.WaitForAngular in NgNavigation methods (Fast, Forward and Refresh)
* ([dd6d015](https://github.com/bbaia/protractor-net/commit/dd6d015b1bee0cfebee5201e845772ee4d280ff8)) Add support for non-Angular pages
* ([188beec](https://github.com/bbaia/protractor-net/commit/188beec9fce3d0a5f21748fda7899fd253f28d74)) Made NgWebDriver.WaitForAngular public
* ([487fd34](https://github.com/bbaia/protractor-net/commit/487fd34165bf57036215a6aedda039124a3da00c)) Add ability to use NgBy methods as custom filter with FindsByAttribute (Page Objects pattern support)
* ([2a89f43](https://github.com/bbaia/protractor-net/commit/2a89f43242e9c28e2645e194e09e5f437931dff8)) Remove obsolete NgBy methods (Input, TextArea, Select)
* ([ef2a3aa](https://github.com/bbaia/protractor-net/commit/ef2a3aa71fa115a882557848e388c8aa5169ae8c)) Update to Selenium.WebDriver v2.53

0.6.0 / 2015-07-14
==================

* ([015799c](https://github.com/bbaia/protractor-net/commit/015799c84fe45a62839a8e1fe585a23c8f7b0306)) Add support for ng-repeat-start and ng-repeat-end elements
* ([1a2af2c](https://github.com/bbaia/protractor-net/commit/1a2af2cf15ca83355c32e9c9ca5676c7831889c3)) Update to Selenium.WebDriver v2.46
* ([8d85e80](https://github.com/bbaia/protractor-net/commit/8d85e80104a1a5efc4ddacff164adf13f2f4e215)) Update WaitForAngular client script (Improve error message and use Angular testability api if available)

0.5.0 / 2015-04-04
==================

* ([3a24008](https://github.com/bbaia/protractor-net/commit/3a24008861b061f0c9b0e34cfa2f0b98ba3cc55b)) Add NgBy.Model for getting elements by model regardless of element tag name
* ([633a6e5](https://github.com/bbaia/protractor-net/commit/633a6e541a28860bfc1c041771a30821c03fa7a7)) Update to Selenium.WebDriver v2.45

0.4.0 / 2014-12-03
==================

* ([4cb2bdc](https://github.com/bbaia/protractor-net/commit/4cb2bdceb4d04175c230cce9b9def6102299673d)) Add the ability to use the Actions class
* ([762b553](https://github.com/bbaia/protractor-net/commit/762b553a0bbae65273f16d1362ca1088e21620ac)) Update to Selenium.WebDriver v2.44

0.3.0 / 2013-10-27
==================

* ([078495e](https://github.com/bbaia/protractor-net/commit/078495e024827e0947d0b54d59f8825c7f83ba5d)) Add .NET 3.5 support
* ([a1003ef](https://github.com/bbaia/protractor-net/commit/a1003ef980ee2ec1908c92d8aa09575062e14d6b)) Add NgWebElement.Evaluate method
* ([068cced](https://github.com/bbaia/protractor-net/commit/068cced5db43440bba6a6ff5b774bb02c1265084)) Fix NgWebDriver.Url with IE (IEDriverServer.exe)

0.2.0 / 2013-10-20
==================

* Initial release