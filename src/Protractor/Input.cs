namespace Protractor
{
    /// <summary>
    /// Mechanism used to find input elements by their model name.
    /// </summary>
    public class Input : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public Input(string model)
            : base(ClientSideScripts.FindInputs, model) { }
    }
}