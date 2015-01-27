namespace Protractor
{
    /// <summary>
    /// Mechanism to find selected option elements by their model name
    /// </summary>
    public class SelectedOption : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public SelectedOption(string model)
            : base(ClientSideScripts.FindSelectedOptions, model) { }
    }
}