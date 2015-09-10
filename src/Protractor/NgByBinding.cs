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
            : base(ClientSideScripts.FindBindings, binding)
        {
        }
    }
}
