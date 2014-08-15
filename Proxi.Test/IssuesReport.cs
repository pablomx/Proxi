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
    public class IssuesReport
    {
        [Fact]
        public void Spring_behavior_does_not_support_non_target_proxies_on_types_without_a_default_constructor()
        {
            var expected = "ack";
            var spring = new ProxyFactory(ProxyBehavior.Spring);
            Assert.Throws<InvalidOperationException>(()=> spring.Create<Hoo>(mi => expected).Name);

            // linfu does...
            var linfu = new ProxyFactory(ProxyBehavior.LinFu);
            Assert.Same(expected, linfu.Create<Hoo>(mi => expected).Name);
        }

        [Fact]
        public void Castle_behavior_does_not_support_non_target_proxies_on_types_without_a_default_constructor()
        {
            var expected = "ack";
            var castle = new ProxyFactory(ProxyBehavior.Castle);
            Assert.Throws<ArgumentException>(() => castle.Create<Hoo>(mi => expected).Name);
        }

        [Fact]
        public void Castle_behavior_does_not_support_class_proxies_without_default_ctor()
        {
            var hoo = new Hoo("Hoo", 1);
            var interceptor = new ForwardExecutionInterceptor();
            var castle = new ProxyFactory(ProxyBehavior.Castle);
            Assert.Throws <System.ArgumentException>(() => castle.Create<Hoo>(hoo, interceptor));

            // linfu does...
            var linfu = new ProxyFactory(ProxyBehavior.LinFu);
            Assert.Same("Hoo",linfu.Create<Hoo>(hoo, interceptor).Name);

            // spring does...
            var spring = new ProxyFactory(ProxyBehavior.Spring);
            Assert.Same("Hoo", spring.Create<Hoo>(hoo, interceptor).Name);
        }

        [Fact]
        public void Spring_behavior_does_not_support_abstract_types() 
        {
            var expected = "ack";
            var spring = new ProxyFactory(ProxyBehavior.Spring);
            var proxy3 = Assert.Throws<InvalidOperationException>(() => spring.Create<Ioo>(mi => expected));

            // castle does...
            var castle = new ProxyFactory(ProxyBehavior.Castle);
            var proxy1 = castle.Create<Ioo>(mi => expected);
            Assert.Same(expected, proxy1.AbstractMethod());

            // linfu does...
            var lifu = new ProxyFactory(ProxyBehavior.LinFu);
            var proxy2 = castle.Create<Ioo>(mi => expected);
            Assert.Same(expected, proxy2.AbstractMethod());
        }

        //LinFu.DynamicProxy does not support out/ref parameters. It crashes badly.
        //[Fact]
        //public void Intercepts_a_method_using_out_parameters() 
        //{
        //	int a;
        //	IFoo target = new Foo();
        //	var factory = new ProxyFactory(AppConfig.ProxyBehavior);
        //	var proxy = factory.Create(target, new ForwardExecutionInterceptor());			
        //	proxy.Go(out a);			
        //	Assert.Equal(1, a);
        //}

    }
}
