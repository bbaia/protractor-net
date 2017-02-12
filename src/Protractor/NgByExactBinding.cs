using System;

namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.ExactBinding() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByExactBinding : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByExactBinding"/>.
        /// </summary>
        /// <param name="binding">The exact binding, e.g. '{{cat.name}}'.</param>
        public NgByExactBinding(string binding)
            : base(ClientSideScripts.FindBindings, binding, true)
        {
            base.Description = "NgBy.ExactBinding: " + binding;
        }
    }
}
