using OpenQA.Selenium;

namespace Protractor
{
    public static class NgBy
    {
        public static By Binding(string binding)
        {
            return new JavaScriptBy(ClientSideScripts.FindBindings, binding);
        }

        public static By Input(string model)
        {
            return new JavaScriptBy(ClientSideScripts.FindInputs, model);
        }

        public static By TextArea(string model)
        {
            return new JavaScriptBy(ClientSideScripts.FindTextArea, model);
        }

        public static By Select(string model)
        {
            return new JavaScriptBy(ClientSideScripts.FindSelects, model);
        }

        public static By SelectedOption(string model)
        {
            return new JavaScriptBy(ClientSideScripts.FindSelectedOptions, model);
        }

        public static By Repeater(string repeat)
        {
            return new JavaScriptBy(ClientSideScripts.FindAllRepeaterRows, repeat);
        }
    }
}