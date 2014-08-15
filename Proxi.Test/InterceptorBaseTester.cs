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
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxi;

namespace Proxi.Tests
{
    public class InterceptorBaseTester
    {
        [Fact]
		public void Should_run_a_callback_before_target_execution()
        {
            bool ack = false;
            var target = new Foo();
            var interceptor = new OnBeforeInterceptor {Action = ()=> { Assert.False(target.Executed); ack = true; }};
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create<IFoo>(target, interceptor);            
            proxy.Go();
            Assert.True(ack);
			Assert.True(target.Executed); 
        }

        [Fact]
        public void Should_skip_target_execution_and_return_an_arbitrary_value()
        {
            var target = new Foo();
			var interceptor = new OnInvokeInterceptor("ack");
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
            var proxy = factory.Create<IFoo>(target, interceptor);
			Assert.Equal("ack", proxy.Go());
			Assert.False(target.Executed); 
        }

		[Fact]
        public void Should_catch_an_exception_and_run_a_callback_before_and_after_the_exception_is_rethrown()
		{
			int ack = 0;
			var target = new Foo();
			var interceptor = new OnCatchAndOnFinallyInterceptor{ Action = ()=> { Assert.True(target.Executed); ++ack; } };
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
			var proxy = factory.Create<IFoo>(target, interceptor);
            Assert.Throws<InvalidOperationException>(() => proxy.Fail());
			Assert.Equal(2, ack);
		}

		[Fact]
        public void Should_run_a_callback_after_target_execution()
		{
			bool ack = false;
			var target = new Foo();
			var interceptor = new OnAfterInterceptor{ Action = ()=> { Assert.True(target.Executed); ack = true; } };
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
			var proxy = factory.Create<IFoo>(target, interceptor);
			proxy.Go();
			Assert.True(ack);
		}

		[Fact]
        public void Should_replace_target_execution_result()
		{
			var target = new Foo();
			var interceptor = new OnReturnInterceptor("ack");
            var factory = new ProxyFactory(AppConfig.ProxyBehavior);
			var proxy = factory.Create<IFoo>(target, interceptor);
			proxy.Go();
			Assert.Equal("ack", proxy.Go());
			Assert.True(target.Executed);
		}
    }

}
