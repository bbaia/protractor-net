namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.Binding() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByBinding : JavaScriptBy
    {
        /// <summary>
        /// NgByBinding ctor.
        /// </summary>
        /// <param name="binding"></param>
        public NgByBinding(string binding)
            : base(ClientSideScripts.FindBindings, binding)
        {
        }
    }
}
