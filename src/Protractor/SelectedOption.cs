namespace Protractor
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectedOption : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public SelectedOption(string arg) : base(ClientSideScripts.FindSelectedOptions, arg)
        {
        }
    }
}
