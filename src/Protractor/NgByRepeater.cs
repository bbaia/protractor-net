using System;

namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.Repeater() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByRepeater : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByRepeater"/>.
        /// </summary>
        /// <param name="repeat">The text of the repeater, e.g. 'cat in cats'.</param>
        public NgByRepeater(string repeat)
            : base(ClientSideScripts.FindAllRepeaterRows, repeat, false)
        {
            base.Description = "NgBy.Repeater: " + repeat;
        }

        /// <summary>
        /// Creates a new instance of <see cref="NgByRepeater"/>.
        /// </summary>
        /// <param name="repeat">The text of the repeater, e.g. 'cat in cats'.</param>
        /// <param name="exactMatch">
        /// Indicates whether or not the repeater needs to be matched exactly. By default, false.
        /// </param>
        [Obsolete("Use 'NgByExactRepeater' instead.", false)]
        public NgByRepeater(string repeat, bool exactMatch)
            : base(ClientSideScripts.FindAllRepeaterRows, repeat, exactMatch)
        {
            base.Description = "NgBy.Repeater: " + repeat;
        }
    }
}
