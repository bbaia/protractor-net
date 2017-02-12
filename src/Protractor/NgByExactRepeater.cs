using System;

namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.ExactRepeater() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByExactRepeater : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByRepeater"/>.
        /// </summary>
        /// <param name="repeat">The exact text of the repeater, e.g. 'cat in cats'.</param>
        public NgByExactRepeater(string repeat)
            : base(ClientSideScripts.FindAllRepeaterRows, repeat, true)
        {
            base.Description = "NgBy.ExactRepeater: " + repeat;
        }
    }
}
