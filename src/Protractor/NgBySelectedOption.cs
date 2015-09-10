namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.SelectedOption() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgBySelectedOption : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByBinding"/>.
        /// </summary>
        /// <param name="model">The model name.</param>
        public NgBySelectedOption(string model)
            : base(ClientSideScripts.FindSelectedOptions, model)
        {
        }
    }
}
