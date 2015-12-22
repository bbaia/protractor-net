namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.SelectedOption() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgBySelectedOption : JavaScriptBy
    {
        /// <summary>
        /// NgBySelectedOption ctor.
        /// </summary>
        /// <param name="model"></param>
        public NgBySelectedOption(string model)
            : base(ClientSideScripts.FindSelectedOptions, model)
        {
        }
    }
}
