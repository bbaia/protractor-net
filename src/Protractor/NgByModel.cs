namespace Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.Model() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByModel : JavaScriptBy
    {
        /// <summary>
        /// NgByRepeater ctor.
        /// </summary>
        /// <param name="model"></param>
        public NgByModel(string model)
            : base(ClientSideScripts.FindModel, model)
        {
        }
    }
}
