namespace Protractor
{
    /// <summary>
    /// 
    /// </summary>
    public class Binding : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public Binding(string arg) : base(ClientSideScripts.FindBindings, arg)
        {

        }
    }
}
