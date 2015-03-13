using OpenQA.Selenium;

namespace Protractor
{
    /// <summary>
    /// Mechanism used to locate elements within Angular applications by binding, model, etc.
    /// </summary>
    public static class NgBy
    {
        /// <summary>
        /// Gets a mechanism to find elements by their Angular binding.
        /// </summary>
        /// <param name="binding">The binding, e.g. '{{cat.name}}'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Binding(string binding)
        {
            return new JavaScriptBy(ClientSideScripts.FindBindings, binding);
        }

        /// <summary>
        /// Gets a mechanism to find input elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Input(string model)
        {
            return new JavaScriptBy(ClientSideScripts.FindInputs, model);
        }

        /// <summary>
        /// Gets the mechanism to find an element by model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static By Model(string model)
        {
            return new JavaScriptBy(ClientSideScripts.FindModel, model);
        }

        /// <summary>
        /// Gets a mechanism to find textarea elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By TextArea(string model)
        {
            return new JavaScriptBy(ClientSideScripts.FindTextArea, model);
        }

        /// <summary>
        /// Gets a mechanism to find select elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Select(string model)
        {
            return new JavaScriptBy(ClientSideScripts.FindSelects, model);
        }

        /// <summary>
        /// Gets a mechanism to find select option elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By SelectedOption(string model)
        {
            return new JavaScriptBy(ClientSideScripts.FindSelectedOptions, model);
        }

        /// <summary>
        /// Gets a mechanism to find all rows of an ng-repeat.
        /// </summary>
        /// <param name="repeat">The text of the repeater, e.g. 'cat in cats'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Repeater(string repeat)
        {
            return new JavaScriptBy(ClientSideScripts.FindAllRepeaterRows, repeat);
        }
    }
}