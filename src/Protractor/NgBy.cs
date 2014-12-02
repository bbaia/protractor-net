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
            return new Binding(binding);
        }

        /// <summary>
        /// Gets a mechanism to find input elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Input(string model)
        {
            return new Input(model);
        }

        /// <summary>
        /// Gets a mechanism to find textarea elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By TextArea(string model)
        {
            return new TextArea(model);
        }

        /// <summary>
        /// Gets a mechanism to find select elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Select(string model)
        {
            return new Select(model);
        }

        /// <summary>
        /// Gets a mechanism to find select option elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By SelectedOption(string model)
        {
            return new SelectedOption(model);
        }

        /// <summary>
        /// Gets a mechanism to find all rows of an ng-repeat.
        /// </summary>
        /// <param name="repeat">The text of the repeater, e.g. 'cat in cats'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Repeater(string repeat)
        {
            return new Repeater(repeat);
        }
    }
}