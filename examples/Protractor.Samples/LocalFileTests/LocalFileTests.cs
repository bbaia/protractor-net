using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Protractor.Extensions;

namespace Protractor.Test
{

    [TestFixture]
    public class LocalFileTests
    {
        private StringBuilder verificationErrors = new StringBuilder();
        private IWebDriver driver;
        private NgWebDriver ngDriver;

        // private String testpage;

        [TestFixtureSetUp]
        public void SetUp()
        {
			driver = new PhantomJSDriver();
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(60));
            ngDriver = new NgWebDriver(driver);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception) { } /* Ignore cleanup errors */
            Assert.IsEmpty(verificationErrors.ToString());
        }
        private void GetPageContent(string testpage)
        {
            String base_url = new System.Uri(Path.Combine(Directory.GetCurrentDirectory(), testpage)).AbsoluteUri;
            ngDriver.Navigate().GoToUrl(base_url);
        }

        [Test]
        public void ShouldDropDown()
        {
            GetPageContent("ng_dropdown.htm");
            string optionsCountry = "country for (country, states) in countries";
            ReadOnlyCollection<NgWebElement> ng_countries = ngDriver.FindElements(NgBy.Options(optionsCountry));

            Assert.IsTrue(4 == ng_countries.Count);
            Assert.IsTrue(ng_countries[0].Enabled);
            string optionsState = "state for (state,city) in states";
            NgWebElement ng_state = ngDriver.FindElement(NgBy.Options(optionsState));
            Assert.IsFalse(ng_state.Enabled);
            SelectElement countries = new SelectElement(ngDriver.FindElement(NgBy.Model("states")).WrappedElement);
            countries.SelectByText("Australia");
            Thread.Sleep(1000);
            Assert.IsTrue(ng_state.Enabled);
            NgWebElement ng_selected_country = ngDriver.FindElement(NgBy.SelectedOption(optionsCountry));
            // TODO:debug (works in Java client)
            // Assert.IsNotNull(ng_selected_country.WrappedElement);
            // ng_countries = ngDriver.FindElements(NgBy.Options(optionsCountry));
            NgWebElement ng_country = ng_countries.First(o => o.Selected);
            StringAssert.IsMatch("Australia", ng_country.Text);
        }

        [Test]
        public void ShouldDropDownWatch()
        {
            GetPageContent("ng_dropdown_watch.htm");
            string optionsCountry = "country for country in countries";
            ReadOnlyCollection<NgWebElement> ng_countries = ngDriver.FindElements(NgBy.Options(optionsCountry));

            Assert.IsTrue(3 == ng_countries.Count);
            Assert.IsTrue(ng_countries[0].Enabled);
            string optionsState = "state for state in states";
            NgWebElement ng_state = ngDriver.FindElement(NgBy.Options(optionsState));
            Assert.IsFalse(ng_state.Enabled);
            SelectElement countries = new SelectElement(ngDriver.FindElement(NgBy.Model("country")).WrappedElement);
            countries.SelectByText("china");
            Thread.Sleep(1000);
            Assert.IsTrue(ng_state.Enabled);
            NgWebElement ng_selected_country = ngDriver.FindElement(NgBy.SelectedOption("country"));
            Assert.IsNotNull(ng_selected_country.WrappedElement);
            NgWebElement ng_country = ng_countries.First(o => o.Selected);
            StringAssert.IsMatch("china", ng_country.Text);

        }

        [Test]
        public void ShouldEvaluateIf()
        {
            GetPageContent("ng_watch_ng_if.htm");
            IWebElement button = ngDriver.FindElement(By.CssSelector("button.btn"));
            NgWebElement ng_button = new NgWebElement(ngDriver, button);
            Object state = ng_button.Evaluate("!house.frontDoor.isOpen");
            Assert.IsTrue(Convert.ToBoolean(state));
            StringAssert.IsMatch("house.frontDoor.open()", button.GetAttribute("ng-click"));
            StringAssert.IsMatch("Open Door", button.Text);
            button.Click();
        }

        [Test]
        public void ShouldFindRepeaterSelectedtOption()
        {
            GetPageContent("ng_repeat_selected.htm");
            NgWebElement ng_element = ngDriver.FindElement(NgBy.SelectedRepeaterOption("fruit in Fruits"));
            StringAssert.IsMatch("Mango", ng_element.Text);
        }


        [Test]
        public void ShouldChangeRepeaterSelectedtOption()
        {
            GetPageContent("ng_repeat_selected.htm");
            NgWebElement ng_element = ngDriver.FindElement(NgBy.SelectedRepeaterOption("fruit in Fruits"));
            StringAssert.IsMatch("Mango", ng_element.Text);
            ReadOnlyCollection<NgWebElement> ng_elements = ngDriver.FindElements(NgBy.Repeater("fruit in Fruits"));
            ng_element = ng_elements.First(o => String.Compare("Orange", o.Text,
                                                                    StringComparison.InvariantCulture) == 0);
            ng_element.Click();
            string text = ng_element.Text;
            // to trigger WaitForAngular
            Assert.IsTrue(ng_element.Displayed);
            // reload
            ng_element = ngDriver.FindElement(NgBy.SelectedRepeaterOption("fruit in Fruits"));
            StringAssert.IsMatch("Orange", ng_element.Text);

        }


        [Test]
        public void ShouldHandleMultiSelect()
        // appears to be broken in PahtomJS / working in desktop browsers
        {
            Actions actions = new Actions(ngDriver.WrappedDriver);
            GetPageContent("ng_multi_select.htm");
            IWebElement element = ngDriver.FindElement(NgBy.Model("selectedValues"));
            // use core Selenium
            IList<IWebElement> options = new SelectElement(element).Options;
            IEnumerator<IWebElement> etr = options.Where(o => Convert.ToBoolean(o.GetAttribute("selected"))).GetEnumerator();
            while (etr.MoveNext())
            {
                Console.Error.WriteLine(etr.Current.Text);
            }
            foreach (IWebElement option in options)
            {
                // http://selenium.googlecode.com/svn/trunk/docs/api/dotnet/html/AllMembers_T_OpenQA_Selenium_Keys.htm
                actions.KeyDown(Keys.Control).Click(option).KeyUp(Keys.Control).Build().Perform();
                // triggers ngDriver.WaitForAngular()
                Assert.IsNotEmpty(ngDriver.Url);
            }
            // re-read select options
            element = ngDriver.FindElement(NgBy.Model("selectedValues"));
            options = new SelectElement(element).Options;
            etr = options.Where(o => Convert.ToBoolean(o.GetAttribute("selected"))).GetEnumerator();
            while (etr.MoveNext())
            {
                Console.Error.WriteLine(etr.Current.Text);
            }
        }

        [Test]
        public void ShouldPrintOrderByFieldColumn()
        {
            GetPageContent("ng_headers_sort_example2.htm");
            String[] headers = new String[] { "First Name", "Last Name", "Age" };
            foreach (String header in headers)
            {
                for (int cnt = 0; cnt != 2; cnt++)
                {
                    IWebElement headerElement = ngDriver.FindElement(By.XPath("//th/a[contains(text(),'" + header + "')]"));
                    Console.Error.WriteLine("Clicking on header: " + header);
                    headerElement.Click();
                    // triggers ngDriver.WaitForAngular()
                    Assert.IsNotEmpty(ngDriver.Url);
                    ReadOnlyCollection<NgWebElement> ng_emps = ngDriver.FindElements(NgBy.Repeater("emp in data.employees"));
                    NgWebElement ng_emp = ng_emps[0];
                    String field = ng_emp.GetAttribute("ng-order-by");
                    Console.Error.WriteLine(field + ": " + ng_emp.Evaluate(field).ToString());
                    String empField = "emp." + ng_emp.Evaluate(field);
                    Console.Error.WriteLine(empField + ":");
                    var ng_emp_enumerator = ng_emps.GetEnumerator();
                    ng_emp_enumerator.Reset();
                    while (ng_emp_enumerator.MoveNext())
                    {
                        ng_emp = (NgWebElement)ng_emp_enumerator.Current;
                        if (ng_emp.Text == null)
                        {
                            break;
                        }
                        Assert.IsNotNull(ng_emp.WrappedElement);

                        // Console.Error.WriteLine(ngEmp.getAttribute("innerHTML"));
                        try
                        {
                            NgWebElement ng_column = ng_emp.FindElement(NgBy.Binding(empField));
                            Assert.IsNotNull(ng_column);
                            Console.Error.WriteLine(ng_column.Text);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex.ToString());
                        }
                    }
                }
            }
        }

        [Test]
        public void ShouldFindOrderByField()
        {
            GetPageContent("ng_headers_sort_example1.htm");

            String[] headers = new String[] { "First Name", "Last Name", "Age" };
            foreach (String header in headers)
            {
                IWebElement headerelement = ngDriver.FindElement(By.XPath(String.Format("//th/a[contains(text(),'{0}')]", header)));
                Console.Error.WriteLine(header);
                headerelement.Click();
                // to trigger WaitForAngular
                Assert.IsNotEmpty(ngDriver.Url);
                IWebElement emp = ngDriver.FindElement(NgBy.Repeater("emp in data.employees"));
                NgWebElement ngRow = new NgWebElement(ngDriver, emp);
                String orderByField = emp.GetAttribute("ng-order-by");
                Console.Error.WriteLine(orderByField + ": " + ngRow.Evaluate(orderByField).ToString());
            }


            ReadOnlyCollection<NgWebElement> ng_people = ngDriver.FindElements(NgBy.Repeater("person in people"));
            var ng_people_enumerator = ng_people.GetEnumerator();
            ng_people_enumerator.Reset();
            while (ng_people_enumerator.MoveNext())
            {
                NgWebElement ng_person = (NgWebElement)ng_people_enumerator.Current;
                if (ng_person.Text == null)
                {
                    break;
                }
                NgWebElement ng_name = ng_person.FindElement(NgBy.Binding("person.Name"));
                Assert.IsNotNull(ng_name.WrappedElement);
                Object obj_country = ng_person.Evaluate("person.Country");
                Assert.IsNotNull(obj_country);
                if (String.Compare("Around the Horn", ng_name.Text) == 0)
                {
                    StringAssert.IsMatch("UK", obj_country.ToString());
                }
            }
        }

        [Test]
        public void ShouldFindElementByModel()
        {
            //  NOTE: works with Angular 1.2.13, fails with Angular 1.4.9
            GetPageContent("ng_pattern_validate.htm");
            NgWebElement ng_input = ngDriver.FindElement(NgBy.Model("myVal"));
            ng_input.Clear();
            NgWebElement ng_valid = ngDriver.FindElement(NgBy.Binding("form.value.$valid"));
            StringAssert.IsMatch("false", ng_valid.Text);

            NgWebElement ng_pattern = ngDriver.FindElement(NgBy.Binding("form.value.$error.pattern"));
            StringAssert.IsMatch("false", ng_pattern.Text);

            NgWebElement ng_required = ngDriver.FindElement(NgBy.Binding("!!form.value.$error.required"));
            StringAssert.IsMatch("true", ng_required.Text);

            ng_input.SendKeys("42");
            Assert.IsTrue(ng_input.Displayed);
            ng_valid = ngDriver.FindElement(NgBy.Binding("form.value.$valid"));
            StringAssert.IsMatch("true", ng_valid.Text);

            ng_pattern = ngDriver.FindElement(NgBy.Binding("form.value.$error.pattern"));
            StringAssert.IsMatch("false", ng_pattern.Text);

            ng_required = ngDriver.FindElement(NgBy.Binding("!!form.value.$error.required"));
            StringAssert.IsMatch("false", ng_required.Text);
        }

        [Test]
        public void ShouldFindElementByRepeaterColumn()
        {
            GetPageContent("ng_service.htm");
            ReadOnlyCollection<NgWebElement> ng_countries = ngDriver.FindElements(NgBy.RepeaterColumn("person in people", "person.Country"));

            Assert.AreEqual(3, ng_countries.Count(o => String.Compare("Mexico", o.Text,
                                                     StringComparison.InvariantCulture) == 0));
        }

        [Test]
        public void ShouldFindSelectedtOption()
        {
            GetPageContent("ng_select_array.htm");
            //  NOTE: works with Angular 1.2.13, fails with Angular 1.4.9
            NgWebElement ng_element = ngDriver.FindElement(NgBy.SelectedOption("myChoice"));
            StringAssert.IsMatch("three", ng_element.Text);
            Assert.IsTrue(ng_element.Displayed);
        }

        [Test]
        public void ShouldChangeSelectedtOption()
        {
            GetPageContent("ng_select_array.htm");
            ReadOnlyCollection<NgWebElement> ng_elements = ngDriver.FindElements(NgBy.Repeater("option in options"));
            NgWebElement ng_element = ng_elements.First(o => String.Compare("two", o.Text,
                                                                    StringComparison.InvariantCulture) == 0);
            ng_element.Click();
            string text = ng_element.Text;
            // to trigger WaitForAngular
            Assert.IsTrue(ng_element.Displayed);

            ng_element = ngDriver.FindElement(NgBy.SelectedOption("myChoice"));
            StringAssert.IsMatch(text, ng_element.Text);
            // Assert.IsTrue(ng_element.Displayed);
        }

        [Test]
        public void ShouldFindCells()
        {
            //  NOTE: works with Angular 1.2.13, fails with Angular 1.4.9
            GetPageContent("ng_repeat_start_end.htm");
            ReadOnlyCollection<NgWebElement> elements = ngDriver.FindElements(NgBy.RepeaterColumn("definition in definitions", "definition.text"));
            Assert.AreEqual(2, elements.Count);
            StringAssert.IsMatch("Lorem ipsum", elements[0].Text);
        }

        [Test]
        public void ShouldFindOptions()
        {
            // base_url = "http://www.java2s.com/Tutorials/AngularJSDemo/n/ng_options_with_object_example.htm";
            GetPageContent("ng_options_with_object.htm");
            ReadOnlyCollection<NgWebElement> elements = ngDriver.FindElements(NgBy.Options("c.name for c in colors"));
            Assert.AreEqual(5, elements.Count);
            try
            {
                List<Dictionary<String, String>> result = elements[0].ScopeOf();
            }
            catch (InvalidOperationException)
            {
                // Maximum call stack size exceeded.            
                // TODO
            }
            StringAssert.IsMatch("black", elements[0].Text);
            StringAssert.IsMatch("white", elements[1].Text);
        }

        [Test]
        public void ShouldFindRows()
        {
            GetPageContent("ng_repeat_start_end.htm");
            ReadOnlyCollection<NgWebElement> elements = ngDriver.FindElements(NgBy.Repeater("definition in definitions"));
            Assert.IsTrue(elements[0].Displayed);

            StringAssert.AreEqualIgnoringCase(elements[0].Text, "Foo");
        }

        [Test]
        public void ShouldUpload()
        {
            GetPageContent("ng_upload1.htm");
            // need to run static page in local web server - otherwise test would not actually do anything
            // ngDriver.Navigate().GoToUrl("http://localhost:8080/ng_upload1.htm");
            
            IWebElement file = driver.FindElement(By.CssSelector("div[ng-controller = 'myCtrl'] > input[type='file']"));
            Assert.IsNotNull(file);
            StringAssert.AreEqualIgnoringCase(file.GetAttribute("file-model"), "myFile");
            String localPath = CreateTempFile("lorem ipsum dolor sit amet");
            
            
            IAllowsFileDetection fileDetectionDriver = driver as IAllowsFileDetection;
            if (fileDetectionDriver == null)
            {
                Assert.Fail("driver does not support file detection. This should not be");
            }

            fileDetectionDriver.FileDetector = new LocalFileDetector();

            try
            {
                file.SendKeys(localPath);
            }
            catch (WebDriverException e)
            {
                // the operation has timed out
                Console.Error.WriteLine(e.Message);
            }
            NgWebElement button = ngDriver.FindElement(NgBy.ButtonText("Upload"));
            button.Click();
            NgWebElement ng_file = new NgWebElement(ngDriver, file);
            Object myFile = ng_file.Evaluate("myFile");
            if (myFile != null)
            {
                Dictionary<String,Object> result = (Dictionary<String,Object>) myFile;
                Assert.IsTrue(result.Keys.Contains("name"));
                Assert.IsTrue(result.Keys.Contains("type"));
                Assert.IsTrue(result.Keys.Contains("size"));
            }
            else
            {
                Console.Error.WriteLine("myFile is null");
            }
            String script = "var e = angular.element(arguments[0]); var f = e.scope().myFile; if (f){return f.name} else {return null;}";
            try
            {
                Object result = ((IJavaScriptExecutor)driver).ExecuteScript(script, ng_file);
                if (result != null)
                {
                    Console.Error.WriteLine(result.ToString());
                }
                else
                {
                    Console.Error.WriteLine("result is null");
                }
            }
            catch (InvalidOperationException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        [Test]
        public void ShouldFindAllBindings()
        {
            GetPageContent("ng_directive_binding.htm");
            IWebElement container = ngDriver.FindElement(By.CssSelector("body div"));
            Console.Error.WriteLine(container.GetAttribute("innerHTML"));
            ReadOnlyCollection<NgWebElement> elements = ngDriver.FindElements(NgBy.Binding("name"));
            Assert.AreEqual(5, elements.Count);
            foreach (NgWebElement element in elements)
            {
                Console.Error.WriteLine(element.GetAttribute("outerHTML"));
                Console.Error.WriteLine(String.Format("Identity: {0}", element.IdentityOf()));
                Console.Error.WriteLine(String.Format("Text: {0}", element.Text));
            }
        }

        [Test]
        public void ShouldAngularTodoApp()
        {
            GetPageContent("ng_todo.htm");
            ReadOnlyCollection<NgWebElement> ng_todo_elements = ngDriver.FindElements(NgBy.Repeater("todo in todoList.todos"));
            String ng_identity = ng_todo_elements[0].IdentityOf();
            // <input type="checkbox" ng-model="todo.done" class="ng-pristine ng-untouched ng-valid">
            // <span class="done-true">learn angular</span>
            List<Dictionary<String, String>> todo_scope_data = ng_todo_elements[0].ScopeDataOf("todoList.todos");
            int todo_index = todo_scope_data.FindIndex(o => String.Equals(o["text"], "build an angular app"));
            Assert.AreEqual(1, todo_index);
            //foreach (var row in todo_scope_data)
            //{
            //    foreach (string key in row.Keys)
            //    {
            //        Console.Error.WriteLine(key + " " + row[key]);
            //    }
            //}
        }
        
        private string CreateTempFile(string content)
        {
            FileInfo testFile = new FileInfo("webdriver.tmp");
            if (testFile.Exists)
            {
                testFile.Delete();
            }
            StreamWriter testFileWriter = testFile.CreateText();
            testFileWriter.WriteLine(content);
            testFileWriter.Close();
            return testFile.FullName;
        }

    }
}