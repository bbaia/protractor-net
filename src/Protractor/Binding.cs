namespace Protractor
{
    /// <summary>
    /// Mechanism used to find elements by their Angular binding.
    /// </summary>
    public class Binding : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        public Binding(string binding)
            : base(ClientSideScripts.FindBindings, binding) { }
    }
}