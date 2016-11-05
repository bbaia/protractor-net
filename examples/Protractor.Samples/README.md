# Samples description

## Basic

E2E testing against [AngularJS's homepage](http://angularjs.org/).

Show basic usage of the library.

## MockHttpBackend

E2E testing against the [AngularJS tutorial](http://docs.angularjs.org/tutorial) Step 7 sample.

Use the [ngMockE2E](http://docs.angularjs.org/api/ngMockE2E.$httpBackend) Angular module to mock the HTTP backend. 

## PageObjects

E2E testing against the [AngularJS tutorial](http://docs.angularjs.org/tutorial) Step 7 sample.

Use the [Page Objects](https://code.google.com/p/selenium/wiki/PageObjects) pattern to make your tests more readable.

# Running tests

Tests are written with [NUnit](http://nunit.org/).

There are several ways to execute these tests:
* Using NUnit TestAdpater for Visual Studio 2012/2013 from the [NUnitTestAdapter](https://www.nuget.org/packages/NUnitTestAdapter) NuGet package.
* Using Visual Studio extensions like [ReSharper](http://www.jetbrains.com/resharper/) or [TestDriven.net](http://www.testdriven.net/).
* Using the command line or GUI provided by the [NUnit.Runners](https://www.nuget.org/packages/NUnit.Runners) NuGet package.
