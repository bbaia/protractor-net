namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.Repeater() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByRepeater : JavaScriptBy
    {
        /// <summary>
        /// NgByRepeater ctor.
        /// </summary>
        /// <param name="repeat"></param>
        public NgByRepeater(string repeat)
            : base(ClientSideScripts.FindAllRepeaterRows, repeat)
        {
        }
    }
}
