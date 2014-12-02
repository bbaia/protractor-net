namespace Protractor
{
    /// <summary>
    /// 
    /// </summary>
    public class Select : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public Select(string arg) : base(ClientSideScripts.FindSelects, arg)
        {
        }
    }
}
