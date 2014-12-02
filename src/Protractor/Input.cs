namespace Protractor
{
    /// <summary>
    /// 
    /// </summary>
    public class Input : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public Input(string arg) : base(ClientSideScripts.FindInputs, arg)
        {
        }
    }
}
