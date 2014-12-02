namespace Protractor
{
    /// <summary>
    /// 
    /// </summary>
    public class Repeater : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public Repeater(string arg) : base(ClientSideScripts.FindAllRepeaterRows, arg)
        {
        }
    }
}
