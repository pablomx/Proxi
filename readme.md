## Project Description

Proxi (**Prox**y **I**nterface) is a light-weight library that offers a common interface for generating proxies at runtime (aka dynamic proxies). Making possible for developers to leverage runtime proxy generation without having to couple their frameworks, libraries or applications to an specific implementation.

Utilize Proxi to generate runtime-built proxies is not really different from using a proxy provider. In fact, Proxi has a seamless and modern API that can help to make the process of creating proxies simpler and more intuitive.

## Supported Proxy Providers

* [Castle Dynamic Proxy](http://www.castleproject.org/dynamicproxy/)
* [Spring.NET](http://www.springframework.net/)
* [LinFu](http://code.google.com/p/linfu/)
* [Unity](http://unity.codeplex.com/)

## Why Proxi?

This project is an effort to...
* **Improve flexibility and adaptability.** Ideally frameworks and libraries should not impose the use of a proxy provider to the applications that are built on top of them. By utilizing Proxi frameworks and libraries can delegate the decision of choosing or changing a proxy provider to final applications.
* **Minimize the impact of making a mistake.** It is commonly a bigger problem not to be prepared to reverse a wrong decision than the wrong decision itself. Proxi aims to keep your proxy provider decoupled from your application making easy to change it at any time.

## How does Proxi compare to a proxy provider?

From an application standpoint, Proxi can be understood as a proxy provider. It has an API that is relatively similar to some proxy provider APIs. However, technically, Proxi should be defined as a common interface that meditates the communication between an application and its proxy provider.

## How easy is to replace a proxy provider?

Proxi can abstract out the idea of having a proxy provider to the point where it is even possible to replace it without having to compile again.

## What do I need to utilize Proxi?

Download the latest release from this website.
Select 'Add Reference...' on your Visual Studio project.
Locate and select Proxi.dll.

## Where can I get proxy providers?

Proxi distribution includes a binary version and licenses of all supported proxy providers. 

## Quick Overview

**How to choose a proxy provider?**

```c#
var factory = new ProxyFactory(ProxyBehavior.YourProxyProvider); // [Spring](Castle_LinFu)
```

**How to create a method interceptor?**

```c#
class NullArgumentInterceptor : IInterceptor
{
	public object Run(IMethodInvocation mi)
	{
		if (mi.Arguments.Any(a => a == null)) throw new ArgumentNullException(); // throws if any argument is null
		return mi.Method.Invoke(mi.Target, mi.Arguments); // executes method on target
	}
}
```

**How to create a class proxy?**

```c#
var target = new Goo(); // public class with virtual members
var factory = new ProxyFactory(ProxyBehavior.Spring);
var proxy = factory.Create(target, new NullArgumentInterceptor());
proxy.Go(null); // throws
```

**How to create an interface proxy?**

```c#
var target = new Foo(); // public class that implements IFoo 
var factory = new ProxyFactory(ProxyBehavior.Spring);
var proxy = factory.Create<IFoo>(target, new NullArgumentInterceptor());
proxy.Go(null); // throws
```
