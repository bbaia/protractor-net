using System;

namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.Binding() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByBinding : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByBinding"/>.
        /// </summary>
        /// <param name="binding">The binding, e.g. '{{cat.name}}'.</param>
        public NgByBinding(string binding)
            : base(ClientSideScripts.FindBindings, binding, false)
        {
            base.Description = "NgBy.Binding: " + binding;
        }

        /// <summary>
        /// Creates a new instance of <see cref="NgByBinding"/>.
        /// </summary>
        /// <param name="binding">The binding, e.g. '{{cat.name}}'.</param>
        /// <param name="exactMatch">
        /// Indicates whether or not the binding needs to be matched exactly. By default, false.
        /// </param>
        [Obsolete("Use 'NgByExactBinding' instead.", false)]
        public NgByBinding(string binding, bool exactMatch)
            : base(ClientSideScripts.FindBindings, binding, exactMatch)
        {
            base.Description = "NgBy.Binding: " + binding;
        }
    }
}
