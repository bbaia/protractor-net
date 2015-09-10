namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.Repeater() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByRepeater : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByBinding"/>.
        /// </summary>
        /// <param name="repeat">The text of the repeater, e.g. 'cat in cats'.</param>
        public NgByRepeater(string repeat)
            : base(ClientSideScripts.FindAllRepeaterRows, repeat)
        {
        }
    }
}
