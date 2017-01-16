using System;

namespace Protractor
{
    /// <summary>
    /// Module automatically installed by Protractor when a page is loaded with Angular 1.
    /// </summary>
    internal class Ng1BaseModule : NgModule
    {
        private const string ModuleName = "protractorBaseModule_";

        private const string ModuleScript = "angular.module('" + ModuleName + @"', [])
.config([
  '$compileProvider',
  function($compileProvider) {
    if ($compileProvider.debugInfoEnabled) {
      $compileProvider.debugInfoEnabled(true);
    }
  }
]);";

        public Ng1BaseModule()
            : base(ModuleName, ModuleScript)
        {
        }
    }
}
