namespace Protractor
{
    /// <summary>
    /// Represents an Angular module to load before Angular is loaded.
    /// <para/>
    /// The modules will be registered after existing modules,
    /// so any module registered will override preexisting modules with the same name.
    /// </summary>
    public class NgModule
    {
        /// <summary>
        /// Gets or sets the name of the module to load or override.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the JavaScript to load the module.
        /// </summary>
        public string Script { get; protected set; }

        /// <summary>
        /// Creates a new instance of <see cref="NgModule"/>.
        /// </summary>
        /// <param name="name">The name of the module to load or override.</param>
        /// <param name="script">The JavaScript to load the module.</param>
        public NgModule(string name, string script)
        {
            this.Name = name;
            this.Script = script;
        }
    }
}
