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
    public class ClassProxyTester
    {
        [Fact]
        public void Should_create_a_non_target_proxy_using_a_generic_type_and_an_interceptor()
        {
            var interceptor = new ReturnValueInterceptor("ack");
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create<Goo>(interceptor);
            Assert.Equal("ack", proxy.Go());
        }

        [Fact]
        public void Should_create_a_proxy_using_a_target_object_and_an_interceptor()
        {
            var interceptor = new ForwardExecutionInterceptor();
            var target = new Goo();
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create(target, interceptor);
            Assert.Equal("ack", proxy.Go());
        }

        [Fact]
        public void Should_create_a_non_target_proxy_using_a_lambda_and_a_list_of_types_to_implement()
        {
            var types = new[] { typeof(IDisposable), typeof(ICloneable) };
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create<Goo>(mi => "ack", types);
            Assert.Equal("ack", proxy.Go());
            Assert.True(typeof(IDisposable).IsAssignableFrom(proxy.GetType()));
            Assert.True(typeof(ICloneable).IsAssignableFrom(proxy.GetType()));
        }

        [Fact]
        public void Should_create_a_non_target_proxy_using_a_delegate()
        {
            Func<IMethodInvocation, object> del = delegate(IMethodInvocation mi) { return "ack"; };
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create<Goo>(del);
            Assert.Equal("ack", proxy.Go());
        }

        [Fact]
        public void Should_create_a_proxy_using_a_target_object_and_a_lambda()
        {
            var target = new Goo();
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create(target, mi => mi.Method.Invoke(mi.Target, mi.Arguments));
            Assert.Equal("ack", proxy.Go());
        }

    }
}
