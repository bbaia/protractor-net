Protractor for .NET
===================

The .NET port of [Protractor](https://github.com/angular/protractor), an end to end test framework for Angular applications.

Protractor for .NET is built on top of [Selenium WebDriver](http://www.seleniumhq.org/projects/webdriver/) C# binding.

## Get it from NuGet!

    PM> Install-Package Protractor

Supports Microsoft .NET Framework 3.5 and higher.

## Write tests!

```csharp
[Test]
public void ShouldGreetUsingBinding()
{
    using (var ngDriver = new NgWebDriver(new ChromeDriver()))
    {
        ngDriver.Url = "http://www.angularjs.org";
        ngDriver.FindElement(NgBy.Model("yourName")).SendKeys("Julie");
        Assert.AreEqual("Hello Julie!", ngDriver.FindElement(NgBy.Binding("yourName")).Text);
    }
}
```
