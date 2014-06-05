AutoRegress
===========

A tiny library for automating system regression tests.

>Write tests cases, not tests.

Use AutoRegress to create a snapshot of the results of your class methods on the file system and verify they remain constant as you make changes.

Usage
----

```js
using AutoRegress;

// When then class is done
var foo  = new Foo();
Tester.StoreStateForClass(foo);

// ...
// Some refactoring later
// ...

var isUnchanged = Tester.CheckStateForClass(foo);
Assert.IsTrue(isUnchanged);

```

Wrappers
--

AutoRegress caches the serialised result of all public declared nullary instance methods, so for more complicated scenarios you can create wrapper classes around your objects to document your test cases and perform transformations.

```js
public class ControllerWrapper
{
    public string GetUser()
    {
        int wellKnownValue = 5; // User 5 is Dave
        var controller = new UserController();
        var response = controller.Get(wellKnownValue);
        return response.Content.ReadAsStringAsync().Result;
    }
}    
```
