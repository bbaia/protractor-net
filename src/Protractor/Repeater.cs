namespace Protractor
{
    /// <summary>
    /// Mechanism to find all rows of an ng-repeat
    /// </summary>
    public class Repeater : JavaScriptBy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repeat"></param>
        public Repeater(string repeat)
            : base(ClientSideScripts.FindAllRepeaterRows, repeat) { }
    }
}