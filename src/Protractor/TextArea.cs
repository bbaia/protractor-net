namespace Protractor
{
    /// <summary>
    /// Mechanism to find textarea elements by their model name
    /// </summary>
    public class TextArea : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public TextArea(string model)
            : base(ClientSideScripts.FindTextArea, model) { }
    }
}