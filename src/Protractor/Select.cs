namespace Protractor
{
    /// <summary>
    /// Mechanism to find select elements by their model name.
    /// </summary>
    public class Select : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public Select(string model)
            : base(ClientSideScripts.FindSelects, model) { }
    }
}