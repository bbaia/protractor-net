using System;
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
            return new NgByBinding(binding);
        }

        /// <summary>
        /// Gets a mechanism to find elements by their Angular binding.
        /// </summary>
        /// <param name="binding">The binding, e.g. '{{cat.name}}'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        /// <param name="exactMatch">
        /// Indicates whether or not the binding needs to be matched exactly. By default false.
        /// </param>
        [Obsolete("Use 'ExactBinding' instead.")]
        public static By Binding(string binding, bool exactMatch)
        {
            return new NgByBinding(binding, exactMatch);
        }

        /// <summary>
        /// Gets a mechanism to find elements by their exact Angular binding.
        /// </summary>
        /// <param name="binding">The exact binding, e.g. '{{cat.name}}'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By ExactBinding(string binding)
        {
            return new NgByExactBinding(binding);
        }

        /// <summary>
        /// Gets a mechanism to find elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Model(string model)
        {
            return new NgByModel(model);
        }

        /// <summary>
        /// Gets a mechanism to find select option elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By SelectedOption(string model)
        {
            return new NgBySelectedOption(model);
        }

        /// <summary>
        /// Gets a mechanism to find all rows of an ng-repeat.
        /// </summary>
        /// <param name="repeat">The text of the repeater, e.g. 'cat in cats'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Repeater(string repeat)
        {
            return new NgByRepeater(repeat);
        }

        /// <summary>
        /// Gets a mechanism to find all rows of an ng-repeat.
        /// </summary>
        /// <param name="repeat">The text of the repeater, e.g. 'cat in cats'.</param>
        /// <param name="exactMatch">
        /// Indicates whether or not the repeater needs to be matched exactly. By default, false.
        /// </param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        [Obsolete("Use 'ExactRepeater' instead.")]
        public static By Repeater(string repeat, bool exactMatch)
        {
            return new NgByRepeater(repeat, exactMatch);
        }

        /// <summary>
        /// Gets a mechanism to find all rows of an ng-repeat.
        /// </summary>
        /// <param name="repeat">The exact text of the repeater, e.g. 'cat in cats'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By ExactRepeater(string repeat)
        {
            return new NgByExactRepeater(repeat);
        }
    }
}