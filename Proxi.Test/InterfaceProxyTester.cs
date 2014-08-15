/* •——————————————————————————————————————————————————————————————————————————•
   | Copyright © Pablo Orozco (pablo@orozco.me).                              |
   | All rights reserved.                                                     |
   |                                                                          |
   | Licensed under the Apache License, Version 2.0 (the "License");          |
   | you may not use this file except in compliance with the License.         |
   | You may obtain a copy of the License at                                  |
   |                                                                          |
   | http://www.apache.org/licenses/LICENSE-2.0                               |
   |                                                                          |
   | Unless required by applicable law or agreed to in writing, software      |
   | distributed under the License is distributed on an "AS IS" BASIS,        |
   | WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. |
   | See the License for the specific language governing permissions and      |
   | limitations under the License.                                           |
   •——————————————————————————————————————————————————————————————————————————• */

using System;
using System.Linq;
using Xunit;
using Proxi;

namespace Proxi.Tests
{
    public class InterfaceProxyTester
    {
        [Fact]
		public void Should_create_a_non_target_proxy_using_a_generic_type_and_an_interceptor()
        {
			var interceptor = new ReturnValueInterceptor("ack");
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create<IFoo>(interceptor);
            Assert.Equal("ack", proxy.Go());
        }

        [Fact]
        public void Should_create_a_non_target_proxy_using_a_type_and_an_interceptor()
        {
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = (IFoo) factory.Create(typeof(IFoo), new ReturnValueInterceptor("ack"));
            Assert.Equal("ack", proxy.Go());
        }

        [Fact]
        public void Should_create_a_proxy_using_a_target_object_and_an_interceptor_that_forwards_execution()
        {
            IFoo target = new Foo();
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
			var interceptor = new ForwardExecutionInterceptor(); //-> Example of how to create an interceptor implementing IInterceptor
            var proxy = factory.Create(target, interceptor);
            Assert.Equal("ack", proxy.Go());
        }

		[Fact]
        public void Should_create_a_proxy_using_a_target_object_and_an_interceptor_that_validates_method_parameters()
		{
			IFoo target = new Foo();
			var interceptor = new NullArgumentInterceptor(); //-> Example of how to create an interceptor extending InterceptorBase
			var factory = new ProxyFactory(AppConfig.ProxyBehavior);
			var proxy = factory.Create(target, interceptor);
			Assert.Throws<ArgumentNullException>(() => proxy.GoTo(null));
		}

        [Fact]
        public void Should_create_a_non_target_proxy_using_an_anonymous_method()
        {
            Func<IMethodInvocation, object> del = mi => "ack";
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create<IFoo>( del );
            Assert.Equal("ack", proxy.Go());
        }

        [Fact]
        public void Should_create_a_proxy_using_a_target_object_and_a_lambda()
        {
            IFoo target = new Foo();
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create(target, mi => mi.Method.Invoke(mi.Target, mi.Arguments));
            Assert.Equal("ack", proxy.Go());
        }

		[Fact]
        public void Should_create_a_non_target_proxy_using_a_lambda_and_a_list_of_types_to_implement()
		{
			var factory = new ProxyFactory(AppConfig.ProxyBehavior);
			var proxy = factory.Create<IFoo>(mi => "ack", new[] { typeof(IDisposable), typeof(ICloneable) });
			Assert.Equal("ack", proxy.Go());
			Assert.True(typeof(IDisposable).IsAssignableFrom(proxy.GetType()));
			Assert.True(typeof(ICloneable).IsAssignableFrom(proxy.GetType()));
		}

		[Fact]
        public void Should_create_a_non_target_proxy_using_a_lambda()
		{
			var factory = new ProxyFactory(AppConfig.ProxyBehavior);
			var proxy = factory.Create<IFoo>(mi => "ack");
			Assert.Equal("ack", proxy.Go());
		}

		[Fact]
        public void Should_create_a_proxy_using_a_lambda()
		{
			var target = new Foo();
			var factory = new ProxyFactory(AppConfig.ProxyBehavior);
			var proxy = factory.Create<IFoo>(mi => target.Go()); //-> non-reflective call
			Assert.Equal("ack", proxy.Go());
		}
    }

}
